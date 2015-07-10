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
    public class Repeat : StructuredPattern
    {
        static string XML_REPRESENTATION
                = "<Repeat {0} pattern= \"{1}\"  mode= \"{2}\"  threshold= \"{3}\" impact=\"{4}\"> \n" +
                   "{5} \n" +
                   "</Repeat>";

        public ObservableCollection<PairingElement> RepeatPairs = null;

        [Display(Name = "Mode",
            Description = "Type of repeat direct or inverted.")]
        public RepeatStyle Mode { get; set; }

        public enum RepeatStyle
        {
            DIRECT = 1,
            INVERTED = 2
        }

        [Display(Name = "Repeat name",
            Description = "Name of pattern to repeat.")]
        public string RepeatPattern { get; set; }

        public Repeat() : base("Repeat") 
        {
            RepeatPairs = new ObservableCollection<PairingElement>();
            Mode = RepeatStyle.DIRECT;
        }

        private new string BuildChildNodes()
        {
            StringBuilder sb = new StringBuilder();

            foreach (PairingElement pair in RepeatPairs)
            {
                sb.Append(Indentation);
                sb.Append(pair.ToString());
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public override string ElementXML()
        {
            return

            string.Format(XML_REPRESENTATION,
                PatternName == "" || PatternName == null ? "" : "name= \"" + PatternName + "\"",
                RepeatPattern,
                Mode,
                Threshold,
                Impact,
                BuildChildNodes());

        }

    }
}
