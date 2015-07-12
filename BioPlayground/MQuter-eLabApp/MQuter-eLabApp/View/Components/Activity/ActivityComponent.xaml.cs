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
using MQuter_eLabApp.ViewModel;
using MQuter_eLabApp.View.Panels;

namespace MQuter_eLabApp.View.Components.Activity
{
    /// <summary>
    /// All the line drawing codes are enclosed in the workflow controller class for now.
    /// For consistency purpose it should be moved to here at the end of this project.
    /// </summary>
    public partial class ActivityComponent : UserControl, IActivityComponent, IDisposable
    {
        #region Event 

        public delegate void ActivityDeletedEventHandler(object sender, EventArgs e);

        public event ActivityDeletedEventHandler ComponentDeleted;

        protected virtual void OnActivityComponentDeleted(EventArgs e)
        {
            if (ComponentDeleted != null)
                ComponentDeleted(this, e);
        }

        #endregion


        #region Private Variables

        private Exception errorObj = null;
        private bool _componentHasFocus = false;
        private List<ActivityConnection> _outputConn = new List<ActivityConnection>();
        private List<ActivityConnection> _inputConn = new List<ActivityConnection>();

        #endregion

        #region Setters and Getters 

        public new void CaptureMouse()
        {
            base.CaptureMouse();
        }

        public void ReleaseMouse()
        {
            base.ReleaseMouseCapture();
        }

        public new string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        public new Object DataContext
        {
            get { return base.DataContext; }
            set 
            {
                if ((value is ActivityModel) &&
                    (value as ActivityModel).ActivityClass == "EndClass")
                    OutputGate.Visibility = System.Windows.Visibility.Collapsed;

                else
                    if ((value is ActivityModel) &&
                        (value as ActivityModel).ActivityClass == "WorkflowStartClass")
                        InputGate.Visibility = System.Windows.Visibility.Collapsed;

                base.DataContext = value; 
            }
        }

        public List<ActivityConnection> InputConn
        {
            get { return _inputConn; }
            set { _inputConn = value; }
        }

        public List<ActivityConnection> OutputConn
        {
            get{ return _outputConn; }
            set { _outputConn = value; }
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

        public ActivityConnection GetLatestOutputLine()
        {
            return OutputConn.Count > 0 ? this.OutputConn[OutputConn.Count - 1] : null;
        }

        public bool HasFocus
        {
            get
            {
                return _componentHasFocus;
            }
            set
            {
                this._componentHasFocus = value;
            }
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

        public void ReleaseFocusOnGates()
        {
            OutputGate.HasFocus = false;
        }

        public bool OutputGatesHasFocus()
        {
            return OutputGate.HasFocus ? true : false;
        }

        public Point GetComponentPosition()
        {
            double Y = (double) this.GetValue(Canvas.TopProperty);
            double X = (double) this.GetValue(Canvas.LeftProperty);

            return new Point(X, Y);
        }

        public void SetComponentPosition(Point newPosition)
        {
            this.SetValue(Canvas.LeftProperty, newPosition.X);
            this.SetValue(Canvas.TopProperty, newPosition.Y);
        }

        #endregion

        #region Constructor

        public ActivityComponent()
        {
            InitializeComponent();
            InitializeGates();
        }

        private void InitializeGates()
        {
            InputGate.MasterControl = this;
            OutputGate.MasterControl = this;
        }

        #endregion Constructor

        public Exception ActivityParamError
        {
            get { return this.errorObj; }
            set { this.errorObj = value; }
        }

        public IOGateComponent GetForcusedGate()
        {
            return 
                InputGate.HasFocus ? InputGate :
                OutputGate.HasFocus ? OutputGate : null;
        }

        /// <summary>
        /// Component on deleted.
        /// 1) Remove all connected lines
        /// 2) If Parent Canvas is not null, remove it from parent canvas as well.
        /// 3) Fire component deletion event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Canvas parentCanvas = this.Parent as Canvas;

            if (parentCanvas != null)
                parentCanvas.Children.Remove(this);

            foreach (ConnectionLines.ActivityConnection conn in _inputConn)
                conn.DeleteLine(this);

            foreach (ConnectionLines.ActivityConnection conn in _outputConn)
                conn.DeleteLine(this);

            OnActivityComponentDeleted(new EventArgs());
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// IDisposable
        /// </summary>
        public void Dispose()
        {
             _componentHasFocus = false;
             _outputConn = null;
             _inputConn = null;
        }

        public bool InputGateHasFocus()
        {
            return InputGate.HasFocus;
        }

        private void InputGate_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InputGate.HasFocus = true;
            if ((errorObj != null) && 
                    (InputGate.GateStatus == IOGateComponent.Status.ConnectedWithErr))
            {
                ReportException(this.errorObj);
            }
        }

        private void ReportException(Exception e)
        {
            ExceptionPanel exPanel = new ExceptionPanel();
            exPanel.MessageBox.Text = e.Message;
            exPanel.Title = "Validation fail.";
            exPanel.Show();   
        }
    }
}
