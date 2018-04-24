using System;
using System.Collections.Generic;
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
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppInstallerFileGenerator.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPackageView : Page
	{
        private TextBox _filePathTextBox;
        private String _filePath;
        private ComboBox _packageTypeComboBox;
        private ComboBox _processorTypeComboBox;
        private PackageType _packageType;
        private String _version;
        private TextBox _versionTextBox;
        private String _publisher;
        private TextBox _publisherTextBox;
        private String _name;
        private TextBox _nameTextBox;
        private String _resourceId;
        private TextBox _resourceIdTextBox;
        private StackPanel _resourceIdStackPanel;
        private ProcessorArchitecture _processorArchitecture;

        private StackPanel _processorTypeStackPanel;

        /***************************************************************************
        * 
        * Constructor
        *
        ***************************************************************************/

        public MainPackageView()
		{
			this.InitializeComponent();

            _filePathTextBox = (TextBox)this.FindName("File_Path_Text_Box");
            _packageTypeComboBox = (ComboBox)this.FindName("Package_Type_Combo_Box");
            _versionTextBox = (TextBox)this.FindName("Version_Text_Box");
            _publisherTextBox = (TextBox)this.FindName("Publisher_Text_Box");
            _nameTextBox = (TextBox)this.FindName("Name_Text_Box");
            _resourceIdTextBox = (TextBox)this.FindName("Resource_Id_Text_Box");
            _processorTypeComboBox = (ComboBox)this.FindName("Processor_Type_Combo_Box");

            _processorTypeStackPanel = (StackPanel)this.FindName("Processor_Type_Stack_Panel");
            _resourceIdStackPanel = (StackPanel)this.FindName("Resource_Id_Stack_Panel");
        }

        /***************************************************************************
       * 
       * Lifecycle Methods
       *
       ***************************************************************************/

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _filePath = App.MainPackageFilePath;
            _packageType = App.MainPackageType;
            _version = App.MainPackageVersion;
            _publisher = App.MainPackagePublisher;
            _name = App.MainPackageName;
            _resourceId = App.MainPackageResourceId;
            _processorArchitecture = App.MainPackageProcessorArchitecture;

            _versionTextBox.Text = _version;
            _publisherTextBox.Text = _publisher;
            _nameTextBox.Text = _name;
            _resourceIdTextBox.Text = _resourceId; 
            _filePathTextBox.Text = _filePath;

            //Set Package Type Selection
            if (_packageType == PackageType.Appx)
            {
                _packageTypeComboBox.SelectedIndex = 0;
            } else if (_packageType == PackageType.Appxbundle)
            {
                _packageTypeComboBox.SelectedIndex = 1;
            }

            //Set Processor Architecture Selection
            if (_processorArchitecture == ProcessorArchitecture.none)
            {
                _processorTypeComboBox.SelectedIndex = 0;
            }
            else if (_processorArchitecture == ProcessorArchitecture.x64)
            {
                _processorTypeComboBox.SelectedIndex = 1;
            }
            else if (_processorArchitecture == ProcessorArchitecture.x86)
            {
                _processorTypeComboBox.SelectedIndex = 2;
            }
            else if (_processorArchitecture == ProcessorArchitecture.arm)
            {
                _processorTypeComboBox.SelectedIndex = 3;
            }
            else if (_processorArchitecture == ProcessorArchitecture.neutral)
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
            if (_packageType == PackageType.Appx)
            {
                _processorTypeStackPanel.Visibility = Visibility.Visible;
                _resourceIdStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                _processorTypeStackPanel.Visibility = Visibility.Collapsed;
                _resourceIdStackPanel.Visibility = Visibility.Collapsed;
                _processorArchitecture = ProcessorArchitecture.none; 
            }
        }


        private void _save()
        {
            App.MainPackageFilePath = _filePath;
            App.MainPackageType = _packageType;
            App.MainPackageVersion = _version;
            App.MainPackagePublisher = _publisher;
            App.MainPackageName = _name;
            App.MainPackageResourceId = _resourceId;
            App.MainPackageProcessorArchitecture = _processorArchitecture;
        }

        private void File_Path_Text_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            _filePath = _filePathTextBox.Text;
            _save();
        }

        private void Package_Type_Combo_Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int value = _packageTypeComboBox.SelectedIndex;

            if (value == 0)
            { 
                _packageType = PackageType.Appx;
            }
            else if (value == 1)
            {
                _packageType = PackageType.Appxbundle;
            }
            _reloadViews();
            _save();
        }

        private void Version_Text_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            _version = _versionTextBox.Text;
            _save();
        }

        private void Publisher_Text_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            _publisher = _publisherTextBox.Text;
            _save();
        }

        private void Name_Text_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            _name = _nameTextBox.Text;
            _save();
        }

        private void Resource_Id_Text_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            _resourceId = _resourceIdTextBox.Text;
            _save();
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            AppShell.Current.AppFrame.Navigate(AppShell.Current.navlist[2].DestPage);
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            AppShell.Current.AppFrame.Navigate(AppShell.Current.navlist[0].DestPage);
        }
        
        private void Processor_Type_Combo_Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int value = _processorTypeComboBox.SelectedIndex;

            if (value == 0)
            {
                _processorArchitecture = ProcessorArchitecture.none;
            }
            else if (value == 1)
            {
                _processorArchitecture = ProcessorArchitecture.x64;
            }
            else if (value == 2)
            {
                _processorArchitecture = ProcessorArchitecture.x86;
            }
            else if (value == 3)
            {
                _processorArchitecture = ProcessorArchitecture.arm;
            }
            else if (value == 4)
            {
                _processorArchitecture = ProcessorArchitecture.neutral;
            }
            
            _save();
        }

    }
}
