using AppInstallerFileGenerator.Model;
using System;
using System.Collections.Generic;
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
	public sealed partial class OptionalPackagesView : Page, INotifyPropertyChanged
    {

        private TextBox _filePathTextBox;
        private ComboBox _packageTypeComboBox;
        private TextBox _versionTextBox;
        private TextBox _publisherTextBox;
        private TextBox _nameTextBox;
        private ToggleSwitch _optionalPackagesSwitch;
        private ComboBox _processorTypeComboBox;

        private StackPanel _packageInfoStackPanel;
        private StackPanel _processorTypeStackPanel;

        private String[] _filePaths;
        private PackageType[] _packageTypes;
        private String[] _versions;
        private String[] _publishers;
        private String[] _names;
        private ProcessorArchitecture[] _processorArchitectures;

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
            

            _filePathTextBox = (TextBox)this.FindName("File_Path_Text_Box");
            _packageTypeComboBox = (ComboBox)this.FindName("Package_Type_Combo_Box");
            _versionTextBox = (TextBox)this.FindName("Version_Text_Box");
            _publisherTextBox = (TextBox)this.FindName("Publisher_Text_Box");
            _nameTextBox = (TextBox)this.FindName("Name_Text_Box");
            _processorTypeComboBox = (ComboBox)this.FindName("Processor_Type_Combo_Box");

            _processorTypeStackPanel = (StackPanel)this.FindName("Processor_Type_Stack_Panel");
            _packageInfoStackPanel = (StackPanel)this.FindName("Package_Info_Stack_Panel");
        }

        /***************************************************************************
       * 
       * Lifecycle Methods
       *
       ***************************************************************************/

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _filePaths = App.OptionalPackageFilePaths;
            _packageTypes = App.OptionalPackageTypes;
            _versions = App.OptionalPackageVersions;
            _publishers = App.OptionalPackagePublishers;
            _names = App.OptionalPackageNames;
            _processorArchitectures = App.OptionalPackageProcessorArchitectures;


            for (int i = 0; i < _filePaths.Length; i++)
            {
                _filePathTextBox.Text = _filePaths[i];
                _versionTextBox.Text = _versions[i];
                _publisherTextBox.Text = _publishers[i];
                _nameTextBox.Text = _names[i];
            }

            //Set Package Type Selection

            if (_packageTypes.Length != 0) {
                if (_packageTypes[0] == PackageType.Appx)
                {
                    _packageTypeComboBox.SelectedIndex = 0;
                }
                else if (_packageTypes[0] == PackageType.Appxbundle)
                {
                    _packageTypeComboBox.SelectedIndex = 1;
                }
            }

            //Set Processor Architecture Selection
            if (_processorArchitectures[0] == ProcessorArchitecture.none)
            {
                _processorTypeComboBox.SelectedIndex = 0;
            }
            else if (_processorArchitectures[0] == ProcessorArchitecture.x64)
            {
                _processorTypeComboBox.SelectedIndex = 1;
            }
            else if (_processorArchitectures[0] == ProcessorArchitecture.x86)
            {
                _processorTypeComboBox.SelectedIndex = 2;
            }
            else if (_processorArchitectures[0] == ProcessorArchitecture.arm)
            {
                _processorTypeComboBox.SelectedIndex = 3;
            }
            else if (_processorArchitectures[0] == ProcessorArchitecture.neutral)
            {
                _processorTypeComboBox.SelectedIndex = 4;
            }
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
                _packageInfoStackPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                _packageInfoStackPanel.Visibility = Visibility.Visible;
            }

            for (int i = 0; i < _packageTypes.Length; i++)
            {
                if (_packageTypes[i] == PackageType.Appx)
                {
                    _processorTypeStackPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    _processorTypeStackPanel.Visibility = Visibility.Collapsed;
                    _processorArchitectures[0] = ProcessorArchitecture.none; 
                }
            }
        }

        private void _save()
        {
            for (int i = 0; i < _packageTypes.Length; i++)
            { 
                App.OptionalPackageFilePaths[0] = _filePaths[i];
                App.OptionalPackageTypes[0] = _packageTypes[i];
                App.OptionalPackageVersions[0] = _versions[i];
                App.OptionalPackagePublishers[0] = _publishers[i];
                App.OptionalPackageNames[0] = _names[i];
                App.OptionalPackageProcessorArchitectures[0] = _processorArchitectures[i];
                App.IsOptionalPackages = _isOptionalPackages;
            }
        }

        private void File_Path_Text_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            _filePaths[0] = _filePathTextBox.Text;
            _save();
        }

        private void Package_Type_Combo_Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int value = _packageTypeComboBox.SelectedIndex;

            if (value == 0)
            {
                _packageTypes[0] = PackageType.Appx;
            }
            else if (value == 1)
            {
                _packageTypes[0] = PackageType.Appxbundle;
            }
            _reloadViews();
            _save();
        }

        private void Processor_Type_Combo_Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int value = _processorTypeComboBox.SelectedIndex;

            if (value == 0)
            {
                _processorArchitectures[0] = ProcessorArchitecture.none;
            }
            else if (value == 1)
            {
                _processorArchitectures[0] = ProcessorArchitecture.x64;
            }
            else if (value == 2)
            {
                _processorArchitectures[0] = ProcessorArchitecture.x86;
            }
            else if (value == 3)
            {
                _processorArchitectures[0] = ProcessorArchitecture.arm;
            }
            else if (value == 4)
            {
                _processorArchitectures[0] = ProcessorArchitecture.neutral;
            }

            _save();
        }

        private void Version_Text_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            _versions[0] = _versionTextBox.Text;
            _save();
        }

        private void Publisher_Text_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            _publishers[0] = _publisherTextBox.Text;
            _save();
        }

        private void Name_Text_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            _names[0] = _nameTextBox.Text;
            _save();
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
