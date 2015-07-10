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
using BioPatMLEditor.PatternControls.PatternModels;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;

namespace BioPatMLEditor.PatternControls.PatternAttrEditor
{
    /// <summary>
    /// This class builds the child window and bines the dataform to the selected model's attribute
    /// for configuration. Sub tag window only be rendered for users when editing 
    /// complex pattern types such as PWM (Which has configurable rows), Blocks and others...
    /// </summary>
    public sealed partial class AttributeEditor : ChildWindow
    {
        /// <summary>
        /// The data source for our dataForm...
        /// </summary>
        internal PagedCollectionView pcv { get; set; }

        /// <summary>
        /// Default constructor nothing much here
        /// </summary>
        public AttributeEditor()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Re-initialize all components in this child UI.
        /// This method also checks whether or not the selected pattern is a complex type. 
        /// If yes, the data form will render a special tag to accomodate the complex type settings.
        /// </summary>
        /// <param name="p">The selected Pattern</param>
        public void RebuildPanel(PatternBase p)
        {
            this.PnlPatternSetter.CommandButtonsVisibility = DataFormCommandButtonsVisibility.Commit;
            this.PnlPatternSetter.CurrentItem = p;

            #region Checks whether the selected pattern is a complex type

            #region if pattern is ["Position Weighted Matrix"]
            if (p is PWM)
            {
                pcv = new PagedCollectionView((p as PWM).RowElements);
                DataContext = pcv;
                SubElementView.Header = "PWM row elements";
                SubPanelDF.Header = "Add a PWM row here.";
            }
            #endregion

            #region else if pattern is a ["Block"]
            else if (p is BioPatMLEditor.PatternControls.PatternModels.Block)
            {
                pcv = new PagedCollectionView((p as BioPatMLEditor.PatternControls.PatternModels.Block).BlockElements);
                DataContext = pcv;
                SubPanelDF.Header = "Add your Block(s) here.";
                SubElementView.Header = "Block sequences";
            }
            #endregion

            #region else if pattern is a ["Composition"]
            else if (p is Composition)
            {
                pcv = new PagedCollectionView((p as Composition).compElements);
                DataContext = pcv;
                SubPanelDF.Header = "Add your Symbol & Weight here.";
                SubElementView.Header = "Symbol Weights";
            }
            #endregion

            #region else if pattern is a ["Repeat"]
            else if (p is Repeat)
            {
                pcv = new PagedCollectionView((p as Repeat).RepeatPairs);
                DataContext = pcv;
                SubElementView.Header = "Pairings";
                SubPanelDF.Header = "Add your pairings here.";
            }
            #endregion

            #region Otherwise just another ordinary pattern, not a complex one.
            else
                //Patterns with no sub elements
                SubElementView.Visibility = System.Windows.Visibility.Collapsed;
            #endregion

            #endregion
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; 
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}
