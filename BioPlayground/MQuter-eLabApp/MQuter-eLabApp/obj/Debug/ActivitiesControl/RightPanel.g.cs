﻿#pragma checksum "C:\Users\Tendious\Documents\Visual Studio 2010\Projects\MQuter-eLabApp\MQuter-eLabApp\ActivitiesControl\RightPanel.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7E78DB8F32BE405B71BEA02379080264"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SStuff.FlowControls;
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


namespace MQuter_eLabApp.ActivitiesControl {
    
    
    public partial class RightPanel : System.Windows.Controls.UserControl {
        
        internal System.Windows.Media.Animation.Storyboard AnimateViewActivities;
        
        internal System.Windows.Media.Animation.Storyboard AnimateBackToCategory;
        
        internal System.Windows.Controls.TabControl tabControl;
        
        internal System.Windows.Controls.Button lblHeader;
        
        internal SStuff.FlowControls.FlowItemsControl3D _3DFlowPanel;
        
        internal System.Windows.Controls.Canvas Screens;
        
        internal System.Windows.Controls.StackPanel FrntSPActivityCat;
        
        internal System.Windows.Controls.StackPanel BckSPActivities;
        
        internal System.Windows.Controls.ListBox listActivity;
        
        internal System.Windows.Controls.Button btnActCat2;
        
        internal System.Windows.Controls.Button btnBack;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/MQuter-eLabApp;component/ActivitiesControl/RightPanel.xaml", System.UriKind.Relative));
            this.AnimateViewActivities = ((System.Windows.Media.Animation.Storyboard)(this.FindName("AnimateViewActivities")));
            this.AnimateBackToCategory = ((System.Windows.Media.Animation.Storyboard)(this.FindName("AnimateBackToCategory")));
            this.tabControl = ((System.Windows.Controls.TabControl)(this.FindName("tabControl")));
            this.lblHeader = ((System.Windows.Controls.Button)(this.FindName("lblHeader")));
            this._3DFlowPanel = ((SStuff.FlowControls.FlowItemsControl3D)(this.FindName("_3DFlowPanel")));
            this.Screens = ((System.Windows.Controls.Canvas)(this.FindName("Screens")));
            this.FrntSPActivityCat = ((System.Windows.Controls.StackPanel)(this.FindName("FrntSPActivityCat")));
            this.BckSPActivities = ((System.Windows.Controls.StackPanel)(this.FindName("BckSPActivities")));
            this.listActivity = ((System.Windows.Controls.ListBox)(this.FindName("listActivity")));
            this.btnActCat2 = ((System.Windows.Controls.Button)(this.FindName("btnActCat2")));
            this.btnBack = ((System.Windows.Controls.Button)(this.FindName("btnBack")));
        }
    }
}
