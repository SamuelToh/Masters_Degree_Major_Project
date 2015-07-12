using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MQuter_eLabApp.View.Components.Activity
{
    public partial class IOGateComponent : UserControl
    {
        private bool _hasFocus;
        private bool _isInnerGate = false; //Indicates that this is a special IO gate
        private IActivityComponent _masterControl;
        private Status _status;

        public enum Status
        {
            ConnectedWithErr = 0,
            Normal = 1,
            NeedsConfiguration = 2,
            OK = 3
        };

        public IOGateComponent()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += new MouseButtonEventHandler(IOGateComponent_MouseLeftButtonDown);
            this.MouseLeave += new MouseEventHandler(IOGateComponent_MouseLeave);
            this.Loaded += new RoutedEventHandler(IOGateComponent_Loaded);
        }

        void IOGateComponent_Loaded(object sender, RoutedEventArgs e)
        {
            IOGateComponent gate = sender as IOGateComponent;
            
            //We determine to see whether himself is an inner object of complex type
            if (gate.MasterControl is ForLoopComponent)
            {
                ForLoopComponent master = gate.MasterControl as ForLoopComponent;
                
                if((gate == master.InnerOutputGate) || (gate == master.InnerInputGate))
                    _isInnerGate = true;
            }   
                
        }

        public bool IsInnerGate
        {
            get
            {
                return _isInnerGate;
            }
        }

        public IActivityComponent MasterControl
        {
            get { return _masterControl; }
            set { _masterControl = value; }
        }

        public bool HasFocus
        {
            get { return this._hasFocus; }
            set { this._hasFocus = value; }
        }

        public Status GateStatus
        {
            get { return this._status; }
            set 
            { 
                this._status = value;
                #region Set the Gate coloring (based on its current status)
                switch (value)
                {
                    case Status.Normal :
                        GateBrush.Color = Color.FromArgb
                            (0xFF, 0x7E, 0x7E, 0x7E);
                        break;

                    case Status.ConnectedWithErr :
                        GateBrush.Color = Color.FromArgb
                            (0xFF, 0x99, 0x1B, 0x1B);
                        break;

                    case Status.NeedsConfiguration :
                        GateBrush.Color = Color.FromArgb
                            (0xFF, 0xE5, 0xFF, 0x00);
                        break;

                    case Status.OK:
                        GateBrush.Color = Color.FromArgb
                            (0xFF, 0x40, 0xF1, 0x02);
                        break;
                }
                #endregion Set the Gate coloring (based on its current status)
            }
        }


        #region Event Handlers 

        void IOGateComponent_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HasFocus = true;
        }

        void IOGateComponent_MouseLeave(object sender, MouseEventArgs e)
        {
            HasFocus = false;
        }

        #endregion Event Handlers


    }
}
