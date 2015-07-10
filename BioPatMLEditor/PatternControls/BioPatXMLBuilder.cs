using System;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using BioPatMLEditor.PatternControls.PatternModels;

namespace BioPatMLEditor.PatternControls
{
    /// <summary>
    /// A simple class that tells the pattern model what to add for their header and footer
    /// when compiling their BioPatML Language.
    /// </summary>
    public sealed class BioPatXMLBuilder
    {
        /// <summary>
        /// The standard start and end of our XML
        /// </summary>
        public static string PATTERN_XML_HEADER_FOOTER =
            "<?xml version=\"1.0\"?>" + Environment.NewLine +
             Environment.NewLine +
            "<BioPatML xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" + Environment.NewLine +
                "\t xsi:noNamespaceSchemaLocation=\"BioPatML.xsd\">" + Environment.NewLine +
                 Environment.NewLine + 
                "\t\t<Definition name = \"{0}\">" + Environment.NewLine +
                    "\t\t\t{1}" +
                    Environment.NewLine +
                "\t\t</Definition>" + Environment.NewLine +
                  Environment.NewLine + 
            "</BioPatML>";

        /// <summary>
        /// This method wraps up the body of XML with the appropriate header and footer.
        /// </summary>
        /// <param name="mainPattern">The first pattern found in our pattern tree.</param>
        /// <param name="DefinitionName">Name of definition we want this structure to be called.</param>
        /// <returns></returns>
        public static string BuildPatternXML(PatternBase mainPattern, string DefinitionName) 
        {
            return
                string.Format(PATTERN_XML_HEADER_FOOTER,
                                                DefinitionName,
                                                mainPattern.ElementXML());

        }
    }
}
