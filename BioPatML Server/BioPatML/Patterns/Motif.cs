using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using QUT.Bio.BioPatML.Symbols;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Common.XML;
using QUT.Bio.BioPatML.Sequences;

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
    /// This class implements a pattern described by a motif sequence. Alternative
    /// symbols at a position are allowed and implemented as MetaSymbols.
    /// </summary>
    public sealed class Motif : Pattern
    {
        #region -- Automatic Properties -- 
        /// <summary>
        /// List of symbols in this motif element
        /// </summary>
        private List<Symbol> MotifSymbols { get; set; }

        #endregion

        #region -- Constructors -- 

        /// <summary>
        /// Internal Constructor. 
        /// This constructor will create a motif pattern with a unique name.
        /// </summary>
        internal Motif() : base("Motif" + Id.ToString()) { MotifSymbols = new List<Symbol>(); }

        /// <summary>
        /// Creates a new motif pattern. The motif is provided as a letter string
        /// </summary>
        /// <param name="name">Name of the pattern.</param>
        /// <param name="alphabetName">Name of the alphabet of the motif.</param>
        /// <param name="motif">Motif description.</param>
        /// <param name="threshold">Similarity threshold. </param>
        public Motif
            (String name, String alphabetName, String motif, double threshold)
                : base(name)
        {
            MotifSymbols = new List<Symbol>(); 
            Threshold = threshold; //set threshold value to base class
            Parse(alphabetName, motif);
        }

        #endregion

        #region -- Properties --

        /// <summary>
        /// Gets the letters of the motif pattern.
        /// </summary>
        public String Letters
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < MotifSymbols.Count; i++)
                {
                    Symbol sym = MotifSymbols[i];

                    if (sym.Name == "Alternative")
                        sb.Append("[" + ((SymbolMeta)sym).Letters + "]");

                    else
                        sb.Append(sym.Letter);

                }

                return sb.ToString();
            }
        }

        #endregion -- Properties --

        #region -- Public Methods --

        /// <summary>
        /// Parses the motif description and generates a symbol array that describes
        /// the motif. Alternatives are described by MetaSymbols.
        /// </summary>
        /// <param name="alphabet">Alphabet of the motif. </param>
        /// <param name="motif">Motif description.</param>
        /// <returns>Returns a symbol array.</returns>
        public static List<Symbol> Symbols
            (Alphabet alphabet, String motif)
        {
            List<Symbol> mySymbols = new List<Symbol>();
            SymbolMeta alternative = null;

            for (int i = 0; i < motif.Length; i++)
            {
                char letter = motif[i];

                if (letter == '[')
                {
                    if (alternative != null)
                        throw new ArgumentException
                            ("'[' within alternative is not permitted");

                    alternative = new SymbolMeta('#', "ALT", "Alternative");
                }

                else
                    if (letter == ']')
                    {
                        if (alternative == null)
                            throw new ArgumentException
                                ("Opening bracket for ']' is missing!");

                        mySymbols.Add(alternative);
                        alternative = null;
                    }

                 else 
                     if (alternative != null)
                        alternative.Add(alphabet[letter]);

                 else
                     mySymbols.Add(alphabet[letter]);

            }

            if (alternative != null)
                throw new ArgumentException
                    ("']' is missing");

            return mySymbols;

        }

        /// <summary>
        /// Implementation of the IMatcher interface. An any pattern matches any sequence.
        /// <see cref="QUT.Bio.BioPatML.Patterns.IMatcher">IMatcher interface</see>.
        /// </summary>
        /// <param name="sequence">Sequence to compare with.</param>
        /// <param name="position">Matching position.</param>
        /// <returns>A match object containning the search result</returns>
        public override Match Match
            (Sequence sequence, int position)
        {
            int length = MotifSymbols.Count;
            int mismatches = 0;
            int maxMismatches = (int)(length * (1 - Threshold));

            for (int i = 0; i < length; i++)

                if (!MotifSymbols[i].Equals(sequence.GetSymbol(position + i)))
                    if (++mismatches > maxMismatches)
                        return (null);

            Matched.Set(sequence, position, length, sequence.Strand,
                                (Double)(length - mismatches) / length);

            return (Matched);
        }

        /// <summary>
        /// Parses the motif and creates an internal representation as symbol array.
        /// </summary>
        /// <param name="alphabetName">Name of the alphabet of the motif.</param>
        /// <param name="motif">Motif description.</param>
        private void Parse
            (String alphabetName, String motif)
        {
            MotifSymbols = Symbols(AlphabetFactory.Instance(alphabetName), motif);
        }


        #endregion 

        #region -- BioPatML XML Read Component --

        /// <summary>
        /// Reads the parameters and populate the attributes for this pattern.
        /// </summary>
        /// <param name="node">Motif Pattern node</param>
        /// <param name="definition">The container encapsulating this pattern</param>
        public override void ReadNode(XmlNode node, Definition definition)
        {
            PatternName = XMLHelper.GetAttrValueString(node, "name");

            Parse(XMLHelper.GetAttrValueString(node, "alphabet"),
                    XMLHelper.GetAttrValueString(node, "motif"));

            Threshold = XMLHelper.GetAttrValDouble(node, "threshold");
            Impact = XMLHelper.GetAttrValDouble(node, "impact");
        }

        #endregion
    }
}
