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
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace BioPatMLEditor.PatternControls.PatternModels
{
    public class Alignment : PatternBase
    {
        static string XML_REPRESENTATION
               = "<Alignment {0} pattern= \"{1}\"  position= \"{2}\"  offset= \"{3}\"  impact= \"{3}\"> \n" +
                  "{4} \n" +
                  "</Alignment>";

        [Display(Name = "Align name",
            Description = "Name of pattern to Align with.")]
        public string AlignPattern { get; set; }

        public int Offset { get; set; }

        [Display(Name = "Position",
            Description = "Symbolic position (START, END, CENTER) within the predecessor match.")]
        public Position SymbolicPos { get; set; }

        public enum Position
        {
            START = 1,
            END = 2,
            CENTER = 3
        }

        public Alignment() : base("Alignment", "", 1.0) { SymbolicPos = Position.START; }

        public override string ElementXML()
        {
            return

            string.Format(XML_REPRESENTATION,
                PatternName == "" || PatternName == null ? "" : "name= \"" + PatternName + "\"",
                AlignPattern,
                SymbolicPos,
                Offset,
                Impact);

        }
    }
}
