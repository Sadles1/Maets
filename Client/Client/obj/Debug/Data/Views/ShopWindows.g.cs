﻿#pragma checksum "..\..\..\..\Data\Views\ShopWindows.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "EDCE50DF834D2639BAC8EED93BE819D3DC9D8999584A882C20034D83034F1AEA"
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
    /// ShopWindows
    /// </summary>
    public partial class ShopWindows : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\..\Data\Views\ShopWindows.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imMainImage;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\Data\Views\ShopWindows.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbLogin;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\Data\Views\ShopWindows.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbName;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\Data\Views\ShopWindows.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView Lv;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\Data\Views\ShopWindows.xaml"
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
            System.Uri resourceLocater = new System.Uri("/Client;component/data/views/shopwindows.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Data\Views\ShopWindows.xaml"
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
            
            #line 7 "..\..\..\..\Data\Views\ShopWindows.xaml"
            ((Client.ShopWindows)(target)).Initialized += new System.EventHandler(this.Window_Initialized);
            
            #line default
            #line hidden
            return;
            case 2:
            this.imMainImage = ((System.Windows.Controls.Image)(target));
            return;
            case 3:
            this.lbLogin = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.lbName = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.Lv = ((System.Windows.Controls.ListView)(target));
            return;
            case 6:
            this.btnExit = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\..\Data\Views\ShopWindows.xaml"
            this.btnExit.Click += new System.Windows.RoutedEventHandler(this.btnExit_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
