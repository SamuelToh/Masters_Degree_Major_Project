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
using MQuter_eLabApp.Events;
using MQuter_eLabApp.Model;
using MQuter_eLabApp.View.Components.Activity;
using MQuter_eLabApp.View.Panels;

namespace MQuter_eLabApp.View.Editors
{
    public partial class ActivityEditor : ChildWindow
    {
        /// <summary>
        /// There can be more than 1 inputs to an Activity
        /// </summary>
        private List<IActivityComponent> _sources = new List<IActivityComponent>();

        /// <summary>
        /// This flag deters whether the current editor has any commited changes to it or not.
        /// </summary>
        private bool flagParamHasChanges = false;

        /// <summary>
        /// The target where our inputs are connected from
        /// </summary>
        private IActivityComponent Outbound { get; set; }

        public delegate void ValidationFailEventHandler(object sender, EventArgs e);

        public event ValidationFailEventHandler ValidationFail;

        // Invoke the Changed event; called whenever activities on canvas changes:
        protected virtual void OnValidationFail(EventArgs e)
        {
            if (ValidationFail != null)
                ValidationFail(this, e);
        }

        #region Various constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ActivityEditor()
        {
            InitializeComponent();
            this.ParamEditorCanvas.ParamValueChanged 
                += new DrawingCanvas.ParameterCanvas.OnParamValueChangedEventHandler
                                                    (ParamEditorCanvas_ParamValueChanged);
            this.ParamEditorCanvas.childTest = this;
        }

        void ParamEditorCanvas_ParamValueChanged(object sender, EventArgs e)
        {
            this.flagParamHasChanges = true;
        }

        public ActivityEditor(IActivityComponent outboundActivity)
            : this()
        {
            Outbound = outboundActivity;
            ParamEditorCanvas.AddOutboundActivity(outboundActivity);
        }

        public ActivityEditor(IActivityComponent source, IActivityComponent outboundActivity)
            : this(outboundActivity)
        {
            this.AddSource(source);
        }

        public ActivityEditor(List<IActivityComponent> sources, IActivityComponent outboundActivity)
            : this(outboundActivity)
        {
            foreach (IActivityComponent source in sources)
            {
                //We first add an reference to collection belonging to this class
                this.AddSource(source);
            }
        }

        public ActivityEditor(List<IActivityComponent> sources, IActivityComponent[] sourceNestedComps, IActivityComponent outboundActivity)
            : this(outboundActivity)
        {
            foreach (IActivityComponent source in sources)
            {
                //We first add an reference to collection belonging to this class
                this.AddSource(source);
            }

            //for each nested comp in for loop do this.
            foreach (IActivityComponent source in sourceNestedComps)
            {
                //We first add an reference to collection belonging to this class
                this.AddSource(source);
            }
        }

        public ActivityEditor(List<IActivityComponent> sources, IActivityComponent outboundActivity, IActivityComponent[] nestedComps)
            : this(sources, outboundActivity)
        {
            foreach (IActivityComponent component in nestedComps)
            {
                //We first add an reference to collection belonging to this class
                ParamEditorCanvas.AddOutboundActivity(component);
            }
        }

        #endregion Various constructors

        #region Public Methods

        public bool RemoveSource(IActivityComponent source)
        {
            if (_sources.Contains(source))
            {
                _sources.Remove(source);
                ParamEditorCanvas.RemoveSourceActivity(source);
                return true;
            }

            return false;
        }

        public void AddSource(IActivityComponent source)
        {
            _sources.Add(source);
            ParamEditorCanvas.AddSourceActivity(source);
        }

        public void UpdateSources(List<IActivityComponent> sources)
        {
            foreach (IActivityComponent source in sources)
            {
                if(!_sources.Contains(source))
                {
                    this.AddSource(source);
                }
            }
        }

        public IEnumerable<UIElement> GetElementsInMenuCoordinates
                                                    (Point location, UIElement uiComponent)
        {
            //This method is slightly different from the one in WorkflowController.cs
            //We take the coordinate from the child window instead of the application root
            GeneralTransform generalTransform = uiComponent.TransformToVisual(this);
            Point pnts = generalTransform.Transform(location);
            IEnumerable<UIElement> elements = VisualTreeHelper.FindElementsInHostCoordinates(pnts, this);
            return elements;
        }

        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            bool flagIsAlright = false;

            //Temp solution for Gate lights 
            //Validation rule goes here
            try
            {
                using (ActivityValidator validator = new ActivityValidator())
                {
                    validator.ValidateActivities(_sources, Outbound);
                    flagIsAlright = true;
                };

            }
            catch (Exception ex)
            {
                if (flagParamHasChanges)
                {
                    OnValidationFail(new ActivityValidationEventArgs(ex));
                    ReportException(ex);
                }
            }

            if (flagParamHasChanges)
                UpdateGateStatus(flagIsAlright);
        }

        /// <summary>
        /// Only report exception that says something about local file reading.
        /// </summary>
        /// <param name="e"></param>
        private void ReportException(Exception e)
        {
            //if (e is ArgumentException)
            //{
                ExceptionPanel exPanel = new ExceptionPanel();
                exPanel.MessageBox.Text = e.Message;
                exPanel.Title = "Warning!";
                exPanel.Show();
                //UpdateGateStatus(true); //still go ahead for warning sign
            //}
        }

        private void UpdateGateStatus(bool isOK)
        {
            IOGateComponent.Status Status = isOK 
                ? IOGateComponent.Status.OK : IOGateComponent.Status.ConnectedWithErr;


            foreach (IActivityComponent component in _sources)
            {
                IOGateComponent gate = component.GetOutputGate;
                gate.GateStatus = Status;
            }

            IOGateComponent inputGate = null;

            if (Outbound is ForLoopComponent)
            {
                inputGate = (Outbound as ForLoopComponent).InputGate;
            }
            else
            {
                inputGate = (Outbound as ActivityComponent).InputGate;
            }

            inputGate.GateStatus = Status;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

       

    }
}

