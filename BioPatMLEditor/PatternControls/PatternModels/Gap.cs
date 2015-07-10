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
    public class Gap : RegionalPatterns
    {
        //To do: a list for weights
        internal static string XML_REPRESENTATION =
            "<Gap {0} minimum = \"{1}\" maximum = \"{2}\" threshold = \"{3}\" impact = \"{4}\" />";

        internal static string WEIGHTED_XML_REPRESENTATION =
            "<Gap {0} minimum = \"{1}\" maximum = \"{2}\" threshold = \"{3}\" impact = \"{4}\">\n" +
                   "{5} \n" +
            "</Gap>";
        
        [Display(Name = "Weights",
           Description = "Weight vector for length preferences (optional).")]
        public string Weights { get; set; }

        public Gap() : base("Gap") { }

        public override string ElementXML()
        {
            if(HasWeights())
                return 
                    string.Format(WEIGHTED_XML_REPRESENTATION,
                                    PatternName == "" || PatternName == null ? "" : "name=\"" + PatternName + "\"",
                                    Minimum,
                                    Maximum,
                                    Threshold,
                                    Impact,
                                    "<Weights>" + Weights + "</Weights>");

            //else return the oridinary string representation
            return

            string.Format(XML_REPRESENTATION,
                PatternName == "" || PatternName == null ? "" : "name=\"" + PatternName + "\"",
                Minimum,
                Maximum,
                Threshold,
                Impact);

        }

        private bool HasWeights()
        {
            return Weights == "" || Weights == null ? false : true;
        }

    }
}
