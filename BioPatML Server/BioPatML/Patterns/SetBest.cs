 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Sequences;

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
    /// This class implements a pattern set which returns the best match
    /// of all patterns in the set for a given position. 
    /// </summary>
    public class SetBest : Set
    {
        #region -- Constructors --

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SetBest() : base ("SetBest" + Id.ToString()) { }

        /// <summary>
        ///  Constructs an empty pattern set. Use add() to add pattern to the set.
        ///  Only pattern matches with similarity above or equal the specified 
        ///  similarity threshold will be accepted as matches of the pattern set. 
        /// </summary>
        /// <param name="name">name of SetBest Element</param>
        /// <param name="threshold">Similarity threshold</param>
        public SetBest(string name, double threshold)
            : base (name)
        {
            Threshold = threshold;
        }


        #endregion

        #region -- IMatcher implementation --

        /// <summary>
        /// The implementation ensures that
        /// a match fails for a given position if there is no match. Otherwise the
        /// matcher might return a match at a different position.
        /// <see cref="QUT.Bio.BioPatML.Patterns.IPattern">IPattern Match(Sequence, int) method</see>
        /// </summary>
        /// <param name="seq"> The sequence for comparing</param>
        /// <param name="position"> Matching position</param>
        /// <returns></returns>
        public override Match Match(Sequence seq, int position)
        {
            Match maxMatch = null;
            int min_inc = int.MaxValue;

            for (int i = 0; i < base.Count; i++)
            {
                IPattern pattern = this[i];
                Match match = pattern.Match(seq, position);

                // store minimum increment
                int inc = pattern.Increment;
                if (inc < min_inc)
                    min_inc = inc;

                // store match with maximum similarity above threshold
                if (match != null && (match.Similarity >= Threshold)
                        && (maxMatch == null || match.Similarity > maxMatch.Similarity))
                {
                    maxMatch = match;
                }

            }

            increment = min_inc;
            return (maxMatch);
        }

        #endregion
    }
}
