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
using BioPatMLEditor.PatternControls.PatternModels;

namespace BioPatMLEditor.PatternControls.ContextMenu
{
    /// <summary>
    /// The entry menu which contains options like Add/Remove/Implement and Clear all pattern.
    /// </summary>
    public sealed class MainMenu : BaseContextMenu
    {
        /// <summary>
        /// A reference to our Pattern View Tree (The current Pattern tree)
        /// </summary>
        internal TreeView _tree;

        /// <summary>
        /// The Parent UI of our Context Menu
        /// </summary>
        PatternBuilderPanel parent;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="parent">The Parent UI that Calls this class</param>
        public MainMenu(PatternBuilderPanel parent)
        {
            this.parent = parent;
            _tree = parent.PatternViewTree; //just a easy reference to our tree.
        }

        internal void CancelContextMenu(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Builds a grid with the following options "Add" / Remove / Implement and Clear all
        /// options.
        /// </summary>
        /// <returns>Returns a grid.</returns>
        protected override FrameworkElement GetContent()
        {
            Grid grid = new Grid() { Width = 110, Height = 115 };
            Border border = new Border() { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(3), Background = new SolidColorBrush(Colors.LightGray) };
            //grid.Children.Add(border);

            StackPanel sp = new StackPanel()
            {
                Background = new SolidColorBrush(Colors.LightGray),
                Orientation = Orientation.Vertical,
                Width = 110,
                Height = 115 
            };

            ListBox options = new ListBox();

            //We only allow adding of sub pattern if parent pattern is a set/series
            //otherwise if the tree is currently empty
            if (CanAdd())
            {
                TextBlock add = new TextBlock() { Text = "Add Pattern", Width = 100 };
                add.MouseLeftButtonUp += new MouseButtonEventHandler(AddPattern);
                options.Items.Add(add);
            }

            TextBlock remove = new TextBlock() { Text = "Remove Pattern", Width = 100 };
            remove.MouseLeftButtonUp += new MouseButtonEventHandler(RemovePattern);

            TextBlock implement = new TextBlock() { Text = "Implement", Width = 100 };
            implement.MouseLeftButtonUp += new MouseButtonEventHandler(ImplementPattern);

            TextBlock clear = new TextBlock() { Text = "Clear All", Width = 100 };
            clear.MouseLeftButtonUp += new MouseButtonEventHandler(ClearAllPattern);

            TextBlock cancel = new TextBlock() { Text = "Cancel Menu", Width = 100 };
            cancel.MouseLeftButtonUp += new MouseButtonEventHandler(CancelContextMenu);

           
            options.Items.Add(remove);
            options.Items.Add(implement);
            options.Items.Add(clear);
            options.Items.Add(cancel);

            sp.Children.Add(options);

            return sp;
        }

        private bool CanAdd()
        {
            if (_tree.Items.Count < 2)
                return true;

            //Scenario 1: First item is Set or Series
            if (_tree.Items[1] is Series || 
                _tree.Items[1] is Set)
                return true;

            //other scenario we need not consider
            return false;
        }

        void ClearAllPattern(object sender, MouseButtonEventArgs e)
        {
            //we want to preserve our heading item thus make a temp copy of it first
            //and repopulate it back when the tree is cleared.
            Object template = _tree.Items[0];
            _tree.Items.Clear();
            _tree.Items.Add(template);
            Close();
        }

        void ImplementPattern(object sender, MouseButtonEventArgs e)
        {
            parent.ImplementBioPatML();
            Close();
        }

        void AddPattern(object sender, MouseButtonEventArgs e)
        {
            PatternAddMenu menu = new PatternAddMenu(_tree);
            menu.Show(e.GetPosition(_grid), 0, -130);
            Close();
        }

        void RemovePattern(object sender, MouseButtonEventArgs e)
        {
            PatternRemoveMenu menu = new PatternRemoveMenu(_tree);
            menu.Show(e.GetPosition(_grid), 0,0);
            Close();
        }
    }
}
