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
    public class BlockElement : SubElements
    {
        private string _XMLElement
                            = "<Sequence> {0} </Sequence>";

        private string _BlockSequence { get; set; }


        [Display(Name = "Block sequence",
            Description = "The set of aligned sequence")]
        public string BlockSequence
        {
            get { return _BlockSequence; }

            set { _BlockSequence = value; NotifyPropertyChanged("BlockSequence"); }
        }

        public override string ToString()
        {
            return
                string.Format
                        (_XMLElement, _BlockSequence);
        }
    }
}
