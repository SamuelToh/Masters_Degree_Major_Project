using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DB = System.Diagnostics.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Common.XML;
using QUT.Bio.BioPatML.Alphabets;
using BioPatML.Test;
using System.Xml.Linq;
using QUT.Bio.BioPatML.Readers;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestPatterns {
	[TestClass]
	public class TestSeriesAll {

		[TestMethod]
		public void TestMatch1 () {
			Sequence seq = new Sequence( AlphabetType.DNA, "taaacc" );
			SeriesAll series = new SeriesAll();
			FeatureList matches;

			series.Add( new Motif( "motif1", AlphabetType.DNA, "aa", 0.5 ) );

			matches = seq.Search( 1, seq.Length, series );
			Assert.AreEqual( 4, matches.Count );
			Assert.AreEqual( "ta", matches[ 0 ].Letters() ); //There might be some error with data structure
			Assert.AreEqual( 1, matches[ 0 ].Start );
			Assert.AreEqual( "aa", matches[ 1 ].Letters() );
			Assert.AreEqual( 2, matches[ 1 ].Start );
			Assert.AreEqual( "aa", matches[ 2 ].Letters() );
			Assert.AreEqual( 3, matches[ 2 ].Start );
			Assert.AreEqual( "ac", matches[ 3 ].Letters() );
			Assert.AreEqual( 4, matches[ 3 ].Start );

			series.Add( new Gap( "gap1", 1, 2, 1 ) );
			series.Add( new Motif( "motif2", AlphabetType.DNA, "cc", 0.5 ) );
			matches = seq.Search( 1, seq.Length, series );
			Assert.AreEqual( 3, matches.Count() );
			Assert.AreEqual( "taaac", matches[ 0 ].Letters() );
			Assert.AreEqual( 0.666, ( (Match) matches[ 0 ] ).Similarity, 1e-3 );
			Assert.AreEqual( "taaacc", matches[ 1 ].Letters() );
			Assert.AreEqual( 0.833, ( (Match) matches[ 1 ] ).Similarity, 1e-3 );
			Assert.AreEqual( "aaacc", matches[ 2 ].Letters() );
			Assert.AreEqual( 1.000, ( (Match) matches[ 2 ] ).Similarity, 1e-3 );
		}

		[TestMethod]
		/** Tests the match method of a series of patterns with a weighted gap */
		public void TestMatchWeightedGap () {
			Sequence seq = new Sequence( AlphabetType.DNA, "taaacc" );
			SeriesAll series = new SeriesAll();

			series.Add( new Motif( "motif1", AlphabetType.DNA, "ta", 1.0 ) );
			series.Add( new Gap( "gap", 1, 2, 1, new double[] { 0, 1 }, 0.0 ) );
			series.Add( new Motif( "motif2", AlphabetType.DNA, "cc", 0.5 ) );
			FeatureList matches = seq.Search( 1, seq.Length, series );
			Assert.AreEqual( "taaac", matches[ 0 ].Letters() );
			Assert.AreEqual( 0.500, ( (Match) matches[ 0 ] ).Similarity, 1e-3 );
			Assert.AreEqual( "taaacc", matches[ 1 ].Letters() );
			Assert.AreEqual( 1.000, ( (Match) matches[ 1 ] ).Similarity, 1e-3 );
		}

		[TestMethod]
		/** Tests the match method of a series of patterns with two gaps */
		public void TestMatchTwoGaps () {
			Sequence seq = new Sequence( AlphabetType.DNA, "taaaagccc" );
			SeriesAll series = new SeriesAll();

			series.Add( new Motif( "motif1", AlphabetType.DNA, "ta", 1.0 ) );
			series.Add( new Gap( "gap1", 1, 2, 1 ) );
			series.Add( new Motif( "motif2", AlphabetType.DNA, "aa", 0.5 ) );
			series.Add( new Gap( "gap2", 1, 2, 1 ) );
			series.Add( new Motif( "motif3", AlphabetType.DNA, "cc", 1.0 ) );
			FeatureList matches = seq.Search( 1, seq.Length, series );
			Assert.AreEqual( 3, matches.Count );
			Assert.AreEqual( "taaaagcc", matches[0].Letters() );
			Assert.AreEqual( 1.0, ( (Match) matches[0] ).Similarity, 1e-1 );
			Assert.AreEqual( "taaaagccc", matches[1].Letters() );
			Assert.AreEqual( 1.0, ( (Match) matches[1] ).Similarity, 1e-1 );
			Assert.AreEqual( "taaaagccc", matches[2].Letters() );
			Assert.AreEqual( 0.9, ( (Match) matches[2] ).Similarity, 1e-1 );
		}

		[TestMethod]
		/** Tests the match method of a series of patterns with two gaps */
		public void TestMatchSigma70 () {
			Sequence seq = new Sequence( AlphabetType.DNA, "cgagagagcg attatatcga ctaaacagaa aatgtcaaac aacttgtcaa aaaacagaag" );
			SeriesAll series = new SeriesAll();

			series.Add( new Motif( "", AlphabetType.DNA, "attata", 1.0 ) );
			series.Add( new Gap( "", 1, 200, 1 ) );
			series.Add( new Motif( "", AlphabetType.DNA, "tgtcaa", 1.0 ) );

			FeatureList matches = seq.Search( 2, seq.Length, series );
			Assert.AreEqual( 2, matches.Count );

			System.Diagnostics.Debug.WriteLine( series.ToXml().ToString() );

			//Assert.AreEqual( "taaaagcc", matches[0].Letters() );
			//Assert.AreEqual( 1.0, ( (Match) matches[0] ).Similarity, 1e-1 );
			//Assert.AreEqual( "taaaagccc", matches[1].Letters() );
			//Assert.AreEqual( 1.0, ( (Match) matches[1] ).Similarity, 1e-1 );
			//Assert.AreEqual( "taaaagccc", matches[2].Letters() );
			//Assert.AreEqual( 0.9, ( (Match) matches[2] ).Similarity, 1e-1 );
		}

		[TestMethod]
		/** Tests the match method of a series of patterns with two gaps */
		public void TestMatchSigma70InChlamydia () {
			SequenceList sequences = null;

			using ( BioPatMBF_Reader reader = new BioPatMBF_Reader() ) {
				sequences = reader.Read(
					Global.GetResourceReader( "data/GenBank/NC_000117-Chlamydia trachomatis D-UW-3CX.gbk" )
				);
			}

			Sequence seq = sequences[0];
			SeriesAll series = new SeriesAll();

			series.Add( new Motif( "", AlphabetType.DNA, "attata", 1.0 ) );
			series.Add( new Gap( "", 1, 200, 1 ) );
			series.Add( new Motif( "", AlphabetType.DNA, "tgtcaa", 1.0 ) );

			FeatureList matches = seq.Search( 2, seq.Length, series );
			Assert.AreEqual( 11, matches.Count );

			//Assert.AreEqual( "taaaagcc", matches[0].Letters() );
			//Assert.AreEqual( 1.0, ( (Match) matches[0] ).Similarity, 1e-1 );
			//Assert.AreEqual( "taaaagccc", matches[1].Letters() );
			//Assert.AreEqual( 1.0, ( (Match) matches[1] ).Similarity, 1e-1 );
			//Assert.AreEqual( "taaaagccc", matches[2].Letters() );
			//Assert.AreEqual( 0.9, ( (Match) matches[2] ).Similarity, 1e-1 );
		}

		[TestMethod]
		public void TestRead () {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader( "BioPatMLXML/SeriesAll.xml" ) );
			Series pattern = (Series) definition.Pattern;

			Assert.AreEqual( "Series", definition.Name );
			Assert.AreEqual( "series", pattern.Name );
			Assert.AreEqual( 0.7, pattern.Threshold, 1e-3 );
			Assert.AreEqual( 0.9, pattern.Impact, 1e-3 );

			Assert.AreEqual( "motif1", pattern.Patterns[0].Name );
			Assert.AreEqual( "any", pattern.Patterns[1].Name );
			Assert.AreEqual( "motif2", pattern.Patterns[2].Name );
		}

		[TestMethod]
		public void TestToXml () {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader( "BioPatMLXML/SeriesAll.xml" ) );

			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
			Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );

			Series pattern = (Series) def2.Pattern;

			Assert.AreEqual( "Series", def2.Name );
			Assert.AreEqual( "series", pattern.Name );
			Assert.AreEqual( 0.7, pattern.Threshold, 1e-3 );
			Assert.AreEqual( 0.9, pattern.Impact, 1e-3 );

			Assert.AreEqual( "motif1", pattern.Patterns[0].Name );
			Assert.AreEqual( "any", pattern.Patterns[1].Name );
			Assert.AreEqual( "motif2", pattern.Patterns[2].Name );
		}
	}
}
