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
    public sealed class PatternRemoveMenu : BaseContextMenu
    {
        /// <summary>
        /// A reference to the parent UI's PatternTreeView component
        /// </summary>
        internal TreeView _tree;

        /// <summary>
        /// Default Constructor that takes in a pattern tree view structure
        /// </summary>
        /// <param name="tree"></param>
        public PatternRemoveMenu(TreeView tree)
        {
            _tree = tree;
        }
         
        void CancelContextMenu(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        protected override FrameworkElement GetContent()
        {
            int totalHeight = 0;
            ListBox removeOptions = new ListBox();

            for(int i = 0 ; i < _tree.Items.Count; i ++)
            
                if (_tree.Items[i] is PatternBase)
                {
                    totalHeight += 25; //default height of an item
                    TextBlock pattern = new TextBlock() 
                    {
                        Text = (_tree.Items[i] as PatternBase).BioPatternName,
                        Name = i.ToString(), //store the id as name so we can remove it later...
                        Width = 150
                    };

                    pattern.MouseLeftButtonUp += new MouseButtonEventHandler(RemoveSelectedPattern);
                    removeOptions.Items.Add(pattern);
                }

            Grid grid = new Grid() 
            { 
                Height = totalHeight,
                Width = 170
            };
            grid.Children.Add(removeOptions);
            return grid;
        }

        void RemoveSelectedPattern(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock)
            {
                int index = int.Parse((sender as TextBlock).Name);
                _tree.Items.RemoveAt(index);
            }

            Close();

        }

      
    }
}
