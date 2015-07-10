using System;
using System.Net;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BioPatMLEditor.PatternControls.PatternModels
{
    public abstract class StructuredPattern : SearchPatterns
    {
        [Display(AutoGenerateField=false)]
        public List<PatternBase> ListOfChilds { get; set; }

        public StructuredPattern(string patternTypeName)
            : base(patternTypeName)
        {
            ListOfChilds = new List<PatternBase>();
        }


        protected String BuildChildNodes()
        {
            StringBuilder sb = new StringBuilder();

            foreach (PatternBase childPattern in ListOfChilds)
            {
                sb.Append(Indentation);
                sb.Append(childPattern.ElementXML());
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

    }
}
