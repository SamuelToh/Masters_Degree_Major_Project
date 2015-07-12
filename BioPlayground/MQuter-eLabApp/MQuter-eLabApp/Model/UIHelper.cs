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

namespace MQuter_eLabApp.Model
{
    public class UIHelper
    {
        public static IEnumerable<UIElement> GetElementsAtHostCoordinates
                                            (Point location, UIElement uiComponent)
        {
            //The API FindElementsInHostCoordinates in SL work based on global system coordinate
            GeneralTransform generalTransform = uiComponent.TransformToVisual(Application.Current.RootVisual);
            Point pnts = generalTransform.Transform(location);
            IEnumerable<UIElement> elements = VisualTreeHelper.FindElementsInHostCoordinates(pnts, Application.Current.RootVisual);
            return elements;
        }

        public static void UpdateCompLocation(UserControl control, Point location, int Z)
        {
            Canvas.SetTop(control, location.Y);
            Canvas.SetLeft(control, location.X);
            Canvas.SetZIndex(control, Z);
        }

        public static Point CalculateElementPos
            (Point newMousePos, Point oldMousePos, FrameworkElement fe)
        {
            //We use the new position substract the old mouse position to find its X, & Y 
            //Increment and then we calculate where the object should be.
            double deltaV = newMousePos.Y - oldMousePos.Y;
            double deltaH = newMousePos.X - oldMousePos.X;
            double newTop = deltaV + (double)fe.GetValue(Canvas.TopProperty);
            double newLeft = deltaH + (double)fe.GetValue(Canvas.LeftProperty);

            return new Point(newLeft, newTop);
        }

        public static Point CalculateElementPos
           (Point newMousePos, Point oldMousePos,
            MQuter_eLabApp.View.Components.Activity.IActivityComponent ActivityComp)
        {
            //We use the new position substract the old mouse position to find its X, & Y 
            //Increment and then we calculate where the object should be.
            double deltaV = newMousePos.Y - oldMousePos.Y;
            double deltaH = newMousePos.X - oldMousePos.X;

            Point ActivityPosition = ActivityComp.GetComponentPosition();

            double newTop = deltaV + ActivityPosition.Y;
            double newLeft = deltaH + ActivityPosition.X;

            return new Point(newLeft, newTop);
        }

        public static Point CalcPositionShifted(Point prevLoc, Point currLoc)
        {
            Point shiftedLocation = new Point();
            shiftedLocation.X = currLoc.X - prevLoc.X;
            shiftedLocation.Y = currLoc.Y - prevLoc.Y;

            return shiftedLocation;
        }


   
    }
}
