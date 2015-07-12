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
    /// This parameter container is raised when annotation is added to the canvas
    /// </summary>
    public class AnnotationEventArgs : EventArgs
    {
        /// <summary>
        /// The choosen type of annotation to be drawn
        /// </summary>
        public IAnnotationModel AnnotationContainer { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="annotationContainer">See IAnnotation Model interface for more info.</param>
        public AnnotationEventArgs(IAnnotationModel annotationContainer)
        {
            this.AnnotationContainer = annotationContainer;
        }
    }
}
