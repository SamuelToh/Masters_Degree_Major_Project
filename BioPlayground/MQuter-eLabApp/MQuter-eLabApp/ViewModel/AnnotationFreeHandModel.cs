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

namespace MQuter_eLabApp.ViewModel
{
    public class AnnotationFreeHandModel : IAnnotationModel
    {
        private List<Line> freeHandCoordinates = new List<Line>();
        public Color StrokeColor { get; set; }

        public AnnotationFreeHandModel()
        {
            StrokeColor = (Colors.White);
            //StrokeThickness = 15.0;
        }

        public AnnotationFreeHandModel(Line initializationPtr)
            : this()
        {
            freeHandCoordinates.Add(initializationPtr);
        }

        private void ConfigureLineAttr(Line line)
        {
            line.Stroke = new SolidColorBrush(StrokeColor);
            line.StrokeThickness = 2.0;
        }

        public void AddCoordinate(Line line)
        {
            ConfigureLineAttr(line);
            this.Coordinates.Add(line);
        }

        public List<Line> Coordinates
        {
            get
            {
                return freeHandCoordinates;
            }

            set
            {
                freeHandCoordinates = value;
            }
        }

        public void UpdateDrawingColor(Color color)
        {
            foreach (Line line in freeHandCoordinates)
            {
                line.Stroke = (new SolidColorBrush(StrokeColor));
            }
        }

        public void FinalizeAnnotationStyle() { }
    }
}
