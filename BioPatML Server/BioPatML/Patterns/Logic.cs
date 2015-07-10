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
    /// This class describes a logic pattern. The logic pattern performs
    /// boolean operations such as AND, OR or XOR over a set of patterns.
    /// </summary>
    public class Logic : PatternComplex
    {
        #region -- Private Fields --
        /// <summary>
        /// Logical operation: <para></para>
        /// The Key values are "AND, OR, XOR"
        /// </summary>
        private string logicOperator;

        #endregion

        #region -- Constructor --
        /// <summary>
        /// The usual no param constructor, 
        /// builds an Logic pattern with an "Logic" + unique generated id name.
        /// <para></para>
        /// 
        /// </summary>
        internal Logic() : base("Logic" + Id.ToString()) { }

        /// <summary>
        /// A standard logic constructor, this constructor is recommended because
        /// it fills the neccessary attributes up for processing.
        /// </summary>
        /// <param name="name">Name of the pattern.</param>
        /// <param name="operation">Logical operation to perform.</param>
        /// <param name="threshold">Similarity threshold.</param>
        public Logic
            (string name, string operation, double threshold)
            : base(name)
        {
            Threshold = threshold;
            Operation = operation;
        }

        #endregion

        #region -- Properties --
        /// <summary>
        /// Gets/Sets the operation for logic object.
        /// <para></para>
        /// Usually we do not need to reset the operation to perform searches but
        /// in cases where there is a need to tweak the logic you may use this property.
        /// </summary>
        public String Operation
        {
            get { return logicOperator; }
            set
            {
                if (!(value.Equals("AND")  ||
                        value.Equals("OR") ||
                        value.Equals("XOR")))

                    throw new ArgumentException
                        ("Invalid logical operation: " + value);

                this.logicOperator= value;
            }
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// Attach a searching pattern to our logic list
        /// </summary>
        /// <param name="pattern">Your desired pattern</param>
        public void Add(IPattern pattern)
        {
            Patterns.Add(pattern);
        }

        /// <summary>
        /// Returns a string representation of our logic's name 
        /// plus operation and a list of pattern names attached to logic.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Logic: " + PatternName + " operation=" + Operation + "  ");
            for (int i = 0; i < base.Count; i++)
                sb.Append("{" + base.Patterns[i].PatternName + "}");
            return (sb.ToString());
        }

        #endregion

        #region -- IMatcher Members --

        /// <summary>
        /// Implementation of the IMatcher interface. An any pattern matches any sequence.
        /// <see cref="QUT.Bio.BioPatML.Patterns.IMatcher">IMatcher interface</see>.
        /// </summary>
        /// <param name="sequence">Sequence to compare with.</param>
        /// <param name="position">Matching position.</param>
        /// <returns>A match object containning the search result</returns>
        public override Match Match(Sequence sequence, int position)
        {
            Match match = Matched;
            match.SubMatchedList = null;
               
            foreach(IPattern pattern in Patterns) {
                Match nextMatch = pattern.Match(sequence, position);
                
                if(nextMatch != null)
                    match.Add(nextMatch);
                
            }
                
            int numPattern = Patterns.Count;
            int numMatches = match.SubMatchNumber;
    
            match.CalcSimilarity();              // mean sim. over all sub-matches

            if (match.Similarity < Threshold ||
                (Operation.Equals("AND") && numMatches != numPattern) ||
                (Operation.Equals("OR")  && numMatches == 0) ||
                (Operation.Equals("XOR") && numMatches > 1))
            {
                match.Similarity = 0.0;
                match.SubMatchedList = null;
                return null;
            }

            // fill match object according to sub-matches
            match.CalcStartEnd();
            match.CalcLength();
            match.Strand = sequence.Strand;
            match.SetSequence(sequence);
            return match;
        }

        #endregion

        #region -- BioPatML XML Read Component --

        /// <summary>
        /// Reads the parameters and populate the attributes for our Logic pattern.
        /// </summary>
        /// <param name="node">Logic Pattern node</param>
        /// <param name="definition">The container encapsulating this pattern</param>
        public override void ReadNode(XmlNode node, Definition definition)
        {
            PatternName = (XMLHelper.GetAttrValueString(node, "name"));
            Threshold = (XMLHelper.GetAttrValDouble(node, "threshold"));
            Impact = (XMLHelper.GetAttrValDouble(node, "impact"));
            Operation = (XMLHelper.GetAttrValueString(node, "operation"));

            node = node.FirstChild;                    // go to patterns of the set
            while (node != null)
            {                           // read all patterns
                if (node.NodeType == XmlNodeType.Element)   // skip non element nodes
                    Add(PatternComplex.ReadPattern(node, definition));   // read pattern
                node = node.NextSibling;
            }
        }

        #endregion   
    }
}
