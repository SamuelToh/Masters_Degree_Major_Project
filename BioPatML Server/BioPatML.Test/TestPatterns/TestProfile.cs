using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Common.XML;
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
	public class TestProfile {
		[TestMethod]
		/** Tests the adding of patterns to a profile */
		public void TestAdd () {
			IPattern pattern1, pattern2;
			ProfileElement element1, element2;
			ProfileProxy pf = new ProfileProxy();
			Assert.AreEqual( 0, pf.Count );

			pattern1 = new Motif( "motif1", AlphabetType.DNA, "ac", 0 );
			element1 = pf.Add( null, ProfileElement.AlignmentType.NONE, 0, 0, pattern1 );
			Assert.AreEqual( 1, pf.Count );
			Assert.AreEqual( pattern1, element1.Pattern );

			pattern2 = new Motif( "motif2", AlphabetType.DNA, "tg", 0 );
			element2 = pf.Add( element1, ProfileElement.AlignmentType.START, -2, -1, pattern2 );
			Assert.AreEqual( 2, pf.Count );
			Assert.AreEqual( ProfileElement.AlignmentType.START, element2.Alignment );
			Assert.AreEqual( -2, element2.MinGap );
			Assert.AreEqual( -1, element2.MaxGap );
			Assert.AreEqual( ProfileElement.AlignmentType.START, element2.Alignment );
			Assert.AreEqual( element1, element2.RefElement );
		}

		[TestMethod]
		/** Tests the getter of patterns */
		public void TestGet () {
			ProfileProxy pf = new ProfileProxy();
			Motif pat1 = new Motif( "motif1", AlphabetType.DNA, "ac", 0 );
			Motif pat2 = new Motif( "motif2", AlphabetType.DNA, "ag", 0 );
			pf.Add( pat1 );
			pf.Add( pat2 );
			Assert.AreEqual( pat1, pf.Pattern( 0 ) );
			Assert.AreEqual( pat2, pf.Pattern( 1 ) );
		}


		[TestMethod]
		/** Tests the calc. of increments */
		public void TestGetIncrement () {
			Sequence seq = new Sequence( AlphabetType.DNA, "aggtccagtccagcgt" );
			Profile profile = new ProfileAll();
			profile.Add( new RegularExp( "regex1", "ag" ) );
			profile.Add( -1, 1, new RegularExp( "regex2", "gt" ) );
			FeatureList matches = seq.Search( 0, 0, profile );
			Assert.AreEqual( 3, matches.Count );
			Assert.AreEqual( "aggt", matches[0].Letters() );
			Assert.AreEqual( "agt", matches[1].Letters() );
			Assert.AreEqual( "agcgt", matches[2].Letters() );
		}

		//A proxy test
		private class ProfileProxy : Profile {
			public override Match Match ( Sequence sequence, int position ) {
				return null;
			}
		}
	}
}
