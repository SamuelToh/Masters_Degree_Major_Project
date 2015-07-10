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
    public class Iteration : StructuredPattern
    {
        static string XML_REPRESENTATION
            = "<Iteration {0} minimum = \"{1}\" maximum = \"{2}\" threshold = \"{3}\" impact= \"{4}\"> \n" +
               "{5} \n" +
               "</Iteration>";

        public int MinOccurences { get; set; }

        public int MaxOccurences { get; set; }

        public Iteration() : base("Iteration") { }

        public override string ElementXML()
        {
            return

                string.Format(XML_REPRESENTATION,
                 PatternName == "" || PatternName == null ? "" : "name='" + PatternName + "'",
                 MinOccurences,
                 MaxOccurences,
                 Threshold,
                 Impact,
                 BuildChildNodes());
        }
    }
}
