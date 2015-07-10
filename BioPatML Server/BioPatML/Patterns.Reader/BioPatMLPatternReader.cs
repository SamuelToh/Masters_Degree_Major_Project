using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Common.XML;

/*****************| Queensland University Of Technology |********************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Patterns.Reader
{
    /// <summary>
    /// The Entry level for using this library, allows user to put in a BioPatML file into the Read method
    /// and the library will parse the file and return it as a IPattern from package: QUT.BioPatML.Patterns.
    /// <para></para>
    /// Here is an example of how the pattern file should roughly look like;
    /// <para></para>
    /// <example>
    /// <code>
    /// <xml version="1.0"><para></para>
    /// <BioPatML xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" 
    ///         xsi:noNamespaceSchemaLocation="BioPatML.xsd"><para></para>
    /// <Definition><para></para>
    ///   <Profile name="profile" threshold="0.7" mode="ALL" ><para></para>
    ///     <Element name="element1"><para></para>
    ///       <Motif name="motif1" alphabet="DNA" sequence="actg" threshold="0.9"/><para></para>
    ///     </Element><para></para>
    ///     <Element name="element2" reference="element1" minGap="1" maxGap="2" 
    ///              alignment="END" ><para></para>
    ///       <Motif name="motif2" alphabet="DNA" sequence="actg" threshold="0.9"/><para></para>
    ///     </Element><para></para>
    ///   </Profile><para></para>
    /// </Definition>
    /// </BioPatML>
    /// </xml>
    /// 
    /// </code>
    /// </example>
    /// <para></para>
    /// </summary>
    public sealed class BioPatMLPatternReader : IDisposable
    {
        #region -- Automatic C# Properties --
        /// <summary>
        /// Variable holding to our BioPatML XML Document
        /// </summary>
        private XmlDocument BioPatML { get; set; }

        /// <summary>
        /// A default constructor initializing the essential needs
        /// </summary>
        public BioPatMLPatternReader() { this.BioPatML = new XmlDocument(); }

        #endregion

        #region -- Pattern Reader Method --

        /// <summary>
        /// Process the given BioPatML XML file path and returns 
        /// a Definition containnign list of search patterns
        /// </summary>
        /// <param name="fileURL">The BioPatML file path</param>
        /// <returns>A definition containning a collection of sub patterns, if exist.</returns>
        public Definition ReadBioPatML(string fileURL)
        {
            if (BioPatML == null)
                BioPatML = new XmlDocument();

            BioPatML.Load(fileURL);

            XMLHelper.ValidateXML(BioPatML);
           
            //no exception thrown, we are safe to say the document is valid.

            return Definition.ReadNode(BioPatML.DocumentElement.FirstChild); 
        }

        /// <summary>
        /// Process the processed BioPatML XML file and returns 
        /// a Definition containning a list of search patterns
        /// </summary>
        /// <param name="bioDoc">Loads in a premade document</param>
        /// <returns>A definition containning a collection of sub patterns, if exist.</returns>
        public Definition ReadBioPatML(XmlDocument bioDoc)
        {
            if (BioPatML == null)
                BioPatML = new XmlDocument();

            BioPatML = (bioDoc);

            XMLHelper.ValidateXML(BioPatML);

            return Definition.ReadNode(BioPatML.DocumentElement.FirstChild); 
        }

        /// <summary>
        /// Process the raw xml data and return a collection of patterns as Definition object.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>A definition containning a collection of sub patterns, if exist.</returns>
        public Definition ReadBioPatML(TextReader reader)
        {
            if (BioPatML == null)
                BioPatML = new XmlDocument();

            BioPatML.Load(reader);

            XMLHelper.ValidateXML(BioPatML);

            return Definition.ReadNode(BioPatML.DocumentElement.FirstChild); 
        }

        #endregion

        #region -- IDisposable Members --

        /// <summary>
        /// Method to dispose on all objects implementing IDisposable interface
        /// </summary>
        public void Dispose() { BioPatML = null; }

        #endregion
    }
}
