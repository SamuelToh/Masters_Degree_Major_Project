using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using QUT.Bio.BioPatML.Alphabets;

/***************************************************************************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Symbols
{
    /// <summary>
    ///  This interface describes an array of symbols. Classes which implement this
    ///  interface can be used as sequences.
    /// </summary>
    public interface ISymbolArray 
    {
        /// <summary>
        /// Gets the symbol at the specified index position in Symbol Array.
        /// </summary>
        /// <param name="index"> Index position (zero based!) </param>
        /// <returns> Return the symbol for the given index. </returns>
        Symbol SymbolAt(int index);

        /// <summary>
        /// Total number of elements in our Symbol Array.
        /// </summary>
        int Length { get; }
        
        /// <summary>
        /// Gets the Alphabet property of this Symbol.
        /// </summary>
        Alphabet Alphabet { get; }

    }
}
