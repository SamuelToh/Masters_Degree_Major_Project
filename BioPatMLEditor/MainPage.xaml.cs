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
using System.ComponentModel;
using BioPatMLEditor.ServiceReference1;

namespace BioPatMLEditor
{
    /// <summary>
    /// The base UI of the BioPatML Editor. This implementation contains all the
    /// necessary wiring of other important UI components.
    /// </summary>
    public partial class MainPage : UserControl
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            //Initialization
            panelPatternBuilder.refTxtCurrPattern = editorController.txtSelectedPattern; //Pass a handler to our panel builder
            panelPatternBuilder.refPatternRepoUC = UCRepository;
            editorController.parentFrameWork = this; //A very bad design
        }

        /// <summary>
        /// Builds the result panel based on the supplied results.
        /// </summary>
        /// <param name="results">A list of hits</param>
        public void InitializeResultPanel(List<Matched> results)
        {
            EditorResultPanel.Initialize_UIComponents(results, mbfParserBox.ParsedSequences);
            EditorResultPanel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Returns the number of sequences in our parser widgets
        /// </summary>
        public int CountMemorySequences
        {
            get { return mbfParserBox.ParsedSequences.Count; }
        }

        //Maybe a compile result panel method here that takes in result param from searchbot?

        /// <summary>
        /// Returns true if there is a BioPatML xml being implemented successfully
        /// </summary>
        public bool HasImplementedBioPatML()
        {
            return panelPatternBuilder.implementedBioPatML == null ? false : true;
        }

        /// <summary>
        /// Retrieves the parsed sequences, returns null if nothing was parsed.
        /// </summary>
        public List<SequenceContract> GetParsedSequences
        {
            get { return CountMemorySequences > 0 ? mbfParserBox.ParsedSequences : null; }
        }

        /// <summary>
        /// Returns the implemented BioPatml String if something was implemented
        /// </summary>
        public String GetImplementedBioPatML
        {
            get { return panelPatternBuilder.implementedBioPatML; }
        }

    }
}
