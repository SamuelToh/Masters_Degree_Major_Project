using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using QUT.Bio.BioPatML.Symbols;
using QUT.Bio.BioPatML.Common.Structures;

/***************************************************************************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Alphabets
{
    /// <summary>
    /// An alphabet defines a set of symbols. The alphabet class specifically provides 
    /// a method to get the symbol for the corresponding symbol letter which is
    /// for instance needed to convert a string to a sequence of symbols.
    /// <para></para>
    /// The alphabets implements the IEnumerable interface and provides an enumerator
    /// that iterates over the NON-META symbols of the alphabet.
    /// See : 
    /// <see cref="QUT.Bio.BioPatML.Symbols.Symbol"> Symbol </see>
    /// <para></para>
    /// <see cref="QUT.Bio.BioPatML.Sequences.Sequence"> Sequence </see>
    /// </summary>
    [Serializable()]
    public class Alphabet : IEnumerable<Symbol>
    {
        #region -- Private fields --

        /// <summary>
        /// Name of the pre-defined set [DNA, RNA and AA] for the alphabet
        /// </summary>
        private string name;

        /// <summary>
        /// Map for all symbols [e.g. T C G A ] and meta symbols [ X and - ]
        /// </summary>
        private MapChar symbolMap = new MapChar();

        /// <summary>
        /// List of basic symbols = list of non-MetaSymbols 
        /// </summary>
        private List<Symbol> symbolList 
                            = new List<Symbol>();

        #endregion

        #region -- Public Constructors --

        /// <summary>
        ///  Creates an alphabet with the given name.
        /// </summary>
        /// <param name="name"> Name of the alphabet. </param>
        public Alphabet(String name)
        {
            this.name = name;
        }

        #endregion

        #region -- Properties --

#if DEBUG
        /// <summary>
        /// The default symbol which is returned for unknown letters
        /// </summary>
        public Symbol DefaultSymbol { get; set; }
#else
        /// <summary>
        /// The default symbol which is returned for unknown letters
        /// </summary>
        protected Symbol DefaultSymbol { get; set; }
#endif

        /// <summary>
        /// Getter for the name of the alphabet.
        /// </summary>
        public String Name
        {
            get
            {
                return this.name;
            }
        }
        

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Adds a symbol to the alphabet. Symbols within the alphabet can be retrived
        /// by the letter or by index.
        /// </summary>
        /// <param name="symbol">Symbol to add.</param>
        public virtual void Add(Symbol symbol)
        {
            symbolMap.Put(symbol.Letter, symbol);

            if (!(symbol is SymbolMeta))
                symbolList.Add(symbol);

        }

        /// <summary>
        /// Getter for a symbol within the alphabet. For unknown letters of the
        /// alphabet the method returns the symbol set by properties DefaultSymbol
        /// or throws an ArgumentException if no default symbol is set.
        /// 
        /// </summary>
        /// <exception cref="System.ArgumentException">ArgumentException</exception>
        /// <param name="letter">character</param>
        /// <returns></returns>
        public virtual Symbol this[char letter]
        {
            get
            {
                Symbol symbol =
                    (Symbol)symbolMap.Get(letter);


                if (symbol != null)  //Known alphabet
                    return (symbol);


                if (DefaultSymbol == null) //unknown letter and no default symbol
                    throw new ArgumentException
                        ("Unknown letter '" + letter + "' for alphabet " + Name);

                return (DefaultSymbol);
            }
        }

        /// <summary>
        /// Getter for a symbol within the alphabet.
        /// </summary>
        /// <param name="index">The index value of that particular symbol in this Alphabet</param>
        /// <param name="withMetaSymbols">if true, all symbol inclusive of all MetaSymbols. </param>
        /// <returns> Returns the symbol for the given index. </returns>
        public virtual Symbol this[int index, bool withMetaSymbols]
        {
            get
            {
                return ((Symbol)
                    (withMetaSymbols ? symbolMap.Get(index) : symbolList[index]));
            }
        }
 

        /// <summary>
        /// Removes the symbol with the given symbol letter from the symbolMap. 
        /// </summary>
        /// <param name="letter">
        /// Letter of a symbol, e.g 'a' for 'Adenine'
        /// </param>
        public void Remove(char letter)
        {
            symbolList.Remove(this[letter]);
            symbolMap.Remove(letter);
        }

        /// <summary>
        /// Returns the number of alphabet symbols with or without meta symbols. 
        /// </summary>
        /// <param name="withMetaSymbols">
        /// true: the methods returns the number of symbols
        /// including meta symbols, false: otherwise. See { #get(int, boolean)}.
        /// </param>
        /// <returns>
        /// Returns the size of the alphabet.
        /// </returns>
        public virtual int Count(bool withMetaSymbols)
        {
            return (withMetaSymbols ? symbolMap.Size : symbolList.Count);
        }

        /// <summary>
        /// Tests if a letter is a valid letter of the alphabet. This is an efficient
        /// test since all letters the alphabet are stored in a hash.
        /// </summary>
        /// <param name="letter"> Letter of a symbol. </param>
        /// <returns> true: the letter is valid, false: otherwise. </returns>
        public virtual bool IsValid(char letter)
        {
            return (symbolMap.Get(letter) != null);
        }

        /// <summary>
        /// Tests if a symbol is a valid symbol of the alphabet. This is an inefficient
        /// test because in the worst case all symbols of the alphabet have to be 
        /// compared against the symbol. Note that two symbols of two different 
        /// alphabets are equal if there name() references are equal. This means two
        /// symbols with different symbol references but equal name() references are
        /// equal.
        /// </summary>
        /// <param name="symbol"> The symbol to validate. </param>
        /// <returns> True: the symbol is valid, false: otherwise. </returns>
        public virtual bool IsValid(Symbol symbol)
        {
            for (int i = 0; i < this.Count(true); i++)
                if (this[i, true].Name == symbol.Name)
                    return true;


            return false;
        }

        #endregion

        #region IEnumerable<Symbol> Members

        /// <summary>
        /// Returns an enumerator for browsing the symbols within this Alphabet
        /// </summary>
        /// <returns>an AlphabetIEnumerator built based on this class</returns>
        public IEnumerator<Symbol> GetEnumerator()
        {
            return new AlphabetIEnumerator(this);
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator for browsing the symbols within this Alphabet
        /// </summary>
        /// <returns>AlphabetIEnumerator built based on this class</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new AlphabetIEnumerator(this);
        }

        #endregion
    }

    #region -- Enumerator Class for Alphabet --

    /// <summary>
    /// The enumerator class makes browsing of non-meta symbol possible within the Alphabet class.
    /// </summary>
    public class AlphabetIEnumerator : IEnumerator<Symbol>
    {
        /// <summary>
        /// The base Alphabet
        /// </summary>
        private Alphabet alphabet;
        /// <summary>
        /// current browsing index
        /// </summary>
        private int index = -1;

        #region -- Constructor --

        /// <summary>
        /// Building an enumerator base on the given Alphabet
        /// </summary>
        /// <param name="alphabet">The alphabet to be built based on</param>
        public AlphabetIEnumerator(Alphabet alphabet)
        {
            this.alphabet = alphabet;
        }

        #endregion

        #region -- IDisposable Members --

        /// <summary>
        /// Method to dispose on all objects implementing IDispose interface.
        /// </summary>
        public void Dispose() { }

        #endregion

        #region -- Public Members --

        /// <summary>
        /// Returns the current non-meta symbol of this Alphabet based on the index value
        /// </summary>
        public Symbol Current
        {
            get
            {
                if (index < 0)
                    return null;

                return alphabet[index, false];
            }
        }

        #endregion

        #region -- IEnumerator Members --

        /// <summary>
        /// Returns the current non-meta symbol of this Alphabet based on the index value
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get { return alphabet[index, false]; }
        }

        /// <summary>
        /// Iterate to the next symbol within the Alphabet.
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (index < (alphabet.Count(false) - 1))
            {
                index++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reset the iteration index back to first symbol
        /// </summary>
        public void Reset()
        {
            index = -1;
        }

        #endregion
    }

    #endregion -- Enumerator Class for Alphabet --
}
