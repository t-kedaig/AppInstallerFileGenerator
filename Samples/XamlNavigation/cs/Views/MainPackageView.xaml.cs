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
using AppInstallerFileGenerator.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppInstallerFileGenerator.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPackageView : Page
	{
        private TextBox _filePathTextBox;
        private ComboBox _packageTypeComboBox;
        private ComboBox _processorTypeComboBox;
        private TextBox _versionTextBox;
        private TextBox _publisherTextBox;
        private TextBox _nameTextBox;
        private TextBox _resourceIdTextBox;
        private StackPanel _resourceIdStackPanel;
        
        private MainPackage _mainPackage;

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
            _mainPackage = App.MainPackage;

            _versionTextBox.Text = _mainPackage.Version;
            _publisherTextBox.Text = _mainPackage.Publisher;
            _nameTextBox.Text = _mainPackage.Name;
            _resourceIdTextBox.Text = _mainPackage.ResourceId; 
            _filePathTextBox.Text = _mainPackage.FilePath;
            _processorTypeComboBox.SelectedValue = _mainPackage.ProcessorArchitecture;
            _packageTypeComboBox.SelectedValue = _mainPackage.PackageType;

            Debug.WriteLine("_mainPackage" + _mainPackage.ProcessorArchitecture.ToString());
            Debug.WriteLine("_mainPackage" + _mainPackage.PackageType.ToString());

            Debug.WriteLine("App" + App.MainPackage.ProcessorArchitecture.ToString());
            Debug.WriteLine("App" + App.MainPackage.PackageType.ToString());

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
            if (_mainPackage.PackageType == PackageType.MSIX)
            {
                _processorTypeStackPanel.Visibility = Visibility.Visible;
                _resourceIdStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                _processorTypeStackPanel.Visibility = Visibility.Collapsed;
                _resourceIdStackPanel.Visibility = Visibility.Collapsed;
                _mainPackage.ProcessorArchitecture = ProcessorArchitecture.none; 
            }
        }

        private void _save()
        {
            App.MainPackage = _mainPackage;

            Debug.WriteLine("_mainPackage: " + _mainPackage.ProcessorArchitecture.ToString());
            Debug.WriteLine("_mainPackage" + _mainPackage.PackageType.ToString());

            Debug.WriteLine("App" + App.MainPackage.ProcessorArchitecture.ToString());
            Debug.WriteLine("App" + App.MainPackage.PackageType.ToString());

        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            AppShell.Current.AppFrame.Navigate(AppShell.Current.navlist[2].DestPage);
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            AppShell.Current.AppFrame.Navigate(AppShell.Current.navlist[0].DestPage);
        }

        private void Package_Type_Combo_Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _save();
            _reloadViews();
        }

        private void Processor_Type_Combo_Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _save();
            _reloadViews();
        }
    }
}
