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

namespace MQuter_eLabApp.View.Components.Parameter
{
    public partial class ParamIOGate : UserControl
    {
        private ParameterConnection connection;
        private bool isConnecting;

        public ParamIOGate()
        {
            InitializeComponent();
            IsConnecting = false;
        }

        public bool IsConnecting
        {
            get { return isConnecting; }
            set { isConnecting = value; }
        }

        public ParameterConnection ConnectionLine
        {
            get { return connection; }
            set { connection = value; }
        }

    }
}
