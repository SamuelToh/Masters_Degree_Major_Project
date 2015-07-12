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
using MQuter_eLabApp.ViewModel.BioWFMLResultModel;

namespace MQuter_eLabApp.Events
{
    public class ResultSetEventArgs : EventArgs
    {
        public ResultCollections results;

        public ResultSetEventArgs(ResultCollections results)
        {
            this.results = results;
        }
    }
}
