using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;
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
	public class TestSeriesBest {

		[TestMethod]
		public void TestMatch1 () {
			Sequence seq = new Sequence( AlphabetType.DNA, "taaacc" );
			SeriesBest series = new SeriesBest();
			Match match;

			series.Add( new Motif( "Motif1", AlphabetType.DNA, "aa", 0.5 ) );
			match = seq.SearchBest( 1, seq.Length, series );
			Assert.AreEqual( "aa", match.Letters() );
			Assert.AreEqual( 2, match.Position() );
			Assert.AreEqual( 1.00, match.Similarity, 1e-2 );

			series.Add( new Gap( "Gap", 0, 2, 1 ) );
			series.Add( new Motif( "motif2", AlphabetType.DNA, "cc", 0.5 ) );
			match = seq.SearchBest( 1, seq.Length, series );
			Assert.AreEqual( "aaacc", match.Letters() );
			Assert.AreEqual( 2, match.Start );
			Assert.AreEqual( 1.00, match.Similarity, 1e-2 );
		}

		[TestMethod]
		public void TestMatch2 () {
			Sequence seq = new Sequence( AlphabetType.DNA, "taaaccc" );
			SeriesBest series = new SeriesBest();
			Match match;

			series.Add( new Motif( "motif1", AlphabetType.DNA, "ta", 0.5 ) );
			series.Add( new Gap( "gap", 1, 3, 1, new double[] { 1, 2, 1 }, 0.0 ) );
			series.Add( new Motif( "motif2", AlphabetType.DNA, "cc", 0.5 ) );

			match = seq.SearchBest( 1, seq.Length, series );
			Assert.AreEqual( "taaacc", match.Letters() );
			Assert.AreEqual( 1.00, match.Similarity, 1e-2 );
		}

		[TestMethod]
		/**  Tests the match method of a series of patterns with two gaps */
		public void TestMatch3 () {
			Sequence seq = new Sequence( AlphabetType.DNA, "taaaagccca" );
			SeriesBest series = new SeriesBest();
			Match match;

			series.Add( new Motif( "motif1", AlphabetType.DNA, "ta", 0.5 ) );
			series.Add( new Gap( "gap1", 0, 2, 1, new double[] { 1, 2, 1 }, 0.0 ) );
			series.Add( new Motif( "motif2", AlphabetType.DNA, "aa", 1.0 ) );
			series.Add( new Gap( "gap2", 1, 3, 1, new double[] { 2, 1, 1 }, 0.0 ) );
			series.Add( new Motif( "motif3", AlphabetType.DNA, "cc", 0.5 ) );

			match = seq.SearchBest( 1, seq.Length, series );
			Assert.AreEqual( "taaaagcc", match.Letters() );
			Assert.AreEqual( 1.00, match.Similarity, 1e-2 );
		}

		[TestMethod]
		/**  Tests the match method of a series of patterns with consecutive gaps */
		public void TestMatch4 () {
			Sequence seq = new Sequence( AlphabetType.DNA, "taaaacc" );
			SeriesBest series = new SeriesBest();
			Match match;

			series.Add( new Motif( "motif1", AlphabetType.DNA, "ta", 0.7 ) );
			series.Add( new Gap( "gap1", 0, 2, 1, new double[] { 1, 0, 0 }, 0.0 ) );
			series.Add( new Gap( "gap2", 0, 2, 1, new double[] { 0, 1, 0 }, 0.0 ) );
			series.Add( new Motif( "motif2", AlphabetType.DNA, "cc", 0.5 ) );

			match = seq.SearchBest( 1, seq.Length, series );
			Assert.AreEqual( "taaaacc", match.Letters() );
			Assert.AreEqual( 0.75, match.Similarity, 1e-3 );
		}

		[TestMethod]
		/** Tests if the match method finds the best pattern */
		public void TestMatch5 () {
			Sequence seq = new Sequence( AlphabetType.DNA, "taaacc" );
			SeriesBest series = new SeriesBest();
			FeatureList matches;

			series.Add( new Motif( "motif1", AlphabetType.DNA, "aa", 0.5 ) );
			series.Add( new Gap( "gap", 1, 3, 1 ) );
			series.Add( new Motif( "motif2", AlphabetType.DNA, "cc", 0.5 ) );
			matches = seq.Search( 1, seq.Length, series );
			Assert.AreEqual( 2, matches.Count );
			Assert.AreEqual( "taaacc", matches[ 0 ].Letters() );
			Assert.AreEqual( 0.833, ( (Match) matches[ 0 ] ).Similarity, 1e-3 );
			Assert.AreEqual( "aaacc", matches[ 1 ].Letters() );
			Assert.AreEqual( 1.000, ( (Match) matches[ 1 ] ).Similarity, 1e-3 );
		}

		[TestMethod]
		public void TestRead () {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(   "BioPatMLXML/SeriesBest.xml" ) );
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
			Definition definition = DefinitionIO.Read( Global.GetResourceReader( "BioPatMLXML/SeriesBest.xml" ) );

			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
			Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );

			Series pattern = (Series) def2.Pattern;

			Assert.AreEqual( "Series", def2.Name );
			Assert.AreEqual( "series", pattern.Name );
			Assert.AreEqual( 0.7, pattern.Threshold, 1e-3 );
			Assert.AreEqual( 0.9, pattern.Impact, 1e-3 );
			Assert.AreEqual( pattern.GetType().Name, "SeriesBest" );

			Assert.AreEqual( "motif1", pattern.Patterns[0].Name );
			Assert.AreEqual( "any", pattern.Patterns[1].Name );
			Assert.AreEqual( "motif2", pattern.Patterns[2].Name );
		}
	}
}
