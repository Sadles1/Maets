﻿#pragma checksum "..\..\..\..\Data\Views\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B8F3136D9811275E1660703400AFB3C6954933C0"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Client {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\..\..\Data\Views\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbLogin;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\Data\Views\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox tbPassword;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\Data\Views\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox RememberPassword;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\Data\Views\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button resetpassword;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\Data\Views\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Login;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\Data\Views\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Register;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\Data\Views\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbLogin;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\Data\Views\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbPassword;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Data\Views\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnExit;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Client;component/data/views/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Data\Views\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 6 "..\..\..\..\Data\Views\MainWindow.xaml"
            ((Client.MainWindow)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 9 "..\..\..\..\Data\Views\MainWindow.xaml"
            ((Client.MainWindow)(target)).Initialized += new System.EventHandler(this.Window_Initialized);
            
            #line default
            #line hidden
            return;
            case 2:
            this.tbLogin = ((System.Windows.Controls.TextBox)(target));
            
            #line 19 "..\..\..\..\Data\Views\MainWindow.xaml"
            this.tbLogin.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TbLogin_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.tbPassword = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 20 "..\..\..\..\Data\Views\MainWindow.xaml"
            this.tbPassword.PasswordChanged += new System.Windows.RoutedEventHandler(this.TbPassword_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.RememberPassword = ((System.Windows.Controls.CheckBox)(target));
            
            #line 21 "..\..\..\..\Data\Views\MainWindow.xaml"
            this.RememberPassword.Click += new System.Windows.RoutedEventHandler(this.RememberPassword_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.resetpassword = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\..\..\Data\Views\MainWindow.xaml"
            this.resetpassword.Click += new System.Windows.RoutedEventHandler(this.Resetpassword_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Login = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\..\..\Data\Views\MainWindow.xaml"
            this.Login.Click += new System.Windows.RoutedEventHandler(this.Login_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.Register = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\..\Data\Views\MainWindow.xaml"
            this.Register.Click += new System.Windows.RoutedEventHandler(this.Register_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.lbLogin = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.lbPassword = ((System.Windows.Controls.Label)(target));
            return;
            case 10:
            this.btnExit = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\..\..\Data\Views\MainWindow.xaml"
            this.btnExit.Click += new System.Windows.RoutedEventHandler(this.TbExit_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

