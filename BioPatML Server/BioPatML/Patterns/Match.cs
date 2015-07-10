using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;

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
    ///  The match class is an extension of a {Feature} and stores information 
    ///  about a match between a pattern and a sequence. A match object can contain 
    ///  sub matches (which is are organized as a {FeatureList}) when the pattern
    ///  is composed of sub patterns, e.g. {ProfileAll} patterns.
    /// </summary>
    public sealed class Match : Feature, ICloneable
    {
        #region -- Automatic Properties --

        /** The similarity of the match within the interval [0,1] 1.0 means perfect/maximum match. */
        public Double Similarity { get; set; }

        /** Reference to the pattern the match belongs to */
        public IPattern MatchPattern { get; set; }

        #endregion

        #region -- Private fields --
        /// <summary>
        /// Impact weight of the match
        /// </summary>
        private Double impact = 1.0;

        #endregion

        #region -- Constructors -- 

        /// <summary>
        /// Creates a match object.
        /// </summary>
        /// <param name="pattern"> Pattern the match belongs to. </param>
        public Match(IPattern pattern) : base() { this.MatchPattern = pattern; }

        /// <summary>
        /// Creates a match object.
        /// </summary>
        /// <param name="pattern">The referenced matching pattern</param>
        /// <param name="sequence">Sequence the match was found on.</param>
        /// <param name="start">Start position of the match.</param>
        /// <param name="length">Length of the match.</param>
        /// <param name="strand">Strand the match belongs to. +1 = forward strand, 
        /// -1 = backward strand, 0 = n.a. or unknown.</param>
        /// <param name="similarity"></param>
        public Match
            (IPattern pattern, Sequence sequence, int start, int length, int strand, double similarity)
            : base()
        {
            this.MatchPattern = pattern;
            Set(sequence, start, length, strand, similarity);
        }

        /// <summary>
        /// Setter for sequence, start, length, strand and similarity
        /// </summary>
        /// <param name="seq">Sequence the match belongs to.</param>
        /// <param name="start">Start position of the match.</param>
        /// <param name="length">Length of the match,</param>
        /// <param name="strand">Strand the match belongs to. +1 = forward strand, 
        /// -1 = backward strand, 0 = n.a. or unknown.</param>
        /// <param name="similarity">Similarity of the match. Should be in interval [0,1].</param>
        public void Set
            (Sequence seq, int start, int length, int strand, double similarity)
        {
            base.Set(start, start + length - 1, strand);
            this.SetSequence(seq);
            this.Similarity = similarity;
        }

        /// <summary>
        /// Setter for sequence,  strand and similarity.
        /// </summary>
        /// <param name="seq">Sequence the match belongs to.</param>
        /// <param name="strand">
        /// Strand the match belongs to. +1 = forward strand, 
        /// -1 = backward strand, 0 = n.a. or unknown.
        /// </param>
        /// <param name="similarity">
        /// Similarity of the match. Should be in interval [0,1].
        /// </param>
        public void Set
            (Sequence seq, int strand, double similarity)
        {
            base.Strand = (strand);
            this.SetSequence(seq);
            this.Similarity = similarity;
        }

        /// <summary>
        /// Setter for a match object on base of another match object.
        /// </summary>
        /// <param name="match">Match object with initial values.</param>
        public void Set(Match match)
        {
            Set(match.BaseSequence, match.Start, match.Length,
                match.Strand, match.Similarity);

            this.MatchPattern = match.MatchPattern;
            //this.Name = match.MatchPattern.PatternName;
            this.impact = match.impact;

            FeatureList subMatchList = match.SubMatchedList;

            if (subMatchList != null)
            {
                if (MatchedList == null)
                {
                    FeatureList matchList = new FeatureList("SubMatches");

                    for (int i = 0; i < subMatchList.Count; i++)
                        matchList.Add(Clone((Match)subMatchList.Feature(i)));

                    MatchedList = matchList;

                }
                else
                {
                    for (int i = 0; i < subMatchList.Count; i++)
                        ((Match)MatchedList.Feature(i)).Set((Match)subMatchList.Feature(i));
                }
            }
            else
                MatchedList = null;
        }

        #endregion 

        #region -- Properties --

        /// <summary>
        /// Calculates the number of matches based on similarity and match length.
        /// </summary>
        public int Matches
        {
            get
            {
                return ((int)(Similarity * Length));
            }

        }

        /// <summary>
        /// Gets/Sets the impact of the match criteria.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when weight value is less than 0 or more than 1.0
        /// </exception>
        public double Impact
        {
            get
            {
                return this.impact;
            }
            set
            {
                if (value < 0.0 || value > 1.0)
                    throw new ArgumentOutOfRangeException
                        ("Invalid weight : " + value);

                this.impact = value;
            }
        }

        /// <summary>
        /// Retrieve the matched listing
        /// </summary>
        private FeatureList MatchedList
        {
            get
            {
                AnnotatedList list = Features();
                return (list.Count > 0 ? (FeatureList)list.Get(0) : null);
            }
            set
            {
                AnnotatedList lists = Features();
                if (value != null)

                    if (lists.Count == 0)
                        lists.Add(value);
                    else
                        lists.Set(0, value);

                else
                    if (lists.Count > 0)
                        lists.RemoveAt(0);
            }
        }

        /// <summary>
        /// The number of sub matches of this match object.
        /// </summary>
        /// <returns></returns>
        public int SubMatchNumber
        {
            get
            {
                return (MatchedList == null ? 0 : MatchedList.Count);
            }
        }

        /// <summary>
        /// Set/get a list of sub matches
        /// </summary>
        public FeatureList SubMatchedList
        {
            get
            {
                return (MatchedList);
            }
            set
            {
                MatchedList = value;
            }
        }

        /// <summary>
        /// Retrieves that particular sub match by index
        /// </summary>
        /// <param name="index">the index of the submatch</param>
        /// <returns></returns>
        public Match SubMatch(int index)
        {
            return ((Match)MatchedList.Feature(index));
        }

        #endregion -- Properties --

        #region -- Public Methods --

        /// <summary>
        /// Adds a sub match to the list of sub matches.
        /// </summary>
        /// <param name="match">Match to add.</param>
        public void Add(Match match)
        {
            if (MatchedList == null)
                MatchedList = (new FeatureList("SubMatches"));

            MatchedList.Add(match);
        }

        /// <summary>
        /// Adds a sub match to the list of sub matches at the given index position.
        /// </summary>
        /// <param name="index">Index position for insertion.</param>
        /// <param name="match">Match to add.</param>
        public void Add(int index, Match match)
        {
            if (MatchedList == null)
                MatchedList = (new FeatureList("SubMatches"));

            MatchedList.Add(index, match);
        }

        /// <summary>
        /// Getter for a sub match. The number of existing sub matches can be
        /// determined by property SubMatchNumber of this class.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Match GetSubMatch(int index)
        {
            return ((Match)MatchedList.Feature(index));
        }

        /// <summary>
        /// Replaces a sub match which replaces the sub match for the given index.
        /// </summary>
        /// <param name="index">Index of the sub match to replace.</param>
        /// <param name="match">Match to set.</param>
        public void ReplaceSubMatch(int index, Match match)
        {
            MatchedList.Insert(index, match);
        }

        /// <summary>
        /// Calculates the number of mismatches based on similarity and match length.
        /// </summary>
        /// <returns></returns>
        public int CalcMismatches()
        {
            return ((int)(Length - Similarity * Length));
        }

        /// <summary>
        /// Calculates the maximum start and end position over all submatches of
        /// the current match to determine the match length and sets the corresponding
        /// parameters (start, length) of the match. If the match does not
        /// contain any submatches the method returns without changing any
        /// parameters.
        /// </summary>
        public void CalcStartEnd()
        {
            Match match;
            int minStart = int.MaxValue;
            int maxEnd = int.MinValue;

            if (SubMatchNumber == 0)
                return;

            for (int i = 0; i < SubMatchNumber; i++)
            {
                match = GetSubMatch(i);

                if (match.Start < minStart)
                    minStart = match.Start;

                if (match.End > maxEnd)
                    maxEnd = match.End;
            }

            Start = minStart;
            End = maxEnd;
        }

        /// <summary>
        /// Calculates the mean similarity over all submatches and sets the
        /// similarity parameter. If the match does not contain any submatches 
        /// the method returns without changing any parameters.
        /// </summary>
        public void CalcSimilarity()
        {
            double sum = 0.0;
            double wsum = 0.0;

            if (SubMatchNumber == 0)
                return;

            for (int i = 0; i < SubMatchNumber; i++)
            {
                Match match = GetSubMatch(i);
                sum += match.Similarity * match.impact;
                wsum += match.impact;
            }

            Similarity = wsum > 0 ? sum / wsum : 0.0;
        }

        /// <summary>
        ///  Calculates the pure length of match which is the length without gaps and
        ///  without taking overlaps into account. This is the length of the match 
        ///  itself (if it has no sub matches) or the sum of the lengths of the sub
        ///  matches. 
        /// </summary>
        /// <returns></returns>
        public int CalcLength()
        {
            int sum = 0;

            if (SubMatchNumber == 0)
                return (Length);

            for (int i = 0; i < SubMatchNumber; i++)
                sum += GetSubMatch(i).CalcLength();


            return (sum);
        }

        /// <summary>
        /// Creates a string representation.
        /// </summary>
        /// <returns>Returns a string representation of a match. </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{" + Start + ", " + Length + ", " + Strand + ", " +
                        (int)(Similarity * 100) / 100.0 + ", " + Letters());

            for (int i = 0; i < SubMatchNumber; i++)
                sb.Append(", " + GetSubMatch(i).ToString());

            sb.Append("}");

            return (sb.ToString());
        }

        #endregion

        #region -- ICloneable Members -- 

        /// <summary>
        /// return a new copy of this Match
        /// </summary>
        /// <returns></returns>
        object System.ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Creates a deep copy of a match object.
        /// </summary>
        /// <returns>Returns a copy of the match object.</returns>
        public Match Clone()
        {
            return (Clone(this));
        }

        /// <summary>
        /// Creates a deep copy of the given match object.
        /// </summary>
        /// <param name="match">Match object to copy.</param>
        /// <returns>Returns a deep copy.</returns>
        public Match Clone(Match match)
        {
            Match newMatch = new Match(null);
            newMatch.Set(match);

            newMatch.SubMatchedList = null;


            for (int i = 0; i < match.SubMatchNumber; i++)
                newMatch.Add(Clone(match.GetSubMatch(i)));

            return (newMatch);
        }

        #endregion
    }
}
