using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
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
    ///  This class defines an iteration pattern. This pattern iterates over its
    ///  sub-pattern.
    /// </summary>
    public sealed class Iteration : Pattern
    {
        #region -- Automatic Properties --
        /// <summary>
        /// The pattern that is iterated
        /// </summary>
        public IPattern Pattern { get; private set; }

        /// <summary>
        /// Minimum number of iterations required 
        /// </summary>
        public int Minimum { get; private set; }

        /// <summary>
        /// Maximum number of iterations required 
        /// </summary>
        public int Maximum { get; private set; }

        #endregion -- Automatic Properties --

        #region -- Constructors --
        /// <summary>
        /// The usual no param constructor, 
        /// builds an Iteration pattern with an "Iteration" + unique generated id name.
        /// </summary>
        public Iteration() : base("Iteration" + Id.ToString()) { }

        /// <summary>
        /// Constructs an iteration pattern.
        /// </summary>
        /// <param name="name">Name of the pattern.</param>
        /// <param name="pattern">The pattern to iterate over.</param>
        /// <param name="minimum">Minimum number of iterations.</param>
        /// <param name="maximum">Maximum number of iterations.</param>
        /// <param name="threshold">Similarity threshold</param>
        public Iteration
            (String name, IPattern pattern, int minimum, int maximum, double threshold)
            : base(name)
        {
            Threshold = threshold;
            Set(pattern, minimum, maximum);
        }

        /// <summary>
        /// Setter for iteration parameters.
        /// </summary>
        /// <param name="pattern">The pattern to iterate over.</param>
        /// <param name="minimum">Minimum number of iterations. </param>
        /// <param name="maximum">Maximum number of iterations.</param>
        private void Set(IPattern pattern, int minimum, int maximum)
        {
            if (maximum < minimum)
                throw new ArgumentException
                    ("Maximum smaller than minimum in iteration pattern!");

            if (minimum < 1)
                throw new ArgumentException
                    ("Minimum smaller than one in iteration pattern!");

            Pattern = pattern;
            Minimum = minimum;
            Maximum = maximum;
        }

        #endregion -- Constructors --

        #region -- IMatcher Members --
        /// <summary>
        /// Implementation of the IMatcher interface. An any pattern matches any sequence.
        /// <see cref="QUT.Bio.BioPatML.Patterns.IMatcher">IMatcher interface</see>.
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Match Match(Sequence sequence, int position)
        {
            Match match = Matched;
            int counter = 0;

            match.SubMatchedList = (null);
            while (counter < Maximum)
            {
                Match bestMatch = null;
                do
                {  // find best match at current position/iteration
                    Match nextMatch = Pattern.Match(sequence, position);
                    if (nextMatch != null && (bestMatch == null ||
                       nextMatch.Similarity >= bestMatch.Similarity))
                        bestMatch = nextMatch.Clone();
                } while (Pattern.Increment == 0);

                if (bestMatch == null)             // no match found
                    break;

                match.Add(bestMatch);             // save best match as sub-match
                position += bestMatch.Length;   // move position to end of match
                counter++;                        // inc. num. of successfull matches
            }

            match.CalcSimilarity();              // mean sim. over all sub-matches
            if (match.Similarity < Threshold || counter < Minimum)
            {
                match.Similarity = 0.0;
                match.SubMatchedList = null;
                return null;
            }

            // fill match object according to sub-matches
            match.CalcStartEnd();
            match.CalcLength();
            match.Strand = sequence.Strand;
            match.SetSequence(sequence); //TO CHANGE
            return match;
        }

        #endregion

        #region -- BioPatML XML Read Component --
        /// <summary>
        /// Reads the parameters and populate the attributes for this pattern.
        /// </summary>
        /// <param name="node">Any Pattern node</param>
        /// <param name="definition">The container encapsulating this pattern</param>
        public override void ReadNode(XmlNode node, Definition definition)
        {
            PatternName      = XMLHelper.GetAttrValueString(node, "name");
            Threshold = XMLHelper.GetAttrValDouble(node, "threshold");
            Impact    = XMLHelper.GetAttrValDouble(node, "impact");

            Set(PatternComplex.ReadPattern(node.FirstChild, definition),
                        XMLHelper.GetAttrValInt(node, "minimum"),
                        XMLHelper.GetAttrValInt(node, "maximum"));
        }
        #endregion
    }
}
