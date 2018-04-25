using AppInstallerFileGenerator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppInstallerFileGenerator.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class OptionalPackagesView : Page, INotifyPropertyChanged
    {

        private ToggleSwitch _optionalPackagesSwitch;
        private ListView _packageListView;
        private StackPanel _processorTypeStackPanel;
        
        private ObservableCollection<OptionalPackage> _optionalPackages = new ObservableCollection<OptionalPackage>();
        public ObservableCollection<OptionalPackage> OptionalPackages { get { return this._optionalPackages; } }

        private bool _isOptionalPackages;
        public bool IsOptionalPackages
        {
            get
            {
                return this._isOptionalPackages;
            }

            set
            {
                if (value != this._isOptionalPackages)
                {
                    this._isOptionalPackages = value;
                    NotifyPropertyChanged("IsOptionalPackages");
                }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        /***************************************************************************
        * 
        * Constructor
        *
        ***************************************************************************/
        public OptionalPackagesView()
		{
            this.InitializeComponent();
            this.DataContext = this;
            this.NavigationCacheMode = NavigationCacheMode.Required;
            _optionalPackagesSwitch = (ToggleSwitch)this.FindName("Optional_Packages_Switch");
            _packageListView = (ListView)this.FindName("Package_List_View");
            _processorTypeStackPanel = (StackPanel)this.FindName("Processor_Type_Stack_Panel");
            
        }

        /***************************************************************************
       * 
       * Lifecycle Methods
       *
       ***************************************************************************/

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _optionalPackages = App.OptionalPackages;

            Debug.WriteLine("Package 1");
            Debug.WriteLine("File Path: " + _optionalPackages[0].FilePath);
            Debug.WriteLine("Version: " + _optionalPackages[0].Version);
            Debug.WriteLine("Publisher: " + _optionalPackages[0].Publisher);
            Debug.WriteLine("Package Type: " + _optionalPackages[0].PackageType.ToString());
            Debug.WriteLine("Name: " + _optionalPackages[0].Name);

            Debug.WriteLine("Package 2");
            Debug.WriteLine("File Path: " + _optionalPackages[1].FilePath);
            Debug.WriteLine("Version: " + _optionalPackages[1].Version);
            Debug.WriteLine("Publisher: " + _optionalPackages[1].Publisher);
            Debug.WriteLine("Package Type: " + _optionalPackages[1].PackageType.ToString());
            Debug.WriteLine("Name: " + _optionalPackages[1].Name);

            _reloadViews();
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _save();
            base.OnNavigatedFrom(e);
        }


        /***************************************************************************
        * 
        * Private Methods
        *
        ***************************************************************************/

        private void _reloadViews()
        {
            if (!_isOptionalPackages)
            {
                _packageListView.Visibility = Visibility.Collapsed;
            }
            else
            {
                _packageListView.Visibility = Visibility.Visible;
            }

            //TODO: Keith - Handle this type of visibility binding later.
            for (int i = 0; i < _optionalPackages.Count; i++)
            {
                //if (_packageTypes[i] == PackageType.Appx)
                //{
                //    _processorTypeStackPanel.Visibility = Visibility.Visible;
                //}
                //else
                //{
                //    _processorTypeStackPanel.Visibility = Visibility.Collapsed;
                //    _processorArchitectures[0] = ProcessorArchitecture.none; 
                //}
            }
        }

        private void _save()
        {
            App.OptionalPackages = _optionalPackages;
            App.IsOptionalPackages = _isOptionalPackages;
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            AppShell.Current.AppFrame.Navigate(AppShell.Current.navlist2[1].DestPage);
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            AppShell.Current.AppFrame.Navigate(AppShell.Current.navlist[2].DestPage);
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            _reloadViews();
            _save();
        }
    }
}
