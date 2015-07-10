using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using QUT.Bio.BioPatML.Sequences;
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
    ///  This class implements the repeat pattern. A repeat pattern consists of a
    ///  reference pattern and a repeat element pattern which is the repeat of
    ///  the reference pattern. More complex repeats can be built by using profiles
    ///  and repeat element patterns directly. See {RepeatElement} for an example.
    /// </summary>
    public sealed partial class Repeat : Pattern
    {
        #region -- Automatic Properties --

        /// <summary>
        /// The profile which contains the reference pattern and the repeat element 
        /// </summary>
        public IPattern RefPattern { get; private set; }

        /// <summary>
        /// Match mode 
        /// </summary>
        public String Mode { get; private set; }

        /// <summary>
        /// Matcher used to match a repeat 
        /// </summary>
        private IMatcher matcher;

        /// <summary>
        /// Weight matrix for symbol pairing
        /// </summary>
        private double[,] weights;

        
        #endregion -- Protected Fields --

        #region -- Constructors --

        /// <summary>
        /// The Default constructor
        /// </summary>
        internal Repeat() : base("Repeat" + Id.ToString()) { }

        /// <summary>
        ///  Constructs a repeat pattern.
        /// </summary>
        /// <param name="name">Name of the repeat.</param>
        /// <param name="pattern">The Pattern to repeat.</param>
        /// <param name="mode">DIRECT, INVERTED</param>
        /// <param name="threshold">Similarity threshold</param>
        public Repeat
            (string name, IPattern pattern, string mode, double threshold)
            : base(name)
        {
            Threshold = threshold;
            Set(pattern, mode);
            weights = null;
        }


        /// <summary>
        /// Sets the attributes of repeat
        /// </summary>
        /// <param name="pattern">The pattern to repeat.</param>
        /// <param name="mode">Repeat mode: DIRECT, INVERTED</param>
        /// <exception cref="System.ArgumentException">thrown when reference pattern is null</exception>
        private void Set(IPattern pattern, string mode)
        {
            if (pattern == null)
                throw new ArgumentException
                    ("No referece pattern specified!");


            RefPattern = pattern;
            Mode = mode;

            if (Mode.Equals("DIRECT"))
                matcher = new MatcherDirect(this);

            else
                if (mode.Equals("INVERTED"))
                    matcher = new MatcherInverted(this);

                else
                    throw new ArgumentException
                        ("Invalid repeat mode: " + mode);

        }


        #endregion -- Public Constructors --

        #region -- Methods --
        /// <summary>
        /// Setter for a pairing weight.
        /// </summary>
        /// <param name="symbol1">First symbol.</param>
        /// <param name="symbol2">Second symbol.</param>
        /// <param name="weight">Weight for the given pairing of symbols. Has to be in interval [0,1]</param>
        internal void Weight(Symbol symbol1, Symbol symbol2, double weight)
        {
            Weight(symbol1.Letter, symbol2.Letter, weight);
        }

        /// <summary>
        /// Sets the pairing weight
        /// </summary>
        /// <param name="ch1">First character  (One letter code)</param>
        /// <param name="ch2">Second character  (One letter code)</param>
        /// <param name="weight">Returns the weight for the given pairing of symbols.</param>
        /// <exception cref="System.ArgumentException">Thrown when weight is less than 0 or more than 1.0</exception>
        internal void Weight(char ch1, char ch2, double weight)
        {
            if (weight < 0.0 || weight > 1.0)
                throw new ArgumentException
                    ("Invalid pairing weight " + weight);

            if (weights == null)
                /*********************************************************************************
                 * Fixed on 20 March 2010
                 * By Samuel Toh. 
                 * Arraysize changed to 21,21 to accomodate RNA alphabet [u].
                 * If we use the old size 20,20 when the user has a pairing alphabet of u program will hit
                 * array out of bound due to character 'A' - 'U' = 21.
                 * 
                 *******************************************************************************/
                weights = new double[21, 21];

            weights[Index(ch1), Index(ch2)] = weight;
        }

        /// <summary>
        /// Gets a pairing weight
        /// </summary>
        /// <param name="ch1">First character (One letter code)</param>
        /// <param name="ch2">Second character (One letter code)</param>
        /// <returns>Weight for the given pairing of symbols. Has to be
        /// in interval [0,1]</returns>
        public double Weight(char ch1, char ch2)
        {
            return weights[Index(ch1), Index(ch2)];
        }

        /// <summary>
        /// Gets a pairing weight
        /// </summary>
        /// <param name="symbol1">First symbol.</param>
        /// <param name="symbol2">Second symbol</param>
        /// <returns>Returns the weight for the given pairing of symbols.</returns>
        public double Weight(Symbol symbol1, Symbol symbol2)
        {
            return Weight(symbol1.Letter, symbol2.Letter);
        }

        /// <summary>
        /// Calculates the index of character within the weight matrix.
        /// </summary>
        /// <param name="ch">One letter code of a symbol.</param>
        /// <returns>Returns the array index.</returns>
        private int Index(char ch)
        {
            return char.ToUpper(ch) - ('A');
        }

        /// <summary>
        /// Returns a string representation of repeat pattern
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return ("Repeat: " + PatternName + " pattern=" + RefPattern.PatternName);
        }

        #endregion

        #region -- Implementation for IMatcher --

        /// <summary>
        /// The implementation ensures that
        /// a match fails for a given position if there is no match. Otherwise the
        /// matcher might return a match at a different position.
        /// <see cref="QUT.Bio.BioPatML.Patterns.IPattern">IPattern Match(Sequence, int) method</see>
        /// </summary>
        /// <param name="sequence"> the sequence for matching</param>
        /// <param name="position"> position used for matching</param>
        /// <returns>The matched</returns>
        public override Match Match(Sequence sequence, int position)
        {
            return matcher.Match(sequence, position);
        }

        #endregion

        #region -- BioPatML XML Read Component --
        /// <summary>
        /// Implementation of the pattern interface.
        /// <see cref="QUT.Bio.BioPatML.Patterns.IPattern">IPattern ReadNode(Node)</see>
        /// </summary>
        /// <param name="node">The Set[ALL/Best] Element</param>
        /// <param name="definition"></param>
        public override void ReadNode(XmlNode node, Definition definition)
        {
            PatternName = (XMLHelper.GetAttrValueString(node, "name"));
            Threshold = (XMLHelper.GetAttrValDouble(node, "threshold"));
            Impact = (XMLHelper.GetAttrValDouble(node, "impact"));

            Set(definition.Patterns[XMLHelper.GetAttrValueString(node, "pattern")],
                                                XMLHelper.GetAttrValueString(node, "mode"));

            node = node.FirstChild;

            while (node != null)
            {
                if (node.Name.Equals("Pairing"))
                {
                    char original = XMLHelper.GetAttrValueString(node, "original")[0];
                    char repeat = XMLHelper.GetAttrValueString(node, "repeat")[0];
                    double weight = XMLHelper.GetAttrValDouble(node, "weight");
                    Weight(original, repeat, weight);
                }

                node = node.NextSibling;
            }
        }

        #endregion
    }
}




