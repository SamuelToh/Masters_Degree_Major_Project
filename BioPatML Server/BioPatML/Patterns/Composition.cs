using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using QUT.Bio.BioPatML.Symbols;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Common.XML;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Patterns
{
    /// <summary>
    /// This class defines a composition pattern. A composition pattern describes
    /// the symbol composition of a sequence section of variable length.
    /// </summary>
    public sealed partial class Composition : PatternFlexible
    {
        #region -- Fields --

        /// <summary>
        /// Alphabet the composition is based on
        /// </summary>
        public Alphabet Alphabet { get; private set; }

        #endregion

        #region -- Private Fields --

        /// <summary>
        /// Dictionary that maps a symbol to a weight 
        /// </summary>
        private Dictionary<Symbol, Double> SymDictionary { get; set; }

        /// <summary>
        /// Default weight
        /// </summary>
        private double defaultWeight = 0;

        /// <summary>
        /// Maximum weight 
        /// </summary>
        private double maxWeight = -Double.MaxValue;

        /// <summary>
        /// Minimum weight 
        /// </summary>
        private double minWeight = Double.MaxValue;

        /// <summary>
        /// Match mode: ALL or BEST 
        /// </summary>
        private String compositionMode ;

        /** Matcher used to match a composition description against a sequence */
        private IMatcher matcher;

        #endregion -- Private Fields --

        /// <summary>
        /// Default constructor for creating a plain Composition pattern object 
        /// </summary>
        internal Composition() : base("Composition" + Id.ToString()) { SymDictionary = new Dictionary<Symbol, double>(); }

        /// <summary>
        /// Constructs a composition pattern of variable length.
        /// </summary>
        /// <exception cref="System.ArgumentException">When given mode is invalid</exception>
        /// <param name="name">Name of the pattern.</param>
        /// <param name="alphabetName">Name of the alphabet the pattern operates on.</param>
        /// <param name="minLength">Minimum length of the sequence to match. </param>
        /// <param name="maxLength">Maximum length of the sequence to match.</param>
        /// <param name="incLength">Length increment for the pattern.</param>
        /// <param name="mode">Match mode: BEST, ALL</param>
        /// <param name="threshold">Threshold for the composition.</param>
        public Composition
            (String name, String alphabetName,
                int minLength, int maxLength, double incLength,
                String mode, double threshold)
            : base(name)
        {
            base.Set(minLength, maxLength, incLength);
            SymDictionary = new Dictionary<Symbol, double>();
            Mode = mode;
            Threshold = threshold;
            Alphabet = AlphabetFactory.Instance(alphabetName);
        }

        /// <summary>
        /// Gets the mode of this composition pattern
        /// <para></para>
        /// Note: Only internal library has the permission to change the mode of composition,
        /// even so, in most cases we are not suppose to tweak the mode of composition in the middle
        /// of computation.
        /// </summary>
        public string Mode
        {
            get { return this.compositionMode; }

            internal set
            {
                if (value.Equals("ALL"))
                    matcher = new MatcherAll(this);

                else
                    if (value.Equals("BEST"))
                        matcher = new MatcherBest(this);

                    else
                        throw new ArgumentException
                                    ("Invalid match mode: " + value);

                this.compositionMode = value;
            }
        }

        /// <summary>
        /// Adds a symbol and its weight to the composition.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when
        /// duplicate symbols were detected.</exception>
        /// <param name="symbol">Symbol to add.</param>
        /// <param name="weight">Weight of the symbol. Can be any value.</param>
        public void Add(Symbol symbol, double weight)
        {
            if (SymDictionary.ContainsKey(symbol))
                throw new ArgumentException
                    ("Duplicate symbol " + symbol + " in composition!");

            SymDictionary.Add(symbol, weight);

            if (weight > maxWeight)
                maxWeight = weight;
            
            if (weight < minWeight)
                minWeight = weight;
        }

        /// <summary>
        /// Adds a symbol and its weight to the composition.
        /// </summary>
        /// <param name="letter">Letter to add.</param>
        /// <param name="weight">Weight of the symbol. Can be any value.</param>
        public void Add(char letter, double weight)
        {
            Add(Alphabet[letter], weight);
        }

        /// <summary>
        /// Gets the symbol weight
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <returns>Returns the weight for the Symbol or the default weight if no weight
        /// for the symbol is defined.</returns>
        public double Weight(Symbol symbol)
        {
            //Fix on 17 March
            if(SymDictionary.ContainsKey(symbol))
                return SymDictionary[symbol];

            return defaultWeight; 
        }

        /// <summary>
        /// Gets the weight of a symbol
        /// </summary>
        /// <param name="letter">One letter code of the symbol.</param>
        /// <returns>Returns the weight for the Symbol or the default weight if no weight
        /// for the symbol is defined.</returns>
        public double Weight(char letter)
        {
            return Weight(Alphabet[letter]);
        }

        /// <summary>
        /// Gets the default weight value / sets the defaultweight value
        /// (only internal library are allowed to change the value of defaultWeight.
        /// </summary>
        public double DefaultWeight
        {
            get { return defaultWeight; }
            set
            {
                defaultWeight = value;
                if (value > maxWeight)
                    maxWeight = value;
                if (value < minWeight)
                    minWeight = value;
            }
        }

        /// <summary>
        /// Gets the Minimum weight
        /// </summary>
        public double MinWeight
        {
            get { return minWeight; }
        }

        /// <summary>
        /// Gets the maximum weight
        /// </summary>
        public double MaxWeight
        {
            get { return maxWeight; }
        }

        /// <summary>
        /// Implementation of the IMatcher interface. An any pattern matches any sequence.
        /// <see cref="QUT.Bio.BioPatML.Patterns.IMatcher">IMatcher interface</see>.
        /// </summary>
        /// <param name="sequence">Sequence to compare with.</param>
        /// <param name="position">Matching position.</param>
        /// <returns>A match object containning the search result</returns>
        public override Match Match(Sequence sequence, int position)
        {
            return matcher.Match(sequence, position);
        }

        /// <summary>
        /// The increment value used by match
        /// </summary>
        public override int Increment
        {
            get
            {
                return matcher.Increment;
            }
        }

        /// <summary>
        /// Reads the parameters and populate the attributes for this pattern.
        /// </summary>
        /// <param name="node">Composition Pattern node</param>
        /// <param name="definition">The container encapsulating this pattern</param>
        public override void ReadNode(XmlNode node, Definition definition)
        {
            base.ReadNode(node, definition);
            Threshold = XMLHelper.GetAttrValDouble(node, "threshold");
            Impact = XMLHelper.GetAttrValDouble(node, "impact");
            Mode = XMLHelper.GetAttrValueString(node, "mode");

            Alphabet = AlphabetFactory.Instance(XMLHelper.GetAttrValueString(node, "alphabet"));

            node = node.FirstChild;
            while (node != null)
            {           // read all symbol weights of the composition
                if (node.Name.Equals("Symbol"))
                {
                    char letter = XMLHelper.GetAttrValueString(node, "letter")[0];
                    double weight = XMLHelper.GetAttrValDouble(node, "weight");

                    if (!Alphabet.IsValid(letter))
                        throw new ArgumentException
                                    ("Invalid alphabet letter: '" + letter + "'!");
                    
                    Add(Alphabet[letter], weight);
                }

                if (node.Name.Equals("Default"))
                    DefaultWeight = (XMLHelper.GetAttrValDouble(node, "weight"));

                node = node.NextSibling;
            }
        }


    

    }
}
