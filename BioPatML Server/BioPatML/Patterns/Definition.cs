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
    /// This class describes a pattern definition. A pattern definition is composed
    /// of a parameter list, a list of annotations, a list of other pattern 
    /// definition and the actual pattern description. 
    /// </summary>
    public sealed class Definition
    {
        #region -- Automatic Properties --
        /// <summary>
        /// Name of the definition
        /// </summary>
        public string DefinitionName { get; private set; }

        /// <summary>
        /// List of patterns within the definition tag
        /// </summary>
        public PatternList Patterns { get; private set; } 

       /// <summary>
       /// List of sub-definitions 
       /// </summary>
        public DefinitionList Definitions { get; private set; }

        /// <summary>
        /// Static counter used to generate unique names 
        /// </summary>
        private static int Counter { get; set; }

        #endregion

        #region -- Constructors --

        /// <summary>
        /// Constructs an empty definition with an unique default name 
        /// </summary>
        internal Definition()
        {
            Initialize();
            DefinitionName = "Definition" + (Counter++);
        }

        /// <summary>
        /// Constructs an empty pattern definition with a given name. 
        /// </summary>
        /// <param name="name">Name of the definition pattern.</param>
        public Definition(string name)
        {
            Initialize();
            DefinitionName = "Definition" + (Counter++);
            DefinitionName = name;
        }

        /// <summary>
        /// Construct a definition pattern with a desired name and also 
        /// a main pattern.
        /// <para></para>
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="pattern">The main pattern.</param>
        public Definition(string name, IPattern pattern)
        {
            Initialize();
            DefinitionName = "Definition" + (Counter++);
            DefinitionName = name;
            MainPattern = pattern;
        }

        /// <summary>
        /// Initialize the auto-implementated properties 
        /// </summary>
        private void Initialize()
        {
            Patterns = new PatternList();
            Definitions = new DefinitionList();
        }

        #endregion

        #region -- Properties --

        /// <summary>
        /// Gets the main pattern within this Definition (the first element in definition)
        /// Sets the main pattern within the definition.
        /// <para></para>
        /// Note that this
        /// methods replaces the pattern list of the definition by all patterns and
        /// sub-patterns of the given pattern.
        /// </summary>
        public IPattern MainPattern
        {
            get
            {
                return Patterns.Count > 0 ? Patterns[0] : null;
            }

            set
            {
                Patterns.Add(value);

                if (value is PatternComplex)
                    foreach (IPattern subpattern in ((PatternComplex)value).Patterns)
                        MainPattern = subpattern;
            }
        }

        /// <summary>
        /// Gets the sub definition by a given name.
        /// </summary>
        /// <param name="name">The name of sub definition within this definition.</param>
        /// <returns>Sub definition</returns>
        public Definition SubDefinition(String name)
        {
            return Definitions.definition(name);
        }

        #endregion

        #region -- BioPatML XML Read Component --
        /// <summary>
        /// A static method for reading the definition of a pattern.
        /// </summary>
        /// <param name="node">The definition node with the starting tag of Definition</param>
        /// <returns></returns>
        public static Definition ReadNode(XmlNode node)
        {
            Definition definition = new Definition(XMLHelper.GetAttrValueString(node, "name"));
            node = node.FirstChild;

            while (node != null)
            {
                if (node.Name.Equals("Annotations"))
                { /*AnnotationList.read(node, definition);*/}

                else
                    if (node.Name.Equals("Parameters"))
                    { /*ParameterList.read(node, definition);*/}

                else
                    if (node.Name.Equals("Definitions"))
                       DefinitionList.ReadNode(node, definition);


                else
                    if (node.NodeType == XmlNodeType.Element)
                       Pattern.ReadPattern(node, definition);

                node = node.NextSibling;
            }

            return definition;
        }

        /// <summary>
        /// Reads the definition of a pattern.
        /// </summary>
        /// <param name="document">XML document the definition is read from.</param>
        /// <returns></returns>
        public static Definition ReadNode(XmlDocument document)
        {
            return ReadNode(XMLHelper.TopLevelNode(document, "Definition"));
        }

        #endregion
    }
}
