using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Symbols.Indexer;
using QUT.Bio.BioPatML.Alphabets;

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
    /// This abstract class serves as a base class for all accessors.
    /// <para></para>
    /// See 
    /// <see cref="QUT.Bio.BioPatML.Symbols.Accessor.AccessorFactory">AcessorFactory</see>
    /// for a list of available accessors.
    /// </summary>
    public abstract class AccessorBase : IAccessor
    {
        #region -- Protected Fields --

        /// <summary>
        /// length of a section of the symbol array
        /// </summary>
        protected int Length { get; set; }

        /// <summary>
        /// Indexer used to index symbols of the symbol array 
        /// </summary>
        protected internal IIndexer Indexer { get; set; }

        /// <summary>
        /// Symbol array that the accessor accesses
        /// </summary>
        protected ISymbolArray symbols;

        #endregion

        #region -- Constructors --

        /// <summary>
        ///  Creates an accessor.
        /// </summary>
        /// <param name="indexer"> indexer Indexer used to index symbols in the symbol array.</param>
        /// <param name="length">  length Length of a section of the symbol array.</param>
        /// <param name="symbols"> symbols Symbol array.</param>
        public AccessorBase
            (IIndexer indexer, int length, ISymbolArray symbols)
        {
            Indexer = indexer;
            Length = length;
            this.symbols = symbols;
        }

        #endregion

        #region -- Properties of AccessorBase --

        /// <summary>
        ///  Properties for the alphabet of the underlying symbol array.
        /// </summary>
        public Alphabet Alphabet
        {
            get
            {
                return this.symbols.Alphabet;
            }
        }

        /// <summary>
        /// Properties for the symbol array that the accessor is working on.
        /// <para></para>
        /// This is a virtual method.
        /// </summary>
        public virtual ISymbolArray Symbols
        {
            get
            {
                return this.symbols;
            }
        }

        #endregion

        #region ISymbolArray Members

        /// <summary>
        /// Properties for the length of symbol array the accessor works on
        /// </summary>
        int ISymbolArray.Length
        {
            get { return Length; }
        }

        /// <summary>
        ///    Getter for the symbol at the given index position. This method
        ///    relies on the transform method of the derived accessor and might be
        ///    overridden for more complex accessors.
        ///    <para>
        ///    </para>
        ///    This is a virtual Method.
        /// </summary>
        /// <param name="index">position used for extracting the desired symbol</param>
        /// <returns></returns>
        public virtual Symbol SymbolAt(int index)
        {
            return symbols.SymbolAt(Transform(index));
        }

        /// <summary>
        ///  Skeleton method for Transform (To supress the interface's need)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual int Transform(int index)
        {
            //TO BE OVERRIDEN
            return index;
        }

        #endregion
    }
}
