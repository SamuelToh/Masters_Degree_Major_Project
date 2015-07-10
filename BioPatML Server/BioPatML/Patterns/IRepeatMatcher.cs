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
    /// This interface describes a repeat matcher. A repeat matcher creates a match
    /// (if possible) at a given sequence position in accordance with a match 
    /// of a reference pattern. Here an example:
    /// <para></para>
    /// Sequence = xxxxACTGxxxxxxxACTGxx
    /// <para></para>
    ///                 ^           ^
    /// <para></para>
    ///            reference     repeat   
    /// 
    /// </summary>
    public interface IRepeatMatcher
    {
        /// <summary>
        ///  Initializes the matcher. This method must be called every time before
        ///  Match(Match, Match) is called. See Match(Match, Match) method below.
        /// </summary>
        /// <param name="sequence">Sequence</param>
        /// <param name="position"> Position within the sequence (first position is one!) </param>
        /// <param name="refMatch"> Reference match within the sequence </param>
        /// <param name="thresHold"> Reference match within the sequence </param>
        void Init
            (Sequence sequence, int position, Match refMatch, int thresHold);

        /// <summary>
        /// Creates a repeated match of the reference match at the current sequence
        /// position. This is a recursive method to match the complete match tree.
        /// Every time this method is called the 
        /// Init(Sequence, int, Match, int) method must be called beforehand.
        /// The method changes the Matched variable.
        /// </summary>
        /// <param name="match"> The match variable is changed and contains the repeated
        /// match of the reference match.  </param>
        /// <param name="refMatch">Reference match (this match defines the pattern to
        /// repeat.</param>
        /// <returns>Returns the repeated match of the reference match or null
        /// if no such match exists (depends on mismatch threshold).</returns>
        Match Match
            (Match match, Match refMatch);
    }
}
