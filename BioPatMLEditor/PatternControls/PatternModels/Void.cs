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

namespace BioPatMLEditor.PatternControls.PatternModels
{
    public class Void : PatternBase
    {
        static string XML_REPRESENTATION =
           "<Void {0}  impact=\"{1}\" />";

        public Void() : base("Void", "", 1.0) { }

        public override string ElementXML()
        {
            return

            string.Format(XML_REPRESENTATION,
                PatternName == "" || PatternName == null ? "" : "name=\"" + PatternName + "\"",
                Impact);

        }
    }
}
