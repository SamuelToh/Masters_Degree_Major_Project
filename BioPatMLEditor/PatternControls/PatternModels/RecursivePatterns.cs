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
    public class RecursivePatterns : SearchPatterns
    {
        [Display(Name = "Alphabet",
            Description = "The type of pattern. Only DNA / RNA or AA is valid",
            Order = 3)]
        public Alphabets Alphabet { get; set; }

        public enum Alphabets
        {
            RNA = 1,
            DNA = 2,
            AA = 3
        }

        /// <summary>
        /// Returns the Enumeration alphabet value.
        /// Note: by default it returns a DNA value.
        /// </summary>
        /// <param name="value">the string value of alphabet</param>
        /// <returns>The Alphabets, DNA, RNA or AA</returns>
        public static Alphabets GetEnumAlpha(string value)
        {
            switch (value.ToUpper())
            {
                case "AA" :
                    return Alphabets.AA;
                case "RNA" :
                    return Alphabets.RNA;
                case "DNA" :
                    return Alphabets.DNA;

                default :
                    return Alphabets.DNA;
            }
        }

        public RecursivePatterns(string patternTypeName, string alphabet)
            : base(patternTypeName)
        {
            Alphabet = Alphabets.DNA;
        }

    }
}
