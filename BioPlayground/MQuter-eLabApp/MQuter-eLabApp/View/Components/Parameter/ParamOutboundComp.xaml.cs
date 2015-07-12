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
using MQuter_eLabApp.Exceptions;
using MQuter_eLabApp.ViewModel;
using MQuter_eLabApp.View.DrawingCanvas;
using MQuter_eLabApp.View.ConnectionLines;
using MQuter_eLabApp.View.Editors;
using MQuter_eLabApp.View.Panels;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MQuter_eLabApp.View.Components.Parameter
{
    public partial class ParamOutboundComp : UserControl
    {
        private ParameterCanvas _paramCanvas;
        private ParameterConnection outboundLine;

        public delegate void OnParamValueChangedEventHandler(object sender, EventArgs e);

        public event OnParamValueChangedEventHandler ParamValueChanged;

        // Invoke the Changed event; called whenever activities on canvas changes:
        protected virtual void ParamValueChangesEvent(EventArgs e)
        {
            if (ParamValueChanged != null)
                ParamValueChanged(this, e);
        }

        public ParamOutboundComp()
        {
            InitializeComponent();
        }

        public ParamOutboundComp(ParameterCanvas master)
            : this()
        {
            ParamCanvas = master;
        }

        public ParameterCanvas ParamCanvas
        {
            get { return _paramCanvas; }
            set { _paramCanvas = value; }
        }

        #region Event Mouse handlers for Line drawing

        private void ParamGate_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ParamIOGate paramConnector = sender as ParamIOGate;

            if (paramConnector != null)
            {
                Point currPos =
                             new Point(e.GetPosition(ParamCanvas.itemContainer).X,
                                       e.GetPosition(ParamCanvas.itemContainer).Y);

                //Set is painting line equals to true
                paramConnector.IsConnecting = true;

                //Due to the use of data context binding, once the mennu was closed everything wil be lost
                //if(this.outboundLine == null)
                if (paramConnector.ConnectionLine == null)
                {
                    paramConnector.ConnectionLine = new ParameterConnection(currPos);
                    paramConnector.ConnectionLine.StrokeColor = Colors.White;
                    Canvas.SetLeft(paramConnector.ConnectionLine, currPos.X);
                    Canvas.SetTop(paramConnector.ConnectionLine, currPos.Y);
                    Canvas.SetZIndex(paramConnector.ConnectionLine, 3); //We want the user to see the line he is drawing
                    ParamCanvas.Add(paramConnector.ConnectionLine);
                }
                else
                {
                    //outboundLine.Redraw(currPos, currPos);
                    paramConnector.ConnectionLine.Redraw(currPos, currPos);
                }

                paramConnector.CaptureMouse();

            }

            else { return; }

        }

        private void paramGate_MouseMove(object sender, MouseEventArgs e)
        {
            ParamIOGate paramConnector = sender as ParamIOGate;

            if (paramConnector == null)
                return;

            else if
              (paramConnector.IsConnecting)
            {
                Point currPos =
                    new Point(e.GetPosition(ParamCanvas.itemContainer).X,
                              e.GetPosition(ParamCanvas.itemContainer).Y);


                paramConnector.ConnectionLine.Redraw(currPos);

            }
        }

        private void ParamGate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is ParamIOGate)
            {
                ParamIOGate paramGate = sender as ParamIOGate;

                if (paramGate.IsConnecting)
                {
                    Point currPos =
                         new Point(e.GetPosition(ParamCanvas).X,
                                   e.GetPosition(ParamCanvas).Y);

                    //Validate the collision 
                    if (IsValidConnection(currPos, paramGate))
                    {
                        //update it here
                        UpdateInboundParam(currPos, paramGate);
                        outboundLine = paramGate.ConnectionLine;

                        ParamValueChangesEvent(new EventArgs()); //Commitment was made
                    }
                    else
                    {
                        UndoLineDrawing(paramGate);
                    }

                    paramGate.IsConnecting = false;
                    paramGate.ReleaseMouseCapture();
                }
            }
        }

        public void DestroyOutputLine()
        {
            if (outboundLine != null)
            {
                //destroy it
                outboundLine.DeleteLine();
                outboundLine = null;
            }
        }

        private void UndoLineDrawing(ParamIOGate paramGate)
        {
            paramGate.ConnectionLine.DeleteLine();
            paramGate.ConnectionLine = null;

            DestroyOutputLine();//destroy any previous drawn lines
        }

        #endregion Event Mouse handlers for Line drawing

        public bool IsValidConnection(Point location, ParamIOGate self)
        {
            foreach (UIElement ele in ParamCanvas.GetElementsInCanvasCoordinates(location))
            {
                if (ele is ParamIOGate)
                    if ((ele as ParamIOGate) != self)
                        return true;
            }

            return false;
        }

        private ParameterModel GetParamModelByCoord(Point location)
        {
            foreach (UIElement ele in ParamCanvas.GetElementsInCanvasCoordinates(location))
            {
                if ((ele) is ListBoxItem)
                {
                    ListBoxItem item = ele as ListBoxItem;
                    return item.DataContext as ParameterModel;
                }
            }

            throw new UnableLocateCompEx
                ("Could not locate the inbound component at " +
                    "X: " + location.X + " Y: " + location.Y);
        }

        private ParamInputModel source = null;

        public void UpdateInboundParam(Point inboundLocation, ParamIOGate paramGate)
        {
            ParameterConnection line = paramGate.ConnectionLine;

            try
            {
                Point outboundLocation = new Point(line.Connection.X1, line.Connection.Y1);
                ParamInputModel inboundModel = GetParamModelByCoord(inboundLocation) as ParamInputModel;

                if (inboundModel.Value != null)
                {
                    UndoLineDrawing(paramGate);

                    throw new Exception
                        ("This parameter has already got a data source. You can only have one data source," +
                            "try removing its data source before connecting again.");

                }

                ParameterModel outboundModel = GetParamModelByCoord(outboundLocation) as ParameterModel;

                inboundModel.Value = new ParamBinder(outboundModel.ParentName, outboundModel.Name);
                source = inboundModel; //keep a copy of the source for easy alteration of value
                line.LineDeletionEvent += new ParameterConnection.ParamConnectionEventHandler(line_LineDeletionEvent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void ReportException(Exception e)
        {
            ExceptionPanel exPanel = new ExceptionPanel();
            exPanel.MessageBox.Text = e.Message;
            exPanel.Title = "Parameter already has a data source.";
        }

        void line_LineDeletionEvent(object sender, EventArgs e)
        {
            source.Value = null;
        }

        private void MainRect2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point currPos =
                         new Point(e.GetPosition(ParamCanvas).X,
                                   e.GetPosition(ParamCanvas).Y);

            ParameterModel model = GetParamModelByCoord(currPos);
            ObservableCollection<ParameterModel> models = new ObservableCollection<ParameterModel>();

            for (int i = 0; i < model.SiblingModels.Length; i++)
                models.Add(model.SiblingModels[i]);


            ParamValueEditor valueEditor = new ParamValueEditor()
            {
                DataContext = models,
            };

            valueEditor.ParamDataForm.CurrentItem = model;

            valueEditor.Show();

            ParamValueChangesEvent(new EventArgs()); //Commitment was made
        }




    }
}
