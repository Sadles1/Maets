﻿#pragma checksum "..\..\..\..\Data\Views\Korzina.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5B32A493B531E814956FCFD65B8BFE2E037D654494EC9EF90BEDBD2924A13DA9"
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
    /// Korzina
    /// </summary>
    public partial class Korzina : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\..\Data\Views\Korzina.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbSumm;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\Data\Views\Korzina.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnExit;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\Data\Views\Korzina.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFull;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\Data\Views\Korzina.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lvProduct;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\..\Data\Views\Korzina.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Buy;
        
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
            System.Uri resourceLocater = new System.Uri("/Client;component/data/views/korzina.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Data\Views\Korzina.xaml"
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
            
            #line 7 "..\..\..\..\Data\Views\Korzina.xaml"
            ((Client.Korzina)(target)).Initialized += new System.EventHandler(this.Window_Initialized);
            
            #line default
            #line hidden
            return;
            case 2:
            this.tbSumm = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.btnExit = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\..\Data\Views\Korzina.xaml"
            this.btnExit.Click += new System.Windows.RoutedEventHandler(this.TbExit_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnFull = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\..\Data\Views\Korzina.xaml"
            this.btnFull.Click += new System.Windows.RoutedEventHandler(this.BtnFull_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.lvProduct = ((System.Windows.Controls.ListView)(target));
            return;
            case 6:
            this.Buy = ((System.Windows.Controls.Button)(target));
            
            #line 56 "..\..\..\..\Data\Views\Korzina.xaml"
            this.Buy.Click += new System.Windows.RoutedEventHandler(this.Buy_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

