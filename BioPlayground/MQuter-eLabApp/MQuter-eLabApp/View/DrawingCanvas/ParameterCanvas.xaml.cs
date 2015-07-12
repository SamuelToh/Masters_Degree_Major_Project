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
using MQuter_eLabApp.View.Editors;
using MQuter_eLabApp.ViewModel;
using MQuter_eLabApp.View.Components.Activity;
using MQuter_eLabApp.View.Components.Parameter;

namespace MQuter_eLabApp.View.DrawingCanvas
{
    public partial class ParameterCanvas : UserControl
    {

        public delegate void OnParamValueChangedEventHandler(object sender, EventArgs e);

        public event OnParamValueChangedEventHandler ParamValueChanged;

        // Invoke the Changed event; called whenever activities on canvas changes:
        protected virtual void ParamValueChangesEvent(EventArgs e)
        {
            if (ParamValueChanged != null)
                ParamValueChanged(this, e);
        }

        public ActivityEditor childTest;

        private Dictionary<IActivityComponent, ParamOutboundComp> myDictionary;

        public ParameterCanvas()
        {
            InitializeComponent();
            myDictionary = new Dictionary<IActivityComponent, ParamOutboundComp>();
        }

      
        public void AddSourceActivity(IActivityComponent item)
        {
            #region Validation 

            if (item == null)
                throw new ArgumentNullException
                            ("Null is not permitted.");

            #endregion
         
            this.SPSourceActivity.Children.Add
                                (CreateParamComponent(item));
        }

        public void RemoveSourceActivity(IActivityComponent item)
        {
            if (item == null)
                throw new ArgumentNullException("source cannot be null.");
            else if ((item as UIElement) == null)
                throw new ArgumentException("You can only remove an UIElement object.");


            if (myDictionary.ContainsKey(item))
            {
                (item as UIElement).Visibility = System.Windows.Visibility.Collapsed;
                SPSourceActivity.Children.Remove(myDictionary[item]);
                myDictionary[item].DestroyOutputLine(); //destroy its output line (if any exist)
                myDictionary.Remove(item);
            }
            
        }

        public void AddOutboundActivity(IActivityComponent item)
        {
            #region Validation 

            if (item == null)
                throw new ArgumentNullException
                    ("Null is not permitted.");

            #endregion
            
            this.SPMainActivity.Children.Add
                                (CreateOutboundParamComp(item));
        }

        public IEnumerable<UIElement>GetElementsInCanvasCoordinates(Point location)
        {
            return childTest.GetElementsInMenuCoordinates(location, this);
        }

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(FrameworkElement item)
        {
            itemContainer.Children.Add(item);
        }

        private ParamInboundComp CreateOutboundParamComp
                           (IActivityComponent activity)
        {

            ParamInboundComp component = new ParamInboundComp(this)
            {
                Name = activity.Name,
                DataContext = activity.DataContext
            };
            component.ParamValueChanged += new ParamInboundComp.OnParamValueChangedEventHandler(component_ParamValueChanged);

            return component;
            /*return new ParamInboundComp(this)
            {
                Name = activity.Name,
                DataContext = activity.DataContext
            };*/
        }

        private ParamOutboundComp CreateParamComponent
                                   (IActivityComponent activity)
        {
            ParamOutboundComp component = new ParamOutboundComp(this)
            {
                Name = activity.Name,
                DataContext = activity.DataContext
            };

            component.ParamValueChanged += new ParamOutboundComp.OnParamValueChangedEventHandler(component_ParamValueChanged);
            AddReferenceToDictionary(activity, component);

            return component;
        }

        void component_ParamValueChanged(object sender, EventArgs e)
        {
            ParamValueChanged(sender, e);
        }

        private void AddReferenceToDictionary(IActivityComponent activity, ParamOutboundComp paramModel)
        {
            this.myDictionary.Add(activity, paramModel);
        }
    }
}
