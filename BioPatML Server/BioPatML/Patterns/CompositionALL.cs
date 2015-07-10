using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using QUT.Bio.BioPatML.Symbols;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Common.XML;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Patterns
{
    public sealed partial class Composition : PatternFlexible
    {
        #region -- Private class MatcherBest for Composition pattern --

        /// <summary>
        /// Matcher to find all matches of the composition. 
        /// </summary>
        private class MatcherAll : IMatcher
        {
            #region -- Automatic Properties --
            /// <summary>
            /// Matcher to find the best match/length of the composition. 
            /// </summary>
            private Composition Composition;

            /// <summary>
            /// The increment length used by our match methods.
            /// </summary>
            private int increment;

            /// <summary>
            /// Number of elements within our composition pattern
            /// </summary>
            private int CurrLength { get; set; }

            /// <summary>
            /// Counter for the amount of search attempted
            /// </summary>
            private int Counter { get; set; }

            #endregion

            #region -- Constructor --

            /// <summary>
            /// Constructor for inner class Match All
            /// </summary>
            /// <param name="composition"></param>
            public MatcherAll(Composition composition)
            {
                Composition = composition;
                Counter = 0;
            }

            #endregion

            #region IMatcher Members

            /// <summary>
            /// Implementation of the IMatcher interface. An any pattern matches any sequence.
            /// <see cref="QUT.Bio.BioPatML.Patterns.IMatcher">IMatcher interface</see>.
            /// </summary>
            /// <param name="sequence">Sequence to compare with.</param>
            /// <param name="position">Matching position.</param>
            /// <returns>The matched item</returns>
            public Match Match(Sequence sequence, int position)
            {
                Match match = Composition.Matched;

                CurrLength = (int)(Composition.MinLength + Counter * Composition.IncLength + 0.5);
                CurrLength = Math.Min(CurrLength, Composition.MaxLength);
                increment = 0;
                Counter++;

                double sum = 0.0;
                for (int len = 0; len < CurrLength; len++)
                    sum += Composition.Weight(sequence.GetSymbol(position + len));

                if (CurrLength >= Composition.MaxLength)
                {
                    increment = 1;
                    Counter = 0;
                }

                double similarity = sum / (CurrLength * Composition.maxWeight);
                if (similarity < Composition.Threshold)
                    return null;

                match.Set(sequence, position, CurrLength, sequence.Strand, similarity);
                return match;
            }

            /// <summary>
            /// Returns the matched object of our composition pattern
            /// </summary>
            public Match Matched
            {
                get { return Composition.Matched; }
            }

            /// <summary>
            /// Implementing the increment method of 
            /// <see cref="QUT.Bio.BioPatML.Patterns.IMatcher">IMatcher interface</see>
            /// </summary>
            public int Increment
            {
                get { return increment; }
            }

            #endregion -- IMatcher Members --
        }
       #endregion -- Private class MatcherBest for Composition pattern --
    }
}
