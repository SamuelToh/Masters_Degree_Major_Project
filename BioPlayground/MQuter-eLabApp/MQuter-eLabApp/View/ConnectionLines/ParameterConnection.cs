namespace MQuter_eLabApp.View.ConnectionLines
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
    using MQuter_eLabApp.ViewModel;

    #endregion

    public class ParameterConnection : BaseConnection
    {
        #region Constructor

        public delegate void ParamConnectionEventHandler(object sender, EventArgs e);

        public event ParamConnectionEventHandler LineDeletionEvent;

        /// <summary>
        /// This will be triggered once the parameter connection line is deleted, basically
        /// is to tell the connected parameter model to clear its parambinder object.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLineDeleted(EventArgs e)
        {
            if (LineDeletionEvent != null)
            {
                LineDeletionEvent(this, e);
            }
        }


        public ParameterConnection(Point startPtr)
            : base(startPtr)
        {
            StrokeColor = Colors.White;
            Loaded += new RoutedEventHandler(ParameterConnection_Loaded);
        }

        void ParameterConnection_Loaded(object sender, RoutedEventArgs e)
        {
            Connection.MouseLeftButtonDown += new MouseButtonEventHandler(Connection_MouseLeftButtonDown);
        }

        void Connection_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StrokeColor = Colors.Red;
            Connection.StrokeThickness = 3.0;
            Connection.Stroke = new SolidColorBrush(StrokeColor);
        }

        #endregion

        #region Public Method

        public void FinalizeLine()
        {
            Canvas.SetZIndex(this, 1);

        }

        public new void DeleteLine()
        {
            base.DeleteLine();
            OnLineDeleted(new EventArgs());
        }

        #endregion
    }
}
