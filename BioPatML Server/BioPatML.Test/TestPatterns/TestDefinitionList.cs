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
    public class TestDefinitionList
    {

        /** Tests the constructor */
        [TestMethod]
        public void TestConstructor()
        {
            DefinitionList definitions = new DefinitionList();
            Assert.AreEqual(0, definitions.Count);
        }

        /** Tests the Adding of definitions */
        [TestMethod]
        public void TestAdd()
        {
            DefinitionList definitions = new DefinitionList();
            definitions.Add(new Definition("test1"));
            definitions.Add(new Definition("test2"));
            Assert.AreEqual(2, definitions.Count);
        }

        /** Test the getter for definitions */
        [TestMethod]
        public void TestDefinition()
        {
            DefinitionList definitions = new DefinitionList();
            Definition definition = new Definition("test1");
            definition.Definitions.Add(new Definition("test11"));
            definitions.Add(definition);
            definitions.Add(new Definition("test2"));
            definitions.Add(new Definition("test3"));
            Assert.AreEqual("test1", definitions.definition("test1").Name);
            Assert.AreEqual("test11", definitions.definition("test1.test11").Name);
            Assert.AreEqual("test2", definitions.definition("test2").Name);
            Assert.AreEqual("test3", definitions.definition("test3").Name);
            Assert.AreEqual(null, definitions.definition("test4"));
        }

        [TestMethod]
        public void TestRead()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(  "BioPatMLXML/BioPatMLdefinitionList1.test" ) );
            DefinitionList definitions = definition.Definitions;

            Assert.AreEqual("Def", definition.Name);
            Assert.AreEqual("Def1", definitions.definition("Def1").Name);
            Assert.AreEqual("Def11", definitions.definition("Def1.Def11").Name);
            Assert.AreEqual("Def2", definitions.definition("Def2").Name);
            Assert.AreEqual("Def3", definitions.definition("Def3").Name);
        }
  
         /** Tests the reading of a definition list with imports. */
        [TestMethod]  
        public void TestReadImport()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(  "BioPatMLXML/BioPatMLdefinitionList2.test" ) );
            DefinitionList definitions = definition.Definitions;

			// TODO: This does not match the contents of the referenced document.

			//Assert.AreEqual("Def", definition.Name);
			//Assert.AreEqual("Def1", definitions.definition("Def1").Name);
			//Assert.AreEqual("Def11", definitions.definition("Def1.Def11").Name);
			//Assert.AreEqual("Def2", definitions.definition("Def1.Def2").Name);
			//Assert.AreEqual("Def3", definitions.definition("Def1.Def3").Name);
        }
  
    }
}
