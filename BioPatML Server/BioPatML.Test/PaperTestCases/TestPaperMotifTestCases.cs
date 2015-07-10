using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Readers;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;
using DB = System.Diagnostics.Debug;
using BioPatML.Test;

/*****************| Queensland  University Of Technology |*******************
 *  Author                   : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 *                             
 * All these additional test scenarios were taken from The BioPatML paper.
 * 
 ***************************************************************************/
namespace TestBioPatML.PaperTestCases {
	/// <summary>
	/// Common recursive pattern found in biology sequences.
	/// </summary>
	[TestClass]
	public class TestPaperMotifTestCases {
		/// <summary>
		/// All these test files are taken from MBF test suite
		/// </summary>
		// private ILog log;
		private int SearchPosition { get; set; } //The position we wan to start search with
		private SequenceList BioList;
		private Definition MyPatterns;
		private string BiopatMLFilePath = string.Empty;
		private const string _singleDnaSeqGenBankFilename = @"data\GenBank\D12555.gbk";
		private const string _sampleGenBankFile1 = @"data\GenBank\AE001582.gbk";
		private const string _sampleGenBankFile2 = @"data\GenBank\AF032047.gbk";

		//[TestFixtureSetUp]
		//public void FixtureSetUp()
		//{
		//    BasicConfigurator.Configure();
		//    log = LogManager.GetLogger(this.ToString());
		//}

		/// <summary>
		/// Setup the test items
		/// </summary>
		[TestInitialize]
		public void SetUp () {
			this.BiopatMLFilePath = string.Empty;
			this.BioList = null;
			this.MyPatterns = null;
			SearchPosition = 1;
		}

		/// <summary>
		/// Test element Motif.
		/// The famous pribnow-box will be used for testing in this case
		/// </summary>
		[TestMethod]
		public void TestMotifPattern_Motif () {
			BiopatMLFilePath = "BioPaperTestData/MotifPattern/Motif.xml";

			using ( BioPatMBF_Reader gbReader = new BioPatMBF_Reader() ) {
				BioList = gbReader.Read( Global.GetResourceReader( _singleDnaSeqGenBankFilename ) );
			}

			MyPatterns = DefinitionIO.Read( Global.GetResourceReader( BiopatMLFilePath ) );

			FeatureList Matches = BioList[0].Search( SearchPosition, BioList[0].Length, MyPatterns.Pattern );

			//According to Jacobi library total matches should be 57 
			Assert.AreEqual( 57, Matches.Count );
			Assert.AreEqual( "Pribnow-box", Matches.Name );
			//Perform some random checks from the 57 list

			Match matched = (Match) Matches[10]; //try get the 11th matched

			Assert.AreEqual( 0.66, matched.Similarity, 1e-2 );
			Assert.AreEqual( 6, matched.Length );
			Assert.AreEqual( "ttttat", matched.Letters() );

			//try the first match
			matched = (Match) Matches[0];
			Assert.AreEqual( AlphabetFactory.Instance( AlphabetType.DNA ), matched.Alphabet );
			Assert.AreEqual( 6, matched.Length );
			Assert.AreEqual( 0.5, matched.Similarity, 1e-2 );

			// Check the last match
			matched = (Match) Matches[56];
			Assert.AreEqual( 0.5, matched.Similarity, 1e-2 );
			Assert.AreEqual( "tttctt", matched.Letters() );

		}

		/// <summary>
		/// This test cases uses the C6 zinc finger in regular expression to search for matches
		/// within a sequence.
		/// </summary>
		[TestMethod]
		public void TestMotifPattern_RegularEx () {
			BiopatMLFilePath = "BioPaperTestData/MotifPattern/Regex.xml";

			Sequence dnaZincFinger = new Sequence( AlphabetType.AA, "ccccccaaaaaaccccccccactcttccccccccccctctctcccgcgctcacctggctcccccccccaatccgc" );

			MyPatterns = DefinitionIO.Read( Global.GetResourceReader( BiopatMLFilePath ) );

			FeatureList Matches = dnaZincFinger.Search( SearchPosition, dnaZincFinger.Length, MyPatterns.Pattern );

			//There should be 7 matches
			Assert.AreEqual( 7, Matches.Count );
			Assert.AreEqual( "C6 zinc-finger", Matches.Name );

			Match matched = (Match) Matches[0];

			Assert.AreEqual( 3, matched.Start );
			Assert.AreEqual( 27, matched.Length );
			Assert.AreEqual( 29, matched.End );
			Assert.AreEqual( "CCCCAAAAAACCCCCCCCACTCTTCCC", matched.Letters() );

			matched = (Match) Matches[1];

			Assert.AreEqual( 14, matched.Start );
			Assert.AreEqual( 28, matched.Length );
			Assert.AreEqual( 41, matched.End );
			Assert.AreEqual( "CCCCCCCACTCTTCCCCCCCCCCCTCTC", matched.Letters() );

			matched = (Match) Matches[2];
			Assert.AreEqual( "CCCCACTCTTCCCCCCCCCCCTCTCTCC", matched.Letters() );

			matched = (Match) Matches[3];
			Assert.AreEqual( "CTTCCCCCCCCCCCTCTCTCCCGCGCTC", matched.Letters() );
		}

