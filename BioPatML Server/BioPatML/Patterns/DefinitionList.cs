using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
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
    /// Implementation of a container (list) encapsulating other sub-defininitions
    /// </summary>
    public sealed class DefinitionList
    {
        #region -- Automatic Properties --

        /// <summary>
        /// Maps pattern names to definitions 
        /// </summary>
        private Dictionary<String, Definition> NamesDictionary { get; set; }

        #endregion

        #region -- Constructor --

        /// <summary>
        /// A default constructor helping to initialize the auto implementation of properties
        /// </summary>
        public DefinitionList()
        {
            NamesDictionary = new Dictionary<string, Definition>();
        }

        #endregion

        #region -- Properties --

        /// <summary>
        /// Counts and return the number of elements within the patter: name dictionary.
        /// </summary>
        public int Count
        {
            get { return NamesDictionary.Count; }
        }

        /// <summary>
        /// Retrieves the desired definition element by the unique definition name.
        /// </summary>
        /// <param name="name">Name of definition</param>
        /// <returns>The definition object</returns>
        public Definition this[String name]
        {
            get
            {
                return NamesDictionary.ContainsKey(name) ? NamesDictionary[name] : null; //fix on 19 march 2010
            }
        }


        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Adds a definition to the list. The name of the definition must be unique.
        /// No two definitions with the same name can be stored in the list.
        /// </summary>
        /// <exception cref="System.ArgumentException">thrown when name already exist</exception>
        /// <param name="definition">Definition to add.</param>
        public void Add(Definition definition)
        {
            String name = definition.DefinitionName;

            if (NamesDictionary.ContainsKey(name))
                throw new ArgumentException("Duplicate definition name: " + name);

            NamesDictionary.Add(name, definition);
        }

        /// <summary>
        /// Reads a definition from a URI and adds it to the list of definitions.
        /// </summary>
        /// <param name="uri">URI to load the definition from.</param>
        public void Add(String uri)
        {
            XmlDocument doc = XMLHelper.Load(uri);
            Definition definition = Definition.ReadNode(doc);
            Add(definition);
        }

        /// <summary>
        /// Gets a definition by name.
        /// </summary>
        /// <param name="name"> Name of the definition. Note that the name can be the dot
        /// separated path to any sub-definition with in the tree of definitions,
        /// e.g. def0.defsub3.defsubsub1</param>
        /// <returns></returns>
        public Definition definition(String name)
        {
            String[] names = Regex.Split(name, "\\.");
            
            Definition definition = this[names[0]];

            for (int i = 1; i < names.Length && definition != null; i++)
                definition = definition.Definitions[names[i]];


            return definition;
        }

        /// <summary>
        /// Returns a list of browsable definitions.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Definition> Definitions()
        {
            return NamesDictionary.Values;
        }

        #endregion

        #region -- BioPatML XML Read Component --

        /// <summary>
        /// Reads a list of definitions at the specified node. 
        /// This method recursivly calls the reading methods of the different definitions.
        /// </summary>
        /// <param name="node">Node of the XML the reading starts with.</param>
        /// <param name="def">Definition which definition list will be extend with
        /// all read definitions.</param>
        public static void ReadNode(XmlNode node, Definition def)
        {
            node = node.FirstChild;
            
            while (node != null)
            {
                if (node.Name.Equals("Definition"))
                    def.Definitions.Add(Definition.ReadNode(node));

                else
                    if (node.Name.Equals("Import"))
                        def.Definitions.Add(XMLHelper.GetAttrValueString(node, "uri"));


                node = node.NextSibling;
            }
        }

        #endregion
    }
}
