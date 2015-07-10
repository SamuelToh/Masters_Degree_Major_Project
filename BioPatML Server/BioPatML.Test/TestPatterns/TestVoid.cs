using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML;
using QUT.Bio.BioPatML.Common.XML;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Alphabets;
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
    public class TestVoid
    {
        /** test for match at sequence start */
        [TestMethod]
        public void TestMatchStart()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "atgc");
            VoidPattern pattern = new VoidPattern("Void");
            Match match = pattern.Match(seq, 1);
            Assert.AreEqual(1, match.Start);
            Assert.AreEqual(0, match.End);
            Assert.AreEqual(0, match.Length);
            Assert.AreEqual("", match.Letters());
        }  

        [TestMethod]
        public void TestRead()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(   "BioPatMLXML/Void.xml" ) );
            VoidPattern pattern = (VoidPattern) ((Series) definition.Pattern).Patterns[1];

            Assert.AreEqual("Void", definition.Name);
            Assert.AreEqual("void", pattern.Name);
            Assert.AreEqual(0.9, pattern.Impact, 1e-3);
        }

        [TestMethod]
        public void TestToXml()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(   "BioPatMLXML/Void.xml" ) );

			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
			Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );

			VoidPattern pattern = (VoidPattern) ( (Series) def2.Pattern ).Patterns[1];

            Assert.AreEqual("Void", def2.Name);
            Assert.AreEqual("void", pattern.Name);
            Assert.AreEqual(0.9, pattern.Impact, 1e-3);
        }
    }
}
