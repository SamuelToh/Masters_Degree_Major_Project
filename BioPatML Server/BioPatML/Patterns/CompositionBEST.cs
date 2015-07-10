using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using QUT.Bio.BioPatML.Symbols;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Common.XML;

/*****************| Queensland University Of Technology |********************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Patterns
{
    /// <summary>
    /// This class defines a composition pattern. A composition pattern describes
    /// the symbol composition of a sequence section of variable length.
    /// </summary>
    public sealed partial class Composition : PatternFlexible
    {
        #region -- Private class MatcherBest for Composition pattern --

        /// <summary>
        /// Matcher to find the best match/length of the composition. 
        /// </summary>
        private class MatcherBest : IMatcher
        {
            #region -- Automatic Properties --

            /// <summary>
            /// The composition pattern which the matcher feeds on.
            /// </summary>
            private Composition Composition { get; set; }

            #endregion

            #region -- Constructor --
            /// <summary>
            /// Main constructor for building matcher implementation on our composition pattern.
            /// </summary>
            /// <param name="composition">Composition which the matcher sits on.</param>
            public MatcherBest(Composition composition)
            {
                Composition = composition;
            }

            #endregion

            #region -- IMatcher Members --
            /// <summary>
            /// Gets the position increment after matching a pattern. Some pattern
            /// can match several times with different length at the same position. In
            /// this case the increment is zero until all matches are performed. For some
            /// patterns an increment greater than one can be performed, e.g. string
            /// searching with the Boyer-Moore algorithm. 
            /// </summary>
            public int Increment
            {
                get { return 1; }
            }

            /// <summary>
            /// Returns the matched object of our composition pattern
            /// </summary>
            public Match Matched
            {
                get
                {
                    return Composition.Matched;
                }
            }

            /// <summary>
            /// Implementation of the IMatcher interface. An any pattern matches any sequence.
            /// <see cref="QUT.Bio.BioPatML.Patterns.IMatcher">IMatcher interface</see>.
            /// </summary>
            /// <param name="sequence">Sequence to compare with.</param>
            /// <param name="position">Matching position.</param>
            /// <returns>The matched item</returns>
            public Match Match(Sequence sequence, int position)
            {
                Match match = Matched;
                int maxLen = Composition.MaxLength;
                int minLen = Composition.MinLength;
                double incLen = Composition.IncLength;

                double sum = 0.0;

                for (int len = 1; len <= minLen; len++)
                    sum += Composition.Weight(sequence.GetSymbol(position + len - 1));
                double bestSum = sum;
                int bestLen = minLen;
                for (int len = minLen + 1; len <= maxLen; len++)
                {
                    sum += Composition.Weight(sequence.GetSymbol(position + len - 1));
                    double diff = len - minLen;
                    double rest = Math.Abs(diff - incLen * (int)(diff / incLen + 0.5));
                    if (sum / len >= bestSum / bestLen && rest < 0.5)
                    {
                        bestSum = sum;
                        bestLen = len;
                    }
                }


                double similarity = bestSum / (bestLen * Composition.MaxWeight);
                if (similarity < Composition.Threshold)
                    return null;

                match.Set(sequence, position, bestLen, sequence.Strand, similarity);
                return match;
            }

            #endregion
        }

        #endregion -- Private class MatcherBest for Composition pattern --
    }
}
