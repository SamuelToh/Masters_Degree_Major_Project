using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Patterns;
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
    /// This class implements the matching method of a profile which trys to maximze
    /// the similarity by choosing an appropriate gap lengths between the profile
    /// patterns. However, only a greedy heuristics is implemented which doesn't
    /// gurantees that the global maximum is found.
    /// </summary>
    public sealed class ProfileBest : Profile
    {
        #region -- Private Field --

        /// <summary>
        /// storage for max. match information 
        /// </summary>
        private Match maxMatch;

        #endregion

        #region -- Public Constructors --

        /// <summary>
        /// Default constructor, constructing an empty profile best
        /// </summary>
        public ProfileBest() : base("ProfileBest" + Id.ToString()) { maxMatch = new Match(this); }

        /// <summary>
        /// Constructs an empty profile.
        /// </summary>
        /// <param name="name">Name for element profile best</param>
        /// <param name="threshold">Similarity threshold.</param>
        public ProfileBest(string name, double threshold)
            : base (name)
        {
            maxMatch = new Match(this);
            Threshold = threshold;
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Find the match with the highest score for a gap followed by a pattern.
        /// </summary>
        /// <param name="element">A gap followed by a pattern.</param>
        /// <param name="seq">Sequence where the pattern is searched in.</param>
        /// <param name="position">Current start position within the sequence.</param>
        /// <returns>
        /// Returns a match object or null if there is no match with the
        /// given similarity or higher. 
        /// </returns>
        private Match MaxMatch
            (ProfileElement element, Sequence seq, int position)
        {
            Match match;
            int start = position + element.MinGap;
            int end = position + element.MaxGap;
            IPattern pattern = element.Pattern;

            maxMatch.Set(null, 0, 0, 0, -Double.MaxValue);
            for (int pos = start; pos <= end; pos++)
            {
                do
                {
                    match = pattern.Match(seq, pos);
                    if (match != null && match.Similarity > maxMatch.Similarity)
                        maxMatch.Set(match);
                } while (pattern.Increment == 0);
            }

            return (maxMatch.Similarity < Threshold ? null : maxMatch);

        }

        /// <summary>
        /// Implementation of the pattern interface. A match is successful if
        /// every pattern of the profile matches successfully and the mean
        /// similarity is higher or equal than the profile similarity threshold
        /// <see cref="QUT.Bio.BioPatML.Patterns.IMatcher">IMatcher Match Method</see>
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Match Match(BioPatML.Sequences.Sequence sequence, int position)
        {
            Match match = Matched;
            Match maxMatch = null;
            int patternNumber = patternList.Count;

            for (int index = 0; index < patternNumber; index++)
            {
                ProfileElement element = this[index];
                position = index > 0 ? element.GapStart : position;
                maxMatch = MaxMatch(element, sequence, position);
                if (maxMatch == null)
                    return (null);
                element.Pattern.Matched.Set(maxMatch);
            }

            match.CalcSimilarity();
            if (match.Similarity < Threshold)
                return (null);

            match.CalcStartEnd();
            match.Strand = sequence.Strand;
            match.SetSequence(sequence);

            return (match);
        }

        #endregion
    }
}
