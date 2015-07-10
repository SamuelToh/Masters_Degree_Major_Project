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
    /// This class implements a pattern set which returns all matches
    /// of all patterns in the set for a given position. 
    /// </summary>
    public class SetAll : Set
    {
        #region -- Automatic Properties  --

        /// <summary>
        /// pattern index of pattern within the set
        /// </summary>
        private int Index { get; set; }

        /// <summary>
        /// Least increment
        /// </summary>
        private int Min_inc { get; set; }

        #endregion

        #region -- Public Constructors --

        /// <summary>
        /// The default constructor
        /// </summary>
        public SetAll() : base("Set" + Id.ToString()) { Min_inc = int.MaxValue; }

        /// <summary>
        /// Constructs an empty pattern set. Use add() to add pattern to the set.
        /// Only pattern matches with similarity above or equal the specified 
        /// similarity threshold will be accepted as matches of the pattern set. 
        /// </summary>
        /// <param name="name">Name of SetAll element</param>
        /// <param name="threshold"> Similarity threshold </param>
        public SetAll(string name, double threshold)
            : base(name)
        {
            Min_inc = int.MaxValue;
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
        /// <param name="sequence"> The sequence for comparing</param>
        /// <param name="position"> Matching position</param>
        /// <returns></returns>
        public override Match Match(BioPatML.Sequences.Sequence sequence, int position)
        {
            while (Index < base.Count)
            {
                IPattern pattern = this[Index++];
                Match match = pattern.Match(sequence, position);

                // store minimum increment
                int inc = pattern.Increment;
                if (inc < Min_inc)
                    Min_inc = inc;

                if (match != null && (match.Similarity >= Threshold))
                {
                    increment = 0;
                    return match;
                }

            }

            Index = 0;
            increment = Min_inc;
            Min_inc = int.MaxValue;

            return null;
        }

        #endregion
    }
}
