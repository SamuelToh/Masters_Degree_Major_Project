using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Symbols;
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
    ///  This class describes an accessor which reads the symbol array in a 
    ///  circular way.
    /// </summary>
    internal sealed class AccessorCircular : AccessorBase
    {
        #region -- Public Constructor --

        /// <summary>
        ///  Creates a symbol array accessor on base of the provided symbol array.
        /// </summary>
        /// <param name="indexer"> Indexer used to index symbols in the symbol array. </param>
        /// <param name="length">  Length of a section of the symbol array.</param>
        /// <param name="symbols"> Symbol array.</param>
        public AccessorCircular
            (IIndexer indexer, int length, ISymbolArray symbols)
            : base(indexer, length, symbols)
        { /* No implementation needed */ }

        #endregion

        #region -- Public Method --

        /// <summary>
        ///  Transforms the given index to an index within the symbol array.
        ///  Describes a circular index. If the index is out of bounds it is mapped to 
        ///  a valid index thus, the method will never hit exception out of bound error.
        /// </summary>
        /// <param name="index">The desire value for transformation.</param>
        /// <returns>The transformed position value, used for retriving our desired symbol
        /// from symbol array.</returns>
        public override int Transform(int index)
        {
            index = index % Length;

            return (index < 0 ? Indexer.Transform(Length + index)
                : Indexer.Transform(index));
        }

        #endregion
    }
}
