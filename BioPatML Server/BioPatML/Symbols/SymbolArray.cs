using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Alphabets;

/***************************************************************************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Symbols
{
    //[Serializable]
    /// <summary>
    ///  This class implements a wrapper class for an array of
    ///  <see cref="QUT.Bio.BioPatML.Symbols.Symbol"> Symbol </see>.
    /// </summary>
    public class SymbolArray : ISymbolArray
    {
        #region -- Public Fields --

        /// <summary>
        /// Alphabet of the symbol array
        /// </summary>
        public Alphabet SymAlphabet { get; internal set; }

        #endregion

        #region -- Private Fields --

        /// <summary>
        /// An array acting as a storage for our symbols.
        /// </summary>
        private Symbol[] Symbols { get; set; }
        
        #endregion

        #region -- Public Constructors --
        /// <summary>
        ///   Constructs a <see cref="QUT.Bio.BioPatML.Symbols.SymbolArray"> Symbol Array </see> 
        ///   base on the given symbol array.
        ///   <para></para>
        ///   Note - the symbol array is not copied. Any changes in the
        ///   symbol array will effect the content of SymbolArray as well.
        ///   <para></para>
        ///   Furthermore there is no check if the symbols within the array are valid.
        /// </summary>
        /// <param name="alphabet"> The alphabet the symbols in the array belong to. </param>
        /// <param name="symbols">  Array of symbols. </param>
        public SymbolArray
            (Alphabet alphabet, Symbol[] symbols)
        {
            Symbols = symbols;
            SymAlphabet = alphabet;
        }

        /// <summary>
        ///   Constructs a <see cref="QUT.Bio.BioPatML.Symbols.SymbolArray">SymbolArray</see>
        ///   based on the given character sequence.
        ///   <para></para>
        ///   The characters are converted to symbols according to the specified
        ///   alphabet. Unknown characters will be converted to the default symbol if
        ///   one is defined.
        /// </summary>
        /// <param name="alphabet"> The alphabet used to convert letters to symbols. </param>
        /// <param name="characters"> charSequence A character sequence with alphabet letters. </param>
        public SymbolArray
            (Alphabet alphabet, 
             IEnumerable<char> characters)
        {
            Symbols = new Symbol[characters.Count()];
            int i = 0;

            foreach (char ch in characters)
                Symbols[i++] = alphabet[ch];

            SymAlphabet = alphabet;

        }

        #endregion

        #region -- Properties of Symbol Array --

        /// <summary>
        /// Gets the symbol at the specified index position in Symbol Array.
        /// <see cref="QUT.Bio.BioPatML.Symbols.ISymbolArray"> 
        /// ISymbolArray - Method SymbolAt </see>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Symbol SymbolAt(int index)
        {
            return Symbols[index];
        }

        /// <summary>
        /// Total number of elements in our Symbol Array.
        /// </summary>
        public int Length
        {
            get
            {
                return Symbols.Length;
            }
        }

        /// <summary>
        ///  Gets the Alphabet property of this Symbol.
        /// </summary>
        public Alphabet Alphabet
        {
            get
            {
                return SymAlphabet;
            }
        }

        #endregion

        #region -- Overriding Method --

        /// <summary>
        ///  Retrieves the letters of this data structure
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();

            for (int i = 0; i < Symbols.Length; i++)
                buff.Append(Symbols[i].Letter);

            return (buff.ToString());
        }

        #endregion
    }
}
