﻿#pragma checksum "C:\Users\Tendious\Documents\Visual Studio 2010\Projects\MQuter-eLabApp\MQuter-eLabApp\View\Components\ParamInboundComp.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4EA8C5393BDAA5AD76C24A5284A5CADF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace MQuter_eLabApp.View {
    
    
    public partial class ParamInboundComp : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid SourceActivity;
        
        internal System.Windows.Shapes.Rectangle MainRect3;
        
        internal System.Windows.Controls.TextBlock Title7;
        
        internal System.Windows.Controls.ListBox listParameters;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/MQuter-eLabApp;component/View/Components/ParamInboundComp.xaml", System.UriKind.Relative));
            this.SourceActivity = ((System.Windows.Controls.Grid)(this.FindName("SourceActivity")));
            this.MainRect3 = ((System.Windows.Shapes.Rectangle)(this.FindName("MainRect3")));
            this.Title7 = ((System.Windows.Controls.TextBlock)(this.FindName("Title7")));
            this.listParameters = ((System.Windows.Controls.ListBox)(this.FindName("listParameters")));
        }
    }
}
