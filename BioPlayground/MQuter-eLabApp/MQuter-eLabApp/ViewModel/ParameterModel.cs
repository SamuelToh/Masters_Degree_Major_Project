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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MQuter_eLabApp.ViewModel
{
    /// <summary>
    /// Each parameter model represents a [parameter] of an activity.
    /// 
    /// e.g. If should an activity has 3 parameters, namely;
    /// "Name", typeof (String)
    /// "Age", typeof (int)
    /// "IsMale", typeof (boolean)
    /// 
    /// That activity will have 3 parameter models with its field populated
    /// respectively.
    /// </summary>
    [Bindable(true)]
    public class ParameterModel
    {
        #region Properties

        /// <summary>
        /// Default constructor
        /// </summary>
        public ParameterModel() { }

        public ParameterModel(bool isMandatory)
        {
            this.isMandatory = isMandatory;
        }

        /// <summary>
        /// The data type of this parameter
        /// </summary>
        [Bindable(false)]
        [Editable(false)]
        [Display(Name= "DataType", Order=1)]
        public String DataType { get; set; }

        /// <summary>
        /// Couple of lines telling more about this param
        /// </summary>
        [Display(AutoGenerateField = false)]
        public String Description { get; set; }

        /// <summary>
        /// Is in or outbound param
        /// </summary>
        [Bindable(false)]
        [Display(AutoGenerateField = false)]
        public bool IsInputParam { get; set; }

        /// <summary>
        /// The label to be displayed when this parameter is rendered on the UI
        /// </summary>
        [Bindable(false)]
        [Display(Description = "Name", Order = 2)]
        [Editable(false)]
        public String Label { get; set; }

        /// <summary>
        /// The unique identifier for this parameter
        /// </summary>
        [Bindable(false)]
        [Display(AutoGenerateField = false, Order = 3)]
        public String Name { get; set; }

        /// <summary>
        /// The unique identifier for its parent.
        /// </summary>
        [Bindable(false)]
        [Display(AutoGenerateField = false)]
        public String ParentName { get; set; }


        [Display(AutoGenerateField = false)]
        public ParameterModel[] SiblingModels
        {
            get;
            set;
        }

        /// <summary>
        /// A bad design to change later
        /// </summary>
        internal bool isMandatory = false;

        #endregion Properties 
    }
}
