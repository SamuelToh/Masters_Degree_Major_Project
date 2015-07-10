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
    public class PairingElement : SubElements
    {
        private string _XMLElement
                            = "<Pairing original=\"{0}\"  repeat=\"{1}\"    weight=\"{2}\"/>";

        private string _Original { get; set; }

        private string _Repeat { get; set; }

        private double _Weight { get; set; }

        [Display(Name = "Original character",
            Description = "The original value")]
        public string Original
        {
            get { return _Original; }

            set { _Original = value; NotifyPropertyChanged("Original"); }
        }

        [Display(Name = "Repeat character",
            Description = "The original value")]
        public string Repeat
        {
            get { return _Repeat; }

            set { _Repeat = value; NotifyPropertyChanged("Repeat"); }
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
                        (_XMLElement, _Original, _Repeat, _Weight);
        }


    }
}
