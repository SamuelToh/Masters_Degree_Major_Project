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
    public class Constraint : PatternBase
    {
        static string XML_REPRESENTATION
                = "<Constraint {0} position= \"{1}\"  offset= \"{2}\"  impact= \"{3}\" />";

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

        public Constraint() : base("Constraint", "", 1.0) { SymbolicPos = Position.START;  }

        public override string ElementXML()
        {
            return

            string.Format(XML_REPRESENTATION,
                PatternName == "" || PatternName == null ? "" : "name= \"" + PatternName + "\"",
                SymbolicPos,
                Offset,
                Impact);

        }
    }
}
