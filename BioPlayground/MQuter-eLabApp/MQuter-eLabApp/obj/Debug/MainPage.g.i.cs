﻿#pragma checksum "C:\Users\Tendious\Documents\Visual Studio 2010\Projects\MQuter-eLabApp\MQuter-eLabApp\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "567811A1BA48E894E806D7664EB5989B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MQuter_eLabApp.View.DrawingCanvas;
using MQuter_eLabApp.View.MenuBar;
using MQuter_eLabApp.View.Panels;
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


namespace MQuter_eLabApp {
    
    
    public partial class MainPage : System.Windows.Controls.UserControl {
        
        internal System.Windows.Media.Animation.Storyboard ShineTest;
        
        internal System.Windows.Media.Animation.Storyboard ShowResult;
        
        internal System.Windows.Media.Animation.Storyboard HideResult;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid GridHeader;
        
        internal System.Windows.Controls.TextBlock TestLabel;
        
        internal System.Windows.Controls.Button btnFullScreen;
        
        internal System.Windows.Controls.Canvas WFBuilderSpace;
        
        internal MQuter_eLabApp.View.MenuBar.eLabMenu eLabMenuBar;
        
        internal MQuter_eLabApp.View.DrawingCanvas.WorkflowCanvas WorkflowCanvas0;
        
        internal MQuter_eLabApp.View.MenuBar.AnnotationToolbar AnnotationToolBar0;
        
        internal MQuter_eLabApp.View.Panels.ResultPanel resultPanel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/MQuter-eLabApp;component/MainPage.xaml", System.UriKind.Relative));
            this.ShineTest = ((System.Windows.Media.Animation.Storyboard)(this.FindName("ShineTest")));
            this.ShowResult = ((System.Windows.Media.Animation.Storyboard)(this.FindName("ShowResult")));
            this.HideResult = ((System.Windows.Media.Animation.Storyboard)(this.FindName("HideResult")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.GridHeader = ((System.Windows.Controls.Grid)(this.FindName("GridHeader")));
            this.TestLabel = ((System.Windows.Controls.TextBlock)(this.FindName("TestLabel")));
            this.btnFullScreen = ((System.Windows.Controls.Button)(this.FindName("btnFullScreen")));
            this.WFBuilderSpace = ((System.Windows.Controls.Canvas)(this.FindName("WFBuilderSpace")));
            this.eLabMenuBar = ((MQuter_eLabApp.View.MenuBar.eLabMenu)(this.FindName("eLabMenuBar")));
            this.WorkflowCanvas0 = ((MQuter_eLabApp.View.DrawingCanvas.WorkflowCanvas)(this.FindName("WorkflowCanvas0")));
            this.AnnotationToolBar0 = ((MQuter_eLabApp.View.MenuBar.AnnotationToolbar)(this.FindName("AnnotationToolBar0")));
            this.resultPanel = ((MQuter_eLabApp.View.Panels.ResultPanel)(this.FindName("resultPanel")));
        }
    }
}

