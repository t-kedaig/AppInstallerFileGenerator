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
                    xdw.WriteAttributeString("Uri", App.AppInstallerFilePath);
                    xdw.WriteAttributeString("Version", App.AppInstallerVersionNumber);

                    //Main Package Content
                    if (App.MainPackageType == PackageType.Appx)
                    {
                        DataContractSerializer mainPackageDCS = new DataContractSerializer(typeof(MainPackage));
                        MainPackage mainPackage = new MainPackage(App.MainPackageFilePath, App.MainPackageVersion, App.MainPackagePublisher, App.MainPackageName, App.MainPackageType, App.MainPackageProcessorArchitecture, App.MainPackageResourceId); //TODO: Placeholder
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
                        MainBundle mainBundle = new MainBundle(App.MainPackageFilePath, App.MainPackageVersion, App.MainPackagePublisher, App.MainPackageName); //TODO: Placeholder
                        mainBundleDCS.WriteStartObject(xdw, mainBundle);
                        xdw.WriteAttributeString("Uri", mainBundle.FilePath);
                        xdw.WriteAttributeString("Version", mainBundle.Version);
                        xdw.WriteAttributeString("Publisher", mainBundle.Publisher);
                        xdw.WriteAttributeString("Name", mainBundle.Name);
                        mainBundleDCS.WriteEndObject(xdw);
                    }

                    //Optional Packages Content
                    OptionalPackage[] optionalPackages = new OptionalPackage[App.OptionalPackageFilePaths.Length]; //TODO: Once App.cs has list of optional packages, just get that here.
                    DataContractSerializer optionalPackageDCS = new DataContractSerializer(typeof(OptionalPackage));
                    if (optionalPackages.Length > 0)
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
                    if (relatedPackages.Length > 0)
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
                    if (dependencies.Length > 0)
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

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            AppShell.Current.AppFrame.Navigate(AppShell.Current.navlist[5].DestPage);
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
    }
}
