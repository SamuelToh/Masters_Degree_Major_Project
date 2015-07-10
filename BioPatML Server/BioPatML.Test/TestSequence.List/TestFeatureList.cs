using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Alphabets;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestSequence.List {
	[TestClass]
	public class TestFeatureList {

		[TestMethod]
		/** Tests the construction of a feature list */
		public void TestConstructor () {
			Sequence seq = new Sequence( AlphabetType.DNA, "acgt" );
			FeatureList featureList = new FeatureList( "Test" );
			seq.AddFeatures( featureList );
			Assert.AreEqual( 1, seq.FeatureLists.Count );
			Assert.AreEqual( "Test", featureList.Name );
			// Assert.AreEqual(seq, featureList.GetSequence());
		}

		[TestMethod]
		/** Test getter for feature list name */
		public void TestSetGetName () {
			FeatureList featureList = new FeatureList( "Test" );
			Assert.AreEqual( "Test", featureList.Name );
		}

		[TestMethod]
		/** Tests the adding of features */
		public void TestAdd () {
			Sequence seq = new Sequence( AlphabetType.DNA, "acgt" );
			FeatureList featureList = new FeatureList( "Test" );
			seq.AddFeatures( featureList );
			Feature feature1 = new Feature( "Test1", 1, 3, +1 );
			Feature feature2 = new Feature( "Test2", 1, 3, +1 );
			featureList.Add( feature1, false );
			featureList.Add( feature2, true );
			Assert.AreEqual( 2, featureList.Count );
			Assert.AreEqual( null, feature1.BaseSequence );
			Assert.AreEqual( seq, feature2.BaseSequence );
		}

		[TestMethod]
		/** Tests the adding of feature lists */
		public void TestAddList () {
			FeatureList featureList1 = new FeatureList( "Test" );
			featureList1.Add( new Feature( "Test1", 1, 3, +1 ) );
			featureList1.Add( new Feature( "Test2", 1, 3, +1 ) );
			FeatureList featureList2 = new FeatureList( "Test" );
			featureList2.Add( new Feature( "Test3", 1, 3, +1 ) );
			featureList2.Add( new Feature( "Test4", 1, 3, +1 ) );
			featureList1.Append( featureList2, true );
			Assert.AreEqual( "Test1", featureList1[0].Name );
			Assert.AreEqual( "Test2", featureList1[1].Name );
			Assert.AreEqual( "Test3", featureList1[2].Name );
			Assert.AreEqual( "Test4", featureList1[3].Name );
		}

		[TestMethod]
		/** Tests the getter of features */
		public void TestFeature () {
			FeatureList featureList = new FeatureList( "Test" );
			Feature feature1 = new Feature( "Test1", 1, 3, +1 );
			Feature feature2 = new Feature( "Test2", 1, 3, +1 );
			featureList.Add( feature1, false );
			featureList.Add( feature2, true );

			var t = featureList[ 0 ];
			bool ans = feature1.Equals( featureList[0] );

			Assert.IsTrue( feature1.Equals( featureList[0] ) );
			Assert.IsTrue( feature2.Equals( featureList[1] ) );
			Assert.IsTrue( feature2.Equals( featureList[-1] ) );
			Assert.IsTrue( feature1.Equals( featureList[2] ) );
			Assert.IsTrue( feature2.Equals( featureList[-3] ) );
			Assert.IsTrue( feature1.Equals( featureList[4] ) );
			Assert.IsTrue( feature1.Equals( featureList["Test1"] ) );
			Assert.IsTrue( feature2.Equals( featureList["Test2"] ) );


		}

		[TestMethod]
		/** Tests the search for a feature which covers a given position */
		public void TestInside () {
			Sequence seq = new Sequence( AlphabetType.DNA, "acgtactgactg" );
			FeatureList featureList = new FeatureList( "Test" );
			Feature feature1 = new Feature( "Test1", 2, 4, +1 );
			Feature feature2 = new Feature( "Test2", 7, 10, +1 );
			featureList.Add( feature1 );
			featureList.Add( feature2 );
			featureList.AttachSequence( seq );
			Assert.AreEqual( feature1, featureList.Inside( seq, 2 ) );
			Assert.AreEqual( feature1, featureList.Inside( seq, 4 ) );
			Assert.AreEqual( null, featureList.Inside( seq, 1 ) );
			Assert.AreEqual( null, featureList.Inside( seq, 5 ) );
			Assert.AreEqual( feature2, featureList.Inside( seq, 7 ) );
			Assert.AreEqual( feature2, featureList.Inside( seq, 10 ) );
			Assert.AreEqual( null, featureList.Inside( seq, 6 ) );
			Assert.AreEqual( null, featureList.Inside( seq, 11 ) );
		}

	}
}
