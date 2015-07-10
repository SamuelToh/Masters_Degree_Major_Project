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
    ///  This class implements the matching method of a profile which trys to match
    ///  all possible combinations of gap lengths and patterns the profile allows.
    /// </summary>
    public sealed class ProfileAll : Profile
    {
        #region -- Fields --
        /// <summary>
        /// Increment for profile shift over the sequence 
        /// </summary>
        private int increment = 0;

        /// <summary>
        /// pattern index of profile patterns
        /// </summary>
        private int Index { get; set; }

        #endregion

        #region -- Properties --
        /// <summary>
        /// Getter for the searching increment. The profile increment is only increased
        /// when all possible match configurations are explored.
        /// </summary>
        /// <returns>Returns one. </returns>
        public override int Increment
        {
            get
            {
                return (increment);
            }
        }

        #endregion -- Properties --

        #region -- Constructors --

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProfileAll() : base("ProfileAll" + Id.ToString()) { }

        /// <summary>
        /// Constructs an empty profile with a name and a given threshold value
        /// </summary>
        /// <param name="name">Name of profile element</param>
        /// <param name="threshold">Similarity threshold.</param>
        public ProfileAll(string name, double threshold)
            : base(name)
        {
            Threshold = threshold;
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Find the next match of the pattern.
        /// </summary>
        /// <param name="element">A gap followed by a pattern.</param>
        /// <param name="seq">Sequence where the pattern is searched in.</param>
        /// <param name="position">Current start position within the sequence.</param>
        /// <returns>Returns a match object or null if there is no match with the
        /// given similarity or higer. </returns>
        private Match NextMatch
            (ProfileElement element, Sequence seq, int position)
        {
            Match match;
            int pos;
            IPattern pattern = element.Pattern;

            while (element.CurrGap <= element.MaxGap)
            {
                pos = position + element.CurrGap;
                match = pattern.Match(seq, pos);
                element.CurrGap += pattern.Increment;

                if (match != null)
                    return (match);
            }

            return (null);
        }

        /// <summary>
        /// Implementation of the pattern interface. A match is successful if
        /// every pattern of the profile matches successfully and the mean
        /// similarity is higher or equal than the profile similarity threshold.
        /// This matching method trys to match all possibilities which are 
        /// allowed be the profile patterns and the gaps in between.
        /// <see cref="QUT.Bio.BioPatML.Patterns.IMatcher">IMatcher Match Method</see>
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Match Match(BioPatML.Sequences.Sequence sequence, int position)
        {
            Match match = Matched;
            Match nextMatch = null;
            int patternNumber = Count; //patternList.Count;

            increment = 0;
            while (Index >= 0)
            {
                ProfileElement element = this[Index];

                position = Index > 0 ? element.GapStart : position;

                nextMatch = NextMatch(element, sequence, position);
                if (nextMatch == null)
                {
                    element.ResetGap();
                    Index--;
                    continue;
                }

                element.Pattern.Matched.Set(nextMatch);

                Index++;

                if (Index >= patternNumber)
                {
                    Index = patternNumber - 1;

                    match.CalcSimilarity();
                    if (match.Similarity < Threshold)
                        return (null);

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

        #endregion -- Public Methods
    }
}
