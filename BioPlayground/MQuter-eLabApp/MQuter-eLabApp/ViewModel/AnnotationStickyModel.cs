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
    public class AnnotationStickyModel : IAnnotationModel
    {
        private TextBox annotation = new TextBox();
        static int counter = 0;
        public double StrokeThickness { get; set; }
        
        public double Width 
        {
            get
            {
                return annotationText.Width;
            }
            set
            {
                annotationText.Width = value;
            }
        }
        
        
        public double Height 
        {
            get
            {
                return annotationText.Height;
            }
            set
            {
                annotationText.Height = value;
            }    
        }

        public AnnotationStickyModel()
        {
            annotationText = new TextBox()
            {
                 Name = "AnnotationNote" + counter++,
                 Width = 15.0,
                 Height = 20,
                 Text = "Edit your annotation here.",
                 AcceptsReturn = true,
                 Background = new SolidColorBrush(Colors.Transparent),
                 Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xF5, 0x9E, 0x02)),
                 FontSize = 15,
                 FontFamily = new FontFamily("Andalus"),
                 CaretBrush = new SolidColorBrush(Colors.White)
            };
        }

        public void FinalizeAnnotationStyle()
        {
            annotationText.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        public TextBox annotationText
        {
            get
            {
                return annotation;
            }

            set
            {
                annotation = value;
            }
        }

      
    }
}
