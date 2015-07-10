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
    ///  This class implements an indexer for the reversed/backward strand.
    /// </summary>
    internal sealed class IndexerReverse : Indexer
    {
        #region -- Private Field --

        /** start index for the reversed index */
        internal int Start { get; set; }

        #endregion

        #region -- Public Constructor --

        /// <summary>
        ///  Creates the indexer for the backward direction.
        /// </summary>
        /// <param name="offset"> Offset of the indexer.</param>
        /// <param name="length"> Length of the SymbolArray the indexer is working on. </param>
        public IndexerReverse(int offset, int length)
            : base(offset)
        {
            Start = length - 1;
        }

        #endregion

        #region Method : Overriding the Transform method for indexer in Reverse direction

        /// <summary>
        ///  Transforms the given index to a new index.
        ///  <see>IIndexer#transform(int)</see>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override int Transform(int index)
        {
            return (Start - Offset - index);
        }

        #endregion
    }
}
