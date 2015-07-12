using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MQuter_eLabApp.Model;
using MQuter_eLabApp.ViewModel;
using MQuter_eLabApp.Events;
using System.Windows.Controls.Primitives;

namespace MQuter_eLabApp.View.MenuBar
{
    public partial class AnnotationToolbar : UserControl
    {
        private Point onClickLocation { get; set; }
       
        #region Events

        public delegate void AnnoateEventHandler(object sender, EventArgs e);
        public delegate void SelectEventHandler(object sender);

        public event AnnoateEventHandler AddAnnotation;
        public event SelectEventHandler SelectAnnotation;

        protected virtual void OnNewAnnotation(EventArgs e)
        {
            if (AddAnnotation != null)
                AddAnnotation(this, e);
        }

        protected virtual void OnSelectAnnotation(object sender)
        {
            if (SelectAnnotation != null)
                SelectAnnotation(this);
        }

        #endregion

        /// <summary>
        /// The name of component where you want to draw on.
        /// </summary>
        public String CanvasName { get; set; }

        public AnnotationToolbar()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton clickedBtn = sender as ToggleButton;

            if (clickedBtn.Name == "btnSelectDrawings")
            {
                MessageBox.Show("Shifting of annotation item not supported yet.");
                //OnSelectAnnotation(this);
            }

            else if (clickedBtn.Name == "btnFreeHandDrawing")
            {
                OnNewAnnotation
                    (new AnnotationEventArgs
                        (new AnnotationFreeHandModel()));
            }

            else if (clickedBtn.Name == "btnRectangleDrawing")
            {
                OnNewAnnotation
                    (new AnnotationEventArgs
                        (new AnnotationRectangleModel(
                                (new Rectangle()
                                {
                                    Stroke = new SolidColorBrush(Colors.Red), //Colors should be dynamic
                                    StrokeThickness = 2.5,
                                    Width = 0,
                                    Height = 0.0
                                }))));
            }

            else if (clickedBtn.Name == "btnEllipseDrawing")
            {
                OnNewAnnotation
                    (new AnnotationEventArgs
                        (new AnnotationEllipseModel(
                                (new Ellipse()
                                {
                                    Stroke = new SolidColorBrush(Colors.Blue), //Colors should be dynamic
                                    StrokeThickness = 2.5,
                                    Width = 0,
                                    Height = 0.0
                                }))));
            }

            else if (clickedBtn.Name == "btnStickyNote")
            {
                OnNewAnnotation
                    (new AnnotationEventArgs
                                (new AnnotationStickyModel()
                                {
                                    //Textbox does not allows color configuration 
                                    StrokeThickness = 2.5,
                                    Width = 0,
                                    Height = 0.0
                                }));
            }

            else if (clickedBtn.Name == "btnChangeColor")
            {
            }
        }

       

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            Button trigger = sender as Button;

            if (trigger.Content.ToString() == "Hide")
            {
                HideMenu.Begin();   
            }
            else
            {
                ShowMenu.Begin();
            }
        }

        #region The enclose contains logic for shifting the entire control; basically all the mouse events

        private void HorizontalTemplate_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Opacity = 0.5;

            onClickLocation = 
                             new Point(e.GetPosition(null).X,
                                       e.GetPosition(null).Y);

            this.HorizontalTemplate.CaptureMouse();
        }

        private void HorizontalTemplate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Opacity = 1;
            this.HorizontalTemplate.ReleaseMouseCapture();
        }

        private void HorizontalTemplate_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.HorizontalTemplate.CaptureMouse())
            {
                Point newPosition = new Point(e.GetPosition(null).X,
                                              e.GetPosition(null).Y);

                Point positionMoved =
                    new Point(newPosition.X - onClickLocation.X,
                              newPosition.Y - onClickLocation.Y);

                onClickLocation = newPosition;
                
                double currX = Convert.ToDouble(this.GetValue(Canvas.LeftProperty));
                double currY = Convert.ToDouble(this.GetValue(Canvas.TopProperty));

                Double coordinateX =
                    (Convert.ToDouble(GetValue(Canvas.LeftProperty)) + positionMoved.X);

                Double coordinateY =
                   (Convert.ToDouble(GetValue(Canvas.TopProperty)) + positionMoved.Y);

                this.SetValue(Canvas.LeftProperty, coordinateX);
                this.SetValue(Canvas.TopProperty, coordinateY);
            }
        }

        #endregion
    }
}
