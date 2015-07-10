using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Alphabets;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestPatterns {
	[TestClass]
	public class TestProfileBest {
		[TestMethod]
		public void TestMatch () {
			Sequence seq = new Sequence( AlphabetType.DNA, "aaaacc" );
			ProfileBest pf = new ProfileBest( "test", 0.0 );
			Match Match;
			ProfileElement element;

			element = pf.Add( new Motif( "motif1", AlphabetType.DNA, "aa", 0 ) );

			Match = pf.Match( seq, 1 );
			Assert.AreEqual( 1, Match.Start );
			Assert.AreEqual( 2, Match.Length );
			Assert.AreEqual( 1, Match.Strand );
			Assert.AreEqual( 1.0, Match.Similarity, 1e-3 );
			Assert.AreEqual( "aa", Match.Letters() );

			Match = pf.Match( seq, 4 );
			Assert.AreEqual( 0.5, Match.Similarity, 1e-3 );
			Assert.AreEqual( "ac", Match.Letters() );

			pf.Add( element, ProfileElement.AlignmentType.END, -1, 1, new Motif( "motif2", AlphabetType.DNA, "ac", 0 ) );
			Match = pf.Match( seq, 1 );
			Assert.AreEqual( 1, Match.Start );
			Assert.AreEqual( 5, Match.Length );
			Assert.AreEqual( 1, Match.Strand );
			Assert.AreEqual( 1.0, Match.Similarity, 1e-3 );
			Assert.AreEqual( "aaaac", Match.Letters() );
			Assert.AreEqual( "aa", Match.SubMatches[0].Letters() );
			Assert.AreEqual( "ac", Match.SubMatches[1].Letters() );

			Match = pf.Match( seq, 3 );
			Assert.AreEqual( 3, Match.Start );
			Assert.AreEqual( 3, Match.Length );
			Assert.AreEqual( 1, Match.Strand );
			Assert.AreEqual( 1.0, Match.Similarity, 1e-3 );
			Assert.AreEqual( "aac", Match.Letters() );

			Match = pf.Match( seq, 4 );
			Assert.AreEqual( 4, Match.Start );
			Assert.AreEqual( 3, Match.Length );
			Assert.AreEqual( 1, Match.Strand );
			Assert.AreEqual( 0.5, Match.Similarity, 1e-3 );
			Assert.AreEqual( "acc", Match.Letters() );
			Assert.AreEqual( "ac", Match.SubMatches[0].Letters() );
			Assert.AreEqual( "cc", Match.SubMatches[1].Letters() );
		}

	}
}
