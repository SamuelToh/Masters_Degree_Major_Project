using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

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
   /// The symbol class represents each character of a strand (a sequence of characters of an Alphabet).
   /// A symbol is identified by its name.
   /// <para></para>
   /// We can safely assume that two symbols are always equal if they have identical names. 
   /// In other words, symbols in different Alphabets but with equal names are both similiar symbols.
   /// <para></para>
   /// (e.g an Adenine in RNA and DNA are always the same)
   /// </summary>
    [Serializable()]
    public class Symbol
    {
        #region -- Public Fields --
        /// <summary>
        /// The opposite letter of symbol is also known as complement symbol 
        /// </summary>
        public Symbol Complement { get; set; }

        /// <summary>
        ///  The symbol letter
        /// </summary>
        public char Letter { get; internal set; }

        /// <summary>
        /// Name of symbol
        /// </summary>
        public String Name { get; internal set; }

        #endregion

        #region -- Protected Fields --

        /// <summary>
        /// The three letter code word for the symbol
        /// </summary>
        protected String Code { get; set; }

        #endregion

        #region -- Public Constructors --

        /// <summary>
        /// Simplest form of constructing a symbol.
        /// The complement of this symbol is set to the symbol itself.
        /// A symbol is identified by its name
        /// and two symbols are equal if they have the same name.
        /// </summary>
        /// <param name="letter"> Letter of the symbol. </param>
        /// <param name="code"> The three letter code for symbol. </param>
        /// <param name="name"> Full name for this symbol. </param>
        public Symbol(char letter, String code, String name)
        {
            Letter = letter;
            Complement = this;
            Name = String.Intern(name);
            Code = code;
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Two symbols are equal if the have same name or at least on of the is 
        /// a meta symbol which matches a set of symbols. This methods expects
        /// a non-null symbol as argument.
        /// <para></para>
        /// First the name of symbol is compared and 2nd comparison is done based on its meta type
        /// </summary>
        /// <param name="symbol">symbol Symbol to compare with.</param>
        /// <returns>true: symbols are equal, false: otherwise.</returns>
        public virtual Boolean Equals(Symbol symbol)
        {
            if (symbol.Name == this.Name)
                return true;


            if(symbol is SymbolMeta)
                return symbol.Equals(this);
 

            return false;
        }

        /// <summary>
        /// Returns a string representation of this symbol
        /// </summary>
        /// <returns> Name of Symbol </returns>
        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}
