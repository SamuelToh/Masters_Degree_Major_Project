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
    /// This class implements a pattern series which returns the best match
    /// of all patterns in the series for a given position. 
    /// </summary>
    public class SeriesBest : Series
    {
        #region -- Constructors --

        /// <summary>
        /// Default constructor that generates a name with unique ID 
        /// </summary>
        public SeriesBest()
            : base("Series" + Id.ToString()) {}

        /// <summary>
        /// Constructs an empty series.
        /// </summary>
        /// <param name="name">Name of pattern</param>
        /// <param name="threshold">Similarity threshold. </param>
        public SeriesBest(string name, double threshold)
            : base (name)
        {
            Threshold = threshold;
        }

        #endregion

        #region -- Public Methods --

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
            Match match = Matched;
            Match bestMatch = match.Clone();
            Match nextMatch = null;
            int patternNumber = Patterns.Count;
            int index = 0;

            bestMatch.Similarity = -1.0;


            while (index >= 0)
            {
                IPattern pattern = this[index];

                position = index > 0 ? match.GetSubMatch(index - 1).End + 1 : position;
                nextMatch = pattern.Match(sequence, position);

                if (nextMatch == null)
                {
                    while (--index >= 0 && this[index].Increment != 0) ;
                    continue;
                }

                pattern.Matched.Set(nextMatch);

                index++;

                if (index == patternNumber)
                {
                    while (--index >= 0 && this[index].Increment != 0) ;
                    match.CalcSimilarity();
                    if (match.Similarity >= Threshold
                            && match.Similarity > bestMatch.Similarity)
                        bestMatch.Set(match);
                }
            }

            if (bestMatch.Similarity < 0)
                return (null);

            match.Set(bestMatch);
            match.CalcStartEnd();
            match.Strand = sequence.Strand;
            match.SetSequence(sequence);

            return (match);
        }

        #endregion
    }
}
