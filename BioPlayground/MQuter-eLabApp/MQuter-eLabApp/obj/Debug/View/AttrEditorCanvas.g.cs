﻿#pragma checksum "C:\Users\Tendious\Documents\Visual Studio 2010\Projects\MQuter-eLabApp\MQuter-eLabApp\View\AttrEditorCanvas.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "92B31DCFEBBA57E9FBC035C710F32369"
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
    
    
    public partial class AttrEditorCanvas : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel SPSourceActivity;
        
        internal System.Windows.Controls.StackPanel SPMainActivity;
        
        internal System.Windows.Controls.Canvas ConnectorCanvas;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/MQuter-eLabApp;component/View/AttrEditorCanvas.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.SPSourceActivity = ((System.Windows.Controls.StackPanel)(this.FindName("SPSourceActivity")));
            this.SPMainActivity = ((System.Windows.Controls.StackPanel)(this.FindName("SPMainActivity")));
            this.ConnectorCanvas = ((System.Windows.Controls.Canvas)(this.FindName("ConnectorCanvas")));
        }
    }
}
