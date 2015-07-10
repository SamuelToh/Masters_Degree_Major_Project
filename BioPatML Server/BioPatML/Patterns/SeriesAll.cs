using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    /// This class implements a pattern series which returns all matches
    /// of the pattern series for a given position. 
    /// </summary>
    public class SeriesAll : Series
    {
        #region -- Private Fields --

        /// <summary>
        /// pattern index of profile patterns
        /// </summary>
        private int Index { get; set; }

        /// <summary>
        /// position increment after a match
        /// </summary>
        private int increment;

        #endregion

        #region -- Public Constructors --
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SeriesAll() 
            : base("SeriesAll" + Id.ToString()) 
        { /*NO IMPLEMENTATION */}


        /// <summary>
        /// Constructs an empty series. 
        /// </summary>
        /// <param name="name">Name for seriesAll</param>
        /// <param name="threshold">Similarity threshold.</param>
        protected SeriesAll(string name, double threshold)
            : base(name)
        {
            Threshold = threshold;
        }

        #endregion

        #region -- Properties --

        /// <summary>
        /// Returns the increment value for next start position of search.
        /// </summary>
        public override int Increment
        {
            get
            {
                return (increment);
            }
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// The implementation ensures that
        /// a match fails for a given position if there is no match. Otherwise the
        /// matcher might return a match at a different position.
        /// <see cref="QUT.Bio.BioPatML.Patterns.IPattern">IPattern Match(Sequence, int) method</see>
        /// </summary>
        /// <param name="sequence"> the sequence for matching</param>
        /// <param name="position"> position used for matching</param>
        /// <returns>The matched</returns>
        public override Match Match(BioPatML.Sequences.Sequence sequence, int position)
        {
            Match match = Matched;
            Match nextMatch = null;
            int patternNumber = Patterns.Count;

            while (Index >= 0)
            {
                IPattern pattern = this[Index];

                position = Index > 0 ? match.GetSubMatch(Index - 1).End + 1 : position;
                nextMatch = pattern.Match(sequence, position);

                if (nextMatch == null)
                {
                    while (--Index >= 0 && this[Index].Increment > 0) ;
                    continue;
                }
                pattern.Matched.Set(nextMatch);

                Index++;
                if (Index == patternNumber)
                {
                    while (--Index >= 0 && this[Index].Increment > 0) ;

                    increment = 0;

                    match.CalcSimilarity();

                    if (match.Similarity < Threshold)
                        return null;

                    match.CalcStartEnd();
                    match.Strand = sequence.Strand;
                    match.SetSequence(sequence);

                    return (match);
                }
            }

            increment = base.Increment;
            Index = 0;
            return (null);
        }


        #endregion
    }
}
