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
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace BioPatMLEditor.PatternControls.PatternModels
{
    public sealed class PWM : RecursivePatterns
    {
        static string XML_REPRESENTATION
                        = "<PWM {0} alphabet= \"{1}\"  threshold= \"{2}\"  impact= \"{3}\"> \n" +
                           "{4} \n" +
                           "</PWM>";

        //[Display(AutoGenerateField = false)]
        public ObservableCollection<PWMRow> RowElements = null;

        public PWM() : base("Position Weighted Matrix", "DNA") 
        {
            RowElements = new ObservableCollection<PWMRow>();
        }

        private String BuildChildNodes()
        {
            StringBuilder sb = new StringBuilder();

            foreach (PWMRow row in RowElements)
            {
                sb.Append(Indentation);
                sb.Append(row.ToString());
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public override string ElementXML()
        {
            return

            string.Format(XML_REPRESENTATION,
                PatternName == "" || PatternName == null ? "" : "name= \"" + PatternName + "\"",
                Alphabet,
                Threshold,
                Impact,
                BuildChildNodes());

        }

    }
}
