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
    public class ParamInputModel : ParameterModel
    {

        private object valueobj { get; set; }

        /// <summary>
        /// The object value for this parameter
        /// 
        /// If the value is a complex type such as an data object
        /// then ParamBinder class model is attached to this value attr
        /// @see paramBinder class for more info on how complex type 
        /// values are bined using that class.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public object Value
        {
            get
            {
                return valueobj;
            }
            set
            {
                if (value != null)
                {
                    ValueStr = (value as ParamBinder).Source + "." +
                               (value as ParamBinder).Property;
                    valueobj = value;
                }
                else
                {
                    //clear everything
                    valuestr = null;
                    valueobj = null;
                }
            }
        }

       
        [Display(ShortName = "Mandatory", Order = 4)]
        [Editable(false)]
        public bool IsMandatory
        {
            get
            {
                return base.isMandatory;
            }
            set
            {
                base.isMandatory = value;
            }
        }

        private string valuestr { get; set; }

        [Display(Description = "Value", Order = 5)]
        public string ValueStr
        {
            get
            {
                return valuestr;
            }
            set
            {
                Value = null;
                valuestr = value;
            }
        }

    }
}
