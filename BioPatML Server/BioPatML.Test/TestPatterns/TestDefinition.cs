using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Common.XML;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Sequences;
using BioPatML.Test;
using System.Xml.Linq;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestPatterns
{
    [TestClass]
    public class TestDefinition
    {
        /** Tests the constructor */
        [TestMethod]
        public void TestConstructor()
        {
            Definition definition = new Definition("test", new VoidPattern("VoidPattern"));
            Assert.AreEqual("test", definition.Name);
            Assert.AreEqual("VoidPattern", definition.Pattern.Name);
            Assert.IsNotNull(definition.Pattern);
            Assert.IsNotNull(definition.Definitions);
        }

        /** Tests the setter for a pattern */
        [TestMethod]
        public void TestPattern()
        {
            Series series1 = new SeriesBest("Series1", 1.0);
            series1.Add(new VoidPattern("VoidPattern1"));
            series1.Add(new VoidPattern("VoidPattern2"));
            Series series2 = new SeriesBest("Series2", 1.0);
            series2.Add(new VoidPattern("VoidPattern21"));
            series2.Add(series1);

            Definition definition = new Definition("test");
            definition.Pattern = (series2);
            Assert.AreEqual("Series2", definition.Pattern.Name);
            Assert.AreEqual("VoidPattern21", definition.Patterns[1].Name);
            Assert.AreEqual("Series1", definition.Patterns[2].Name);
            Assert.AreEqual("VoidPattern1", definition.Patterns[3].Name);
            Assert.AreEqual("VoidPattern2", definition.Patterns[4].Name);
        }


        /** Tests the setter and getter for a sub-definitions */
        [TestMethod]
        public void TestSubDefinition()
        {
            DefinitionList definitions = new DefinitionList();
            Definition definition = new Definition("test1", new VoidPattern("VoidPattern1"));
            definition.Definitions.Add(new Definition("test11", new VoidPattern("VoidPattern11")));
            definitions.Add(definition);
            definitions.Add(new Definition("test2", new VoidPattern("VoidPattern2")));
            definitions.Add(new Definition("test3", new VoidPattern("VoidPattern3")));
            Assert.AreEqual("VoidPattern1", definitions.definition("test1").Pattern.Name);
            Assert.AreEqual("VoidPattern11", definitions.definition("test1.test11").Pattern.Name);
            Assert.AreEqual("VoidPattern2", definitions.definition("test2").Pattern.Name);
            Assert.AreEqual("VoidPattern3", definitions.definition("test3").Pattern.Name);
            Assert.AreEqual(null, definitions.definition("test4"));
        }

        [TestMethod]
        public void TestReadDocument()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(  "BioPatMLXML/Definition.xml" ) );
            DefinitionList definitions = (DefinitionList)definition.Definitions;

            Assert.AreEqual("Def", definition.Name);
            Assert.AreEqual("motif", definition.Pattern.Name);
            Assert.AreEqual(2, definitions.Count);
            Assert.AreEqual("Def2", definitions.definition("Def2").Name);
            Assert.AreEqual("Def21", definitions.definition("Def2.Def21").Name);
            Assert.AreEqual("Def3", definitions.definition("Def3").Name);
        }

        [TestMethod]
        public void TestToXml()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader( "BioPatMLXML/Definition.xml" ) );
			
			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
			Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );
			
			DefinitionList definitions = (DefinitionList) def2.Definitions;

            Assert.AreEqual("Def", definition.Name);
            Assert.AreEqual("motif", definition.Pattern.Name);
            Assert.AreEqual(2, definitions.Count);
            Assert.AreEqual("Def2", definitions.definition("Def2").Name);
            Assert.AreEqual("Def21", definitions.definition("Def2.Def21").Name);
            Assert.AreEqual("Def3", definitions.definition("Def3").Name);
		}

    }
}
