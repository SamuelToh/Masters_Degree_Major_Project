using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Symbols;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Sequences
{
    /// <summary>
    ///  This interface defines a sequence.
    /// </summary>
    public interface ISequence
    {
        /// <summary>
        /// Gets a symbol of the sequence by position.
        /// <para></para>
        /// This method can be confusing because its functionality is similiar to SymbolAt
        /// . In symbolAt the starting index is 0 however here we uses position where the first
        /// element index is 1.
        /// </summary>
        /// <param name="position">position Position of the sequence element. The first position in a
        /// sequence is one, however the position is not bound and "invalid" positions
        /// will be mapped to valid symbols according to the policy of the sequence
        /// accessor.
        /// As a consequence the GetSymbol method usually behaves different for 
        /// the main sequence and a subsequence. </param>
        /// <returns> Symbol of the sequence at the specified position. </returns>
        Symbol GetSymbol(int position);

        /// <summary>
        /// Gets the total number of character within sequence
        /// </summary>
        int Length { get; }

        /// <summary>
        ///  Identifies the strand direction of sequence
        /// </summary>
        int Strand { get; }

    }
}
