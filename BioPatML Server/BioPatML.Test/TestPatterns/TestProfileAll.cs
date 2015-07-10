using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Common.XML;
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
	public class TestProfileAll {
		[TestMethod]
		/** Tests the match method of a sequence of patterns */
		public void TestMatch1 () {
			Sequence seq = new Sequence( AlphabetType.DNA, "baaacc" );
			ProfileAll pf = new ProfileAll();
			Motif motif;
			ProfileElement element;
			FeatureList matches;

			motif = new Motif( "motif1", AlphabetType.DNA, "aa", 0.5 );
			element = pf.Add( motif );
			matches = seq.Search( 1, seq.Length, pf );
			Assert.AreEqual( 4, matches.Count );
			Assert.AreEqual( "ba", matches[ 0 ].Letters() );
			Assert.AreEqual( 1, matches[ 0 ].Start );
			Assert.AreEqual( "aa", matches[ 1 ].Letters() );
			Assert.AreEqual( 2, matches[ 1 ].Start );
			Assert.AreEqual( "aa", matches[ 2 ].Letters() );
			Assert.AreEqual( 3, matches[ 2 ].Start );
			Assert.AreEqual( "ac", matches[ 3 ].Letters() );
			Assert.AreEqual( 4, matches[ 3 ].Start );

			motif = new Motif( "motif2", AlphabetType.DNA, "acc", 0.5 );
			element = pf.Add( element, ProfileElement.AlignmentType.END, 0, 1, motif );
			matches = seq.Search( 1, seq.Length, pf );
			Assert.AreEqual( 3, matches.Count );
			Assert.AreEqual( "baaac", matches[ 0 ].Letters() );
			Assert.AreEqual( 1, matches[ 0 ].Start );
			Assert.AreEqual( "baaacc", matches[ 1 ].Letters() );
			Assert.AreEqual( 1, matches[ 1 ].Start );
			Assert.AreEqual( "aaacc", matches[ 2 ].Letters() );
			Assert.AreEqual( 2, matches[ 2 ].Start );


			motif = new Motif( "motif3", AlphabetType.DNA, "cc", 1.0 );
			element = pf.Add( element, ProfileElement.AlignmentType.END, -2, 1, motif );
			matches = seq.Search( 1, seq.Length, pf );
			Assert.AreEqual( 3, matches.Count );
			Assert.AreEqual( "baaacc", matches[ 0 ].Letters() );
			Assert.AreEqual( 1, matches[ 0 ].Start );
			Assert.AreEqual( "baaacc", matches[ 1 ].Letters() );
			Assert.AreEqual( 1, matches[ 1 ].Start );
			Assert.AreEqual( "aaacc", matches[ 2 ].Letters() );
			Assert.AreEqual( 2, matches[ 2 ].Start );
		}


		[TestMethod]
		/** Tests the match method of a sequence of patterns with different alignment */
		public void TestMatch2 () {
			Sequence seq = new Sequence( AlphabetType.DNA, "baaacc" );
			ProfileAll pf = new ProfileAll();
			Motif motif;
			ProfileElement element;
			FeatureList matches;

			motif = new Motif( "motif1", AlphabetType.DNA, "aa", 1.0 );
			element = pf.Add( motif );
			matches = seq.Search( 1, seq.Length, pf );
			Assert.AreEqual( 2, matches.Count );
			Assert.AreEqual( 2, matches[ 0 ].Start );
			Assert.AreEqual( "aa", matches[ 0 ].Letters() );
			Assert.AreEqual( 3, matches[ 1 ].Start );
			Assert.AreEqual( "aa", matches[ 1 ].Letters() );


			motif = new Motif( "motif2", AlphabetType.DNA, "baaa", 0.5 );
			pf.Add( element, ProfileElement.AlignmentType.START, -1, 0, motif );
			matches = seq.Search( 1, seq.Length, pf );
			Assert.AreEqual( 3, matches.Count );
			Assert.AreEqual( 1, matches[ 0 ].Start );
			Assert.AreEqual( "baaa", matches[ 0 ].Letters() );
			Assert.AreEqual( 2, matches[ 1 ].Start );
			Assert.AreEqual( "aaac", matches[ 1 ].Letters() );
			Assert.AreEqual( 2, matches[ 2 ].Start );
			Assert.AreEqual( "aaac", matches[ 2 ].Letters() );

			motif = new Motif( "motif3", AlphabetType.DNA, "ac", 1.0 );
			pf.Add( element, ProfileElement.AlignmentType.CENTER, 0, 1, motif );
			matches = seq.Search( 1, seq.Length, pf );
			Assert.AreEqual( 3, matches.Count );
			Assert.AreEqual( 1, matches[ 0 ].Start );
			Assert.AreEqual( "baaac", matches[ 0 ].Letters() );
			Assert.AreEqual( 2, matches[ 1 ].Start );
			Assert.AreEqual( "aaac", matches[ 1 ].Letters() );
			Assert.AreEqual( 2, matches[ 2 ].Start );
			Assert.AreEqual( "aaac", matches[ 2 ].Letters() );
		}

		[TestMethod]
		/** Tests the match method for a hierarchical pattern */
		public void TestMatch3 () {
			Sequence seq = new Sequence( AlphabetType.DNA, "baaacc" );
			ProfileAll pf1 = new ProfileAll();
			ProfileAll pf2 = new ProfileAll();
			Motif motif;
			ProfileElement element;
			FeatureList matches;

			motif = new Motif( "motif1", AlphabetType.DNA, "aa", 1.0 );
			element = pf1.Add( motif );
			motif = new Motif( "motif2", AlphabetType.DNA, "aa", 1.0 );
			element = pf1.Add( element, ProfileElement.AlignmentType.START, 0, 1, motif );
			matches = seq.Search( 1, seq.Length, pf1 );
			Assert.AreEqual( 3, matches.Count );
			Assert.AreEqual( "aa", matches[ 0 ].Letters() );
			Assert.AreEqual( "aaa", matches[ 1 ].Letters() );
			Assert.AreEqual( "aa", matches[ 2 ].Letters() );

			motif = new Motif( "motif3", AlphabetType.DNA, "aa", 1.0 );
			element = pf2.Add( motif );
			motif = new Motif( "motif4", AlphabetType.DNA, "cc", 1.0 );
			element = pf2.Add( element, ProfileElement.AlignmentType.CENTER, 0, 2, motif );
			matches = seq.Search( 1, seq.Length, pf2 );
			Assert.AreEqual( 2, matches.Count );
			Assert.AreEqual( "aaacc", matches[ 0 ].Letters() );
			Assert.AreEqual( "aacc", matches[ 1 ].Letters() );

			pf1.Add( pf1[0], ProfileElement.AlignmentType.END, -1, 0, pf2 );
			pf2.Threshold = ( 1.0 );
			matches = seq.Search( 1, seq.Length, pf1 );
			Assert.AreEqual( 2, matches.Count );
			Assert.AreEqual( "aaacc", matches[ 0 ].Letters() );
			Assert.AreEqual( "aaacc", matches[ 1 ].Letters() );
		}

		[TestMethod]
		/** Tests the match method for two patterns with a gap of zero */
		public void TestMatch4 () {
			Sequence seq = new Sequence( AlphabetType.DNA, "baaacc" );
			ProfileAll pf = new ProfileAll();
			Motif motif;
			ProfileElement element;
			FeatureList matches;

			motif = new Motif( "motif1", AlphabetType.DNA, "aa", 1.0 );
			element = pf.Add( motif );
			motif = new Motif( "motif2", AlphabetType.DNA, "cc", 1.0 );
			element = pf.Add( element, ProfileElement.AlignmentType.END, 0, 0, motif );

			matches = seq.Search( 1, seq.Length, pf );
			Assert.AreEqual( 1, matches.Count );
			Assert.AreEqual( "aacc", matches[ 0 ].Letters() );
			Assert.AreEqual( 3, matches[ 0 ].Start );
		}

		[TestMethod]
		/** Tests the match method for a pattern which matches all the time */
		public void TestMatch5 () {
			Sequence seq = new Sequence( AlphabetType.DNA, "bacgt" );
			ProfileAll pf = new ProfileAll();
			Motif motif;
			FeatureList matches;

			motif = new Motif( "motif1", AlphabetType.DNA, "aa", 0.0 );
			pf.Add( motif );

			matches = seq.Search( 1, seq.Length, pf );
			Assert.AreEqual( 4, matches.Count );
			Assert.AreEqual( "ba", matches[ 0 ].Letters() );
			Assert.AreEqual( 1, matches[ 0 ].Start );
			Assert.AreEqual( "ac", matches[ 1 ].Letters() );
			Assert.AreEqual( 2, matches[ 1 ].Start );
		}


	}
}
