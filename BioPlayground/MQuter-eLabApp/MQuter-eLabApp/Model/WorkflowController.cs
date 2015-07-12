namespace MQuter_eLabApp.Model
{
    #region Directives

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
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media.Imaging;
    using MQuter_eLabApp.View;
    using MQuter_eLabApp.ViewModel;
    using MQuter_eLabApp.Events;
    using MQuter_eLabApp.View.ConnectionLines;
    using MQuter_eLabApp.View.Components.Activity;

#if SILVERLIGHT
    using Microsoft.Windows;
    using SW = Microsoft.Windows;
#else
        using SW = System.Windows;
#endif

    #endregion

    /// <summary>
    /// TODO: documentation
    /// </summary>
    public class WorkflowController : DragDropTarget<Canvas, UIElement>
    {
        #region Events

        public delegate void ActivityLinkageEventHandler(object sender, EventArgs e);
        public delegate void ActivityChangedEventHandler(object sender, EventArgs e);
        public delegate void ActivityNestingEventHandler(object sender, EventArgs e);

        public event ActivityChangedEventHandler ActivityChanged;
        public event ActivityNestingEventHandler ActivityParentChanged;
        public event ActivityLinkageEventHandler ConnectionChanged;

        // Invoke the Changed event; called whenever activities on canvas changes:
        protected virtual void OnActivityChanged(EventArgs e)
        {
            if (ActivityChanged != null)
                ActivityChanged(this, e);
        }

        protected virtual void OnActivityParentChanged(EventArgs e)
        {
            if (ActivityParentChanged != null)
                ActivityParentChanged(this, e);
        }

        protected virtual void OnConnectionChanged(EventArgs e)
        {
            if (ConnectionChanged != null)
                ConnectionChanged(this, e);
        }

        #endregion


        static int counter = 0;
        public Point MseLocation { get; set; }
        public Dictionary<String, ActivityComponent> Activities = new Dictionary<string, ActivityComponent>();

        protected override void OnDrop(SW.DragEventArgs args)
        {
            //Only called when over an existing child item
            args.Handled = true;

            Canvas dropTarget = GetDropTarget(args);


            SelectionCollection selectionCollection = GetSelectionCollection(args.Data.GetData(args.Data.GetFormats()[0]));

            foreach (Selection selection in selectionCollection.Reverse())
            {
                if (CanAddItem(dropTarget, selection.Item))
                {
                    AddItem(dropTarget, selection.Item);
                }
            }
        }

        private SelectionCollection GetSelectionCollection(object data)
        {
            ItemDragEventArgs args = data as ItemDragEventArgs;
            if (args != null)
                data = args.Data;

            return ToSelectionCollection(data);
        }

        private SelectionCollection ToSelectionCollection(object data)
        {
            if (data == null)
            {
                return new SelectionCollection();
            }

            SelectionCollection selectionCollection = data as SelectionCollection;
            if (selectionCollection == null)
            {
                selectionCollection = new SelectionCollection();
                Selection selection = data as Selection;
                if (selection != null)
                {
                    selection = new Selection(data);
                }

                selectionCollection.Add(selection);
            }

            return selectionCollection;
        }

        protected override void AddItem(Canvas itemsControl, object data)
        {
            Canvas container = itemsControl as Canvas;

            if (data is ActivityModel)
            {
                ActivityModel dataModel = data as ActivityModel;
                string componentName = dataModel.DisplayLabel + "_" + counter++;
                UserControl component = null;

                switch (dataModel.ActivityClassification)
                {
                    case ActivityModel.ActivityType.Standard:
                        {
                            component = new ActivityComponent()
                            {
                                Name = componentName,
                                DataContext = dataModel.CloneModel(componentName) //clone with installation of the unique name 
                            };

                            //Wire up deletion event
                            (component as ActivityComponent).ComponentDeleted 
                                += new ActivityComponent.ActivityDeletedEventHandler(WorkflowController_ComponentDeleted);

                            break;
                        }

                    case ActivityModel.ActivityType.ForLoop:
                        {
                            component = new ForLoopComponent()
                            {
                                Name = componentName,
                                DataContext = dataModel.CloneModel(componentName) //clone with installation of the unique name 
                            };
                            break;
                        }
                }

                //Component on move event
                component.MouseLeftButtonDown += new MouseButtonEventHandler(component_MouseLeftButtonDown);
                component.MouseLeftButtonUp += new MouseButtonEventHandler(component_MouseLeftButtonUp);
                component.MouseMove += new MouseEventHandler(component_MouseMove);
                

                UIHelper.UpdateCompLocation(component, MseLocation, 2);

                container.Children.Add(component);

                OnActivityChanged
                        (new ActivityEventArgs
                            (component.Name, component.DataContext as ActivityModel, true));
             
            }
        }

        void WorkflowController_ComponentDeleted(object sender, EventArgs e)
        {
            ActivityComponent component = sender as ActivityComponent;

            if(component != null)
                OnActivityChanged
                        (new ActivityEventArgs
                            (component.Name, component.DataContext as ActivityModel, false));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="self"></param>
        /// <param name="gate">Hack code written to retrieve the gate when connection is valid</param>
        /// <returns></returns>
        private bool IsValidConnection(Point location, IActivityComponent self, ref IOGateComponent gate)
        {
            Canvas parentCanvas = (self as UserControl).Parent as Canvas;

            foreach (UIElement ele in UIHelper.GetElementsAtHostCoordinates(location, parentCanvas))
                if (ele is IOGateComponent)
                    if ((ele as IOGateComponent).Name.Contains("InputGate")
                     && (ele as IOGateComponent).MasterControl != self)
                    {
                        gate = ele as IOGateComponent;
                        return true;
                    }

            return false;

        }

        /// <summary>
        /// This method should only be called when a connection is valid.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="location"></param>
        private void AddConnToTargetElement
            (ActivityConnection line, Point location, Canvas sourceCanvas, IOGateComponent targetGate, IOGateComponent sourceGate)
        {
            IEnumerable<UIElement> elements = UIHelper.GetElementsAtHostCoordinates
                                                                (location, sourceCanvas);

            foreach (UIElement uiElement in elements)
                if (uiElement is IActivityComponent)
                {
                    IActivityComponent component = uiElement as IActivityComponent;
                    component.InputConn.Add(line);
                    line.OutputSource = component; //Add target as a ref to line
                    bool IsAddingOfConnectionLine = true;

                    targetGate.GateStatus = IOGateComponent.Status.NeedsConfiguration;
                    sourceGate.GateStatus = IOGateComponent.Status.NeedsConfiguration;

                    if ((!targetGate.IsInnerGate) && (!sourceGate.IsInnerGate))
                    {
                        //Fire event on connection added.
                        OnConnectionChanged
                            (new ConnectionEventArgs
                                (line.InputSource.Name, line.OutputSource.Name, IsAddingOfConnectionLine));

                        //Wire up a connection delete event
                        line.ConnectionRemove += new ActivityConnection.OnConnRemovedEventHandler(line_ConnectionRemove);
                    }
                    break;
                }

        }

        void line_ConnectionRemove(object sender, EventArgs e)
        {
            OnConnectionChanged(e as ConnectionEventArgs);
        }

        #region Event Handlers 

        #region Mouse related event variables

        private bool isMouseCaptured;
        private double compVerticalPosition;
        private double compHorizontalPosition;

        /// <summary>
        /// A reference to the For loop component whom received focus recently.
        /// </summary>
        private ForLoopComponent refForcusedLoopComponent;

        #endregion

        private ForLoopComponent HasComplexComponentAtHostCoord(Point location, Canvas parentCanvas)
        {
            foreach (UIElement ele in UIHelper.GetElementsAtHostCoordinates(location, parentCanvas))
                if (ele is ForLoopComponent)
                        return ele as ForLoopComponent;

            return null;
        }


        void component_MouseMove(object sender, MouseEventArgs e)
        {
            IActivityComponent item = sender as IActivityComponent;
            Canvas parentCanvas = (item as UserControl).Parent as Canvas;

            #region If - Drawing connection lines

            if (item.OutputGatesHasFocus())
            {
                Point currPos =
                    new Point(e.GetPosition(parentCanvas).X,
                              e.GetPosition(parentCanvas).Y);


                ActivityConnection line = item.GetLatestOutputLine();
                line.Redraw(currPos);
                //item.OutputConn.Redraw(currPos);

                return;
            }
            #endregion If - Drawing connection lines

            #region Else If - Has mouse capture for component (Shifting Component)

            if (isMouseCaptured)
            {
                #region If the moving component is not a complex type, we check for interception between standard activity and complex activity

                if (item is ActivityComponent)
                {
                    ForLoopComponent intersect = HasComplexComponentAtHostCoord(e.GetPosition(parentCanvas), parentCanvas);

                    if (intersect != null) //Intersects!
                    {
                        intersect.HasFocus = true;
                        refForcusedLoopComponent = intersect;
                    }
                    //Shifted out from previous intersection
                    else if (intersect == null && refForcusedLoopComponent != null)
                    {
                        refForcusedLoopComponent.HasFocus = false;
                        refForcusedLoopComponent = null;
                    }
                }
                else if (item is ForLoopComponent)
                {
                    if ((item as ForLoopComponent).IsAnchored)
                        return;
                }
                
                #endregion

                Point newMousePos = new Point(e.GetPosition(null).X, e.GetPosition(null).Y);
                Point oldMousePos = new Point(compHorizontalPosition, compVerticalPosition);
                Point itemPosition = UIHelper.CalculateElementPos(newMousePos, oldMousePos, item);

                // We check for "out of canvas" condition
                UserControl control = item as UserControl;

                #region Check for Element out of bound 
                bool isOutX = false, isOutY = false;

                if (itemPosition.X < parentCanvas.MinWidth) { isOutX = true; itemPosition.X = 0; }
                if (itemPosition.Y < parentCanvas.MinHeight) { isOutY = true;  itemPosition.Y = 0; }

                if (itemPosition.X + control.Width > parentCanvas.MaxWidth)
                    itemPosition.X = parentCanvas.MaxWidth - control.Width;
                
                if (itemPosition.Y + control.Height > parentCanvas.MaxHeight)
                    itemPosition.Y = parentCanvas.MaxHeight - control.Height;

                #endregion

                // Set new position of object.
                item.SetComponentPosition(itemPosition);

                #region If - Activity already has conn lines (Update them as well)

                foreach (ActivityConnection input in item.InputConn)
                {
                    if((item is ForLoopComponent) && (input.Parent as Canvas).Name != "WorkflowContainer")
                    {
                        continue;
                    }

                    double XInc = isOutX ? 0 : newMousePos.X - oldMousePos.X;
                    double YInc = isOutY ? 0 : newMousePos.Y - oldMousePos.Y;

                    input.IncEndLocation (XInc, YInc);
                }

                foreach (ActivityConnection output in item.OutputConn)
                {
                    if ((item is ForLoopComponent) && (output.Parent as Canvas).Name != "WorkflowContainer")
                    {
                        continue;
                    }

                    //Still didn't really fix the connection line prob
                    double XInc = isOutX ? 0 : newMousePos.X - oldMousePos.X;
                    double YInc = isOutY ? 0 : newMousePos.Y - oldMousePos.Y;

                    output.IncStartLocation(XInc, YInc);
                }

                #endregion

                compVerticalPosition = newMousePos.Y;
                compHorizontalPosition = newMousePos.X;
            }

            #endregion Else If - Has mouse capture for component (Shifting Component)
        }

        void component_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is IActivityComponent)
            {
                IActivityComponent item = sender as IActivityComponent;
                //item.HasFocus = true;

                #region If it is a complex component - do the following
                if (item is ForLoopComponent)
                {
                    refForcusedLoopComponent = item as ForLoopComponent;
                }
                #endregion

                //If output gate has focus (we are drawing lines)
                if (item.OutputGatesHasFocus())
                {
                    IOGateComponent forcusedGate = item.GetForcusedGate();

                    Canvas parentCanvas = (item as UserControl).Parent as Canvas;
                    Point currPos =
                         new Point(e.GetPosition(parentCanvas).X,
                                   e.GetPosition(parentCanvas).Y);

                    ActivityConnection conn = new ActivityConnection(item, currPos);
                    item.OutputConn.Add(conn);
                    Canvas.SetLeft(conn, currPos.X);
                    Canvas.SetTop(conn, currPos.Y);
                    Canvas.SetZIndex(conn, 1);


                    parentCanvas.Children.Add(conn);
                }
                else if (item.InputGateHasFocus())
                    return;

                compVerticalPosition = e.GetPosition(null).Y;
                compHorizontalPosition = e.GetPosition(null).X;
                isMouseCaptured = true;
                item.CaptureMouse();
            }
        }

        void component_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is IActivityComponent)
            {
                IActivityComponent item = sender as IActivityComponent;

                #region Drawing of connection line

                if (item.OutputGatesHasFocus())
                {
                    Canvas parentCanvas = (item as UserControl).Parent as Canvas;

                    Point currPos =
                         new Point(e.GetPosition(parentCanvas).X,
                                   e.GetPosition(parentCanvas).Y);

                    ActivityConnection connLine = item.GetLatestOutputLine();
                    IOGateComponent itemGate = null;

                    //Validate the collision 
                    if (!IsValidConnection(currPos, item, ref itemGate))
                    {
                        //Canvas parentCanvas = (item as UserControl).Parent as Canvas;
                        parentCanvas.Children.Remove(connLine);
                        connLine.DeleteLine();
                        item.OutputConn.Remove(connLine);
                    }
                    else
                    {
                        //Valid connection, add the line to the input obj
                        IOGateComponent sourceGate = item.GetForcusedGate();
                        AddConnToTargetElement(connLine, currPos, parentCanvas, itemGate, sourceGate);
                    }

                    item.GetForcusedGate().HasFocus = false;
                    //item.GetOutputGate.HasFocus = false; //check here
                }

                #endregion

                isMouseCaptured = false;
                //item.HasFocus = false;
                item.ReleaseMouse();
                compVerticalPosition = -1;
                compHorizontalPosition = -1;

                #region Check whether there is any referenced for-loop component that has focus
                
                if (refForcusedLoopComponent != null && item is ActivityComponent)
                {
                    //Add to canvas...
                    Canvas parentCanvas = (item as UserControl).Parent as Canvas;

                    if (refForcusedLoopComponent.ForLoopInnerCanvas != parentCanvas)
                    {
                        parentCanvas.Children.Remove(item as UserControl);
                        refForcusedLoopComponent.AddActivityToInnerCanvas(item as UserControl, e);

                        OnActivityParentChanged
                                (new NestedActivityEventArgs
                                    (refForcusedLoopComponent.Name, item.Name));

                    }
                    
                    //WorkflowContainer.Children.Remove(item as UserControl);
                    
                    //refForcusedLoopComponent.HasFocus = false;
                    refForcusedLoopComponent = null;
                }
                else if (item is ForLoopComponent)
                {
                    //(item as ForLoopComponent).HasFocus = false;
                    refForcusedLoopComponent = null;
                }
                
                #endregion
            }
        }

        #endregion

        #region Interface implementations

        protected override void OnDragOver(SW.DragEventArgs e)
        {
            //Only called when over an existing child item
            base.OnDragOver(e);
        }

        protected override void OnGiveFeedback(SW.GiveFeedbackEventArgs args)
        {
            //Only called when over an existing child item
            base.OnGiveFeedback(args);
        }


        protected override void OnDragEnter(SW.DragEventArgs args)
        {
            //Only called when over an existing child item
            base.OnDragEnter(args);
        }

        protected override void OnDragEvent(Microsoft.Windows.DragEventArgs args)
        {
            //base.OnDragEvent(args);

            SW.DragDropEffects effects = args.AllowedEffects;

            //effects &= ~(SW.DragDropEffects.Move | SW.DragDropEffects.Link | SW.DragDropEffects.Copy);

            args.Effects = effects;
            args.Handled = true;

        }

        protected override bool CanAddItem(Canvas itemsControl, object data)
        {
            return true;
        }

        protected override bool CanRemove(Canvas itemsControl)
        {
            return false;
        }

        protected override UIElement ContainerFromIndex(Canvas itemsControl, int index)
        {
            return null;
        }

        protected override int GetItemCount(Canvas itemsControl)
        {
            return 0; // (itemsControl as BlockCanvas).ActivitiesContainer.Children.Count;
        }

        protected override Panel GetItemsHost(Canvas itemsControl)
        {
            return null; // (itemsControl as BlockCanvas).ActivitiesContainer; //itemsControl;
        }

        protected override int? IndexFromContainer(Canvas itemsControl, UIElement itemContainer)
        {
            throw new NotImplementedException();
        }

        protected override void InsertItem(Canvas itemsControl, int index, object data)
        {
            throw new NotImplementedException();
        }

        protected override bool IsItemContainerOfItemsControl(Canvas itemsControl, DependencyObject itemContainer)
        {
            //return true;
            return false;
        }

        protected override object ItemFromContainer(Canvas itemsControl, UIElement itemContainer)
        {
            throw new NotImplementedException();
        }

        protected override void RemoveItem(Canvas itemsControl, object data)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
