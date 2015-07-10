using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Patterns;
using System.Xml;
using QUT.Bio.BioPatML.Common.XML;
using QUT.Bio.BioPatML.Alphabets;
using BioPatML.Test;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestPatterns {
	[TestClass]
	public class TestGap {

		[TestMethod]
		public void TestMatch () {
			Gap gap = new Gap( "test", 0, 3, 1 );
			Sequence seq = new Sequence( AlphabetType.DNA, "act" );
			FeatureList matches = seq.Search( 0, 0, gap );
			Assert.AreEqual( 9, matches.Count );
			Assert.AreEqual( "", matches[ 0 ].Letters() );
			Assert.AreEqual( "a", matches[ 1 ].Letters() );
			Assert.AreEqual( "ac", matches[ 2 ].Letters() );
			Assert.AreEqual( "act", matches[ 3 ].Letters() );
			Assert.AreEqual( "", matches[ 4 ].Letters() );
			Assert.AreEqual( "c", matches[ 5 ].Letters() );
			Assert.AreEqual( "ct", matches[ 6 ].Letters() );
			Assert.AreEqual( "", matches[ 7 ].Letters() );
			Assert.AreEqual( "t", matches[ 8 ].Letters() );
		}

		/** Tests the matching with increment. */
		public void TestMatchIncrement () {
			Gap gap = new Gap( "test", 1, 8, 1.7 );
			Sequence seq = new Sequence( AlphabetType.DNA, "actgactg" );
			Match match = gap.Match( seq, 1 );
			Assert.AreEqual( "a", match.Letters() );
			Assert.AreEqual( 0, gap.Increment );

			match = gap.Match( seq, 1 );
			Assert.AreEqual( "act", match.Letters() );
			Assert.AreEqual( 0, gap.Increment );

			match = gap.Match( seq, 1 );
			Assert.AreEqual( "actg", match.Letters() );
			Assert.AreEqual( 0, gap.Increment );

			match = gap.Match( seq, 1 );
			Assert.AreEqual( "actgac", match.Letters() );
			Assert.AreEqual( 0, gap.Increment );

			match = gap.Match( seq, 1 );
			Assert.AreEqual( "actgactg", match.Letters() );
			Assert.AreEqual( 1, gap.Increment );
		}



		[TestMethod]
		/** Tests the weigthed matching  */
		public void TestMatchWeighted () {
			Gap gap = new Gap( "test", 1, 4, 1, new double[] { 0.5, 0.5, 0.5 }, 0.0 );
			Sequence seq = new Sequence( AlphabetType.DNA, "actga" );
			Match match = gap.Match( seq, 1 );
			Assert.AreEqual( "a", match.Letters() );
			Assert.AreEqual( 1.0, match.Similarity, 1e-1 );
		}


		[TestMethod]
		/** Tests the weigthed matching  */
		public void TestMatchWeighted1 () {
			Gap gap = new Gap( "test", 1, 4, 1, new double[] { 1, 5, 10 }, 0.0 );
			Sequence seq = new Sequence( AlphabetType.DNA, "actga" );
			Match match = gap.Match( seq, 1 );
			Assert.AreEqual( "a", match.Letters() );
			Assert.AreEqual( 0.1, match.Similarity, 1e-1 );
			Assert.AreEqual( 0, gap.Increment );

			match = gap.Match( seq, 1 );
			Assert.AreEqual( "ac", match.Letters() );
			Assert.AreEqual( 0.5, match.Similarity, 1e-1 );
			Assert.AreEqual( 0, gap.Increment );

			match = gap.Match( seq, 1 );
			Assert.AreEqual( "act", match.Letters() );
			Assert.AreEqual( 1.0, match.Similarity, 1e-1 );
			Assert.AreEqual( 0, gap.Increment );

			match = gap.Match( seq, 1 );
			Assert.AreEqual( "actg", match.Letters() );
			Assert.AreEqual( 1.0, match.Similarity, 1e-1 );
			Assert.AreEqual( 1, gap.Increment );
		}

		[TestMethod]
		/** Tests the weigthed matching  */
		public void TestMatchWeighted2 () {
			Gap gap = new Gap( "test", 1, 4, 1, new double[] { 1, 5, 10 }, 0.0 );
			Sequence seq = new Sequence( AlphabetType.DNA, "act" );
			FeatureList matches = seq.Search( 0, 0, gap );
			Assert.AreEqual( 6, matches.Count );
			Assert.AreEqual( "a", matches[ 0 ].Letters() );
			Assert.AreEqual( 0.1, ( (Match) matches[ 0 ] ).Similarity, 1e-1 );
			Assert.AreEqual( "ac", matches[ 1 ].Letters() );
			Assert.AreEqual( 0.5, ( (Match) matches[ 1 ] ).Similarity, 1e-1 );
			Assert.AreEqual( "act", matches[ 2 ].Letters() );
			Assert.AreEqual( 1.0, ( (Match) matches[ 2 ] ).Similarity, 1e-1 );
			Assert.AreEqual( "c", matches[ 3 ].Letters() );
			Assert.AreEqual( 0.1, ( (Match) matches[ 3 ] ).Similarity, 1e-1 );
			Assert.AreEqual( "ct", matches[ 4 ].Letters() );
			Assert.AreEqual( 0.5, ( (Match) matches[ 4 ] ).Similarity, 1e-1 );
			Assert.AreEqual( "t", matches[ 5 ].Letters() );
			Assert.AreEqual( 0.1, ( (Match) matches[ 5 ] ).Similarity, 1e-1 );
		}

		/** Tests the reading of a gap pattern from an XML document */
		[TestMethod]
		public void TestRead () {

			Definition definition = DefinitionIO.Read( Global.GetResourceReader(  "BioPatMLXML/Gap.xml" ) );
			Gap pattern = (Gap) definition.Pattern;

			Assert.AreEqual( "Gap", definition.Name );
			Assert.AreEqual( "gap", pattern.Name );
			Assert.AreEqual( 6, pattern.MinLength );
			Assert.AreEqual( 8, pattern.MaxLength );
			Assert.AreEqual( 3.14, pattern.IncLength );
			Assert.AreEqual( 0.0, pattern.Threshold );
		}

		/** Tests the reading of a weighted gap pattern from an XML document */
		[TestMethod]
		public void TestReadWeighted () {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(  "BioPatMLXML/GapWeighted.xml" ) );
			Gap pattern = (Gap) definition.Pattern;

			Assert.AreEqual( "Gap", definition.Name );
			Assert.AreEqual( "gap", pattern.Name );
			Assert.AreEqual( 2, pattern.MinLength );
			Assert.AreEqual( 6, pattern.MaxLength );
			Assert.AreEqual( 1.1, pattern.IncLength );
			Assert.AreEqual( 0.0, pattern.TabulateGapSim( 0 ) );
			Assert.AreEqual( 0.8, pattern.TabulateGapSim( 1 ) );
			Assert.AreEqual( 1.0, pattern.TabulateGapSim( 2 ) );
			Assert.AreEqual( 0.9, pattern.Impact );
			Assert.AreEqual( 0.7, pattern.Threshold );

		}

		/** Tests the reading of a weighted gap pattern from an XML document */
		[TestMethod]
		public void TestToXml () {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(  "BioPatMLXML/GapWeighted.xml" ) );

			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
			Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );

			Gap pattern = (Gap) def2.Pattern;

			Assert.AreEqual( "Gap", definition.Name );
			Assert.AreEqual( "gap", pattern.Name );
			Assert.AreEqual( 2, pattern.MinLength );
			Assert.AreEqual( 6, pattern.MaxLength );
			Assert.AreEqual( 1.1, pattern.IncLength );
			Assert.AreEqual( 0.0, pattern.TabulateGapSim( 0 ) );
			Assert.AreEqual( 0.8, pattern.TabulateGapSim( 1 ) );
			Assert.AreEqual( 1.0, pattern.TabulateGapSim( 2 ) );
			Assert.AreEqual( 0.9, pattern.Impact );
			Assert.AreEqual( 0.7, pattern.Threshold );

		}
	}
}
