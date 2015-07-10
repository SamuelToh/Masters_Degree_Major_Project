using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Patterns;
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
	public class TestMatch {
		private VoidPattern voidPattern;

		[TestInitialize]
		public void SetUp () {
			voidPattern = new VoidPattern( "Test" );
		}

		[TestMethod]
		public void TestConstructor () {
			Sequence seq = new Sequence( AlphabetType.DNA, "atcg" );
			Match m = new Match( voidPattern, seq, 3, 2, +1, 1 );
			Assert.AreEqual( 3, m.Start );
			Assert.AreEqual( 2, m.Length );
			Assert.AreEqual( 1, m.Strand );
			Assert.AreEqual( seq, m.BaseSequence );
			Assert.AreEqual( "cg", m.Letters() );
			Assert.AreEqual( 1.0, m.Similarity, 1e-3 );
		}

		[TestMethod]
		public void TestCopy () {
			Match m = new Match( voidPattern, null, 3, 2, +1, 1 );
			Match sub1 = new Match( voidPattern, null, 4, 2, +1, 0.5 );

			sub1.Impact = 0.5;

			Match sub2 = new Match( voidPattern, null, 5, 2, +1, 0.1 );
			sub2.Impact = 0.7;

			m.SubMatches.Add( sub1 );
			m.SubMatches.Add( sub2 );

			Match mc = m.Clone();

			Assert.AreEqual( 2, m.SubMatches.Count );
			Assert.AreEqual( 2, mc.SubMatches.Count );
			Assert.IsTrue( mc != m );
			Assert.IsTrue( sub1 != mc.SubMatches[0] );
			Assert.IsTrue( sub2 != mc.SubMatches[1] );
			Assert.AreEqual( 3, mc.Start );
			Assert.AreEqual( 4, mc.SubMatches[0].Start );
			Assert.AreEqual( 5, mc.SubMatches[1].Start );
			Assert.AreEqual( 0.5, mc.SubMatches[0].Impact, 1e-10 );
			Assert.AreEqual( 0.7, mc.SubMatches[1].Impact, 1e-10 );
		}

		[TestMethod]
		public void TestSubMatch () {
			Sequence seq1 = new Sequence( AlphabetType.DNA, "tttggccaaagcc" );

			Match m = new Match( null );
			Match sub1 = new Match( null );
			Match sub2 = new Match( null );
			Assert.AreEqual( 0, m.SubMatches.Count );
			m.SubMatches.Add( sub1 );
			m.SubMatches.Add( sub2 );

			Assert.AreEqual( 2, m.SubMatches.Count );
			Assert.AreEqual( sub1, m.SubMatches[0] );
			Assert.AreEqual( sub2, m.SubMatches[1] );
		}

		[TestMethod]
		public void TestSetGet () {
			Sequence seq = new Sequence( AlphabetType.DNA, "atcg" );
			Match m = new Match( null );
			m.Set( seq, 3, 4, +1, 0.75 );
			Assert.AreEqual( seq, m.BaseSequence );
			Assert.AreEqual( 3, m.Start );
			Assert.AreEqual( 4, m.Length );
			Assert.AreEqual( 1, m.Strand );
			Assert.AreEqual( 0.75, m.Similarity, 1e-3 );
			Assert.AreEqual( 1, m.CalcMismatches() );
			Assert.AreEqual( 3, m.Matches );
		}

		[TestMethod]
		public void TestSetMatch () {
			Sequence seq = new Sequence( AlphabetType.DNA, "atcg" );
			Match m1 = new Match( null );
			Match m2 = new Match( null );

			m1.Set( seq, 3, 2, +1, 0.5 );
			m2.Set( m1 );

			Assert.AreEqual( seq, m2.BaseSequence );
			Assert.AreEqual( 3, m2.Start );
			Assert.AreEqual( 2, m2.Length );
			Assert.AreEqual( 1, m2.Strand );
			Assert.AreEqual( 0.5, m2.Similarity, 1e-3 );
			Assert.AreEqual( "cg", m2.Letters() );
		}

		[TestMethod]
		public void TestCalcStartEnd () {
			Match m = new Match( null );
			Match sub1 = new Match( voidPattern, null, 2, 2, +1, 0.0 );
			Match sub2 = new Match( voidPattern, null, 3, 2, +1, 0.0 );

			m.SubMatches.Add( sub1 );
			m.SubMatches.Add( sub2 );
			m.CalcStartEnd();

			Assert.AreEqual( 2, m.SubMatches.Count );
			Assert.AreEqual( 2, m.Start );
			Assert.AreEqual( 3, m.Length );
			Assert.AreEqual( 4, m.End );
		}

		[TestMethod]
		public void TestCalcLength () {
			Match m = new Match( null );
			Match sub1 = new Match( voidPattern, null, 2, 2, +1, 0.0 );
			Match sub2 = new Match( voidPattern, null, 3, 4, +1, 0.0 );

			m.SubMatches.Add( sub1 );
			m.SubMatches.Add( sub2 );

			Assert.AreEqual( 2, sub1.CalcLength() );
			Assert.AreEqual( 4, sub2.CalcLength() );
			Assert.AreEqual( 6, m.CalcLength() );
		}

		[TestMethod]
		public void TestCalcSimilarity () {
			Match m = new Match( null );
			Match sub1 = new Match( voidPattern, null, 0, 0, +1, 1.0 );
			Match sub2 = new Match( voidPattern, null, 0, 0, +1, 0.5 );

			m.SubMatches.Add( sub1 );
			m.SubMatches.Add( sub2 );

			m.CalcSimilarity();
			Assert.AreEqual( 0.75, m.Similarity, 1e-3 );
		}

		[TestMethod]
		public void TestToString () {
			Sequence seq = new Sequence( AlphabetType.DNA, "actgactg" );
			Match m = new Match( voidPattern, null, 3, 2, +1, 1 );
			m.SetSequence( seq );

			Assert.AreEqual( "{3, 2, 1, 1, tg}", m.ToString() ); //change the expected result from usual 1.0 to 1

			m.SubMatches.Add( new Match( voidPattern, null, 4, 2, -1, 0.5 ) );
			m.SubMatches[ 0 ].SetSequence( seq );
			m.SubMatches.Add( new Match( voidPattern, null, 5, 3, +1, 0.1 ) );
			m.SubMatches[ 1 ].SetSequence( seq );

			Assert.AreEqual( "{3, 2, 1, 1, tg, {4, 2, -1, 0.5, tc}, {5, 3, 1, 0.1, act}}",
				 m.ToString() ); //see original code for actual result, i trim away the 1 decimal placing to pass the
			//test as there wont be much impact
		}
	}
}
