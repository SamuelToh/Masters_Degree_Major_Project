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
    /// This menu displays all the available patterns for add.
    /// </summary>
    public sealed class PatternAddMenu : BaseContextMenu
    {
        /// <summary>
        /// Tree structure that reference to our pattern view tree
        /// </summary>
        internal TreeView _tree;

        /// <summary>
        /// Default constructor for building the menu.
        /// </summary>
        /// <param name="tree"></param>
        public PatternAddMenu(TreeView tree)
        {
            _tree = tree;
        }

        internal void CancelContextMenu(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Builds a grid filled with our pattern models and sub headings.
        /// </summary>
        /// <returns>A grid with items and sub headings in it.</returns>
        protected override FrameworkElement GetContent()
        {
            Grid grid = new Grid() { Width = 130, Height = 430 };
            Border border = new Border() { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(1), Background = new SolidColorBrush(Colors.LightGray) };
            grid.Children.Add(border);

            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Vertical;

            #region "Regional Patterns Adding..."

            TextBlock HeaderRegional = new TextBlock() { Text = "Regional Patterns", Width = 110 };

            TextBlock any = new TextBlock() { Text = "Any", Width = 110 };
            any.MouseLeftButtonUp += new MouseButtonEventHandler(AddSelectedPattern);

            TextBlock gap = new TextBlock() { Text = "Gap", Width = 110 };
            gap.MouseLeftButtonUp += new MouseButtonEventHandler(AddSelectedPattern);

            TextBlock composite = new TextBlock() { Text = "Composition", Width = 110 };
            composite.MouseLeftButtonUp += new MouseButtonEventHandler(AddSelectedPattern);

            ListBox regOptions = new ListBox();

            regOptions.Items.Add(any);
            regOptions.Items.Add(gap);
            regOptions.Items.Add(composite);

            #endregion

            #region "Recursive Patterns Addings..."

            TextBlock HeaderRecursive = new TextBlock() { Text = "Recursive Patterns", Width = 110 };

            TextBlock block = new TextBlock() { Text = "Block", Width = 110 };
            block.MouseLeftButtonUp += new MouseButtonEventHandler(AddSelectedPattern);

            TextBlock regEx = new TextBlock() { Text = "RegEx", Width = 110 };
            regEx.MouseLeftButtonUp += new MouseButtonEventHandler(AddSelectedPattern);

            TextBlock motif = new TextBlock() { Text = "Motif", Width = 110 };
            motif.MouseLeftButtonUp += new MouseButtonEventHandler(AddSelectedPattern);

            TextBlock prosite = new TextBlock() { Text = "Prosite", Width = 110 };
            prosite.MouseLeftButtonUp += new MouseButtonEventHandler(AddSelectedPattern);

            TextBlock pwm = new TextBlock() { Text = "PWM", Width = 110 };
            pwm.MouseLeftButtonUp += new MouseButtonEventHandler(AddSelectedPattern);

            ListBox recursiveOptions = new ListBox();

            recursiveOptions.Items.Add(block);
            recursiveOptions.Items.Add(regEx);
            recursiveOptions.Items.Add(motif);
            recursiveOptions.Items.Add(prosite);
            recursiveOptions.Items.Add(pwm);

            #endregion

            #region "Structured Patterns Addings..."

            TextBlock HeaderStructured = new TextBlock() { Text = "Structured Patterns", Width = 110 };

            TextBlock set = new TextBlock() { Text = "Set", Width = 110 };
            set.MouseLeftButtonUp += new MouseButtonEventHandler(AddSelectedPattern);

            TextBlock series = new TextBlock() { Text = "Series", Width = 110 };
            series.MouseLeftButtonUp += new MouseButtonEventHandler(AddSelectedPattern);

            TextBlock iterate = new TextBlock() { Text = "Iteration", Width = 110 };
            iterate.MouseLeftButtonUp += new MouseButtonEventHandler(AddSelectedPattern);

            TextBlock repeat = new TextBlock() { Text = "Repeat", Width = 110 };
            repeat.MouseLeftButtonUp += new MouseButtonEventHandler(AddSelectedPattern);

            TextBlock logic = new TextBlock() { Text = "Logic", Width = 110 };
            logic.MouseLeftButtonUp += new MouseButtonEventHandler(AddSelectedPattern);

            ListBox structuredOptions = new ListBox();

            structuredOptions.Items.Add(set);
            structuredOptions.Items.Add(series);
            structuredOptions.Items.Add(iterate);
            structuredOptions.Items.Add(repeat);
            structuredOptions.Items.Add(logic);

            #endregion

            #region "Specials Pattern Addings..."

            TextBlock HeaderSpecials = new TextBlock() { Text = "Specials Patterns", Width = 110 };

            TextBlock align = new TextBlock() { Text = "Alignment", Width = 110 };
            align.MouseLeftButtonUp += new MouseButtonEventHandler(AddSelectedPattern);

            TextBlock constraint = new TextBlock() { Text = "Constraint", Width = 110 };
            constraint.MouseLeftButtonUp += new MouseButtonEventHandler(AddSelectedPattern);

            TextBlock voidz = new TextBlock() { Text = "Void", Width = 110 };
            voidz.MouseLeftButtonUp += new MouseButtonEventHandler(AddSelectedPattern);

            ListBox specialdOptions = new ListBox();

            specialdOptions.Items.Add(align);
            specialdOptions.Items.Add(constraint);
            specialdOptions.Items.Add(voidz);

            #endregion

            sp.Children.Add(HeaderRegional);
            sp.Children.Add(regOptions);

            sp.Children.Add(HeaderRecursive);
            sp.Children.Add(recursiveOptions);

            sp.Children.Add(HeaderStructured);
            sp.Children.Add(structuredOptions);

            sp.Children.Add(HeaderSpecials);
            sp.Children.Add(specialdOptions);

            grid.Children.Add(sp);

            return grid;
        }

        void AddSelectedPattern(object sender, MouseButtonEventArgs e)
        {
            TextBlock source = sender as TextBlock;

            switch (source.Text)
            {
                #region Search for the appropriate pattern name and add it to our tree"
                case "Any":
                    _tree.Items.Add(new Any());
                    break;

                case "Gap" :
                    _tree.Items.Add(new Gap());
                    break;

                case "Composition":
                    _tree.Items.Add(new Composition());
                    break;

                case "Block":
                    _tree.Items.Add(new PatternModels.Block());
                    break;

                case "RegEx":
                    _tree.Items.Add(new RegularExp());
                    break;

                case "Motif":
                    _tree.Items.Add(new Motif());
                    break;

                case "Prosite":
                    _tree.Items.Add(new Prosite());
                    break;

                case "PWM":
                    _tree.Items.Add(new PWM());
                    break;

                case "Set":
                    _tree.Items.Add(new Set());
                    break;

                case "Series":
                    _tree.Items.Add(new Series());
                    break;

                case "Iteration":
                    _tree.Items.Add(new Iteration());
                    break;

                case "Repeat":
                    _tree.Items.Add(new Repeat());
                    break;

                case "Logic":
                    _tree.Items.Add(new Logic());
                    break;

                case "Alignment":
                    _tree.Items.Add(new Iteration());
                    break;

                case "Constraint":
                    _tree.Items.Add(new Repeat());
                    break;

                case "Void":
                    _tree.Items.Add(new PatternModels.Void());
                    break;
                #endregion
            }

            Close();
        }
    }
}
