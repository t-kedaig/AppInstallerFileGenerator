﻿#pragma checksum "C:\Users\t-kedaig\OneDrive - Microsoft\AppInstaller XML Generator\AppInstaller XML Generator\Windows Samples\Samples\XamlNavigation\cs\Views\MainPackageView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "947CA145838FBE44BD65F51605496278"
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
    partial class MainPackageView : 
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
            case 1: // Views\MainPackageView.xaml line 142
                {
                    this.Next_Button = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.Next_Button).Click += this.Next_Button_Click;
                }
                break;
            case 2: // Views\MainPackageView.xaml line 152
                {
                    this.Back_Button = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.Back_Button).Click += this.Back_Button_Click;
                }
                break;
            case 3: // Views\MainPackageView.xaml line 18
                {
                    this.TitlePage = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 4: // Views\MainPackageView.xaml line 42
                {
                    this.Processor_Type_Stack_Panel = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 5: // Views\MainPackageView.xaml line 121
                {
                    this.Resource_Id_Text_Box = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.Resource_Id_Text_Box).TextChanged += this.Resource_Id_Text_Box_TextChanged;
                }
                break;
            case 6: // Views\MainPackageView.xaml line 107
                {
                    this.Name_Text_Box = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.Name_Text_Box).TextChanged += this.Name_Text_Box_TextChanged;
                }
                break;
            case 7: // Views\MainPackageView.xaml line 93
                {
                    this.Publisher_Text_Box = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.Publisher_Text_Box).TextChanged += this.Publisher_Text_Box_TextChanged;
                }
                break;
            case 8: // Views\MainPackageView.xaml line 79
                {
                    this.Version_Text_Box = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.Version_Text_Box).TextChanged += this.Version_Text_Box_TextChanged;
                }
                break;
            case 9: // Views\MainPackageView.xaml line 65
                {
                    this.File_Path_Text_Box = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.File_Path_Text_Box).TextChanged += this.File_Path_Text_Box_TextChanged;
                }
                break;
            case 10: // Views\MainPackageView.xaml line 47
                {
                    this.Processor_Type_Combo_Box = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.Processor_Type_Combo_Box).SelectionChanged += this.Processor_Type_Combo_Box_SelectionChanged;
                }
                break;
            case 11: // Views\MainPackageView.xaml line 32
                {
                    this.Package_Type_Combo_Box = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.Package_Type_Combo_Box).SelectionChanged += this.Package_Type_Combo_Box_SelectionChanged;
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

