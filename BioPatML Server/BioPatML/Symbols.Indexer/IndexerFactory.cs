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
    ///  Factory for indexers. Current available indexers are:
    ///  <para></para>
    /// <see cref="QUT.Bio.BioPatML.Symbols.Indexer.IndexerDirect"> Direct Indexer </see>
    /// <para></para>
    /// <see cref="QUT.Bio.BioPatML.Symbols.Indexer.IndexerReverse"> Reverse Indexer </see>
    /// </summary>
    public static class IndexerFactory
    {
        #region -- Factory Instance Implementations --

        /// <summary>
        ///  Creates an indexer.
        /// </summary>
        /// <param name="type">   Type of the indexer: DIRECT or REVERSE </param>
        /// <param name="offset"> Offset for the indexer </param>
        /// <param name="length"> Length for the indexer</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <returns> an indexer. </returns>
        static public Indexer Instance
            (string type, int offset, int length)
        {
            if (type.Equals("DIRECT"))
                return new IndexerDirect(offset);

            else
                if (type.Equals("REVERSE"))
                    return (new IndexerReverse(offset, length));

            throw new ArgumentException
                ("Invalid indexer type: " + type);
        }

        #endregion
    }
}
