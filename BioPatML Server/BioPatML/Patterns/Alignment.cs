using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Common.XML;
using QUT.Bio.BioPatML.Sequences;

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
    /// This class implements an alignment pattern. An alignment pattern aligns 
    /// the match of another pattern with the start of the following pattern.
    /// The pattern does not consume symbols and it has no length.
    /// </summary>
    public sealed class Alignment : Pattern
    {
        #region -- Automatic Properties -- 
        /// <summary>
        /// Increment for position 
        /// </summary>
        public new int Increment { internal get; set; }

        /// <summary>
        /// Symbolic position: START, END, CENTER 
        /// </summary>
        public String Position { get; private set; }

        /// <summary>
        /// Offset for alignment 
        /// </summary>
        public int Offset {  get; private set; }

        /// <summary>
        /// Reference pattern for alignment
        /// </summary>
        public IPattern Pattern { get; private set; }

        #endregion -- Automatic Properties --

        #region -- Constructors --

        /// <summary>
        /// A constructor mainly for serialization.
        /// </summary>
        internal Alignment() : base("Alignment" + Id.ToString()) { }

        /// <summary>
        /// Constructs an alignment.
        /// </summary>
        /// <param name="name"> Name of the alignment. </param>
        /// <param name="pattern"> Pattern the cursor position is relative to. </param>
        /// <param name="position"> Symboloc position e.g. START, END, CENTER. </param>
        /// <param name="offset"> Offset to the specified alignment. Can be positive or negative.</param>
        public Alignment
            (String name, IPattern pattern, String position, int offset)
            : base(name)
        {
            Set(pattern, position, offset);
        }

        #endregion -- Constructor --

        #region -- Private Methods

        /// <summary>
        /// Sets the alignment parameters
        /// </summary>
        /// <param name="pattern"> Pattern the cursor position is relative to.  </param>
        /// <param name="position"> Symboloc position e.g. START, END, CENTER. </param>
        /// <param name="offset"> Offset to the specified alignment. Can be positive or negative</param>
        private void Set(IPattern pattern, string position, int offset)
        {
            if (pattern == null)
                throw new ArgumentNullException("No reference pattern specified!");

            Pattern  = pattern;
            Position = position;
            Offset = offset;
        }

        #endregion

        #region -- Overriding Members --

        /// <summary>
        /// Implementation of the <see cref="QUT.Bio.BioPatML.Patterns.IPattern">pattern interface</see>.
        /// </summary>
        /// <param name="sequence">Sequence to compare with.</param>
        /// <param name="position">Matching position.</param>
        /// <returns>The result.</returns>
        public override Match Match(Sequence sequence, int position)
        {
            int absAlignPos = AlignPosition();
            int absPos = sequence.Position() + position - 1;

            Increment = absAlignPos - absPos;

            Matched.Set(sequence, position + Increment, 0, sequence.Strand, 1.0);
            return Matched;
        }

        /// <summary>
        /// Reads the parameters for a pattern at the given node.
        /// </summary>
        /// <param name="node">The Alignment pattern node</param>
        /// <param name="definition">Definition encapsulating the pattern</param>
        public override void ReadNode(System.Xml.XmlNode node, Definition definition)
        {
            PatternName = (XMLHelper.GetAttrValueString(node, "name"));
            Impact = (XMLHelper.GetAttrValDouble(node, "impact"));
            Set(definition.Patterns[XMLHelper.GetAttrValueString(node, "pattern")],
                   XMLHelper.GetAttrValueString(node, "position"),
                   XMLHelper.GetAttrValInt(node, "offset"));
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return ("Alignment: " + PatternName +
                " Pattern=" + Pattern.PatternName +
                " Position=" + Position +
                " Offset=" + Offset);
        }

        #endregion -- Overriding Members --

        #region -- Public Methods --

        /// <summary>
        /// Calculates the absolute position of the alignment.
        /// </summary>
        /// <returns>The alignment position value</returns>
        public int AlignPosition()
        {
            Sequence match = Pattern.Matched;

            if (Position.Equals("START"))
                return Offset + match.Position();
            
            else
                if (Position.Equals("END"))
                    return Offset + match.Position() + match.Length;
            
            else
                if (Position.Equals("CENTER"))
                    return Offset + match.Position() + match.Length / 2;

            throw new ArgumentException("Invalid alignment type: " + Position);
        }

        #endregion
    }
}
