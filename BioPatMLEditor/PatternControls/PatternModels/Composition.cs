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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.Text;

namespace BioPatMLEditor.PatternControls.PatternModels
{
    public class Composition : RegionalPatterns
    {
        //To do: a list for weights
        internal static string XML_REPRESENTATION =
            "<Composition {0} alphabet=\"{1}\" mode=\"{2}\" increment=\"{3}\" minimum = \"{4}\" maximum = \"{5}\" threshold = \"{6}\" impact = \"{7}\" />";

        internal static string WEIGHTED_XML_REPRESENTATION =
            "<Composition {0} alphabet=\"{1}\" mode=\"{2}\" increment=\"{3}\" minimum = \"{4}\" maximum = \"{5}\" threshold = \"{6}\" impact = \"{7}\">\n" +
                   "{8} \n" +
            "</Composition>";

        public ObservableCollection<CompositionElement> compElements = null;

        public enum MatchMode
        {
            ALL = 1,
            BEST = 2
        }

        [Display(Name = "Best match :",
           Description = "Match by only the best?")]
        public MatchMode Mode { get; set; }


        [Display(Name = "Alphabet",
            Description = "The type of composition. Only DNA / RNA or AA is valid",
            Order = 3)]
        public Alphabets Alphabet { get; set; }

        public enum Alphabets
        {
            RNA = 1,
            DNA = 2,
            AA = 3
        }

        public Composition() : base("Composition") 
        {
            compElements = new ObservableCollection<CompositionElement>();
            Mode = MatchMode.ALL;
            Alphabet = Alphabets.DNA;
        }

        private string BuildChildNodes()
        {
            StringBuilder sb = new StringBuilder();

            foreach (CompositionElement composition in compElements)
            {
                sb.Append(Indentation);
                sb.Append(composition.ToString());
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public override string ElementXML()
        {
            return

            string.Format(WEIGHTED_XML_REPRESENTATION,
                PatternName == "" || PatternName == null ? "" : "name= \"" + PatternName + "\"",
                Alphabet,
                Mode,
                Increment,
                Minimum,
                Maximum,
                Threshold,
                Impact,
                BuildChildNodes());

        }
    }
}
