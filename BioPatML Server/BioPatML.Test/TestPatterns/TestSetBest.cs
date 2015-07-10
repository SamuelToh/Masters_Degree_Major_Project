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

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestPatterns {
	[TestClass]
	public class TestSetBest {
		[TestMethod]
		public void TestAddGetPattern () {
			SetBest set = new SetBest();
			Assert.AreEqual( 0, set.Count );

			Motif motif = new Motif( "motif1", AlphabetType.DNA, "ACTG", 0.7 );
			set.Add( motif );
			Assert.AreEqual( 1, set.Count );
			Assert.AreEqual( motif, set[0] );
		}

		[TestMethod]
		/** Tests the matching a pattern set against a sequence */
		public void TestMatch () {
			Sequence seq = new Sequence( AlphabetType.DNA, "atgcatgc" );
			SetBest set = new SetBest( "set1", 0.5 );
			set.Add( new Motif( "motif1", AlphabetType.DNA, "tgca", 0.0 ) );
			set.Add( new Motif( "", AlphabetType.DNA, "tgc", 0.0 ) );

			Match match = set.Match( seq, 2 );
			Assert.AreEqual( 2, match.Start );
			Assert.AreEqual( 4, match.Length );
			Assert.AreEqual( 1, match.Strand );
			Assert.AreEqual( 1, match.Similarity, 1e-2 );

			match = set.Match( seq, 1 );
			Assert.AreEqual( null, match );

			match = set.Match( seq, 6 );
			Assert.AreEqual( 6, match.Start );
			Assert.AreEqual( 3, match.Length );
			Assert.AreEqual( 1, match.Similarity, 1e-2 );
		}

		[TestMethod]
		/** Tests if the match method finds the best pattern */
		public void TestMatch1 () {
			Sequence seq = new Sequence( AlphabetType.DNA, "taaacc" );
			SetBest set = new SetBest( "set1", 0.5 );
			set.Add( new Motif( "motif1", AlphabetType.DNA, "taa", 0.0 ) );
			set.Add( new Motif( "motif2", AlphabetType.DNA, "aacc", 0.0 ) );
			FeatureList matches;

			matches = seq.Search( 1, seq.Length, set );
			Assert.AreEqual( 3, matches.Count );
			Assert.AreEqual( "taa", matches[ 0 ].Letters() );
			Assert.AreEqual( 1.00, ( (Match) matches[ 0 ] ).Similarity, 1e-2 );
			Assert.AreEqual( "aaac", matches[ 1 ].Letters() );
			Assert.AreEqual( 0.75, ( (Match) matches[ 1 ] ).Similarity, 1e-2 );
			Assert.AreEqual( "aacc", matches[ 2 ].Letters() );
			Assert.AreEqual( 1.00, ( (Match) matches[ 2 ] ).Similarity, 1e-2 );
		}

		[TestMethod]
		public void TestRead () {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(   "BioPatMLXML/SetBest.xml" ) );
			SetBest pattern = (SetBest) definition.Pattern;

			Assert.AreEqual( "setbest", pattern.Name );
			Assert.AreEqual( 0.7, pattern.Threshold, 1e-3 );
			Assert.AreEqual( 0.9, pattern.Impact, 1e-3 );

			Assert.AreEqual( "motif1", pattern[0].Name );
			Assert.AreEqual( "motif2", pattern[1].Name );
			Assert.AreEqual( "regex1", pattern[2].Name );
		}

		[TestMethod]
		public void TestToXml () {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(   "BioPatMLXML/SetBest.xml" ) );

			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
			Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );

			SetBest pattern = (SetBest) def2.Pattern;

			Assert.AreEqual( "setbest", pattern.Name );
			Assert.AreEqual( 0.7, pattern.Threshold, 1e-3 );
			Assert.AreEqual( 0.9, pattern.Impact, 1e-3 );

			Assert.AreEqual( "motif1", pattern[0].Name );
			Assert.AreEqual( "motif2", pattern[1].Name );
			Assert.AreEqual( "regex1", pattern[2].Name );
		}
	}
}
