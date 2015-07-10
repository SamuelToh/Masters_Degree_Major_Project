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
    ///  This class implements an indexer for the direct/forward strand.
    /// </summary>
    internal sealed class IndexerDirect : Indexer
    {
        #region -- Constructor --

        /// <summary>
        ///  Creates the indexer for the forward direction.
        /// </summary>
        /// <param name="offset"> Offset of the indexer. </param>
        public IndexerDirect(int offset)
            : base(offset)
        { /* No implementations */ }

        #endregion

        #region Method : For override transform method of direct indexing

        /// <summary>
        ///  Transforms the given index to a new index.
        ///  <see>IIndexer#transform(int)</see>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override int Transform(int index)
        {
            return (Offset + index);
        }

        #endregion

    }
}
