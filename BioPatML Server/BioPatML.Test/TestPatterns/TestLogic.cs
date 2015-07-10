using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class TestLogic
    {
        private Sequence seq;
        private Logic logic;

        [TestInitialize]
        public void SetUp()
        {
            seq = new Sequence(AlphabetType.DNA, "ctgcagatgaaa");
            logic = new Logic("logic", Logic.OperationType.AND, 0.0);

            logic.Add(new Motif("motif1", AlphabetType.DNA, "ntg", 1.0));
            logic.Add(new Motif("motif2", AlphabetType.DNA, "cng", 1.0));
            logic.Add(new Motif("motif3", AlphabetType.DNA, "cnn", 1.0));
        }

        [TestMethod]
        public void TestMatchAND()
        {
            logic.Operation = Logic.OperationType.AND;
            Match match = logic.Match(seq, 1);
            Assert.AreEqual(1, match.Start);
            Assert.AreEqual(3, match.Length);
            Assert.AreEqual(1, match.Strand);
            Assert.AreEqual("ctg", match.Letters());
            Assert.AreEqual(seq, match.BaseSequence);
            Assert.AreEqual(1.0, match.Similarity, 1e-3);
            Assert.AreEqual(3, match.SubMatches.Count);

            match = logic.Match(seq, 3);
            Assert.AreEqual(null, match);

            match = seq.SearchBest(0, 0, logic);
            Assert.AreEqual(1, match.Start);
            Assert.AreEqual("ctg", match.Letters());
            Assert.AreEqual(3, match.SubMatches.Count);
        }


        /** Tests the OR logic  */
        [TestMethod]
        public void TestMatchOR()
        {
            logic.Operation = Logic.OperationType.OR;
            Match match = logic.Match(seq, 4);
            Assert.AreEqual(4, match.Start);
            Assert.AreEqual(3, match.Length);
            Assert.AreEqual(1, match.Strand);
            Assert.AreEqual("cag", match.Letters());
            Assert.AreEqual(seq, match.BaseSequence);
            Assert.AreEqual(1.0, match.Similarity, 1e-3);
            Assert.AreEqual(2, match.SubMatches.Count);

            match = logic.Match(seq, 11);
            Assert.AreEqual(null, match);

            match = seq.SearchBest(0, 0, logic);
            Assert.AreEqual(1, match.Start);
            Assert.AreEqual("ctg", match.Letters());
            Assert.AreEqual(3, match.SubMatches.Count);
        }

        /** Tests the EXOR logic  */
        [TestMethod]
        public void TestMatchEXOR()
        {
            logic.Operation = Logic.OperationType.XOR;
            Match match = logic.Match(seq, 7);
            Assert.AreEqual(7, match.Start);
            Assert.AreEqual(3, match.Length);
            Assert.AreEqual(1, match.Strand);
            Assert.AreEqual("atg", match.Letters());
            Assert.AreEqual(seq, match.BaseSequence);
            Assert.AreEqual(1.0, match.Similarity, 1e-3);
            Assert.AreEqual(1, match.SubMatches.Count);

            match = logic.Match(seq, 1);
            Assert.AreEqual(null, match);

            match = seq.SearchBest(0, 0, logic);
            Assert.AreEqual(7, match.Start);
            Assert.AreEqual("atg", match.Letters());
            Assert.AreEqual(1, match.SubMatches.Count);
        }

        [TestMethod]
        public void TestRead()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(  "BioPatMLXML/Logic.xml" ) );
            Logic pattern = (Logic)definition.Pattern;

            Assert.AreEqual("Logic", definition.Name);
            Assert.AreEqual("logic", pattern.Name);
            Assert.AreEqual(Logic.OperationType.AND, pattern.Operation);
            Assert.AreEqual(0.7, pattern.Threshold, 1e-3);
            Assert.AreEqual(0.9, pattern.Impact, 1e-3);

            Assert.AreEqual(2, pattern.Count);
            Assert.AreEqual("motif1", pattern.Patterns[0].Name);
            Assert.AreEqual("motif2", pattern.Patterns[1].Name);
        }

        [TestMethod]
        public void TestToXml()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(  "BioPatMLXML/Logic.xml" ) );

			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
			Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );

			Logic pattern = (Logic) def2.Pattern;

            Assert.AreEqual("Logic", definition.Name);
            Assert.AreEqual("logic", pattern.Name);
            Assert.AreEqual(Logic.OperationType.AND, pattern.Operation);
            Assert.AreEqual(0.7, pattern.Threshold, 1e-3);
            Assert.AreEqual(0.9, pattern.Impact, 1e-3);

            Assert.AreEqual(2, pattern.Count);
            Assert.AreEqual("motif1", pattern.Patterns[0].Name);
            Assert.AreEqual("motif2", pattern.Patterns[1].Name);
        }
    }
}
