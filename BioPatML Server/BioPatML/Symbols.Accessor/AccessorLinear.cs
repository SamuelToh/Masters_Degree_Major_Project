using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Symbols.Indexer;

namespace QUT.Bio.BioPatML.Symbols.Accessor
{
    /// <summary>
    /// Original Author: Dr Stefan Maetschke
    /// Translated By  : Samuel Toh (Email: yu.toh@connect.qut.edu.au)
    /// 
    /// Class Summary: 
    ///  This class describes a symbol array accessor which reads the symbols linear.
    /// </summary>
    internal sealed class AccessorLinear : AccessorBase
    {
        #region -- Private Fields --

        /** The default symbol which is returned when the index is out of bounds */
        private Symbol defaultSymbol;

        #endregion

        #region -- Public Constructor --

        /// <summary>
        ///  Creates an linear accessor.
        /// </summary>
        /// <param name="index">   Indexer used to index symbols in the symbol array. </param>
        /// <param name="length">  Length of a section of the symbol array.</param>
        /// <param name="symbols"> Symbol array.</param>
        public AccessorLinear
            (IIndexer index, int length, ISymbolArray symbols)
            : base (index, length, symbols)
        {
            this.defaultSymbol = symbols.Alphabet['-'];
        }

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
            return Indexer.Transform(index);
        }

        /// <summary>
        ///  Getter for the symbol at the specified index. 
        /// </summary>
        /// <param name="index"> A zero based index for a symbol of the array. </param>
        /// <returns> Returns the symbol at the given index or the default symbols
        ///           of the accessor (usually the gap symbol) if the index is out of bounds. </returns>
        public override Symbol SymbolAt(int index)
        {
            if (index < 0 || index >= Length)
                return defaultSymbol;

            return
                (symbols.SymbolAt(Transform(index)));

        }

        /// <summary>
        ///  Setter for the default symbol which is returned when the index for
        ///  getting a symbol from the array is out of bounds.
        /// </summary>
        /// <param name="defaultSymbol"> The defaultSymbol to set. </param>
        public void SetDefaultSymbol(Symbol defaultSymbol)
        {
            this.defaultSymbol = defaultSymbol;
        }

        #endregion
    }
}