		/// <summary>
		/// Test Prosite element.
		/// Similiar to regularexpression but with some differences in syntax 
		/// <para></para>
		/// Please see 
		/// http://au.expasy.org/txt/prosuser.txt.
		/// <para></para>
		/// For more prosite information
		/// </summary>
		[TestMethod]
		public void TestMotifPattern_Prosite () {
			BiopatMLFilePath = "BioPaperTestData/MotifPattern/Prosite.xml";

			Sequence dnaZincFinger = new Sequence( AlphabetType.AA, "ccccccaaaaaaccccccccactcttccccccccccctctctcccgcgctcacctggctcccccccccaatccgc" );

			MyPatterns = DefinitionIO.Read( Global.GetResourceReader( BiopatMLFilePath ) );

			FeatureList Matches = dnaZincFinger.Search( SearchPosition, dnaZincFinger.Length, MyPatterns.Pattern );

			//There should be 7 matches
			Assert.AreEqual( 7, Matches.Count );
			Assert.AreEqual( "Leucine-zipper", Matches.Name );

			Match matched = (Match) Matches[4];

			Assert.AreEqual( 27, matched.Start );
			Assert.AreEqual( 54, matched.End );
			Assert.AreEqual( "CCCCCCCCCCCTCTCTCCCGCGCTCACC", matched.Letters() );

			matched = (Match) Matches[5];

			Assert.AreEqual( 34, matched.Start );
			Assert.AreEqual( 61, matched.End );
			Assert.AreEqual( "CCCCTCTCTCCCGCGCTCACCTGGCTCC", matched.Letters() );

			matched = (Match) Matches[6];
			Assert.AreEqual( 41, matched.Start );
			Assert.AreEqual( 68, matched.End );
			Assert.AreEqual( "CTCCCGCGCTCACCTGGCTCCCCCCCCC", matched.Letters() );
		}

		/// <summary>
		/// Test block element commonly used in BLOCK databases
		/// </summary>
		[TestMethod]
		public void TestMotifPattern_Block () {
			BiopatMLFilePath = "BioPaperTestData/MotifPattern/Block.xml";

			BioPatMBF_Reader gbReader = new BioPatMBF_Reader();
			BioList = gbReader.Read( Global.GetResourceReader( _sampleGenBankFile2 ) );

			MyPatterns = DefinitionIO.Read( Global.GetResourceReader( BiopatMLFilePath ) );
			FeatureList Matches = BioList[0].Search( SearchPosition, BioList[0].Length, MyPatterns.Pattern );

			Assert.AreEqual( 3, Matches.Count );
			Assert.AreEqual( "Pribnow-box", Matches.Name );

			Match matched = (Match) Matches[0];
			Assert.AreEqual( 0.83, matched.Similarity, 1e-2 );
			Assert.AreEqual( "tataac", matched.Letters() );

			matched = (Match) Matches[1];
			Assert.AreEqual( 0.76, matched.Similarity, 1e-2 );
			Assert.AreEqual( "taacat", matched.Letters() );

			matched = (Match) Matches[2];
			Assert.AreEqual( 0.72, matched.Similarity, 1e-2 );
			Assert.AreEqual( "cataaa", matched.Letters() );
		}

		/// <summary>
		/// Test PWM element 
		/// </summary>
		[TestMethod]
		public void TestMotifPattern_PWM () {
			BiopatMLFilePath = "BioPaperTestData/MotifPattern/PWM.xml";

			using ( BioPatMBF_Reader gbReader = new BioPatMBF_Reader() ) {
				BioList = gbReader.Read( Global.GetResourceReader( _sampleGenBankFile2 ) );
			}

			MyPatterns = DefinitionIO.Read( Global.GetResourceReader( BiopatMLFilePath ) );
			FeatureList Matches = BioList[0].Search( SearchPosition, BioList[0].Length, MyPatterns.Pattern );

			Assert.AreEqual( 14, Matches.Count );
			Assert.AreEqual( "Pribnow-box", Matches.Name );

			Match matched = (Match) Matches[0];
			Assert.AreEqual( 17, matched.Start );
			Assert.AreEqual( 22, matched.End );
			Assert.AreEqual( 0.67, matched.Similarity, 1e-2 );
			Assert.AreEqual( "tctcct", matched.Letters() );

			matched = (Match) Matches[1];

			Assert.AreEqual( 25, matched.Start );
			Assert.AreEqual( 30, matched.End );
			Assert.AreEqual( 0.61, matched.Similarity, 1e-2 );
			Assert.AreEqual( "ttggct", matched.Letters() );

			matched = (Match) Matches[13];
			Assert.AreEqual( 268, matched.Start );
			Assert.AreEqual( 273, matched.End );
			Assert.AreEqual( 0.67, matched.Similarity, 1e-2 );
			Assert.AreEqual( "tgtgct", matched.Letters() );
		}
	}
}
