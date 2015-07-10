using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
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
    /// This class describes a set of patterns which can be matched 
    /// against a sequence.
    /// </summary>
    public class Set : PatternComplex
    {
        #region -- Protected Fields --
        /// <summary>
        /// Position increment after a match 
        /// </summary>
        protected int increment;

        #endregion

        #region -- Constructors --
        /// <summary>
        /// Default constructor that takes in the name of Set element
        /// </summary>
        /// <param name="name">Name of element</param>
        public Set(string name) : base(name) { /* NO IMPLEMENTATION */}

        #endregion

        #region -- Properties --

        /// <summary>
        /// Return the minimum increment over all patterns within the set
        /// 
        /// </summary>
        /// <returns></returns>
        public override int Increment
        {
            get
            {
                return this.increment;
            }
        }

      

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Adds a pattern to the pattern set.
        /// </summary>
        /// <param name="pattern">Pattern to add.</param>
        public void Add(IPattern pattern)
        {
            base.Patterns.Add(pattern);
        }

       
        /// <summary>
        /// Returns a string representation of the pattern set.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Set: '" + PatternName + "'=");

            for (int i = 0; i < base.Count; i++)
            {
                sb.Append("{" + this[i].ToString() + "}");
            }

            return (sb.ToString());
        }

        #endregion

        #region -- BioPatML XML Reader Component --

        /// <summary>
        /// Implementation of the pattern interface.
        /// 
        /// <see cref="QUT.Bio.BioPatML.Patterns.IPattern">IPattern ReadNode(Node)</see>
        /// </summary>
        /// <param name="node">The Set[ALL/Best] Element</param>
        /// <param name="definition"></param>
        public override void ReadNode(XmlNode node, Definition definition)
        {
            PatternName = XMLHelper.GetAttrValueString(node, "name");
            Threshold = XMLHelper.GetAttrValDouble(node, "threshold");
            Impact = XMLHelper.GetAttrValDouble(node, "impact");

            node = node.FirstChild;
            while (node != null)
            {
                if (node.NodeType == XmlNodeType.Element)
                    Add(PatternComplex.ReadPattern(node, definition));
                node = node.NextSibling;
            }
        }


        #endregion
    }
}
