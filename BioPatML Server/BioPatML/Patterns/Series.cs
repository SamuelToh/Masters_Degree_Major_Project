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
    /// This class is the base class for a pattern series. A pattern series is a 
    /// sequence of patterns (gaps are also patterns) which matches if all patterns
    /// of the series match in the given order. There are two derived classes
    /// which return ALL or the BEST match of the series.
    /// </summary>
    public abstract class Series : PatternComplex
    {
        #region -- Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the pattern.</param>
        public Series(string name)
            : base(name) { }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Adds a pattern to the pattern series.
        /// </summary>
        /// <param name="pat">Pattern to add.</param>
        /// <returns>Returns the added pattern.</returns>
        public IPattern Add(IPattern pat)
        {
            Patterns.Add(pat);
            //patternList.Add(pat);
            Matched.Add(pat.Matched);
            return pat;
        }

        /// <summary>
        /// Adds a pattern to the pattern series at the given index position.
        /// </summary>
        /// <param name="index">Index of insertion position.</param>
        /// <param name="pat">Pattern to add.</param>
        /// <returns>Returns the added pattern.</returns>
        public IPattern Add(int index, IPattern pat)
        {
            Patterns.Add(index, pat);
            //patternList.Insert(index, pat);
            Matched.Add(index, pat.Matched);
            return pat;
        }

        /// <summary>
        /// Returns a string representation of the series pattern.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Series: { ");
            for (int i = 0; i < Patterns.Count; i++)
                sb.Append(Patterns[i].ToString()).Append(' ');
            sb.Append(")");
            return sb.ToString();
        }

        #endregion

        #region -- BioPatML XML Read Component -- 
        
        /// <summary>
        /// Reads the parameters and populate the attributes for this pattern.
        /// </summary>
        /// <param name="node">Series Pattern node</param>
        /// <param name="definition">The container encapsulating this pattern</param>
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
