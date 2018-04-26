using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AppInstallerFileGenerator.Views;
using AppInstallerFileGenerator.Model;
using Windows.ApplicationModel.Core;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=402347&clcid=0x409

namespace AppInstallerFileGenerator
{
    using System.Collections.ObjectModel;
    using Views;
    using Windows.UI;
    using Windows.UI.ViewManagement;

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        /// 

        internal static string AppInstallerFilePath = "";
        internal static string AppInstallerVersionNumber = "";

        internal static PackageType MainPackageType = PackageType.Appx;
        internal static String MainPackageFilePath = "";
        internal static String MainPackageVersion = "";
        internal static String MainPackagePublisher = "";
        internal static String MainPackageName = "";
        internal static String MainPackageResourceId = "";
        internal static ProcessorArchitecture MainPackageProcessorArchitecture = ProcessorArchitecture.none;

        internal static bool IsOptionalPackages = false;
        internal static OptionalPackage defaultOptionalPackage = new OptionalPackage();
        internal static OptionalPackage testOptionalPackage = new OptionalPackage("tetwetw", "wtt", "wet", "tew", PackageType.Appx, ProcessorArchitecture.none);

        internal static ObservableCollection<OptionalPackage> OptionalPackages = new ObservableCollection<OptionalPackage>();
        

        //TODO: Keith - remove sample packages.
        
        internal static bool IsRelatedPackages = false;
        internal static PackageType[] RelatedPackageTypes = new PackageType[] {PackageType.Appx};
        internal static String[] RelatedPackageFilePaths = new String[] {"testt"};
        internal static String[] RelatedPackageVersions = new String[] { "testt" };
        internal static String[] RelatedPackagePublishers = new String[] { "testt" };
        internal static String[] RelatedPackageNames = new String[] { "testt" };
        internal static ProcessorArchitecture[] RelatedPackageProcessorArchitectures = new ProcessorArchitecture[] { ProcessorArchitecture.none};

        internal static bool IsDependencies = false;
        internal static PackageType[] DependencyPackageTypes = new PackageType[] {PackageType.Appx};
        internal static String[] DependencyFilePaths = new String[] { "testt" };
        internal static String[] DependencyVersions = new String[] { "testt" };
        internal static String[] DependencyPublishers = new String[] { "testt" };
        internal static String[] DependencyNames = new String[] { "testt" };
        internal static ProcessorArchitecture[] DependencyProcessorArchitectures = new ProcessorArchitecture[] { ProcessorArchitecture.none };

        internal static bool IsCheckUpdates = false;
        internal static int HoursBetweenUpdates = 0;
    

        public App()
        {
            this.InitializeComponent();

            //TODO: Don't Initialize this here. Just do from add button.
            OptionalPackages.Add(defaultOptionalPackage);
          
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

            // Change minimum window size
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(320, 200));

            // Darken the window title bar using a color value to match app theme
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            if (titleBar != null)
            {
                Color titleBarColor = (Color)App.Current.Resources["SystemChromeMediumColor"];
                titleBar.BackgroundColor = titleBarColor;
                titleBar.ButtonBackgroundColor = titleBarColor;
            }

            if (SystemInformationHelpers.IsTenFootExperience)
            {
                // Apply guidance from https://msdn.microsoft.com/windows/uwp/input-and-devices/designing-for-tv
                ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);

                this.Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("ms-appx:///Styles/TenFootStylesheet.xaml")
                });
            }

            AppShell shell = Window.Current.Content as AppShell;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (shell == null)
            {
                // Create a AppShell to act as the navigation context and navigate to the first page
                shell = new AppShell();

                // Set the default language
                shell.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                shell.AppFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
            }

            // Place our app shell in the current Window
            Window.Current.Content = shell;

            if (shell.AppFrame.Content == null)
            {
                // When the navigation stack isn't restored, navigate to the first page
                // suppressing the initial entrance animation.
                shell.AppFrame.Navigate(typeof(AppInstallerView), e.Arguments, new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo());
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

      

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
