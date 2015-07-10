using System;
using System.Net;
using System.Text;
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
    public class Set : StructuredPattern
    {
        static string XML_REPRESENTATION
            = "<Set {0} mode = \"{1}\" threshold = \"{2}\" impact = \"{3}\"> \n" +
               "{4} \n" +
               "</Set>";

        [Display(Name = "Best match :",
            Description = "Match by only the best?")]
        public MatchMode Mode { get; set; }

        public enum MatchMode
        {
            ALL = 1,
            BEST = 2
        }

        public Set() : base("Set") { Mode = MatchMode.ALL; }

        public override string ElementXML()
        {
           return 

               string.Format(XML_REPRESENTATION, 
                PatternName == "" || PatternName == null ? "" : "name='" + PatternName + "'",
                Mode,
                Threshold,
                Impact,
                BuildChildNodes());
        }
    }
}
