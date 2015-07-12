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
using MQuter_eLabApp.View.ConnectionLines;

namespace MQuter_eLabApp.View.Components.Activity
{
    public partial class ForLoopComponent : UserControl, IActivityComponent
    {
        #region Private Variables

        public bool IsAnchored { get; set; }
        private Exception errorObj = null;
        private bool _componentHasFocus = false;
        private List<IActivityComponent> _nestedActivities = new List<IActivityComponent>();
        private List<ActivityConnection> _outputConn = new List<ActivityConnection>();
        private List<ActivityConnection> _inputConn = new List<ActivityConnection>();

        #endregion

        public ForLoopComponent()
        {
            InitializeComponent();
            InitializeGates();
            Outline.Opacity = 0;
        }

        public new void CaptureMouse()
        {
            base.CaptureMouse();
        }

        public void ReleaseMouse()
        {
            base.ReleaseMouseCapture();
        }

        /// <summary>
        /// This method is needed to fulfill the requirement of its interface.
        /// 
        /// When calling from an interface it won't have access to this UI components directly
        /// thus this method is needed.
        /// </summary>
        public IOGateComponent GetOutputGate
        {
            get { return OutputGate; }
        }

        public IOGateComponent GetInnerOutputGate
        {
            get { return InnerOutputGate; }
        }

        public void ReleaseFocusOnGates()
        {
            OutputGate.HasFocus = false;
            GetInnerOutputGate.HasFocus = false;
        }

        public List<IActivityComponent> GetNestedActivity
        {
            get { return _nestedActivities; }
        }

        public bool OutputGatesHasFocus()
        {
            return OutputGate.HasFocus ? true : InnerOutputGate.HasFocus ? true : false;
        }

        public new string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        public new Object DataContext
        {
            get { return base.DataContext; }
            set { base.DataContext = value; }
        }

        public List<ConnectionLines.ActivityConnection> InputConn
        {
            get
            {
                return _inputConn;
            }
            set
            {
                _inputConn = value;
            }
        }

        public List<ConnectionLines.ActivityConnection> OutputConn
        {
            get
            {
                return _outputConn;
            }
            set
            {
                _outputConn = value;
            }
        }

        public List<IActivityComponent> GetInputs
        {
            get 
            {
                List<IActivityComponent> components = new List<IActivityComponent>();

                foreach (ActivityConnection conn in InputConn)
                {
                    components.Add(conn.InputSource);
                }

                return components;
            }
        }

        public bool HasFocus
        {
            get
            {
                return _componentHasFocus;
            }
            set
            {
                this.Outline.Opacity = value == true ? 1 : 0;
                this._componentHasFocus = value;
            }
        }

        public Point GetComponentPosition()
        {
            double Y = (double)this.GetValue(Canvas.TopProperty);
            double X = (double)this.GetValue(Canvas.LeftProperty);

            return new Point(X, Y);
        }

        public void SetComponentPosition(Point newPosition)
        {
            this.SetValue(Canvas.LeftProperty, newPosition.X);
            this.SetValue(Canvas.TopProperty, newPosition.Y);
        }

        public void AddActivityToInnerCanvas(UserControl activity, MouseEventArgs mouse)
        {
            Point posAtCanvas = mouse.GetPosition(ForLoopInnerCanvas);
            activity.SetValue(Canvas.TopProperty, posAtCanvas.Y);
            activity.SetValue(Canvas.LeftProperty, posAtCanvas.X);
            ForLoopInnerCanvas.Children.Add(activity);
            _nestedActivities.Add(activity as IActivityComponent);
        }

        public ConnectionLines.ActivityConnection GetLatestOutputLine()
        {
            //Check that if the source gate is inner or not, only return non inner
            return OutputConn.Count > 0 ? this.OutputConn[OutputConn.Count - 1] : null;
        }

        private void InitializeGates()
        {
            InputGate.MasterControl = this;
            OutputGate.MasterControl = this;

            InnerOutputGate.MasterControl = this;
            InnerInputGate.MasterControl = this;

            OutputGate.GateStatus = IOGateComponent.Status.NeedsConfiguration;
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!HasFocus)
            {
                IsAnchored = true;
                this.HasFocus = true;
            }
            else
            {
                IsAnchored = false;
                this.HasFocus = false;
            }
        }

        public Exception ActivityParamError
        {
            get { return this.errorObj; }
            set { this.errorObj = value; }
        }

        public IOGateComponent GetForcusedGate()
        {
            return
                InputGate.HasFocus ? InputGate :
                OutputGate.HasFocus ? OutputGate : 
                InnerOutputGate.HasFocus ? InnerOutputGate :
                InnerInputGate.HasFocus ? InnerInputGate : null;
        }
        public bool InputGateHasFocus()
        {
            return InputGate.HasFocus;
        }

    }
}
