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
namespace QUT.Bio.BioPatML.Symbols
{
    /// <summary>
    ///  This class implements a meta symbols. A meta symbol is described by a set 
    ///  of symbols which defines which symbols are equal to the meta symbol. If the 
    ///  symbol set is empty all symbols match the meta symbol.
    /// </summary>
    public class SymbolMeta : Symbol
    {
        #region -- Private Fields --
         
        /// <summary>
        /// A List encapsulating all the symbols within this meta
        /// </summary>
        private List<Symbol> symbolset = null;

        #endregion

        #region -- Constructors --
        /// <summary>
        ///  Creates a meta symbol. 
        /// </summary>
        /// <param name="letter"> The one character letter of a symbol.</param>
        /// <param name="code">   The three letter code of the symbol. </param>
        /// <param name="name">   The full name of the symbol.</param>
        public SymbolMeta(char letter, String code, String name)
            : base(letter, code, name) { }

        #endregion

        #region -- Properties --
        /// <summary>
        /// Total number of symbol elements within the symbol set.
        /// </summary>
        /// <returns> Returns the size our symbol set. </returns>
        public int SymbolNumber
        {
            get
            {
                return (symbolset == null ? 0 : symbolset.Count());
            }
        }

        /// <summary>
        /// Gets a symbol element at the specified index.
        /// </summary>
        /// <param name="index">Index of a symbol within the symbol set</param>
        /// <returns>Returns the symbol at the index position.</returns>
        public Symbol this[int index]
        {
            get
            {
                return symbolset[index];
            }
        }


        /// <summary>
        /// Gets a list of symbols the meta symbol matches to.<para></para>
        /// 
        /// </summary>
        public String Letters
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < SymbolNumber; i++)

                    sb.Append(this[i].Letter);

                return sb.ToString();
            }
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        ///  Adds a symbol to the set of symbols described by this meta symbol.
        /// </summary>
        /// <param name="symbol"> The symbol to add. </param>
        public void Add(Symbol symbol)
        {
            if (symbolset == null)
                this.symbolset = new List<Symbol>();

            symbolset.Add(symbol);
        }

        /// <summary>
        ///  Removes the given symbol from the symbol set.
        /// </summary>
        /// <param name="symbol"></param>
        public void Remove(Symbol symbol)
        {
            if (symbolset != null)
                
                if(symbolset.Contains(symbol))
                    symbolset.Remove(symbol);

        }

        /// <summary>
        ///  Replaces the first occurrences of a symbol(symbol1) with a specified replacement symbol (symbol2).
        /// </summary>
        /// <param name="symbol1"> First symbol (to be replaced). </param>
        /// <param name="symbol2"> Second symbol (replacement). </param>
        public void Replace(Symbol symbol1, Symbol symbol2)
        {
            int index = symbolset.IndexOf(symbol1);

            if (index >= 0)
            {
                symbolset.RemoveAt(index);
                symbolset.Insert(index, symbol2);
            }
                
        }
       
        /// <summary>
        ///  Returns true if the symbol set is empty or if the given symbol is in the
        ///  symbol set.
        /// </summary>
        /// <param name="symbol"> Given object </param>
        /// <returns> See summary </returns>
        public override Boolean Equals(Symbol symbol)
        {
            if (symbolset == null || symbol.Name == this.Name)
                return true;

            for (int i = 0; i < SymbolNumber; i++)

                if (this[i].Name == symbol.Name)
                    return true;

            return false;
        }

        #endregion
    }
}
