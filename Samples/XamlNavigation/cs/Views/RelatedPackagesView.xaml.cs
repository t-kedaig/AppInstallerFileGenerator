using AppInstallerFileGenerator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
	public sealed partial class RelatedPackagesView : Page, INotifyPropertyChanged
	{
        private ToggleSwitch _relatedPackagesSwitch;
        private RelativePanel _packageListView;
        private ListView _listView;
        private TextBlock _addNewPackageTextBlock;

        ObservableCollection<RelatedPackage> selectedItems = new ObservableCollection<RelatedPackage>();

        private ObservableCollection<RelatedPackage> _relatedPackages = new ObservableCollection<RelatedPackage>();
        public ObservableCollection<RelatedPackage> RelatedPackages
        {
            get
            {
                return this._relatedPackages;
            }

            set
            {
                if (value != this._relatedPackages)
                {
                    this._relatedPackages = value;
                    NotifyPropertyChanged("RelatedPackages");
                }
            }
        }

        private bool _isRelatedPackages;
        public bool IsRelatedPackages
        {
            get
            {
                return this._isRelatedPackages;
            }

            set
            {
                if (value != this._isRelatedPackages)
                {
                    this._isRelatedPackages = value;
                    NotifyPropertyChanged("IsRelatedPackages");

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
        public RelatedPackagesView()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.NavigationCacheMode = NavigationCacheMode.Required;
            _relatedPackagesSwitch = (ToggleSwitch)this.FindName("Related_Packages_Switch");
            _packageListView = (RelativePanel)this.FindName("Package_Relative_Panel");
            _listView = (ListView)this.FindName("List_View");
            _addNewPackageTextBlock = (TextBlock)this.FindName("Add_New_Package_Text_Block");
        }

        /***************************************************************************
        * 
        * Lifecycle Methods
        *
        ***************************************************************************/

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _relatedPackages = App.RelatedPackages;

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
            if (!_isRelatedPackages)
            {
                _packageListView.Visibility = Visibility.Collapsed;
            }
            else
            {
                _packageListView.Visibility = Visibility.Visible;
            }
        }

        private void _save()
        {
            //Problem is getting null reference exception - trying to access optionalpackages when it is null?
            App.RelatedPackages = _relatedPackages;
            App.IsRelatedPackages = _isRelatedPackages;
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            AppShell.Current.AppFrame.Navigate(AppShell.Current.navlist2[2].DestPage);
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            AppShell.Current.AppFrame.Navigate(AppShell.Current.navlist2[0].DestPage);
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            _reloadViews();
            _save();
        }

        private void Add_New_Package_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _relatedPackages.Add(new RelatedPackage());
            _save();
        }
        //TODO: KEITH - REMOVING THEM DOESNT WORK YET. NEED TO IMPLEMENT CHECK OR FIND OUT A DIFFERENT METHOD. "UNCHECKING" ISNT WORKING AND MAY HAVE TO DO WITH NOT BINDING CORRECTLY --> THIS IS CAUSING NULL EXCEPTION CRASH

        //private void Chck_Checked(object sender, RoutedEventArgs e)
        //{
        //    CheckBox chk = (CheckBox)sender;
        //    RelatedPackage newVal = (RelatedPackage)chk.Tag;
        //    if (chk.IsChecked.HasValue && chk.IsChecked.Value)
        //    {
        //        selectedItems.Add(newVal);
        //    }
        //    else
        //    {
        //        selectedItems.Remove(newVal);
        //    }
        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    foreach (var item in selectedItems)
        //    {
        //        _relatedPackages.Remove(item);
        //    }
        //    selectedItems.Clear();
        //    _save();
        //}
    }
}


//Giving nullreferenceexception..._optionalpackages becomes null after remiving the last element...need to add a (if _optionalPalcages != nulll) somewehre