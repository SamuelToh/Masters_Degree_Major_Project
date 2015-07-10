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
using System.ComponentModel.DataAnnotations;

namespace BioPatMLEditor.PatternControls.PatternModels
{
    public sealed class PWMRow : SubElements
    {
        private string _XMLElement 
                            = "<Row letter=\"{0}\"> {1} </Row>";

        private char _RowKey { get; set; }

        private string _RowValue { get; set; }

        [Display(Name = "Letter    ",
            Description = "PWM row letter. E.g. 'T' 'C' 'G' 'A'")]
        [RegularExpression("[a-zA-Z]", ErrorMessage="Input must be in alphabetical form")]
        public char RowKey
        { 
            get { return _RowKey; }

            set { _RowKey = value; NotifyPropertyChanged("RowKey"); }
        
        }

        [Display(Name = "Row Value",
            Description = "Occurence of letter in that matrix. E.g. 1 -3 -0.58 2.13")]
        public string RowValue
        {
            get { return _RowValue; }

            set { _RowValue = value; NotifyPropertyChanged("RowValue"); }
        }

        public override string ToString()
        {
            return
                string.Format
                        (_XMLElement, _RowKey, _RowValue);
        }

    }
}
