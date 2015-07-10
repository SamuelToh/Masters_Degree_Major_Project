using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Symbols.Indexer;

/***************************************************************************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Symbols.Accessor
{
    /// <summary>
    ///  This class implements a transparent symbol array accessor. 
    /// </summary>
    internal sealed class AccessorTransparent : AccessorBase
    {
        #region -- Public Constructor --

        /// <summary>
        ///  Creates a symbol array accessor on base of the provided symbol array.
        /// </summary>
        /// <param name="index">  Indexer used to index symbols in the symbol array. </param>
        /// <param name="length"> Length of a section of the symbol array. </param>
        /// <param name="symbols">Symbol array. </param>
        public AccessorTransparent
            (IIndexer index, int length, ISymbolArray symbols)
            : base(index, length, symbols)
        { /* NO IMPLEMENTATION*/ }

        #endregion

        #region -- Public Methods --
        /// <summary>
        ///  Transforms the given index to an index within the symbol array by using
        ///  the indexer of the accessor.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override int Transform(int index)
        {
            return (Indexer.Transform(index));
        }

        #endregion
    }
}
