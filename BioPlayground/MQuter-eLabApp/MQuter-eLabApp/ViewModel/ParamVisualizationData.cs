using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace MQuter_eLabApp.ViewModel
{
    public class ParamVisualizationData
    {
        private readonly List<ParameterModel> _items = new List<ParameterModel>();

        public ParamVisualizationData() { }

        /// <summary>
        /// Getter for our items
        /// </summary>
        public List<ParameterModel> Items
        {
            get { return _items; }
        }
    }
}
