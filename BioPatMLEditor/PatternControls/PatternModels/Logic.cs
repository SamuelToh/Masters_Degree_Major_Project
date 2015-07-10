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
    public class Logic : StructuredPattern
    {
        static string XML_REPRESENTATION
           = "<Logic {0} operation= \"{1}\"  threshold= \"{2}\" impact = \"{3}\"> \n" +
              "{4} \n" +
              "</Logic>";

        [Display(Name = "Operation :",
            Description = "Logic operation, AND OR XOR")]
        public Operations Operation { get; set; }

        public enum Operations
        {
            AND = 1,
            OR = 2,
            XOR = 3
        }

        public Logic() : base("Logic") { Operation = Operations.AND; }

        public override string ElementXML()
        {
            return

                string.Format(XML_REPRESENTATION,
                 PatternName == "" || PatternName == null ? "" : "name='" + PatternName + "'",
                 Operation,
                 Threshold,
                 Impact,
                 BuildChildNodes());
        }

    }
}
