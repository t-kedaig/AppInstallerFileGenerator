using AppInstallerFileGenerator;
using AppInstallerFileGenerator.Model;
using System;
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
                    FileStream writer = new FileStream(ApplicationData.Current.LocalFolder.Path + "//GenerationExample.xml", FileMode.Create); //TODO: Create file in proper location

                    AppInstaller appInstaller = new AppInstaller(App.AppInstallerFilePath, App.AppInstallerVersionNumber);

                    //Initialize DCS of type AppInstallerModel
                    XmlDictionaryWriter xdw = XmlDictionaryWriter.CreateTextWriter(writer, Encoding.UTF8);

                    DataContractSerializer appInstallerDCS = new DataContractSerializer(typeof(AppInstaller));

                    //AppInstaller Content
                    appInstallerDCS.WriteStartObject(xdw, appInstaller);
                    xdw.WriteAttributeString("xmlns", "http://schemas.microsoft.com/appx/appinstaller/2017"); //TODO: Keith - Is this current schema reference?
                    xdw.WriteAttributeString("Uri", App.AppInstallerFilePath);
                    xdw.WriteAttributeString("Version", App.AppInstallerVersionNumber);

                    //Main Package Content
                    if (App.MainPackageType == PackageType.Appx)
                    {
                        DataContractSerializer mainPackageDCS = new DataContractSerializer(typeof(MainPackage));
                        MainPackage mainPackage = new MainPackage(App.MainPackageFilePath, App.MainPackageVersion, App.MainPackagePublisher, App.MainPackageName, App.MainPackageType, App.MainPackageProcessorArchitecture, App.MainPackageResourceId); 
                        mainPackageDCS.WriteStartObject(xdw, mainPackage);
                        xdw.WriteAttributeString("Uri", mainPackage.FilePath);
                        xdw.WriteAttributeString("Version", mainPackage.Version);
                        xdw.WriteAttributeString("Publisher", mainPackage.Publisher);
                        xdw.WriteAttributeString("ResourceId", mainPackage.ResourceId);
                        if (mainPackage.ProcessorArchitecture != ProcessorArchitecture.none)
                        {
                            xdw.WriteAttributeString("ProcessorArchitecture", mainPackage.ProcessorArchitecture.ToString());
                        }
                        xdw.WriteAttributeString("Name", mainPackage.Name);
                        mainPackageDCS.WriteEndObject(xdw);
                    } else if (App.MainPackageType == PackageType.Appxbundle)
                    {
                        DataContractSerializer mainBundleDCS = new DataContractSerializer(typeof(MainBundle));
                        MainBundle mainBundle = new MainBundle(App.MainPackageFilePath, App.MainPackageVersion, App.MainPackagePublisher, App.MainPackageName); 
                        mainBundleDCS.WriteStartObject(xdw, mainBundle);
                        xdw.WriteAttributeString("Uri", mainBundle.FilePath);
                        xdw.WriteAttributeString("Version", mainBundle.Version);
                        xdw.WriteAttributeString("Publisher", mainBundle.Publisher);
                        xdw.WriteAttributeString("Name", mainBundle.Name);
                        mainBundleDCS.WriteEndObject(xdw);
                    }

                    //Optional Packages Content
                    OptionalPackage[] optionalPackages = new OptionalPackage[App.OptionalPackageFilePaths.Length]; 
                    DataContractSerializer optionalPackageDCS = new DataContractSerializer(typeof(OptionalPackage));
                    if (optionalPackages.Length > 0 && App.IsOptionalPackages)
                    {
                        optionalPackageDCS.WriteStartObject(xdw, optionalPackages[0]);
                        for (int i = 0; i < optionalPackages.Length; i++)
                        {
                            //Write package or bundle element
                            if (App.OptionalPackageTypes[i] == PackageType.Appx)
                            {
                                Package package = new Package(
                                    App.OptionalPackageFilePaths[i],
                                    App.OptionalPackageVersions[i],
                                    App.OptionalPackagePublishers[i],
                                    App.OptionalPackageNames[i],
                                    App.OptionalPackageTypes[i],
                                    App.OptionalPackageProcessorArchitectures[i]
                                );

                                DataContractSerializer packageDCS = new DataContractSerializer(typeof(Package));
                                packageDCS.WriteStartObject(xdw, package);
                                xdw.WriteAttributeString("Version", package.Version);
                                xdw.WriteAttributeString("Uri", package.FilePath);
                                xdw.WriteAttributeString("Publisher", package.Publisher);
                                if (package.ProcessorArchitecture != ProcessorArchitecture.none)
                                {
                                    xdw.WriteAttributeString("ProcessorArchitecture", package.ProcessorArchitecture.ToString());
                                }
                                xdw.WriteAttributeString("Name", package.Name);
                                packageDCS.WriteEndObject(xdw);
                            }
                            else if (App.OptionalPackageTypes[i] == PackageType.Appxbundle)
                            {
                                Bundle bundle = new Bundle(
                                    App.OptionalPackageFilePaths[i],
                                    App.OptionalPackageVersions[i],
                                    App.OptionalPackagePublishers[i],
                                    App.OptionalPackageNames[i],
                                    App.OptionalPackageTypes[i]
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

                    //Related Packages Content

                    RelatedPackage[] relatedPackages = new RelatedPackage[App.RelatedPackageFilePaths.Length];
                    DataContractSerializer relatedPackageDCS = new DataContractSerializer(typeof(RelatedPackage));
                    if (relatedPackages.Length > 0 && App.IsRelatedPackages)
                    {
                        relatedPackageDCS.WriteStartObject(xdw, relatedPackages[0]);
                        for (int i = 0; i < relatedPackages.Length; i++)
                        {
                            //Write package or bundle element
                            if (App.RelatedPackageTypes[i] == PackageType.Appx)
                            {
                                Package package = new Package(
                                    App.RelatedPackageFilePaths[i],
                                    App.RelatedPackageVersions[i],
                                    App.RelatedPackagePublishers[i],
                                    App.RelatedPackageNames[i],
                                    App.RelatedPackageTypes[i],
                                    App.RelatedPackageProcessorArchitectures[i]
                                );

                                DataContractSerializer packageDCS = new DataContractSerializer(typeof(Package));
                                packageDCS.WriteStartObject(xdw, package);
                                xdw.WriteAttributeString("Version", package.Version);
                                xdw.WriteAttributeString("Uri", package.FilePath);
                                xdw.WriteAttributeString("Publisher", package.Publisher);
                                if (package.ProcessorArchitecture != ProcessorArchitecture.none)
                                {
                                    xdw.WriteAttributeString("ProcessorArchitecture", package.ProcessorArchitecture.ToString());
                                }
                                xdw.WriteAttributeString("Name", package.Name);
                                packageDCS.WriteEndObject(xdw);
                            }
                            else if (App.RelatedPackageTypes[i] == PackageType.Appxbundle)
                            {
                                Bundle bundle = new Bundle(
                                    App.RelatedPackageFilePaths[i],
                                    App.RelatedPackageVersions[i],
                                    App.RelatedPackagePublishers[i],
                                    App.RelatedPackageNames[i],
                                    App.RelatedPackageTypes[i]
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

                    Dependency[] dependencies = new Dependency[App.DependencyFilePaths.Length];
                    DataContractSerializer dependencyDCS = new DataContractSerializer(typeof(Dependency));
                    if (dependencies.Length > 0 && App.IsDependencies)
                    {
                        dependencyDCS.WriteStartObject(xdw, dependencies[0]);
                        for (int i = 0; i < dependencies.Length; i++)
                        {
                            //Write package or bundle element
                            if (App.DependencyPackageTypes[i] == PackageType.Appx)
                            {
                                Package package = new Package(
                                    App.DependencyFilePaths[i],
                                    App.DependencyVersions[i],
                                    App.DependencyPublishers[i],
                                    App.DependencyNames[i],
                                    App.DependencyPackageTypes[i],
                                    App.DependencyProcessorArchitectures[i]
                                );

                                DataContractSerializer packageDCS = new DataContractSerializer(typeof(Package));
                                packageDCS.WriteStartObject(xdw, package);
                                xdw.WriteAttributeString("Version", package.Version);
                                xdw.WriteAttributeString("Uri", package.FilePath);
                                xdw.WriteAttributeString("Publisher", package.Publisher);
                                if (package.ProcessorArchitecture != ProcessorArchitecture.none)
                                {
                                    xdw.WriteAttributeString("ProcessorArchitecture", package.ProcessorArchitecture.ToString());
                                }
                                xdw.WriteAttributeString("Name", package.Name);
                                packageDCS.WriteEndObject(xdw);
                            }
                            else if (App.DependencyPackageTypes[i] == PackageType.Appxbundle)
                            {
                                Bundle bundle = new Bundle(
                                    App.DependencyFilePaths[i],
                                    App.DependencyVersions[i],
                                    App.DependencyPublishers[i],
                                    App.DependencyNames[i],
                                    App.DependencyPackageTypes[i]
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

            if (App.MainPackageFilePath == "" || App.MainPackageName == "" || App.MainPackagePublisher == "" || App.MainPackageVersion == "")
            {
                _displayMissingMainPackageInformationDialog();
                return false;
            }

            if (App.IsOptionalPackages == true)
            {
                for (int i = 0; i < App.OptionalPackageFilePaths.Length; i++)
                {
                    if (App.OptionalPackageFilePaths[i] == "" || App.OptionalPackageNames[i] == "" || App.OptionalPackagePublishers[i] == "" || App.OptionalPackageVersions[i] == "")
                    {
                        _displayMissingOptionalPackageInformationDialog();
                        return false;
                    }
                }
            }

            if (App.IsRelatedPackages == true)
            {
                for (int i = 0; i < App.RelatedPackageFilePaths.Length; i++)
                {
                    if (App.RelatedPackageFilePaths[i] == "" || App.RelatedPackageNames[i] == "" || App.RelatedPackagePublishers[i] == "" || App.RelatedPackageVersions[i] == "")
                    {
                        _displayMissingRelatedPackageInformationDialog();
                        return false;
                    }
                }
            }

            if (App.IsDependencies == true)
            {
                for (int i = 0; i < App.DependencyFilePaths.Length; i++)
                {
                    if (App.DependencyFilePaths[i] == "" || App.DependencyNames[i] == "" || App.DependencyPublishers[i] == "" || App.DependencyVersions[i] == "")
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
    }
}
