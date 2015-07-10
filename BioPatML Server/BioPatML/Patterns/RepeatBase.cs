using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Symbols;
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
    ///  This class implements the repeat pattern. A repeat pattern consists of a
    ///  reference pattern and a repeat element pattern which is the repeat of
    ///  the reference pattern. More complex repeats can be built by using profiles
    ///  and repeat element patterns directly. See {RepeatElement} for an example.
    /// </summary>
    public sealed partial class Repeat : Pattern
    {
        /// <summary>
        /// Base class for repeats.
        /// </summary>
        private abstract class MatcherRepeat : IMatcher
        {
            #region -- Fields --
            /// <summary>
            /// ref of the outter repeat element
            /// </summary>
            protected Repeat repeat;
            /// <summary>
            /// Matching sequence
            /// </summary>
            protected Sequence matchSeq;
            /// <summary>
            /// Number of element in our matchSeq
            /// </summary>
            protected int matchLen;

            protected double remSim;
            #endregion

            #region -- Constructor --
            /// <summary>
            /// The base constructor
            /// </summary>
            /// <param name="repeat">Reference of the repeat element</param>
            public MatcherRepeat(Repeat repeat)
            {
                this.repeat = repeat;
            }

            /// <summary>
            /// Inits the matching procedure in the derived classes.
            /// </summary>
            protected void Init()
            {
                matchSeq = repeat.RefPattern.Matched;
                matchLen = (matchSeq == null ? 0 : matchSeq.Length);
                remSim = matchLen;
            }

            #endregion

            #region -- Protected Methods --
            /// <summary>
            /// Compares two symbol and returns the "similarity" (weight) between the symbols
            /// </summary>
            /// <param name="symbol1">First symbol.</param>
            /// <param name="symbol2">Second symbol.</param>
            /// <returns>Returns the "similarity" (weight) between the symbols.</returns>
            protected double Compare
                (Symbol symbol1, Symbol symbol2)
            {
                if (repeat.weights == null)
                    return symbol1.Equals(symbol2) ? 1.0 : 0.0;

                return repeat.Weight(symbol1, symbol2);
            }

            #endregion

            #region IMatcher Members

            public virtual Match Match(Sequence sequence, int position)
            {
                return matchSeq == null ? null : repeat.Matched;
            }

            public Match Matched
            {
                get { return matchSeq == null ? null : repeat.Matched; }
            }

            public int Increment
            {
                get { return 1; }
            }

            #endregion
        }
    }
}
