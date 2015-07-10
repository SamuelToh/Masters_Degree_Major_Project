using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DB = System.Diagnostics.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Common.XML;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Sequences;
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
namespace TestBioPatML.TestPatterns {
	[TestClass]
	public class TestAlignment {
		private Sequence seq;
		private Series series;
		private Motif motif1;
		private Motif motif2;

		[TestInitialize]
		public void SetUp () {
			seq = new Sequence( AlphabetType.DNA, "atgcatgc" );
			series = new SeriesBest( "series", 1.0 );
			motif1 = new Motif( "motif1", AlphabetType.DNA, "tgc", 1.0 );
			motif2 = new Motif( "motif2", AlphabetType.DNA, "gca", 1.0 );
		}

		/** Test for match at the start */
		[TestMethod]
		public void TestMatchStart () {
			Alignment alignment = new Alignment( "cursor", motif1, AlignmentPosition.START, +1 );
			series.Add( motif1 );
			series.Add( alignment );
			series.Add( motif2 );

			Match match = seq.SearchBest( 0, 0, series );
			Assert.AreEqual( "tgca", match.Letters() );
		}



		/** Test for match at the end */
		[TestMethod]
		public void TestMatchEnd () {
			Alignment alignment = new Alignment( "cursor", motif1, AlignmentPosition.END, -2 );
			series.Add( motif1 );
			series.Add( alignment );
			series.Add( motif2 );

			Match match = seq.SearchBest( 0, 0, series );
			Assert.AreEqual( "tgca", match.Letters() );
		}



		/** Test for match at the center */
		[TestMethod]
		public void TestMatchCenter () {
			Alignment alignment = new Alignment( "cursor", motif1, AlignmentPosition.CENTER, 0 );
			series.Add( motif1 );
			series.Add( alignment );
			series.Add( motif2 );

			Match match = seq.SearchBest( 0, 0, series );
			Assert.AreEqual( "tgca", match.Letters() );
		}


		/** Tests the reading of a motiv from an XML document  */
		[TestMethod]
		public void TestRead () {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader( "BioPatMLXML/Alignment.xml" ) );
			Alignment pattern = (Alignment) ( (Series) definition.Pattern ).Patterns[1];

			Assert.AreEqual( "align1", pattern.Name );
			Assert.AreEqual( "motif1", pattern.Pattern.Name );
			Assert.AreEqual( AlignmentPosition.END, pattern.Position );
			Assert.AreEqual( -2, pattern.Offset );
			Assert.AreEqual( 0.9, pattern.Impact, 1e-3 );
		}

		[TestMethod]
		public void TestToXml () {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader( "BioPatMLXML/Alignment.xml" ) );
			XDocument doc = DefinitionIO.Write( definition );
			Definition definition2 = DefinitionIO.Read( doc );

			Alignment pattern = (Alignment) ( (Series) definition2.Pattern ).Patterns[1];

			Assert.AreEqual( "align1", pattern.Name );
			Assert.AreEqual( "motif1", pattern.Pattern.Name );
			Assert.AreEqual( AlignmentPosition.END, pattern.Position );
			Assert.AreEqual( -2, pattern.Offset );
			Assert.AreEqual( 0.9, pattern.Impact, 1e-3 );

			Assert.IsTrue( doc.ToString().IndexOf( "name=\"auto-" ) < 0 );
		}
	}
}
