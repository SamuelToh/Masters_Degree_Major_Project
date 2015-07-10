using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Collections;

/***************************************************************************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Common.XML
{
    /// <summary>
    /// This class implements some helper functions for XML.
    /// </summary>
    public class XMLHelper : IDisposable
    {
        #region -- Private Fields --

        /** just stores the writer temporarily to make the writing methods simpler */
        [NonSerialized]
        private StreamWriter writer = null;

        /** this defines the indent for writing lines with elements */
        [NonSerialized]
        private String indent = "";

        /** buffer for writing of single characters */
        [NonSerialized]
        private char[] chbuf = new char[1];

        #endregion

        #region -- Public Constructor --

        /// <summary>
        /// Constructs an XML object. This constructor is only necessary when the 
        /// write methods are used. Most methods of this class are static.
        /// </summary>
        /// <param name="writer"></param>
        public XMLHelper(StreamWriter writer)
        {
            this.writer = writer;
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Increases the indent for the write methods.
        /// </summary>
        private void IncIndent()
        {
            indent += " ";
        }

        /// <summary>
        /// Decreases the indent for the write methods.
        /// </summary>
        private void DecIndent()
        {
            indent = indent.Substring(2);
        }

        /// <summary>
        /// Writes the start of an element tag to the {#writer}.
        /// Uses the {#indent} variable.
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="hasAttributes"></param>
        public void WriteElementStart(string elementName, bool hasAttributes)
        {
            writer.Write(indent + "<" + elementName);
            if (!hasAttributes)
                writer.Write(">\n");

            IncIndent();
        }

        /// <summary>
        /// Writes the end of an element tag to the {#writer}.
        /// </summary>
        /// <param name="elementName"> Name of the element or null. </param>
        public void WriteElementEnd(String elementName)
        {
            DecIndent();

            if (elementName == null)
                writer.Write(" />\n");

            else
                writer.Write(indent + "</" + elementName + ">\n");
        }

        /// <summary>
        /// Writes an attribute and its value to the {@link #writer}.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="value">Attribute value.</param>
        public void WriteAttribute(String attributeName, String value)
        {
            if (value != null)
                writer.Write(" " + attributeName + "=" + "\"" + EscapeXML(value) + "\"");
        }

        /// <summary>
        /// Writes the given data string with indent to the {#writer}.
        /// Uses the { #indent} variable.
        /// </summary>
        /// <param name="text">Data text to write.</param>
        public void WriteData(String text)
        {
            writer.Write(indent);
            writer.Write(text);
        }

        /// <summary>
        /// Write the string to the [#writer]
        /// </summary>
        /// <param name="text"></param>
        public void Write(String text)
        {
            writer.Write(text);
        }

        /// <summary>
        /// Writes the given character to the {#writer}. 
        /// </summary>
        /// <param name="ch"></param>
        public void Write(char ch)
        {
            chbuf[0] = ch;
            writer.Write(chbuf);
        }

        #endregion

        #region -- Public Static Methods --
        /// <summary>
        /// Getter for a specific text content of the given node.
        /// </summary>
        /// <param name="node">The desired node</param>
        /// <param name="name">name of the node to look out for</param>
        /// <returns></returns>
        static public String GetTextContent(XmlNode node, String name)
        {
            node = node.FirstChild;

            while (node != null)
            {
                if (node.Name.Equals(name))
                    return (node.InnerText);

                node = node.NextSibling;
            }

            return (null);
        }

        /// <summary>
        /// Getter for the attribute value of an attribute of the given name
        /// for the specified node. The attribute value is returned as a string.
        /// </summary>
        /// <param name="node"> Node </param>
        /// <param name="name" > Name of the attribute. </param>
        /// <returns>
        /// Returns the attribute value as string or null if the attribute
        /// does not exist.
        /// </returns>
        static public String GetAttrValueString(XmlNode node, String name)
        {
            return node.Attributes[name] != null ? node.Attributes[name].Value : null;
        }

        /// <summary>
        ///  Getter for the attribute value of an attribute of the given name
        ///  for the specified node. The attribute value is returned as a double.
        /// </summary>
        /// <param name="node"> Node </param>
        /// <param name="name"> Name of the attribute. </param>
        /// <returns>
        /// Returns the attribute value as a double value or 0.0 if
        /// the attribute does not exist. However in some special cases
        /// we return it as 1.0.
        /// </returns>
        static public Double GetAttrValDouble(XmlNode node, String name)
        {
            String value = GetAttrValueString(node, name);

            //If the user did not set value for impact, we assume it is 1.0 for the moment
            if (name == "increment" && value == null) { return 1.0; }

            //Temp fixed 
            if (name == "impact" && value == null) { return 1.0; }

            return (value != null ? Convert.ToDouble(value) : 0.0);
        }

        /// <summary>
        /// Getter for the attribute value of an attribute of the given name
        /// for the specified node. The attribute value is returned as a integer.
        /// </summary>
        /// <param name="node"> Node</param>
        /// <param name="name"> Name of the attribute. </param>
        /// <returns>
        /// Returns the attribute value as an integer or 0 if the
        /// attribute does not exist.
        /// </returns>
        static public int GetAttrValInt(XmlNode node, String name)
        {
            String value = GetAttrValueString(node, name);
            return (value != null ? Convert.ToInt32(value) : 0);
        }

        /// <summary>
        /// Converts characters with a special meaning in XML to their escaped
        /// representation in XML
        /// </summary>
        /// <param name="text">Text to convert.</param>
        /// <returns>Returns the converted text. </returns>
        static public String EscapeXML(String text)
        {
            StringBuilder sb = new StringBuilder();
            int len = (text != null) ? text.Length : 0;

            for (int i = 0; i < len; i++)
            {
                char ch = text[i];
                switch (ch)
                {
                    case '<': 
                        sb.Append("&lt;");
                        break;
                    case '>':
                        sb.Append("&gt;");
                        break;
                    case '&':
                        sb.Append("&amp;"); 
                        break;
                    case '"': 
                        sb.Append("&quot;"); 
                        break;
                    case '\'': 
                        sb.Append("&apos;");
                        break;

                    default: 
                        sb.Append(ch);
                        break;
                }
            }

            return sb.ToString();
        }

        //temp implementation
        /// <summary>
        /// Loads an XML document from the given file.
        /// </summary>
        /// <param name="fileURI"> XML file to load. </param>
        /// <returns>Load the parsed XML document</returns>
        static public XmlDocument Load(String fileURI)
        {
            XmlDocument document = new XmlDocument();

            document.Load(fileURI);

            return document;
        }

        /// <summary>
        /// Gets the top level node with the given name.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        static public XmlNode TopLevelNode(XmlDocument document, String name)
        {
            XmlNode node = document.DocumentElement.FirstChild;

            while (node != null)
            {
                if (node.Name.Equals(name))
                    return node;

                node = node.NextSibling;
            }

            return null;

        }

        #endregion

        #region -- IDisposable Member --

        /// <summary>
        /// Method to dispose on all objects implementing IDiposable interface.
        /// </summary>
        public virtual void Dispose()
        {
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
        }

        #endregion

        #region -- BioPatML Validator Method --

        /// <summary>
        /// Validates the biopatml file against the corresponding XML schema
        /// </summary>
        /// <param name="document">The XML document ready for verification</param>
        /// <returns>No excepted returns, method might throw exception if fails verification</returns>
        public static void ValidateXML(XmlDocument document)
        {
            XmlDocument doc = new XmlDocument();
            String errMsg = String.Empty;
        
            //doc.Load(documentURL);
            XmlNodeReader nodeReader = new XmlNodeReader(document);

            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add("", "BioPatML.xsd");
            settings.ValidationEventHandler += new ValidationEventHandler(MyValidationEventHandler);

            XmlReader reader = XmlReader.Create(nodeReader, settings);
            while (reader.Read()) ;
        }

        
        /// <summary>
        /// This method will be called if a XML contains invalid content while parsing.
        /// </summary>
        /// <exception cref="System.Xml.XmlException">Thrown when wrong xml format</exception>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void MyValidationEventHandler(object sender,
                                            ValidationEventArgs args)
        {
            throw new XmlException("Invalid XML format! " + args.Message);
        }
        

        #endregion
    }
}
