﻿#pragma checksum "C:\Users\t-kedaig\OneDrive - Microsoft\AppInstaller XML Generator\AppInstaller XML Generator\AppInstallerFileGenerator\Samples\XamlNavigation\cs\Views\DependenciesView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "209A4050D04C5A95CFB234468519871E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppInstallerFileGenerator.Views
{
    partial class DependenciesView : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1: // Views\DependenciesView.xaml line 144
                {
                    this.Next_Button = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.Next_Button).Click += this.Next_Button_Click;
                }
                break;
            case 2: // Views\DependenciesView.xaml line 154
                {
                    this.Back_Button = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.Back_Button).Click += this.Back_Button_Click;
                }
                break;
            case 3: // Views\DependenciesView.xaml line 41
                {
                    this.Package_Info_Stack_Panel = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 4: // Views\DependenciesView.xaml line 57
                {
                    this.Processor_Type_Stack_Panel = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 5: // Views\DependenciesView.xaml line 122
                {
                    this.Name_Text_Box = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.Name_Text_Box).TextChanged += this.Name_Text_Box_TextChanged;
                }
                break;
            case 6: // Views\DependenciesView.xaml line 108
                {
                    this.Publisher_Text_Box = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.Publisher_Text_Box).TextChanged += this.Publisher_Text_Box_TextChanged;
                }
                break;
            case 7: // Views\DependenciesView.xaml line 94
                {
                    this.Version_Text_Box = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.Version_Text_Box).TextChanged += this.Version_Text_Box_TextChanged;
                }
                break;
            case 8: // Views\DependenciesView.xaml line 80
                {
                    this.File_Path_Text_Box = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.File_Path_Text_Box).TextChanged += this.File_Path_Text_Box_TextChanged;
                }
                break;
            case 9: // Views\DependenciesView.xaml line 62
                {
                    this.Processor_Type_Combo_Box = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.Processor_Type_Combo_Box).SelectionChanged += this.Processor_Type_Combo_Box_SelectionChanged;
                }
                break;
            case 10: // Views\DependenciesView.xaml line 48
                {
                    this.Package_Type_Combo_Box = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.Package_Type_Combo_Box).SelectionChanged += this.Package_Type_Combo_Box_SelectionChanged;
                }
                break;
            case 11: // Views\DependenciesView.xaml line 20
                {
                    this.TitlePage = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 12: // Views\DependenciesView.xaml line 30
                {
                    this.Dependencies_Switch = (global::Windows.UI.Xaml.Controls.ToggleSwitch)(target);
                    ((global::Windows.UI.Xaml.Controls.ToggleSwitch)this.Dependencies_Switch).Toggled += this.ToggleSwitch_Toggled;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

