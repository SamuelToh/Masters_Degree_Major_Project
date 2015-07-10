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
    public class RegionalPatterns : SearchPatterns
    {
        [Display(Description = "How accurate you want the match to be performed upon. 1 is the max you can go.",
           Order = 6)]
        public int Minimum { get; set; }

        [Display(Description = "How accurate you want the match to be performed upon. 1 is the max you can go.",
            Order = 7)]
        public int Maximum { get; set; }

        [Display(Description = "How accurate you want the match to be performed upon. 1 is the max you can go.",
            Order = 8)]
        [Range(0.0, 255.0, ErrorMessage = "The Increment value can only be between 0.0 to 255.0.")]
        public double Increment { get; set; }

        public RegionalPatterns(string patternTypeName) : base(patternTypeName) 
        {
            Increment = 1;
            Minimum = 0;
            Maximum = 1;
        }

     
    }
}
