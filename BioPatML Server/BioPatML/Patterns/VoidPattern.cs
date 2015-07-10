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
    /// This class defines a void pattern. This pattern always matches but
    /// the match is of length zero.
    /// </summary>
    public sealed class VoidPattern : Pattern
    {
        #region -- Constructor --
        /// <summary>
        /// Constructor - used when your void element has no unique name
        /// </summary>
        public VoidPattern() : base("Void" + Id.ToString()) { }

        /// <summary>
        /// Same as the above constructor, but with a given name
        /// </summary>
        /// <param name="name">Name of void element</param>
        public VoidPattern(string name) : base(name) { }

        #endregion

        #region -- IMatcher Implementation -- 
        /// <summary>
        /// See interface <see cref="QUT.Bio.BioPatML.Patterns.IMatcher">IMatcher</see>
        /// </summary>
        /// <param name="sequence">the sequence used for matching</param>
        /// <param name="position">position of match</param>
        /// <returns></returns>
        public override Match Match(Sequence sequence, int position)
        {
            Matched.Set(sequence, position, 0, sequence.Strand, 1.0);
            return Matched; 
        }

        #endregion

        #region -- BioPatML XML Read Component --
        /// <summary>
        /// Reads the parameters and populate the attributes for this pattern.
        /// </summary>
        /// <param name="definition">Definition wrapping this node element</param>
        /// <param name="node">The node with name Void</param>
        public override void ReadNode(XmlNode node, Definition definition)
        {
            PatternName = XMLHelper.GetAttrValueString(node, "name");
            Impact = XMLHelper.GetAttrValDouble(node, "impact");
        }

        #endregion

        #region -- ToString() --
        /// <summary>
        /// returns a string representation of void element
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Void: " + PatternName;
        }

        #endregion
    }
}
