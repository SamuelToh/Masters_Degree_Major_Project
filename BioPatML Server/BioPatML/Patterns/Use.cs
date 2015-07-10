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
    /// This class defines a Use pattern. The Use pattern uses a pattern definition to
    /// match a patter. The Use pattern is convenient to use a pattern multiple
    /// times.
    /// </summary>
    public sealed class Use : Pattern
    {
        #region -- Automatic Properties --
        /// <summary>
        /// The definition to use
        /// </summary>
        public Definition Definition { get; set; }  
        
        /// <summary>
        /// main pattern of the definition
        /// </summary>
        private IPattern Pattern { get; set; }

        #endregion

        #region -- Constructor --

        /// <summary>
        /// The default internal constructor, you are not suppose to call this.
        /// Call Use(string, definition) instead
        /// </summary>
        internal Use() : base("Use" + Id.ToString()) { }

        /// <summary>
        /// Constructs a Use pattern.
        /// </summary>
        /// <param name="name">Name of the pattern.</param>
        /// <param name="definition">Reference to the pattern definition to use.</param>
        public Use
            (string name, Definition definition) 
            : base(name)
        {
            Set(definition);
        }

        /// <summary>
        /// Sets the pattern definition used by the Use pattern.
        /// </summary>
        /// <param name="definition"></param>
        private void Set(Definition definition)
        {
            Definition = definition;
            Pattern = definition.MainPattern;
        }

        #endregion

        #region -- Properties / ToString() --
        /// <summary>
        /// Gets the position increment after matching a pattern.
        /// </summary>
        public override int Increment
        {
            get
            {
                return Pattern.Increment;
            }
        }

        /// <summary>
        /// A string representation of Use element.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Use: '" + PatternName + "' -> " + Definition.DefinitionName;
        }

        #endregion

        #region -- IMatcher Implementation --

        /// <summary>
        /// The implementation ensures that
        /// a match fails for a given position if there is no match. Otherwise the
        /// matcher might return a match at a different position.
        /// <see cref="QUT.Bio.BioPatML.Patterns.IPattern">IPattern Match(Sequence, int) method</see>
        /// </summary>
        /// <param name="sequence"> The sequence for comparing</param>
        /// <param name="position"> Matching position</param>
        /// <returns></returns>
        public override Match Match
            (Sequence sequence, int position)
        {
            Match match = Pattern.Match(sequence, position);
            match.MatchPattern = this;
            return match;
        }

        #endregion

        #region -- BioPatML XML Read Component --

        /// <summary>
        /// Reads the parameters for a pattern at the given node.
        /// </summary>
        /// <param name="node">Use element</param>
        /// <param name="definition">Definition clause enclosing the Use element</param>
        public override void ReadNode
            (XmlNode node, Definition definition)
        {
            PatternName = (XMLHelper.GetAttrValueString(node, "name"));
            //Set(definition.Definitions[XMLHelper.GetAttrValueString(node, "definition")]);

            Set(definition.SubDefinition(XMLHelper.GetAttrValueString(node, "definition")));
        }

        #endregion
    }
}
