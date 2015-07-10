using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DB = System.Diagnostics.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Readers;
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
	[TestClass]
	public class TestPaperRegionalTestCases {
		private int SearchPosition { get; set; } //The position we wan to start search with
		private SequenceList BioList;
		private Definition MyPatterns;
		private string BiopatMLFilePath = string.Empty;
		private const string _genBankDataPath = @"data\GenBank";
		private const string _singleProteinSeqGenBankFilename = @"data\GenBank\bioperlwiki.gbk";


		[TestInitialize]
		public void SetUp () {
			this.BiopatMLFilePath = string.Empty;
			this.BioList = null;
			this.MyPatterns = null;
			SearchPosition = 1;
		}

		/// <summary>
		/// Test "Any" element pattern.
		/// </summary>
		[TestMethod]
		public void TestRegionalPattern_Any () {
			BiopatMLFilePath = "BioPaperTestData/RegionalPattern/RegionalAny.xml";

			BioPatMBF_Reader gbReader = new BioPatMBF_Reader();

			BioList = gbReader.Read( Global.GetResourceReader( _singleProteinSeqGenBankFilename ) );

			MyPatterns = DefinitionIO.Read( Global.GetResourceReader( BiopatMLFilePath ) );

			FeatureList Matches = BioList[0].Search( SearchPosition, BioList[0].Length, MyPatterns.Pattern );

			//Total matches according to old jacobi is 309
			Assert.AreEqual( 309, Matches.Count );
			//Checks the first match 
			Assert.AreEqual( 1, Matches[0].Start );
			Assert.AreEqual( 6, Matches[0].End );
			//Checks if the last Match is in correect start and end pos
			Assert.AreEqual( 104, Matches[308].Start );
			Assert.AreEqual( 109, Matches[308].End );
		}

		/// <summary>
		/// Test the Gap pattern.
		/// 
		/// </summary>
		[TestMethod]
		public void TestRegionalPattern_Gap () {
			BiopatMLFilePath = "BioPaperTestData/RegionalPattern/RegionalGap.xml";

			BioPatMBF_Reader gbReader = new BioPatMBF_Reader();
			BioList = gbReader.Read( Global.GetResourceReader( _singleProteinSeqGenBankFilename ) );
			MyPatterns = DefinitionIO.Read( Global.GetResourceReader( BiopatMLFilePath ) );

			FeatureList Matches = BioList[0].Search( SearchPosition, BioList[0].Length, MyPatterns.Pattern );

			//Total matches according to old jacobi is 309
			Assert.AreEqual( 410, Matches.Count );
			//Checks the first match 
			Assert.AreEqual( "MT", Matches[0].MainSequence.Letters( Matches[0].Start, Matches[0].End ) );
			//Checks if the last Match is in correect start and end pos
			Assert.AreEqual( "CE", Matches[409].MainSequence.Letters( Matches[409].Start, Matches[409].End ) );
		}

		/// <summary>
		/// Test the Composition pattern. Composition is an extend of Any and Gap pattern.
		/// Gap and Any does not have constraint over the detail of the region, a match is formed as long as
		/// the min and max length falls within the region.
		/// In order not to ignore the content of a region we use composition element to put constraint on the 
		/// match.
		/// </summary>
		[TestMethod]
		public void TestRegionalPattern_Composition () {
			BiopatMLFilePath = "BioPaperTestData/RegionalPattern/RegionalComposition.xml";

			using ( BioPatMBF_Reader gbReader = new BioPatMBF_Reader() ) {
				BioList = gbReader.Read( Global.GetResourceReader( _singleProteinSeqGenBankFilename ) );
			}

			MyPatterns = DefinitionIO.Read( Global.GetResourceReader( BiopatMLFilePath ) );

			FeatureList Matches = BioList[0].Search( SearchPosition, BioList[0].Length, MyPatterns.Pattern );

			//Total matches according to old jacobi is 309
			Assert.AreEqual( 49, Matches.Count );
			//Checks the first match 
			Assert.AreEqual( "GATLFKTRCLQCHTV", Matches[0].MainSequence.Letters( Matches[0].Start, Matches[0].End ) );
			//Check see if the pattern used to match has the correct name for its matches
			Assert.AreEqual( "transmembrane domain", Matches.Name );
			Assert.AreEqual( 12, Matches[0].Start );
			Assert.AreEqual( 26, Matches[0].End );
			//Checks if the last Match is in correect start and end pos
			Assert.AreEqual( "KDRNDLITYLKKACE", Matches[48].MainSequence.Letters( Matches[48].Start, Matches[48].End ) );
		}
	}
}
