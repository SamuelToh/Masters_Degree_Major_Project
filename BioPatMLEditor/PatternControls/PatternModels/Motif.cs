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

namespace BioPatMLEditor.PatternControls.PatternModels
{
    public class Motif : RecursivePatterns
    {
        static string XML_REPRESENTATION =
            "<Motif {0}  alphabet=\"{1}\"  motif=\"{2}\" threshold=\"{3}\"  impact=\"{4}\" />";

        [Display(Name = "Search sequence", Description = "The sequence you want to search",
          Order = 6)]
        public String SearchPattern { get; set; }
        
        public Motif()
            : base("Motif", "DNA")
        {
            SearchPattern = "TCC";
            Threshold = 0.2;

        }

        public override string ElementXML()
        {
            return 

            string.Format(XML_REPRESENTATION, 
                PatternName == "" || PatternName == null ? "" : "name=\"" + PatternName + "\"",
                Alphabet,
                SearchPattern == "" || SearchPattern == null ? "" : SearchPattern,
                Threshold,
                Impact);

        }
    }
}