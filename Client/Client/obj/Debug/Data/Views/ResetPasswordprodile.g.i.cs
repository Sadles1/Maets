#pragma checksum "..\..\..\..\Data\Views\ResetPasswordprodile.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1C151351F7419C5478E4D6BFDF54F4047E23A5ED"
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


namespace Client
{


    /// <summary>
    /// Resetpasswod
    /// </summary>
    public partial class ResetPasswordprodile : System.Windows.Window, System.Windows.Markup.IComponentConnector
    {

#line default
#line hidden


#line 28 "..\..\..\..\Data\Views\ResetPasswordprodile.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Code;

#line default
#line hidden

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Client;component/data/views/resetpasswordprodile.xaml", System.UriKind.Relative);

#line 1 "..\..\..\..\Data\Views\ResetPasswordprodile.xaml"
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
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.tbPassword = ((System.Windows.Controls.PasswordBox)(target));
                    return;
                case 2:
                    this.tbPassword2 = ((System.Windows.Controls.PasswordBox)(target));
                    return;
                case 3:
                    this.tb = ((System.Windows.Controls.TextBlock)(target));
                    return;
                case 4:
                    this.Code = ((System.Windows.Controls.TextBox)(target));
                    return;
                case 5:
                    this.Codee = ((System.Windows.Controls.TextBox)(target));
                    return;
                case 6:
                    this.Go = ((System.Windows.Controls.Button)(target));

#line 30 "..\..\..\..\Data\Views\ResetPasswordprodile.xaml"
                    this.Go.Click += new System.Windows.RoutedEventHandler(this.Go_Click);

#line default
#line hidden
                    return;
            }
            this._contentLoaded = true;
        }

        internal System.Windows.Controls.PasswordBox tbPassword1;
        internal System.Windows.Controls.PasswordBox tbPassword21;
        internal System.Windows.Controls.TextBlock tb1;
        internal System.Windows.Controls.TextBox Codee1;
        internal System.Windows.Controls.Button Go1;
    }
}

