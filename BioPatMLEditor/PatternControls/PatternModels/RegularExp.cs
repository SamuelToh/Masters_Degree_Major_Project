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
    public class RegularExp : PatternBase //RecursivePatterns Fixed on July 6
    {
        //<Regex  name = "C6 zinc-finger" regex = "C.{2}C.{6}C.{5,6}C.{2}C.{6}C"  />
        static string XML_REPRESENTATION =
            "<Regex {0} regex=\"{1}\"  case=\"{2}\"  impact=\"{3}\" />";

         [Display(Name = "Is CaseSensitive",
            Description = "")]
        public bool IsCaseSensitive { get; set; }

         [Display(Name = "Regular Expression", Description = "The sequence you want to search",
           Order = 6)]
         public String SearchPattern { get; set; }

         public RegularExp() : base("Regular Expression", "", 1.0) //base("Regular Expression", "DNA")
         {
             IsCaseSensitive = true;
         }

         public override string ElementXML()
         {
             return

             string.Format(XML_REPRESENTATION,
                 PatternName == "" || PatternName == null ? "" : "name=\"" + PatternName + "\"",
                 SearchPattern == "" || SearchPattern == null ? "" : SearchPattern,
                 IsCaseSensitive ? "SENSITIVE" : "INSENSITIVE",
                 Impact);

         }
    }
}
