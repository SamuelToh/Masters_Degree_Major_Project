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

namespace MQuter_eLabApp.Exceptions
{
    public class NullValueForMandatoryParam: System.Exception
    {
        public NullValueForMandatoryParam(string msg)
            : base(msg) { }
    }
  
}
