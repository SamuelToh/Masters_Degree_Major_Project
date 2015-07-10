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
    public class Prosite : RecursivePatterns
    {
        static string XML_REPRESENTATION =
                        "<Prosite  {0}  alphabet=\"{1}\"  prosite=\"{2}\"  impact=\"{3}\" />";

        [Display(Name = "Prosite sequence", Description = "The sequence you want to search",
          Order = 6)]
        public string SearchPattern { get; set; }

        public Prosite() : base("Prosite", "DNA") { }

        public override string ElementXML()
        {
            return

            string.Format(XML_REPRESENTATION,
                PatternName == "" || PatternName == null ? "" : "name= \"" + PatternName + "\"",
                Alphabet,
                SearchPattern == "" || SearchPattern == null ? "" : SearchPattern,
                Impact);

        }

    }
}
