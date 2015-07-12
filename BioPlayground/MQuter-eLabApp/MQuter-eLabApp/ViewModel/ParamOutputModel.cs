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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace MQuter_eLabApp.ViewModel
{
    public class ParamOutputModel : ParameterModel
    {
        [Bindable(true)]
        [Display(Name = "Output", Order=4, Description = "Do you wish to mark this output as a viewable data product?")]
        public bool AsDataOutput { get; set; }

        public ParamOutputModel() {  AsDataOutput = false; }
    }
}
