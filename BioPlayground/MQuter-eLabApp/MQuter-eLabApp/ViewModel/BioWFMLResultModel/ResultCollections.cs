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
using System.Collections.ObjectModel;

namespace MQuter_eLabApp.ViewModel.BioWFMLResultModel
{
    public class ResultCollections
    {
        public ObservableCollection<string> propertyNames { get; set; }

        public ObservableCollection<ResultSet> resultCollection { get; set; }

        public ResultCollections() { propertyNames = new ObservableCollection<string>(); resultCollection = new ObservableCollection<ResultSet>(); }
    }
}
