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
    public class CompositionElement : SubElements
    {
        private string _XMLElement
                           = "<Symbol letter=\"{0}\"  weight=\"{1}\" />";

        private char _Letter { get; set; }

        private double _Weight { get; set; }

        [Display(Name = "Symbol Letter",
            Description = "The letter you want to have weight on")]
        public char Letter
        {
            get { return _Letter; }

            set { _Letter = value; NotifyPropertyChanged("Letter"); }
        }

        [Display(Name = "Weight",
            Description = "Weightage of the value in matrix")]
        public double Weight
        {
            get { return _Weight; }

            set { _Weight = value; NotifyPropertyChanged("Weight"); }
        }

        public override string ToString()
        {
            return
                string.Format
                        (_XMLElement, _Letter, _Weight);
        }

    }
}
