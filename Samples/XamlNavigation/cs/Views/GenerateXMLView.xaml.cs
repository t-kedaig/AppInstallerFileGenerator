using AppInstallerFileGenerator;
using AppInstallerFileGenerator.Model;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace AppInstallerFileGenerator.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GenerateXMLView : Page
    {
        private Button _generateButton;
        
        /***************************************************************************
         * 
         * Constructor
         *
         ***************************************************************************/
        public GenerateXMLView()
        {
            this.InitializeComponent();
            _generateButton = (Button)this.FindName("Generate_Button");

        }

        /***************************************************************************
         * 
         * Private Methods
         *
         ***************************************************************************/

        private void Generate_File_Button_Click(object sender, RoutedEventArgs e)
        {
            //Check that all required fields have been filled
            if (!_validateInput())
            {
                return;
            }

            try
            {
                var t = Task.Run(() =>
                {
                    //Create file
                    FileStream writer = new FileStream(ApplicationData.Current.LocalFolder.Path + "//Your_AppInstaller_File_Name.xml", FileMode.Create);

                    AppInstaller appInstaller = new AppInstaller(App.AppInstallerFilePath, App.AppInstallerVersionNumber);

                    //Initialize DCS of type AppInstallerModel
                    XmlDictionaryWriter xdw = XmlDictionaryWriter.CreateTextWriter(writer, Encoding.UTF8);

                    DataContractSerializer appInstallerDCS = new DataContractSerializer(typeof(AppInstaller));

                    //AppInstaller Content
                    appInstallerDCS.WriteStartObject(xdw, appInstaller);
                    xdw.WriteAttributeString("xmlns", "http://schemas.microsoft.com/appx/appinstaller/2017");
                    xdw.WriteAttributeString("Uri", App.AppInstallerFilePath);
                    xdw.WriteAttributeString("Version", App.AppInstallerVersionNumber);

                    //Main Package Content
                    if (App.MainPackage.PackageType == PackageType.MSIX)
                    {
                        DataContractSerializer mainPackageDCS = new DataContractSerializer(typeof(MainPackage));
                        MainPackage mainPackage = new MainPackage(App.MainPackage.FilePath, App.MainPackage.Version, App.MainPackage.Publisher, App.MainPackage.Name, App.MainPackage.PackageType, App.MainPackage.ProcessorArchitecture, App.MainPackage.ResourceId); 
                        mainPackageDCS.WriteStartObject(xdw, mainPackage);
                        xdw.WriteAttributeString("Uri", mainPackage.FilePath);
                        xdw.WriteAttributeString("Version", mainPackage.Version);
                        xdw.WriteAttributeString("Publisher", mainPackage.Publisher);
                        if (mainPackage.ResourceId != "")
                        {
                            xdw.WriteAttributeString("ResourceId", mainPackage.ResourceId);
                        }
                        if (mainPackage.ProcessorArchitecture != ProcessorArchitecture.none && mainPackage.PackageType != PackageType.msixbundle)
                        {
                            xdw.WriteAttributeString("ProcessorArchitecture", mainPackage.ProcessorArchitecture.ToString());
                        }
                        xdw.WriteAttributeString("Name", mainPackage.Name);
                        mainPackageDCS.WriteEndObject(xdw);
                    } else if (App.MainPackage.PackageType == PackageType.msixbundle)
                    {
                        DataContractSerializer mainBundleDCS = new DataContractSerializer(typeof(MainBundle));
                        MainBundle mainBundle = new MainBundle(App.MainPackage.FilePath, App.MainPackage.Version, App.MainPackage.Publisher, App.MainPackage.Name); 
                        mainBundleDCS.WriteStartObject(xdw, mainBundle);
                        xdw.WriteAttributeString("Uri", mainBundle.FilePath);
                        xdw.WriteAttributeString("Version", mainBundle.Version);
                        xdw.WriteAttributeString("Publisher", mainBundle.Publisher);
                        xdw.WriteAttributeString("Name", mainBundle.Name);
                        mainBundleDCS.WriteEndObject(xdw);
                    }

                    //Optional Packages Content
                    ObservableCollection<OptionalPackage> optionalPackages = App.OptionalPackages; 
                    DataContractSerializer optionalPackageDCS = new DataContractSerializer(typeof(OptionalPackage));
                    if (optionalPackages.Count > 0 && App.IsOptionalPackages)
                    {
                        optionalPackageDCS.WriteStartObject(xdw, optionalPackages[0]);
                        for (int i = 0; i < optionalPackages.Count; i++)
                        {
                            //Write package or bundle element
                            if (optionalPackages[i].PackageType == PackageType.MSIX)
                            {
                                Package package = new Package(
                                    optionalPackages[i].FilePath,
                                    optionalPackages[i].Version,
                                    optionalPackages[i].Publisher,
                                    optionalPackages[i].Name,
                                    optionalPackages[i].PackageType,
                                    optionalPackages[i].ProcessorArchitecture            
                                );

                                DataContractSerializer packageDCS = new DataContractSerializer(typeof(Package));
                                packageDCS.WriteStartObject(xdw, package);
                                xdw.WriteAttributeString("Version", package.Version);
                                xdw.WriteAttributeString("Uri", package.FilePath);
                                xdw.WriteAttributeString("Publisher", package.Publisher);
                                if (package.ProcessorArchitecture != ProcessorArchitecture.none && package.PackageType != PackageType.msixbundle)
                                {
                                    xdw.WriteAttributeString("ProcessorArchitecture", package.ProcessorArchitecture.ToString());
                                }
                                xdw.WriteAttributeString("Name", package.Name);
                                packageDCS.WriteEndObject(xdw);
                            }
                            else if (optionalPackages[i].PackageType == PackageType.msixbundle)
                            {
                                Bundle bundle = new Bundle(
                                     optionalPackages[i].FilePath,
                                    optionalPackages[i].Version,
                                    optionalPackages[i].Publisher,
                                    optionalPackages[i].Name,
                                    optionalPackages[i].PackageType
                                );

                                DataContractSerializer bundleDCS = new DataContractSerializer(typeof(Bundle));
                                bundleDCS.WriteStartObject(xdw, bundle);
                                xdw.WriteAttributeString("Version", bundle.Version);
                                xdw.WriteAttributeString("Uri", bundle.FilePath);
                                xdw.WriteAttributeString("Publisher", bundle.Publisher);
                                xdw.WriteAttributeString("Name", bundle.Name);
                                bundleDCS.WriteEndObject(xdw);
                            }
                        }
                        optionalPackageDCS.WriteEndObject(xdw);
                    }


                    //Modification Packages Content
                    ObservableCollection<ModificationPackage> modificationPackages = App.ModificationPackages;
                    DataContractSerializer modificationPackageDCS = new DataContractSerializer(typeof(ModificationPackage));
                    if (modificationPackages.Count > 0 && App.IsModificationPackages)
                    {
                        modificationPackageDCS.WriteStartObject(xdw, modificationPackages[0]);
                        for (int i = 0; i < modificationPackages.Count; i++)
                        {
                            //Write package or bundle element
                            if (modificationPackages[i].PackageType == PackageType.MSIX)
                            {
                                Package package = new Package(
                                    modificationPackages[i].FilePath,
                                    modificationPackages[i].Version,
                                    modificationPackages[i].Publisher,
                                    modificationPackages[i].Name,
                                    modificationPackages[i].PackageType,
                                    modificationPackages[i].ProcessorArchitecture
                                );

                                DataContractSerializer packageDCS = new DataContractSerializer(typeof(Package));
                                packageDCS.WriteStartObject(xdw, package);
                                xdw.WriteAttributeString("Version", package.Version);
                                xdw.WriteAttributeString("Uri", package.FilePath);
                                xdw.WriteAttributeString("Publisher", package.Publisher);
                                if (package.ProcessorArchitecture != ProcessorArchitecture.none && package.PackageType != PackageType.msixbundle)
                                {
                                    xdw.WriteAttributeString("ProcessorArchitecture", package.ProcessorArchitecture.ToString());
                                }
                                xdw.WriteAttributeString("Name", package.Name);
                                packageDCS.WriteEndObject(xdw);
                            }
                            else if (modificationPackages[i].PackageType == PackageType.msixbundle)
                            {
                                Bundle bundle = new Bundle(
                                     modificationPackages[i].FilePath,
                                    modificationPackages[i].Version,
                                    modificationPackages[i].Publisher,
                                   modificationPackages[i].Name,
                                    modificationPackages[i].PackageType
                                );

                                DataContractSerializer bundleDCS = new DataContractSerializer(typeof(Bundle));
                                bundleDCS.WriteStartObject(xdw, bundle);
                                xdw.WriteAttributeString("Version", bundle.Version);
                                xdw.WriteAttributeString("Uri", bundle.FilePath);
                                xdw.WriteAttributeString("Publisher", bundle.Publisher);
                                xdw.WriteAttributeString("Name", bundle.Name);
                                bundleDCS.WriteEndObject(xdw);
                            }
                        }
                        modificationPackageDCS.WriteEndObject(xdw);
                    }

                    //Related Packages Content
                    ObservableCollection<RelatedPackage> relatedPackages = App.RelatedPackages;
                    DataContractSerializer relatedPackageDCS = new DataContractSerializer(typeof(RelatedPackage));
                    if (relatedPackages.Count > 0 && App.IsRelatedPackages)
                    {
                        relatedPackageDCS.WriteStartObject(xdw, relatedPackages[0]);
                        for (int i = 0; i < relatedPackages.Count; i++)
                        {
                            //Write package or bundle element
                            if (relatedPackages[i].PackageType == PackageType.MSIX)
                            {
                                Package package = new Package(
                                    relatedPackages[i].FilePath,
                                    relatedPackages[i].Version,
                                    relatedPackages[i].Publisher,
                                    relatedPackages[i].Name,
                                    relatedPackages[i].PackageType,
                                    relatedPackages[i].ProcessorArchitecture
                                );

                                DataContractSerializer packageDCS = new DataContractSerializer(typeof(Package));
                                packageDCS.WriteStartObject(xdw, package);
                                xdw.WriteAttributeString("Version", package.Version);
                                xdw.WriteAttributeString("Uri", package.FilePath);
                                xdw.WriteAttributeString("Publisher", package.Publisher);
                                if (package.ProcessorArchitecture != ProcessorArchitecture.none && package.PackageType != PackageType.msixbundle)
                                {
                                    xdw.WriteAttributeString("ProcessorArchitecture", package.ProcessorArchitecture.ToString());
                                }
                                xdw.WriteAttributeString("Name", package.Name);
                                packageDCS.WriteEndObject(xdw);
                            }
                            else if (relatedPackages[i].PackageType == PackageType.msixbundle)
                            {
                                Bundle bundle = new Bundle(
                                     relatedPackages[i].FilePath,
                                    relatedPackages[i].Version,
                                    relatedPackages[i].Publisher,
                                   relatedPackages[i].Name,
                                    relatedPackages[i].PackageType
                                );

                                DataContractSerializer bundleDCS = new DataContractSerializer(typeof(Bundle));
                                bundleDCS.WriteStartObject(xdw, bundle);
                                xdw.WriteAttributeString("Version", bundle.Version);
                                xdw.WriteAttributeString("Uri", bundle.FilePath);
                                xdw.WriteAttributeString("Publisher", bundle.Publisher);
                                xdw.WriteAttributeString("Name", bundle.Name);
                                bundleDCS.WriteEndObject(xdw);
                            }
                        }
                        relatedPackageDCS.WriteEndObject(xdw);
                    }


                    //Dependency Content

                    ObservableCollection<Dependency> dependencies = App.Dependencies;
                    DataContractSerializer dependencyDCS = new DataContractSerializer(typeof(Dependency));
                    if (dependencies.Count > 0 && App.IsDependencies)
                    {
                        dependencyDCS.WriteStartObject(xdw, dependencies[0]);
                        for (int i = 0; i < dependencies.Count; i++)
                        {
                            //Write package or bundle element
                            if (dependencies[i].PackageType == PackageType.MSIX)
                            {
                                Package package = new Package(
                                    dependencies[i].FilePath,
                                    dependencies[i].Version,
                                    dependencies[i].Publisher,
                                    dependencies[i].Name,
                                    dependencies[i].PackageType,
                                    dependencies[i].ProcessorArchitecture
                                );

                                DataContractSerializer packageDCS = new DataContractSerializer(typeof(Package));
                                packageDCS.WriteStartObject(xdw, package);
                                xdw.WriteAttributeString("Version", package.Version);
                                xdw.WriteAttributeString("Uri", package.FilePath);
                                xdw.WriteAttributeString("Publisher", package.Publisher);
                                if (package.ProcessorArchitecture != ProcessorArchitecture.none && package.PackageType != PackageType.msixbundle)
                                {
                                    xdw.WriteAttributeString("ProcessorArchitecture", package.ProcessorArchitecture.ToString());
                                }
                                xdw.WriteAttributeString("Name", package.Name);
                                packageDCS.WriteEndObject(xdw);
                            }
                            else if (dependencies[i].PackageType == PackageType.msixbundle)
                            {
                                Bundle bundle = new Bundle(
                                    dependencies[i].FilePath,
                                    dependencies[i].Version,
                                    dependencies[i].Publisher,
                                    dependencies[i].Name,
                                    dependencies[i].PackageType
                                );

                                DataContractSerializer bundleDCS = new DataContractSerializer(typeof(Bundle));
                                bundleDCS.WriteStartObject(xdw, bundle);
                                xdw.WriteAttributeString("Version", bundle.Version);
                                xdw.WriteAttributeString("Uri", bundle.FilePath);
                                xdw.WriteAttributeString("Publisher", bundle.Publisher);
                                xdw.WriteAttributeString("Name", bundle.Name);
                                bundleDCS.WriteEndObject(xdw);
                            }
                        }
                        dependencyDCS.WriteEndObject(xdw);
                    }
                    

                    //Update Settings
                    UpdateSettings updateSettings = new UpdateSettings();
                    
                    //OnLaunch
                    OnLaunch onLaunch = new OnLaunch(App.IsCheckUpdates, App.HoursBetweenUpdates);

                    if (onLaunch.IsCheckUpdates)
                    {
                        DataContractSerializer updateSettingsDCS = new DataContractSerializer(typeof(UpdateSettings));
                        updateSettingsDCS.WriteStartObject(xdw, updateSettings);

                        DataContractSerializer onLaunchDCS = new DataContractSerializer(typeof(OnLaunch));
                        onLaunchDCS.WriteStartObject(xdw, onLaunch);
                        xdw.WriteAttributeString("HoursBetweenUpdateChecks", onLaunch.HoursBetweenUpdateChecks.ToString());
                        onLaunchDCS.WriteEndObject(xdw);
                        updateSettingsDCS.WriteEndObject(xdw);
                    } 


                    appInstallerDCS.WriteEndObject(xdw);
                    xdw.Dispose();
                });
                t.Wait();
            }
            catch (Exception exc)
            {
                Debug.WriteLine("The serialization operation failed: {0} StackTrace: {1}",
                exc.Message, exc.StackTrace);
            }

            //Display dialog
            _displaySuccessDialog();

        }

        private bool _validateInput()
        {
            if (App.AppInstallerFilePath == "" || App.AppInstallerVersionNumber == "")
            {
                _displayMissingAppInstallerInformationDialog();
                return false;
            }

            if (App.MainPackage.FilePath == "" || App.MainPackage.Name == "" || App.MainPackage.Publisher == "" || App.MainPackage.Version == "")
            {
                _displayMissingMainPackageInformationDialog();
                return false;
            }

            if (App.IsOptionalPackages == true)
            {
                for (int i = 0; i < App.OptionalPackages.Count; i++)
                {
                    if (App.OptionalPackages[i].FilePath == "" || App.OptionalPackages[i].Name == "" || App.OptionalPackages[i].Publisher == "" || App.OptionalPackages[i].Version == "")
                    {
                        _displayMissingOptionalPackageInformationDialog();
                        return false;
                    }
                }
            }

            if (App.IsRelatedPackages == true)
            {
                for (int i = 0; i < App.RelatedPackages.Count; i++)
                {
                    if (App.RelatedPackages[i].FilePath == "" || App.RelatedPackages[i].Name == "" || App.RelatedPackages[i].Publisher == "" || App.RelatedPackages[i].Version == "")
                    {
                        _displayMissingRelatedPackageInformationDialog();
                        return false;
                    }
                }
            }

            if (App.IsModificationPackages == true)
            {
                for (int i = 0; i < App.ModificationPackages.Count; i++)
                {
                    if (App.ModificationPackages[i].FilePath == "" || App.ModificationPackages[i].Name == "" || App.ModificationPackages[i].Publisher == "" || App.ModificationPackages[i].Version == "")
                    {
                        _displayMissingModificationPackageInformationDialog();
                        return false;
                    }
                }
            }

            if (App.IsDependencies == true)
            {
                for (int i = 0; i < App.Dependencies.Count; i++)
                {
                    if (App.Dependencies[i].FilePath == "" || App.Dependencies[i].Name == "" || App.Dependencies[i].Publisher == "" || App.Dependencies[i].Version == "")
                    {
                        _displayMissingDependencyInformationDialog();
                        return false;
                    }
                }
            }

            return true;
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            AppShell.Current.AppFrame.Navigate(AppShell.Current.navlist2[2].DestPage);
        }

        private async void _displaySuccessDialog()
        {
            ContentDialog successDialog = new ContentDialog
            {
                Title = "File Created Successfully",
                Content ="The file was created in this location: " + ApplicationData.Current.LocalFolder.Path,
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await successDialog.ShowAsync();
        }

        private async void _displayMissingAppInstallerInformationDialog()
        {
            ContentDialog failDialog = new ContentDialog
            {
                Title = "Error: Incomplete AppInstaller Information",
                Content = "The file could not be created as a required field was left empty on the AppInstaller page. Please fill in all required fields on this page.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await failDialog.ShowAsync();
        }

        private async void _displayMissingMainPackageInformationDialog()
        {
            ContentDialog failDialog = new ContentDialog
            {
                Title = "Error: Incomplete Main Package Information",
                Content = "The file could not be created as a required field was left empty on the Main Package page. Please fill in all required fields on this page.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await failDialog.ShowAsync();
        }

        private async void _displayMissingOptionalPackageInformationDialog()
        {
            ContentDialog failDialog = new ContentDialog
            {
                Title = "Error: Incomplete Optional Package Information",
                Content = "The file could not be created as a required field was left empty on the Optional Package page. Please fill in all required fields on this page.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await failDialog.ShowAsync();
        }

        private async void _displayMissingDependencyInformationDialog()
        {
            ContentDialog failDialog = new ContentDialog
            {
                Title = "Error: Incomplete Dependency Information",
                Content = "The file could not be created as a required field was left empty on the Dependency page. Please fill in all required fields on this page.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await failDialog.ShowAsync();
        }

        private async void _displayMissingRelatedPackageInformationDialog()
        {
            ContentDialog failDialog = new ContentDialog
            {
                Title = "Error: Incomplete Related Package Information",
                Content = "The file could not be created as a required field was left empty on the Related Package page. Please fill in all required fields on this page.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await failDialog.ShowAsync();
        }

        private async void _displayMissingModificationPackageInformationDialog()
        {
            ContentDialog failDialog = new ContentDialog
            {
                Title = "Error: Incomplete Modification Package Information",
                Content = "The file could not be created as a required field was left empty on the Modification Package page. Please fill in all required fields on this page.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await failDialog.ShowAsync();
        }
    }
}
