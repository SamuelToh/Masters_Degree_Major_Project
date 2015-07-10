using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Common.XML;
using QUT.Bio.BioPatML.Alphabets;
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
    public class TestRepeat
    {
        private Series series;
        private Motif motif;
        private Gap gap;

        [TestInitialize]
        public void Setup()
        {
            series = new SeriesBest("series", 0.0);
            motif = new Motif("motif", AlphabetType.DNA, "accg", 1.0);
            gap = new Gap("gap", 3, 3);
            gap.Impact = 0.0;
            series.Add(motif);
            series.Add(gap);
        }

        [TestMethod]
        /** Tests the a match for a direct repeat pattern */
        public void TestMatchDirect()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "ttaccgtttaacgttt");
            Repeat repeat = new Repeat("", motif, Repeat.RepeatType.DIRECT, 0.5);
            series.Add(repeat);

            Match match = series.Match(seq, 3);
            Assert.AreEqual(0.875, match.Similarity);
            Assert.AreEqual("accgtttaacg", match.Letters());
            Assert.AreEqual("aacg", match.SubMatches[2].Letters());
            Assert.AreEqual(0.75, match.SubMatches[2].Similarity);
            Assert.AreEqual(10, match.SubMatches[2].Start);
            Assert.AreEqual(4, match.SubMatches[2].Length);
            Assert.AreEqual(1, match.SubMatches[2].Strand);

            match = seq.SearchBest(0, 0, series);
            Assert.AreEqual("accgtttaacg", match.Letters());

        }

        [TestMethod]
        /** Tests the a match for an inverted repeat pattern */
        public void TestMatchInverted()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "ttaccgtttgcaattt");
            Repeat repeat = new Repeat("repeat", motif, Repeat.RepeatType.INVERTED, 0.5);
            series.Add(repeat);

            Match match = series.Match(seq, 3);
            Assert.AreEqual(0.875, match.Similarity);

            Assert.AreEqual("accgtttgcaa", match.Letters());
            Assert.AreEqual("gcaa", match.SubMatches[2].Letters());
            Assert.AreEqual(0.75, match.SubMatches[2].Similarity);
            Assert.AreEqual(10, match.SubMatches[2].Start);
            Assert.AreEqual(4, match.SubMatches[2].Length);
            Assert.AreEqual(1, match.SubMatches[2].Strand);

            match = seq.SearchBest(0, 0, series);
            Assert.AreEqual("accgtttgcaa", match.Letters());
        }

        [TestMethod]
        /** Tests a weighted repeat */
        public void TestMatchWeighted()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "ttaccgtttaacgttt");
            Repeat repeat = new Repeat("repeat", motif, Repeat.RepeatType.DIRECT, 0.1);
            series.Add(repeat);
            repeat.Weight('a', 'a', 0.3);
            repeat.Weight('c', 'a', 1.0);
            repeat.Weight('g', 'g', 0.7);

            Match match = series.Match(seq, 3);
            Assert.AreEqual(0.75, match.Similarity);
            Assert.AreEqual("accgtttaacg", match.Letters());
            Assert.AreEqual("aacg", match.SubMatches[2].Letters());
            Assert.AreEqual(0.5, match.SubMatches[2].Similarity, 1e-3);

            match = seq.SearchBest(0, 0, series);
            Assert.AreEqual("accgtttaacg", match.Letters());
        }

        /** Tests the reading of a repeat pattern */
        [TestMethod]
        public void TestRead()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(   "BioPatMLXML/Repeat1.test" ) );
            Assert.AreEqual("Repeat", definition.Name);
            Repeat pattern = (Repeat)((Series)definition.Pattern).Patterns[1];

            Assert.AreEqual("repeat", pattern.Name);
            //Assert.AreEqual(0.2, pattern.Impact, 1e-3);
            Assert.AreEqual(0.3, pattern.Threshold, 1e-3);
        }

		[TestMethod]
		public void TestRead1 () {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader( "BioPatMLXML/BioPatMLrepeat1.test" ) );
			Repeat pattern = (Repeat) ( (Series) definition.Pattern ).Patterns[2];

			Assert.AreEqual( "repeat", pattern.Name );

			Assert.AreEqual( 0.9, pattern.Impact, 1e-3 );

			Assert.AreEqual( 0.0, pattern.Weight( 'c', 'c' ) );
			Assert.AreEqual( 0.1, pattern.Weight( 'A', 'a' ) );
			Assert.AreEqual( 0.2, pattern.Weight( 'a', 'c' ) );
			Assert.AreEqual( 0.3, pattern.Weight( 'c', 'g' ) );
			Assert.AreEqual( 0.4, pattern.Weight( 'g', 'c' ) );
			Assert.AreEqual( 0.5, pattern.Weight( 't', 'T' ) );

		}

		[TestMethod]
		public void TestToXml () {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader( "BioPatMLXML/BioPatMLrepeat1.test" ) );

			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
			Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );

			Repeat pattern = (Repeat) ( (Series) def2.Pattern ).Patterns[2];

			Assert.AreEqual( "repeat", pattern.Name );

			Assert.AreEqual( 0.9, pattern.Impact, 1e-3 );

			Assert.AreEqual( 0.0, pattern.Weight( 'c', 'c' ) );
			Assert.AreEqual( 0.1, pattern.Weight( 'A', 'a' ) );
			Assert.AreEqual( 0.2, pattern.Weight( 'a', 'c' ) );
			Assert.AreEqual( 0.3, pattern.Weight( 'c', 'g' ) );
			Assert.AreEqual( 0.4, pattern.Weight( 'g', 'c' ) );
			Assert.AreEqual( 0.5, pattern.Weight( 't', 'T' ) );
		}
	}


}
