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
using MQuter_eLabApp.View;
using MQuter_eLabApp.ViewModel;

namespace MQuter_eLabApp.Model
{
    /// <summary>
    /// The Annotation Manager is the key class that controls drawing
    /// of annotation. It also keeps track of a list of annotations available in
    /// the canvas.
    /// </summary>
    public class AnnotationManager
    {
        private List<IAnnotationModel> annotationModels = new List<IAnnotationModel>();
      
        public bool IsAnnotating { get; set; }

        public bool IsSelectingAnnotation { get; set; }

        public IAnnotationModel AnnotationSubject 
        { 
            get;
            set;
        }

        /// <summary>
        /// The drawing Canvas
        /// </summary>
        private Canvas BlackBoard { get; set; }

        public Point startLocation;

        private Point currMseLocation;

        public AnnotationManager(Canvas blackboard)
        {
            this.BlackBoard = blackboard;
        }

        public Point MseLocation
        {
            get { return currMseLocation; }
            
            set
            {
                Point prevLocation = currMseLocation;
                currMseLocation = value;

                if (IsAnnotating)
                {
                    if (AnnotationSubject is AnnotationFreeHandModel)
                    {
                        UpdateFreeHandDrawing(AnnotationSubject as AnnotationFreeHandModel);
                    }
                    else if (AnnotationSubject is AnnotationRectangleModel)
                    {
                        AnnotationRectangleModel rectModel = AnnotationSubject as AnnotationRectangleModel;
                        UpdateShape(rectModel.AnnotationRectangle, prevLocation);
                    }
                    else if (AnnotationSubject is AnnotationEllipseModel)
                    {
                        AnnotationEllipseModel ellipseModel = AnnotationSubject as AnnotationEllipseModel;
                        UpdateShape(ellipseModel.AnnotationEllipse, prevLocation);
                    }
                    else if (AnnotationSubject is AnnotationStickyModel)
                    {
                         UpdateStickyNote(AnnotationSubject as AnnotationStickyModel);
                    }
                }

                else //bugged method
                    if (IsSelectingAnnotation)
                    {
                        if (AnnotationSubject == null)
                        {
                            AnnotationSubject = GrabSelectedAnnotation();
                            UpdateAnnotationSubjCoordinates(currMseLocation, prevLocation);
                        }

                        //Update the annotation subject to its new position
                        
                    }
            }
        }

        /// <summary>
        /// This method is bugged
        /// </summary>
        /// <param name="currLocation"></param>
        /// <param name="prevLocation"></param>
        private void UpdateAnnotationSubjCoordinates(Point currLocation, Point prevLocation)
        {
            if (AnnotationSubject is AnnotationFreeHandModel)
            {
            }
            else if (AnnotationSubject is AnnotationEllipseModel)
            {
                AnnotationEllipseModel ellipseModel = AnnotationSubject as AnnotationEllipseModel;
                Point currElementPos = new Point(Canvas.GetLeft(ellipseModel.AnnotationEllipse), Canvas.GetTop(ellipseModel.AnnotationEllipse));

                Point shiftedLocation = UIHelper.CalcPositionShifted(prevLocation, currMseLocation);
                Canvas.SetLeft(ellipseModel.AnnotationEllipse, currElementPos.X + shiftedLocation.X);
                Canvas.SetTop(ellipseModel.AnnotationEllipse, currElementPos .Y + shiftedLocation.Y); 
            }
            else if (AnnotationSubject is AnnotationRectangleModel)
            {
            }
            else if (AnnotationSubject is AnnotationFreeHandModel)
            {
            }
        }

        private IAnnotationModel GrabSelectedAnnotation()
        {
            foreach (FrameworkElement element in UIHelper.GetElementsAtHostCoordinates(currMseLocation, BlackBoard))
            {
                if (element.DataContext is IAnnotationModel)
                    return element.DataContext as IAnnotationModel;
            }

            return null;
        }


        private void UpdateStickyNote(AnnotationStickyModel sticky)
        {
            if (!BlackBoard.Children.Contains(sticky.annotationText)) //Bugged seems like it can't detect whether there already exist an annotation
            {
                SetCanvasDependency(sticky.annotationText, startLocation);
                BlackBoard.Children.Add(sticky.annotationText);
                sticky.annotationText.DataContext = sticky;
            }

            Point shiftedLocation = UIHelper.CalcPositionShifted(startLocation, currMseLocation);
            
            //Update X location
            if (shiftedLocation.X > -1)
            {
                sticky.annotationText.Width = shiftedLocation.X;
            }
            else
            {
                shiftedLocation.X = Math.Abs(shiftedLocation.X);
                Canvas.SetLeft(sticky.annotationText, startLocation.X - shiftedLocation.X); //Math.Abs(shiftedLocation.X));
                sticky.annotationText.Width = (shiftedLocation.X);
            }

            if (shiftedLocation.Y > -1)
            {
                sticky.annotationText.Height = shiftedLocation.Y;
            }
            else
            {
                shiftedLocation.Y = Math.Abs(shiftedLocation.Y);
                Canvas.SetTop(sticky.annotationText, startLocation.Y - shiftedLocation.Y); //Math.Abs(shiftedLocation.X));
                sticky.annotationText.Height = (shiftedLocation.Y);
            }
        }

        private void UpdateShape(Shape shape, Point prevLocation)
        {
            if (!BlackBoard.Children.Contains(shape))
            {
                SetCanvasDependency(shape, startLocation);
                BlackBoard.Children.Add(shape);
                shape.DataContext = AnnotationSubject;
            }

            Point shiftedLocation = UIHelper.CalcPositionShifted(startLocation, currMseLocation);
            

            //Update X location
            if (shiftedLocation.X > -1)
            {
                shape.Width = shiftedLocation.X;
            }
            else
            {
                shiftedLocation.X = Math.Abs(shiftedLocation.X);
                Canvas.SetLeft(shape, startLocation.X - shiftedLocation.X); //Math.Abs(shiftedLocation.X));
                shape.Width = (shiftedLocation.X);
            }

            if (shiftedLocation.Y > -1)
            {
                shape.Height = shiftedLocation.Y;
            }
            else 
            {
                shiftedLocation.Y = Math.Abs(shiftedLocation.Y);
                Canvas.SetTop(shape, startLocation.Y - shiftedLocation.Y); //Math.Abs(shiftedLocation.X));
                shape.Height = (shiftedLocation.Y);
            }

        }

        public void FinalizeAnnotationState()
        {
            if (AnnotationSubject is AnnotationStickyModel)
            {
                AnnotationStickyModel stickyNote = AnnotationSubject as AnnotationStickyModel;
                stickyNote.FinalizeAnnotationStyle();
            }
        }

        private void SetCanvasDependency(DependencyObject shape, Point location)
        {
            shape.SetValue(Canvas.LeftProperty, location.X);
            shape.SetValue(Canvas.TopProperty, location.Y);
            shape.SetValue(Canvas.ZIndexProperty, 1);
        }

        private void UpdateFreeHandDrawing(AnnotationFreeHandModel model)
        {

            Line line = new Line() 
            {
                X1 = startLocation.X,
                Y1 = startLocation.Y,
                X2 = currMseLocation.X,
                Y2 = currMseLocation.Y,
                DataContext = model
            };
 
            startLocation = currMseLocation;

            model.AddCoordinate(line);
            BlackBoard.Children.Add(line);
        }
       
    }
}
