using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace BioPatMLEditor.PatternControls.ContextMenu
{
    /// <summary>
    /// This is an abstract class for all pop up menus in the pattern structure box.
    /// </summary>
    public abstract class BaseContextMenu
    {
        private Point _location;
        private bool _isShowing;
        private Popup _popup;
        protected Grid _grid;
        private Canvas _canvas;
        private FrameworkElement _content;

        /// <summary>
        /// Displays the context menu.
        /// </summary>
        /// <param name="location">The X and Y axis of users mouse cursor</param>
        /// <param name="additionalX">Leave this value as default if no special x-axis adjustment is needed</param>
        /// <param name="additionalY">Leave this value as default if no special y-axis adjustment is needed</param>
        public void Show(Point location, int additionalX, int additionalY)
        {
            if (_isShowing)
                throw new InvalidOperationException();

            _isShowing = true;
   
            location.X += additionalX;
            location.Y += additionalY;
            
            _location = location;
            EnsurePopup();
            _popup.IsOpen = true;
        }

        /// <summary>
        /// Shuts down the context menu
        /// </summary>
        public void Close()
        {
            _isShowing = false;

            if (_popup != null)
                _popup.IsOpen = false;

        }

        protected abstract FrameworkElement GetContent();

        internal virtual void OnClickOutside() { Close(); }

        private void EnsurePopup()
        {
            if (_popup != null)
                return;

            _popup = new Popup();
            _grid = new Grid();

            _popup.Child = _grid;

            _canvas = new Canvas();

            _canvas.MouseLeftButtonDown += (sender, args) => { OnClickOutside(); };
            _canvas.MouseRightButtonDown += (sender, args) => { args.Handled = true; OnClickOutside(); };

            _canvas.Background = new SolidColorBrush(Colors.Transparent);

            _grid.Children.Add(_canvas);

            _content = GetContent();

            _content.HorizontalAlignment = HorizontalAlignment.Left;
            _content.VerticalAlignment = VerticalAlignment.Top;
            _content.Margin = new Thickness(_location.X, _location.Y, 0, 0);


            _grid.Children.Add(_content);

            UpdateSize();
        }

        private void OnPluginSizeChanged(object sender, EventArgs e)
        {
            UpdateSize();
        }

        private void UpdateSize()
        {
            _grid.Width = Application.Current.Host.Content.ActualWidth;
            _grid.Height = Application.Current.Host.Content.ActualHeight;

            if (_canvas != null)
            {
                _canvas.Width = _grid.Width;
                _canvas.Height = _grid.Height;
            }
        }

    }
}
