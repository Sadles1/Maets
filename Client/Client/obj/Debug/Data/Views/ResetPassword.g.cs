﻿#pragma checksum "..\..\..\..\Data\Views\ResetPassword.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "52A83272D3B9FFDED393D25F2DE058B6261F6D33"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Client;
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
    /// ResetPassword
    /// </summary>
    public partial class ResetPassword : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 273 "..\..\..\..\Data\Views\ResetPassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border windowFrame;
        
        #line default
        #line hidden
        
        
        #line 299 "..\..\..\..\Data\Views\ResetPassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox tbPassword1;
        
        #line default
        #line hidden
        
        
        #line 302 "..\..\..\..\Data\Views\ResetPassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox tbPassword2;
        
        #line default
        #line hidden
        
        
        #line 304 "..\..\..\..\Data\Views\ResetPassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tb;
        
        #line default
        #line hidden
        
        
        #line 305 "..\..\..\..\Data\Views\ResetPassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Code;
        
        #line default
        #line hidden
        
        
        #line 306 "..\..\..\..\Data\Views\ResetPassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Codee;
        
        #line default
        #line hidden
        
        
        #line 307 "..\..\..\..\Data\Views\ResetPassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button changepassword;
        
        #line default
        #line hidden
        
        
        #line 308 "..\..\..\..\Data\Views\ResetPassword.xaml"
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
            System.Uri resourceLocater = new System.Uri("/Client;component/data/views/resetpassword.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Data\Views\ResetPassword.xaml"
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
            this.windowFrame = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.tbPassword1 = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 3:
            this.tbPassword2 = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 4:
            this.tb = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.Code = ((System.Windows.Controls.TextBox)(target));
            
            #line 305 "..\..\..\..\Data\Views\ResetPassword.xaml"
            this.Code.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.Codee_TextChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Codee = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.changepassword = ((System.Windows.Controls.Button)(target));
            
            #line 307 "..\..\..\..\Data\Views\ResetPassword.xaml"
            this.changepassword.Click += new System.Windows.RoutedEventHandler(this.Changepassword_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnExit = ((System.Windows.Controls.Button)(target));
            
            #line 308 "..\..\..\..\Data\Views\ResetPassword.xaml"
            this.btnExit.Click += new System.Windows.RoutedEventHandler(this.TbExit_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

