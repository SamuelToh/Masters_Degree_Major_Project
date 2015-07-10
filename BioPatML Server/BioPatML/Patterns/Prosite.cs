using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Symbols;
using QUT.Bio.BioPatML.Common.XML;

/*****************| Queensland University Of Technology |********************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Patterns
{
    /// <summary>
    ///  This class implements PROSITE patterns.
    ///  See http://au.expasy.org/txt/prosuser.txt for a description of prosite
    ///  patterns  In the following an excerpt from the prosite description:
    ///  
    ///  
    ///  <para></para>
    ///  The PA (PAttern) lines contains the definition of a PROSITE pattern.
    ///  The patterns are described using the following conventions:
    ///  <list type="bullet">
    ///  <para></para>
    ///  <item><description>
    /// - The standard IUPAC one-letter codes for the amino acids are used.
    /// </description></item>
    /// - The symbol 'x' is used for a position where any amino acid is
    /// accepted.
    /// <para></para>
    ///  <item><description>
    /// - Ambiguities are indicated by listing the acceptable amino acids for a
    /// given position, between square parentheses '[ ]'. For example: [ALT]
    /// stands for Ala or Leu or Thr.
    /// </description></item>
    /// <para></para>
    ///  <item><description>
    /// - Ambiguities are also indicated by listing between a pair of curly
    /// brackets '{ }' the amino acids that are not accepted at a given
    /// position. For example: {AM} stands for any amino acid except Ala and
    /// Met.
    /// </description></item>
    /// <para></para>
    ///  <item><description>
    /// - Each element in a pattern is separated from its neighbor by a '-'.
    /// </description></item>
    /// <para></para>
    ///  <item><description>
    /// - Repetition of an element of the pattern can be indicated by following
    /// that element with a numerical value or a numerical range between
    /// parenthesis. Examples: x(3) corresponds to x-x-x, x(2,4) corresponds
    /// to x-x or x-x-x or x-x-x-x.
    /// </description></item>
    /// <para></para>
    ///  <item><description>
    /// - When a pattern is restricted to either the N- or C-terminal of a
    /// sequence, that pattern either starts with a &lt; symbol or respectively
    /// ends with a '&gt;' symbol. In some rare cases (e.g. PS00267 or PS00539),
    /// '>' can also occur inside square brackets for the C-terminal element.
    /// 'F-[GSTV]-P-R-L-[G>]' means that either 'F-[GSTV]-P-R-L-G' or
    /// 'F-[GSTV]-P-R-L>' are considered.
    /// - A period ends the pattern.
    /// </description></item>
    /// <para></para>
    /// </list>
    /// <para></para>
    /// <example><code>
    /// Examples:<para></para>
    /// [AC]-x-V-x(4)-{ED}. <para></para>
    /// This pattern is translated as: [Ala or Cys]-any-Val-any-any-any-any-{any 
    /// but Glu or Asp}
    /// <para></para>
    /// &lt;A-x-[ST](2)-x(0,1)-V.  <para></para>
    /// This pattern, which must be at the N-terminal of the sequence ('&lt;'), <para></para>
    /// is translated as: Ala-any-[Ser or Thr]-[Ser or Thr]-(any or none)-Val   
    /// </code></example>
    /// </summary>
    public sealed class Prosite : RegularExp
    {
        #region -- Automatic Properties --

        /// <summary>
        /// String with the original prosite pattern 
        /// </summary>
        public String PrositeStr { get; private set; }

        /// <summary>
        /// Alphabet of the prosite pattern 
        /// </summary>
        public Alphabet PrositeAlphabet { get; private set; }

        #endregion -- Protected Fields --

        #region -- Constructors --

        /// <summary>
        /// The Default Constructor
        /// </summary>
        internal Prosite() { }

        /// <summary>
        /// Creates a prosite pattern. Please note, that this pattern will match only
        /// once for a sequence position, even if more matches are possible. The pattern 
        /// is greedy and will match with the longest possible part of the sequence.
        /// <p>
        /// Prosite patterns are internally converted to regular expression using a 
        /// simple character map. As a consequence not all possible syntax errors in 
        /// prosite patterns are recognized and PatternSyntaxException always refer to 
        /// the regular expression. 
        /// </p>
        /// See http://au.expasy.org/txt/prosuser.txt for a description of prosite
        /// patterns.
        /// </summary>
        /// <param name="pattern">Prosite pattern. </param>
        /// <param name="alphabet">Alphabet of the pattern.</param>
        public Prosite(String pattern, Alphabet alphabet)
        {
            this.PrositeAlphabet = alphabet;
            this.Init(Convert(pattern, alphabet), false);
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Converts a pattern in prosite format to a regular expression string.
        /// </summary>
        /// <param name="pattern">Pattern in prosite format.</param>
        /// <param name="alphabet">Alphabet used by the pattern.</param>
        /// <returns>Returns a regular expression string.</returns>
        public String Convert(String pattern, Alphabet alphabet)
        {
            PrositeStr = pattern;
            StringBuilder regex = new StringBuilder();

            for (int i = 0; i < pattern.Length; i++)
            {
                char ch = pattern[i];
                switch (ch)
                {
                    case '(': regex.Append('{'); break;
                    case ')': regex.Append('}'); break;
                    case '{': regex.Append("[^"); break;
                    case '}': regex.Append(']'); break;
                    case '<': regex.Append('^'); break;
                    case '>': regex.Append('$'); break;
                    case 'x': regex.Append('.'); break;
                    case 'X': regex.Append('.'); break;
                    case '.': break;
                    case '-': break;
                    default: regex.Append(Convert(ch, alphabet)); break;
                }
            }

            return regex.ToString();
        }

        /// <summary>
        /// Converts a char first into a symbol and then into a regular expression.
        /// This is usually trivial appart from ambiguity symbols, such as "R" within
        /// the DNA alphabet. They are represented as an alternative, <para></para>e.g. "[AG]
        /// to express the ambiguity.
        /// </summary>
        /// <param name="ch">Character to convert.</param>
        /// <param name="alphabet">Alphabet the character belongs to.</param>
        /// <returns>Returns the converted character.</returns>
        public String Convert(char ch, Alphabet alphabet)
        {
            if (alphabet.IsValid(ch))
            {
                Symbol sym = alphabet[ch];
                if (sym is SymbolMeta)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append('[');
                    for (int i = 0; i < ((SymbolMeta)sym).SymbolNumber; i++)
                        sb.Append(((SymbolMeta)sym)[i].Letter);
                    sb.Append(']');
                    return sb.ToString();
                }
                return "" + sym.Letter;
            }
            return "" + ch;
        }

        #endregion

        #region -- BioPatML XML Read Component --
        /// <summary>
        /// Reads the parameters and populate the attributes for this pattern.
        /// 
        /// </summary>
        /// <param name="node">Prosite Pattern node</param>
        /// <param name="definition">The container encapsulating this pattern</param>
        public override void ReadNode(System.Xml.XmlNode node, Definition definition)
        {
            PatternName = XMLHelper.GetAttrValueString(node, "name");
            Impact = XMLHelper.GetAttrValDouble(node, "impact");
            PrositeAlphabet = AlphabetFactory.Instance(XMLHelper.GetAttrValueString(node, "alphabet"));
            Init(Convert(XMLHelper.GetAttrValueString(node, "prosite"), PrositeAlphabet), false);
        }

        /// <summary>
        /// Returns the prosite string. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.PrositeStr;
        }

        #endregion
    }
}
