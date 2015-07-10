using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/***************************************************************************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Symbols.Indexer
{
    /// <summary>
    ///  An indexer describes how the symbols in an 
    ///  <see cref="QUT.Bio.BioPatML.Symbols.SymbolArray">SymbolArray</see>
    ///  are indexed
    /// </summary>
    public interface IIndexer
    {
        /// <summary>
        ///  Transform the given index to a new index according to the policy of the
        /// </summary>
        /// <param name="index"> Zero based index. </param>
        /// <returns> Returns a transformed index. </returns>
        int Transform(int index);
    }
}
