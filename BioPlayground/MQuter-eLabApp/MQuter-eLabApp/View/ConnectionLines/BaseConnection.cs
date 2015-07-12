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

namespace MQuter_eLabApp.View.ConnectionLines
{
    public class BaseConnection : Canvas
    {
        #region Internal variables

        internal Color StrokeColor;
        internal Line Connection;
        internal Point StartPoint, EndPoint;

        #endregion 

        #region Constructors

        public BaseConnection (Point startPtr)
        {
            Loaded += new RoutedEventHandler(ConnectionLine_Loaded);
            StrokeColor = Colors.White;
            StartPoint = startPtr;
            EndPoint = startPtr;
        }

        #endregion Constructors


        #region Event Handlers

        void ConnectionLine_Loaded(object sender, RoutedEventArgs e)
        {
            Connection = new Line();
            Connection.Stroke = new SolidColorBrush(StrokeColor);
            Connection.StrokeThickness = 4.0;
            Connection.X1 = StartPoint.X;
            Connection.Y1 = StartPoint.Y;
            Connection.X2 = EndPoint.X;
            Connection.Y2 = EndPoint.Y;
            Panel parentPanel = (Panel)Parent;
            parentPanel.Children.Add(Connection); //add the line obj to parent (canvas)
        }

        #endregion

        #region Public Methods

        public Color LineColor
        {
            get { return this.StrokeColor; }
            set { this.StrokeColor = value; }
        }

        public void IncStartLocation(double X, double Y)
        {
            Connection.X1 += X;
            Connection.Y1 += Y;
            StartPoint.X = Connection.X1;
            StartPoint.Y = Connection.Y1;
        }

        public void IncEndLocation(double X, double Y)
        {
            Connection.X2 += X;
            Connection.Y2 += Y;
            EndPoint.X = Connection.X2;
            EndPoint.Y = Connection.Y2;
        }

        public void Redraw(Point inEndPoint)
        {
            EndPoint = inEndPoint;
            Connection.X2 = EndPoint.X;
            Connection.Y2 = EndPoint.Y;
        }

        public void Redraw(Point inStartPoint, Point inEndPoint)
        {
            StartPoint = inStartPoint;
            EndPoint = inEndPoint;
            Connection.X1 = StartPoint.X;
            Connection.Y1 = StartPoint.Y;
            Connection.X2 = EndPoint.X;
            Connection.Y2 = EndPoint.Y;
        }

        public void DeleteLine()
        {
            //A simple way to destroy the line is to shift it somewhere
            //off screen and then dereference it from all component till garabage
            //collection happens... 
            //this.Redraw(new Point(1000, 1000), new Point(1000,1000));
            this.Connection.Visibility = System.Windows.Visibility.Collapsed;
            Panel parentPanel = (Panel)Parent;
            System.Diagnostics.Debug.WriteLine(Parent);

            if(parentPanel != null)
                parentPanel.Children.Remove(this);

            
            //parentPanel.Children.Add(this);
        }

        #endregion Public Methods
    }
}
