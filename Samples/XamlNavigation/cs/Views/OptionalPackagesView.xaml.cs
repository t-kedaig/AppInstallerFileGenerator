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
        private RelativePanel _packageListView;
        private ListView _listView;
        private TextBlock _addNewPackageTextBlock;
        private TextBlock _removePackageTextBlock;

        ObservableCollection<OptionalPackage> selectedItems = new ObservableCollection<OptionalPackage>();

        public ObservableCollection<OptionalPackage> OptionalPackages { get; private set; } = new ObservableCollection<OptionalPackage>();

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
            _packageListView = (RelativePanel)this.FindName("Package_Relative_Panel");
            _listView = (ListView)this.FindName("List_View");
            _addNewPackageTextBlock = (TextBlock)this.FindName("Add_New_Package_Text_Block");
            _removePackageTextBlock = (TextBlock)this.FindName("Remove_Package_Text_Block");
        }

       /***************************************************************************
       * 
       * Lifecycle Methods
       *
       ***************************************************************************/

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            OptionalPackages = App.OptionalPackages;

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

            if (OptionalPackages.Count > 0)
            {
                _removePackageTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                _removePackageTextBlock.Visibility = Visibility.Collapsed;
            }

        }

        private void _save()
        {
            App.OptionalPackages = OptionalPackages;
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

        private void Add_New_Package_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OptionalPackages.Add(new OptionalPackage());
            _reloadViews();
            _save();
        }

        private void Remove_Package_Text_Block_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OptionalPackages.RemoveAt(OptionalPackages.Count - 1);
            _reloadViews();
            _save();
        }
    }
}


