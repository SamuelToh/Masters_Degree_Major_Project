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
using System.Windows.Data;
using BioPatMLEditor.ServiceReference1;
using BioPatMLEditor.ResultsPanel.DisplayEngine;

namespace BioPatMLEditor.ResultsPanel
{
    /// <summary>
    /// This class controls the datagrid and rich textbox of the result panel.
    /// </summary>
    public partial class ResultPanel : UserControl
    {
        internal PagedCollectionView pcv = null;
        internal List<Matched> MatchCollection {get;set;}
        internal List<SequenceContract> Sequences { get; set; }
    
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ResultPanel()
        {
            InitializeComponent();
            //matchCollection = listMatches;
            //Loaded += new RoutedEventHandler(ResultPanel_Loaded);
        }

        /// <summary>
        /// This method builds and populate all of our UI component in the result panel.
        /// </summary>
        /// <param name="matches"></param>
        /// <param name="sequences"></param>
        public void Initialize_UIComponents
                            (List<Matched> matches, List<SequenceContract> sequences)
        {
            MatchCollection = matches;
            Sequences = sequences;
            Build_ResultGrid(matches);
        }

        /// <summary>
        /// Populates our hit result grid.
        /// </summary>
        /// <param name="listMatches">The list of hit result</param>
        private void Build_ResultGrid(List<Matched> listMatches)
        {
            pcv = new PagedCollectionView(MatchCollection);
            pcv.GroupDescriptions.Add(new PropertyGroupDescription("SequenceName"));
            MatchGridList.ItemsSource = pcv;
        }

        /// <summary>
        /// This implementation clears all previous display and uses the 
        /// result display engine to render a new set of hit result onto the
        /// rich text box.
        /// </summary>
        /// <param name="Match"></param>
        public void Build_ResultRichTextArea(Matched Match)
        {
            richTxtDisplay.Blocks.Clear();

            if (Sequences == null)
                throw new Exception
                    ("No sequence to attach to result!" + 
                        "Please check the sequences variable before proceeding.");

            richTxtDisplay.Blocks.Add
                (ResultDisplayLogic.GetRichHeader
                                        (Match.SequenceName, Match.MatchAt_StartPosition, Match.Match_EndPosition));

            foreach(SequenceContract sequence in Sequences)

                if (sequence.Name.Equals(Match.SequenceName))
                {
                    richTxtDisplay.Blocks.Add(ResultDisplayLogic.GetHitDetails
                                                (sequence.Characters,
                                                    Match.MatchAt_StartPosition,
                                                    Match.Match_EndPosition));
                    return;
                }

            throw new Exception
                        ("Unable to find the appropriate sequence to display out hit result!");
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            pcv = null;
            MatchCollection = null;
            Sequences = null;
            //hide this window 
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// Update the UI when selection has being changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MatchGridList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Matched selc = this.MatchGridList.SelectedItem as Matched;
            
            //Refresh is user has selected an item from the grid
            if(selc != null)
                Build_ResultRichTextArea(selc);
 
            //User could have close that one item category thus do nothing
        }
    }
}
