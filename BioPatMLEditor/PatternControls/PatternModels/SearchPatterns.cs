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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BioPatMLEditor.PatternControls.PatternModels
{
    public abstract class SearchPatterns : PatternBase
    {
        [Display(Description = "How exact you want the match to be. Value of 1 means exact match.",
            Order = 3)]
        [Range(0.0, 1.0, ErrorMessage = "The threshold value can only be between 0.0 to 1.0.")]
        public double Threshold { get; set; }

        public SearchPatterns(string patternName)
            : base(patternName, "", 1.0) 
        {
            Threshold = 0.0; //First initialization
        }
    }
}
