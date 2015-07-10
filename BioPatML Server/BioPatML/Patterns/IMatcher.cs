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
    /// This interface describes a matcher. A matcher matches a pattern against a sequence.
    /// </summary>
    public interface IMatcher
    {
        /// <summary>
        /// Matches the pattern with the given sequence at the specified position.
        /// Only matches are successfull with a similarity equal or higher than the 
        /// similarity threshold set.
        /// </summary>
        /// <param name="sequence"> Sequence to compare with.</param>
        /// <param name="position"> Matching position.</param>
        /// <returns>
        /// Returns a Match object or null if there is no match.
        /// The Match object is only vaild until the next call of the 
        /// match method! Its contents will be overwritten. If you want to use a
        /// pattern in different thread, you have to clone the pattern. Note, 
        /// that a match object can have sub matches.
        /// </returns>
        Match Match(Sequence sequence, int position);

        /// <summary>
        /// Gets the internal match object of the pattern
        /// </summary>
        /// <returns>
        /// Returns a reference to the internal match object of the pattern.
        /// </returns>
        Match Matched { get; }

        /// <summary>
        /// Getter for the position increment after matching a pattern. Some pattern
        /// can match several times with different length at the same position. In
        /// this case the increment is zero until all matches are performed. For some
        /// patterns an increment greater than one can be performed, e.g. string
        /// searching with the Boyer-Moore algorithm. 
        /// </summary>
        /// <returns>
        /// Returns the increment of the search position.
        /// </returns>
        int Increment { get; }
    }
}
