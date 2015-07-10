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
    ///  Factory for sequence accessor.
    /// </summary>
    public static class AccessorFactory
    {
        #region -- Factory Constant Variables --
        /// <summary>
        /// Linear reading for direct strand
        /// </summary>
        static public readonly int LIN_DIR = 0;

        /// <summary>
        /// Circular reading style for the sequence 
        /// </summary>
        static public readonly int CIRCULAR = 1;

        /// <summary>
        /// Transparent reading of the sequence. Note: Only for subsequences!
        /// </summary>
        static public readonly int TRANSPARENT = 2;

        /// <summary>
        /// Reading for reverse strand 
        /// </summary>
        static public readonly int REVERSE = 4;

        /// <summary>
        /// Reading for the complement 
        /// </summary>
        static public readonly int COMPLEMENT = 8;

        /// <summary>
        /// Transparent reading of the direct strand
        /// </summary>
        static public readonly int TRANS_DIR = TRANSPARENT;

        /// <summary>
        /// Transparent reading in reverse direction 
        /// </summary>
        static public readonly int TRANS_REV = TRANSPARENT | REVERSE;

        /// <summary>
        /// Circular reading of the direct strand 
        /// </summary>
        static public readonly int CIRC_DIR = CIRCULAR;

        /// <summary>
        /// Linear reading of the reverse complement 
        /// </summary>
        static public readonly int LIN_REV_COMP = REVERSE|COMPLEMENT;

        /// <summary>
        /// Circular reading of the reverse complement 
        /// </summary>
        static public readonly int CIRC_REV_COMP = CIRCULAR|REVERSE|COMPLEMENT;

        /// <summary>
        /// circular reading of the reverse complement 
        /// </summary>
        static public readonly int TRANS_REV_COMP = TRANSPARENT|REVERSE|COMPLEMENT;


        #endregion

        #region -- Factory Instance Implementations --
        /// <summary>
        ///  Creates an accessor with its {Indexer}.
        /// <para></para>
        /// <see cref="QUT.Bio.BioPatML.Symbols.Accessor.AccessorCircular">AccessorCircular</see>
        /// <para></para>
        /// <see cref="QUT.Bio.BioPatML.Symbols.Accessor.AccessorLinear">AccessorLinear</see>
        /// <para></para>
        /// <see cref="QUT.Bio.BioPatML.Symbols.Accessor.AccessorComplement">AccessorComplement</see>
        /// 
        /// </summary>
        /// <param name="type"> Type of the accessor. See constants.</param>
        /// <param name="ioff"> Offset for the indexer.</param>
        /// <param name="ilen"> Length for the indexer.</param>
        /// <param name="length"> Length of the accessor section.</param>
        /// <param name="symbols"> Symbol array the accessor works on.</param>
        /// <returns> Returns an accessor. </returns>
        public static AccessorBase Instance
            (int type, int ioff, int ilen, int length, ISymbolArray symbols)
        {
            AccessorBase accessor   = null;
            Indexer.Indexer indexor = null;


            //Create the chosen Indexer
            if ((type & REVERSE) > 0)
                indexor = new IndexerReverse(ioff, ilen);

            else
                indexor = new IndexerDirect(ioff);


            //create the chosen accessor
            if ((type & CIRCULAR) > 0)
                accessor = new AccessorCircular(indexor, length, symbols);

            else
                if ((type & TRANSPARENT) > 0)
                    accessor = new AccessorTransparent(indexor, length, symbols);

            else
                accessor = new AccessorLinear(indexor, length, symbols);


            if ((type & COMPLEMENT) > 0)
                accessor = new AccessorComplement(accessor);


            return accessor;
        }

        #endregion
    }
}
