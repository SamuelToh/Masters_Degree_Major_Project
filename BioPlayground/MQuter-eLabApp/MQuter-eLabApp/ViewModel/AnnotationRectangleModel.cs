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
    public class AnnotationRectangleModel : IAnnotationModel
    {
        public Rectangle AnnotationRectangle { get; set; }

        public AnnotationRectangleModel(Rectangle rectangle)
        {
            this.AnnotationRectangle = rectangle;
        }

        public void FinalizeAnnotationStyle() { }

    }
}
