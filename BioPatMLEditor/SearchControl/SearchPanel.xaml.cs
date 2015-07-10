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
using System.Windows.Navigation;
using BioPatMLEditor.ServiceReference1;
using BioPatMLEditor.SearchControl.SearchLogic;

namespace BioPatMLEditor.SearchControl
{
    public partial class SearchPanel : Page
    {
        /// <summary>
        /// A list used to store our hits.
        /// </summary>
        private List<Matched> _results { get; set; }

        /// <summary>
        /// The singleton class of our SearchBot
        /// </summary>
        private SearchBot SequenceAnalzer { get; set; }

        /// <summary> 
        /// Compulsary variable
        /// A reference to our parent page so methods can be accessed 
        /// as and when we like.
        /// </summary>
        public MainPage parentFrameWork { get; set; }

        /// <summary>
        /// Expose our result variable to the public...
        /// </summary>
        public List<Matched> results { get { return _results;  } }

        /// <summary>
        /// Constant content value when the search button is clicked and 
        /// there is processing work going on.
        /// </summary>
        const String SEARCHING_CONTENT = "Analysing sequence(s)...";

        /// <summary>
        /// Constant content value when the button is in normal state.
        /// </summary>
        const String NO_ACTION = "Search";
    
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SearchPanel()
        {
            InitializeComponent();
            SequenceAnalzer = SearchBot.Instance(this);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e) { /*Do Nothing */}

        /// <summary>
        /// Event fired when a search has been completed
        /// </summary>
        internal void OnSearchDoneEvent(List<Matched> result)
        {
            this._results = result;
            this.btnSearch.Content = NO_ACTION;
            parentFrameWork.InitializeResultPanel(result); //Fire the result panel;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //Simple way of knowing whether a search has being conducted 
            if(btnSearch.Content.Equals(SEARCHING_CONTENT))
                return;

            if (parentFrameWork.CountMemorySequences < 1)
                return; //No processing is allowed

            if (!parentFrameWork.HasImplementedBioPatML())
                return; //No implemented BioPatML pattern

            btnSearch.Content = SEARCHING_CONTENT;
            SequenceAnalzer.TargetedBioPatML = parentFrameWork.GetImplementedBioPatML;
            SequenceAnalzer.TargetedSequences = parentFrameWork.GetParsedSequences;
            SequenceAnalzer.SearchPattern();
        }
    }
}
