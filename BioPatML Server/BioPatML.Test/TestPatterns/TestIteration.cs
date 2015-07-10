using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Common.XML;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;
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
    public class TestIteration
    {
        /** Tests the constructor */
        [TestMethod]
        public void TestConstructor()
        {
            Motif motif = new Motif("motif", AlphabetType.DNA, "tgn", 0.0);
            Iteration iteration = new Iteration("test", motif, 3, 4, 0.0);

            Assert.AreEqual("test", iteration.Name);
            Assert.AreEqual(motif, iteration.Pattern);
            Assert.AreEqual(3, iteration.Minimum);
            Assert.AreEqual(4, iteration.Maximum);
        }

        /** Tests the match of an iteration pattern */
        [TestMethod]
        public void TestMatch() {
            Sequence  seq   = new Sequence(AlphabetType.DNA,"tttaagaacaagttt");
            Motif     motif = new Motif("motif", AlphabetType.DNA,"aag", 0.5);
            Iteration iteration;
            Match     match;

            iteration = new Iteration("test",motif,1,3,0.0);
            match     = iteration.Match(seq, 4);
            Assert.AreEqual(4, match.Start);
            Assert.AreEqual(9, match.Length);
            Assert.AreEqual(1, match.Strand);
            Assert.AreEqual("aagaacaag", match.Letters());
            Assert.AreEqual(seq, match.BaseSequence);
            Assert.AreEqual(0.888, match.Similarity, 1e-3);
            
            iteration = new Iteration("test",motif,1,1,0.0);
            match     = iteration.Match(seq, 4);
            Assert.AreEqual("aag", match.Letters());
            Assert.AreEqual(1.0, match.Similarity, 1e-3);
            
            iteration = new Iteration("test",motif,1,2,0.0);
            match     = iteration.Match(seq, 4);
            Assert.AreEqual("aagaac", match.Letters());
            Assert.AreEqual(0.833, match.Similarity, 1e-3);
              
            iteration = new Iteration("test",motif,4,5,0.0);
            Assert.AreEqual(null, iteration.Match(seq, 4));
        }

        /** Tests the match of an iteration of a variable pattern */
        [TestMethod]
        public void TestMatchVariablePattern()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "tttaagaacaagttt");
            Gap gap = new Gap("gap", 1, 4, 1, new double[] { 0.0, 0.1, 1.0, 0.5 }, 0.0);
            Iteration iteration;
            Match match;

            iteration = new Iteration("test", gap, 1, 3, 0.0);
            match = iteration.Match(seq, 4);
            Assert.AreEqual("aagaacaag", match.Letters());
            Assert.AreEqual(1.0, match.Similarity, 1e-3);
        }


        /** Tests the sub-match of an iteration pattern */
        [TestMethod]
        public void TestMatchSubMatches()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "tttaagaacaagttt");

            Motif motif = new Motif("motif", AlphabetType.DNA, "aag", 0.5);
            Iteration iteration = new Iteration("test", motif, 1, 3, 0.0);
            Match match = seq.SearchBest(0, 0, iteration);

            Assert.AreEqual("aag", match.Letters());
            Assert.AreEqual(10, match.Start);
            Assert.AreEqual(1.0, match.Similarity);
            Assert.AreEqual("aag", match.SubMatches[0].Letters()); //sub match to properties?

            FeatureList matches = seq.Search(0, 0, iteration);
            Assert.AreEqual(3, matches.Count);
            Assert.AreEqual("aagaacaag", matches[0].Letters());
            Assert.AreEqual(0.88, ((Match)matches[0]).Similarity, 1e-2);
            Assert.AreEqual("aacaag", matches[1].Letters());
            Assert.AreEqual(0.83, ((Match)matches[1]).Similarity, 1e-2);
            Assert.AreEqual("aag", matches[2].Letters());
            Assert.AreEqual(1.00, ((Match)matches[2]).Similarity, 1e-2);
        }

        [TestMethod]
        public void TestRead()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(  "BioPatMLXML/TestIteration.xml" ) );
            Iteration iteration = (Iteration)definition.Pattern;

            Assert.AreEqual("Iterator", definition.Name);
            Assert.AreEqual("iteration", iteration.Name);
            Assert.AreEqual(0.7, iteration.Threshold, 1e-3);
            Assert.AreEqual(0.9, iteration.Impact, 1e-3);
        }

        [TestMethod]
        public void TestToXml()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(  "BioPatMLXML/TestIteration.xml" ) );

			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
			Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );

			Iteration iteration = (Iteration) def2.Pattern;

            Assert.AreEqual("Iterator", definition.Name);
            Assert.AreEqual("iteration", iteration.Name);
            Assert.AreEqual(0.7, iteration.Threshold, 1e-3);
            Assert.AreEqual(0.9, iteration.Impact, 1e-3);
        }
    }
}
