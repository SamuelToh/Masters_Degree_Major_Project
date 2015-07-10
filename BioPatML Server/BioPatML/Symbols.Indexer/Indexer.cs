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
    ///  This abstract class serves as a base for the different Indexers. 
    /// <para></para>
    /// See indexer factory for different kindes of available indexers.
    /// <para></para>
    /// <see cref="QUT.Bio.BioPatML.Symbols.Indexer.IndexerFactory">IndexerFactory</see>
    /// </summary>
    public abstract class Indexer : IIndexer
    {
        #region -- Private Variables -- 
        /// <summary>
        /// Index offset which is used by the indexer 
        /// </summary>
        internal int Offset { get; set; }
        #endregion

        #region -- Public Constructor --

        /// <summary>
        ///  Constructs an indexer with the given offset.
        /// </summary>
        /// <param name="offset"> Offset for indexing. </param>
        protected Indexer(int offset)
        {
            Offset = offset;
        }

        #endregion

        #region Method : Plain skeleton method

        /// <summary>
        ///  Empty Skeleton method
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual int Transform(int index)
        {
            //To be overriden by child classes
            return 0;
        }

        #endregion
    }
}
