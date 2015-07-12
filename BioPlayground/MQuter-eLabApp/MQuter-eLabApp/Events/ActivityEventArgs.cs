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
using MQuter_eLabApp.ViewModel;

namespace MQuter_eLabApp.Events
{
    /// <summary>
    /// This class contains all the essential parameters needed when an activity
    /// is added/removed from the workflow canvas.
    /// </summary>
    public class ActivityEventArgs : EventArgs
    {
        /// <summary>
        /// The unique ID for the transacted activity model
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Stating whether its an addition or removal to the collection
        /// </summary>
        public bool IsAddition { get; set; }

        /// <summary>
        /// The source, activity; data context.
        /// </summary>
        public ActivityModel DataModel { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Unique identifier for the model</param>
        /// <param name="model">The data context</param>
        /// <param name="isAddition">Indicates whether its an addition or removal</param>
        public ActivityEventArgs
            (string name, ActivityModel model, bool isAddition)
        {
            this.Identifier = name;
            this.DataModel = model;
            this.IsAddition = isAddition;
        }
    }
}
