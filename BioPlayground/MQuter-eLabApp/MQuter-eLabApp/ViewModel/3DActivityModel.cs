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

namespace MQuter_eLabApp.ViewModel
{
    public class _3DActivityModel
    {
        /// <summary>
        /// The display name for our 3D flow Item
        /// </summary>
        public string Value { get; set; }

        public override string ToString() { return Value; }
    }
}
