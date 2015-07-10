using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Common.XML;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Patterns;
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
    public class TestMotif
    {
		/** test for match method from pattern interface */
		[TestMethod]
		public void MotifTestMatchAtStart () {
			Sequence seq = new Sequence( AlphabetType.DNA, "aTtgattaca" );
			Motif motif = new Motif( "test", AlphabetType.DNA, "attg", 0.0 );
			Match match = motif.Match( seq, 1 );

			Assert.AreEqual( 1, match.Start );
			Assert.AreEqual( 4, match.Length );
			Assert.AreEqual( 1, match.Strand );
			Assert.AreEqual( 1.0, match.Similarity, 1e-2 );
			Assert.AreEqual( "attg", match.Letters() );
		}

		/** test for match method from pattern interface */
		[TestMethod]
		public void MotifTestTwoMatches () {
			Sequence seq = new Sequence( AlphabetType.DNA, "aTtgattgaca" );
			Motif motif = new Motif( "test", AlphabetType.DNA, "attg", 0.0 );
			Match match = motif.Match( seq, 1 );

			Assert.AreEqual( 1, match.Start );
			Assert.AreEqual( 4, match.Length );
			Assert.AreEqual( 1, match.Strand );
			Assert.AreEqual( 1.0, match.Similarity, 1e-2 );
			Assert.AreEqual( "attg", match.Letters() );

			Match match2 = motif.Match( seq, 2 );
			Assert.AreEqual( 2, match.Start );
			Assert.AreEqual( 4, match.Length );
			Assert.AreEqual( 1, match.Strand );
			Assert.AreEqual( 0.25, match.Similarity, 1e-2 );
			Assert.AreEqual( "ttga", match.Letters() );
		}

		/** test for match method from pattern interface */
		[TestMethod]
		public void MotifTestMatch () {
			Sequence seq = new Sequence( AlphabetType.DNA, "aTtga" );
			Motif motif = new Motif( "test", AlphabetType.DNA, "tgn", 0.0 );
			Match match = motif.Match( seq, 3 );

			Assert.AreEqual( 3, match.Start );
			Assert.AreEqual( 3, match.Length );
			Assert.AreEqual( 1, match.Strand );
			Assert.AreEqual( 1.0, match.Similarity, 1e-2 );

			match = motif.Match( seq, 2 );
			Assert.AreEqual( 2, match.Start );
			Assert.AreEqual( 3, match.Length );
			Assert.AreEqual( 1, match.Strand );
			Assert.AreEqual( 0.66, match.Similarity, 1e-2 );
		}

        /** test for match with alternatives */
        [TestMethod]
		public void MotifTestMatchAlternatives ()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "atgc");
            Motif motif = new Motif("test", AlphabetType.DNA,"[ga]tg[c]", 0.0);
            Match match = motif.Match(seq, 1);
            Assert.AreEqual(1.0, match.Similarity, 1e-2);
        }

        /** test for match similarity */
        [TestMethod]
		public void MotifTestMatchSimiliarity ()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "aCtG", true);

            Assert.AreEqual(2, (new Motif("test", AlphabetType.DNA, "ct", 0)).Match(seq, 1).Length);
            Assert.AreEqual(3, (new Motif("test", AlphabetType.DNA, "ct", 0)).Match(seq, 3).Start);
            Assert.AreEqual(0.0, (new Motif("test", AlphabetType.DNA, "ct", 0)).Match(seq, 1).Similarity);

            Assert.AreEqual(0.5, (new Motif("test", AlphabetType.DNA, "cc", 0)).Match(seq, 1).Similarity, 1e-3);
            Assert.AreEqual(1.0, (new Motif("test", AlphabetType.DNA, "ct", 0)).Match(seq, 2).Similarity, 1e-3);
            Assert.AreEqual(0.0, (new Motif("test", AlphabetType.DNA, "ct", 0)).Match(seq, 3).Similarity, 1e-3);

            Assert.AreEqual(1.0, (new Motif("test", AlphabetType.DNA, "ctga", 0)).Match(seq, 2).Similarity, 1e-3);

            Motif seq1 = new Motif("test", AlphabetType.DNA, "cc", 0.6);
            Assert.AreEqual(null, seq1.Match(seq, 1));
        }

        /** Tests the getter for the letters of a motif */
        [TestMethod]
		public void MotifTestLetters ()
        {
            Motif motif = new Motif("test", AlphabetType.DNA, "tgn[at]C[TG]", 0.0);
            Assert.AreEqual("tg[atcg][at]c[tg]", motif.Letters);
        }

        /** Tests the reading of a motif from an XML document */
        [TestMethod]
		public void MotifTestRead ()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(  "BioPatMLXML/Motif.xml" ) );
            Motif pattern = (Motif)definition.Pattern;

            Assert.AreEqual("Motif", definition.Name);
            Assert.AreEqual("motif", pattern.Name);
            Assert.AreEqual(0.7, pattern.Threshold, 1e-3);
            Assert.AreEqual(0.9, pattern.Impact, 1e-3);
            Assert.AreEqual("ttgaca", pattern.Letters);
        }

        /** Tests the reading of a motif from an XML document */
        [TestMethod]
		public void TestToXml ()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(  "BioPatMLXML/Motif.xml" ) );

			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
			Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );

			Motif pattern = (Motif) def2.Pattern;

            Assert.AreEqual("Motif", def2.Name);
            Assert.AreEqual("motif", pattern.Name);
            Assert.AreEqual(0.7, pattern.Threshold, 1e-3);
            Assert.AreEqual(0.9, pattern.Impact, 1e-3);
            Assert.AreEqual("ttgaca", pattern.Letters);
        }

    }
}
