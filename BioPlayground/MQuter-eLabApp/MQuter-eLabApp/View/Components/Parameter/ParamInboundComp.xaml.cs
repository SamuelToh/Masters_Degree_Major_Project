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
using MQuter_eLabApp.View.Editors;
using MQuter_eLabApp.View.DrawingCanvas;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MQuter_eLabApp.View.Components.Parameter
{
    public partial class ParamInboundComp : UserControl
    {
        private ParameterCanvas _paramCanvas;
        private List<ParamIOGate> inputParamBorders;

        public delegate void OnParamValueChangedEventHandler(object sender, EventArgs e);

        public event OnParamValueChangedEventHandler ParamValueChanged;

        // Invoke the Changed event; called whenever activities on canvas changes:
        protected virtual void ParamValueChangesEvent(EventArgs e)
        {
            if (ParamValueChanged != null)
                ParamValueChanged(this, e);
        }


        public ParamInboundComp()
        {
            InitializeComponent();
            inputParamBorders = new List<ParamIOGate>();
        }

        public ParamInboundComp(ParameterCanvas master)
            : this()
        {
            ParamCanvas = master;
        }

        public ParameterCanvas ParamCanvas
        {
            get { return _paramCanvas; }
            set { _paramCanvas = value; }
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

        private ParameterModel currModel; 

        private void OnInboundParamComp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point currPos =
                         new Point(e.GetPosition(ParamCanvas).X,
                                   e.GetPosition(ParamCanvas).Y);

            ParameterModel model = GetParamModelByCoord(currPos);
            currModel = model;
            ObservableCollection<ParameterModel> models = new ObservableCollection<ParameterModel>();

            for (int i = 0; i < model.SiblingModels.Length; i++)
                models.Add(model.SiblingModels[i]);


            ParamValueEditor valueEditor = new ParamValueEditor()
            {
                DataContext = models,
            };

            valueEditor.OnRefresh += new ParamValueEditor.RefreshEventHandler(valueEditor_OnRefresh);
            valueEditor.ParamDataForm.CurrentItem = model;

            valueEditor.Show();

            ParamValueChangesEvent(new EventArgs()); //Commitment was made
        }

        void valueEditor_OnRefresh(object sender, EventArgs e)
        {
            ParamValueEditor valueEditor = new ParamValueEditor()
            {
                DataContext = currModel,
            };

            valueEditor.OnRefresh += new ParamValueEditor.RefreshEventHandler(valueEditor_OnRefresh);
            valueEditor.ParamDataForm.CurrentItem = currModel;

            valueEditor.Show();
        }

        /// <summary>
        /// Hack code that allows us to paint the mandatory gate this slice of code is
        /// needed so we can have access to item templates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InboundGate_Loaded(object sender, RoutedEventArgs e)
        {
            if (!inputParamBorders.Contains(sender as ParamIOGate))
                inputParamBorders.Add(sender as ParamIOGate);

            //Only invoke this method when we are rendering the last item
            if (inputParamBorders.Count == listParameters.Items.Count)
            {
                for (int i = 0; i < listParameters.Items.Count; i++)
                {
                    if (listParameters.Items[i] is ParamInputModel)
                    {
                        ParamInputModel inputParam = listParameters.Items[i] as ParamInputModel;

                        if (!inputParam.IsMandatory)
                        {
                            inputParamBorders[i].GateBrush.Color = Color.FromArgb
                                                                            (0xFF, 0x40, 0xF1, 0x02);
                        }
                    }

                }
            }
        }

    }
}
