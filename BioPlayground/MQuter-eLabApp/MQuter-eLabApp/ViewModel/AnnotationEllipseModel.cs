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
    public class AnnotationEllipseModel : IAnnotationModel
    {
        public Ellipse AnnotationEllipse { get; set; }

        public AnnotationEllipseModel(Ellipse ellipse)
        {
            this.AnnotationEllipse = ellipse;
        }

        public void FinalizeAnnotationStyle() { }

    }
}
