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
    public class Any : RegionalPatterns
    {
        static string XML_REPRESENTATION =
            "<Any {0}  minimum=\"{1}\"  maximum=\"{2}\" increment=\"{3}\"  impact=\"{4}\" />";

        public Any() : base("Any") { }

        public override string ElementXML()
        {
            return

            string.Format(XML_REPRESENTATION,
                PatternName == "" || PatternName == null ? "" : "name=\"" + PatternName + "\"",
                Minimum,
                Maximum,
                Increment,
                Impact);

        }
    }
}
