using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Readers;
using QUT.Bio.BioPatML.Alphabets;
using BioPatML.Test;

/*****************| Queensland  University Of Technology |*******************
 *  Author                   : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 *                             
 * All these additional test scenarios were taken from The BioPatML paper.
 * 
 ***************************************************************************/
namespace TestBioPatML.PaperTestCases
{
    [TestClass]
    public class TestPaperStructuredTestCases
    {
        /// <summary>
        /// All these test files are taken from MBF test suite
        /// </summary>
        private int SearchPosition { get; set; } //The position we wan to start search with
        private SequenceList BioList;
        private Definition MyPatterns;
        private string BiopatMLFilePath = string.Empty;
        private const string _genBankDataPath = @"data\GenBank";
        private const string _singleDnaSeqGenBankFilename = @"data\GenBank\D12555.gbk";

        [TestInitialize]
        public void SetUp()
        {
            this.BiopatMLFilePath = string.Empty;
            this.BioList = null;
            this.MyPatterns = null;
            SearchPosition = 1;
        }

        /// <summary>
        /// This is a stem-loop pattern found in the papers.
        /// In this pattern it consists of an Any element with a gap that repeats invertedly
        /// </summary>
        [TestMethod]
        public void TestStructuredPattern_SeriesAll()
        {
            BiopatMLFilePath = "BioPaperTestData/StructuredPattern/SeriesAll.xml";

            using (BioPatMBF_Reader gbReader = new BioPatMBF_Reader())
            {
				BioList = gbReader.Read( Global.GetResourceReader( _singleDnaSeqGenBankFilename ) );
            }

			MyPatterns = DefinitionIO.Read( Global.GetResourceReader(   BiopatMLFilePath ) );

			FeatureList Matches = BioList[0].Search( SearchPosition, BioList[0].Length, MyPatterns.Pattern );

            //expecting 49 matches based on the old jacobi result
            Assert.AreEqual(49, Matches.Count);

            Match matched = (Match)Matches[0]; //Query the 1st matched from the list of matches
            Assert.AreEqual(3, matched.SubMatches.Count); //should have 3 sub matches

            Assert.AreEqual("aattt", matched.SubMatches[0].Letters());
            Assert.AreEqual("tataagtg", matched.SubMatches[1].Letters());
            Assert.AreEqual("ttcaa", matched.SubMatches[2].Letters());

            //And finally the main matched
            Assert.AreEqual("aattttataagtgttcaa", matched.Letters());
        }

        /// <summary>
        /// Test combining a series of ordered pattern consisting of 2 motifs with a gap
        /// inbetween them.
        /// This test scenario uses a makeup sequence to save time.
        /// Expected result: 2 matches
        /// </summary>
        [TestMethod]
        public void TestStructuredPattern_SeriesBest()
        {
            BiopatMLFilePath = "BioPaperTestData/StructuredPattern/SeriesBest.xml";

            Sequence sigma70Promoter = new Sequence(AlphabetType.DNA, "ttgagggggttaccatgatcggtattgtttaatattgacatttaagccgttaagctgaagtgataattaggc");

			MyPatterns = DefinitionIO.Read( Global.GetResourceReader(   BiopatMLFilePath ) );

			FeatureList Matches = sigma70Promoter.Search( 1, sigma70Promoter.Length, MyPatterns.Pattern );

            //There should be 1 matches
            Assert.AreEqual(2, Matches.Count);
            Assert.AreEqual("canonical sigma70-promoter", Matches.Name);

            Match matched = (Match)Matches[0];

            //The overall matched
            Assert.AreEqual("ttgagggggttaccatgatcggtattgtttaat", matched.Letters());
            Assert.AreEqual(0.75, matched.Similarity);
            //Check sigma 35
            Assert.AreEqual("ttgagg", matched.SubMatches[0].Letters());
            //Check sigma 10
            Assert.AreEqual("tttaat", matched.SubMatches[2].Letters());

            matched = (Match)Matches[1];
            Assert.AreEqual("ttgacatttaagccgttaagctgaagtgataat", matched.Letters());
            Assert.AreEqual(0.91, matched.Similarity, 1e-2);
        }

        /// <summary>
        /// Testing on a set of RNA patterns 
        /// </summary>
        [TestMethod]
        public void TestStructuredPattern_SetBest()
        {
            BiopatMLFilePath = "BioPaperTestData/StructuredPattern/SetBest.xml";

            Sequence nuclearLocalizationSignal = new Sequence(AlphabetType.RNA, "RKCLQAGMNLEARKTKKRKKRKRRKRPLQMNRPLQMNR");

			MyPatterns = DefinitionIO.Read( Global.GetResourceReader(   BiopatMLFilePath ) );

			FeatureList Matches = nuclearLocalizationSignal.Search( 1, nuclearLocalizationSignal.Length, MyPatterns.Pattern );

            //There should be 1 matches
            Assert.AreEqual(1, Matches.Count);
            Assert.AreEqual("nuclear localization signal", Matches.Name);

            Match matched = (Match)Matches[0];
            Assert.AreEqual(1.0, matched.Similarity);
            Assert.AreEqual("rkkrkr", matched.Letters());
        }

        /// <summary>
        /// Testing on complex pattern type Logic
        /// using "AND" operator
        /// 
        /// Motif A plus Motif B has to match before a match can be finalized
        /// </summary>
        [TestMethod]
        public void TestStructuredPattern_Logic()
        {
            BiopatMLFilePath = "BioPaperTestData/StructuredPattern/Logic.xml";

            Sequence nuclearLocalizationSignal = new Sequence(AlphabetType.RNA, "LLLGLLLLLGLLLLLLLLGLLGGLLLLLGLLLGR");

			MyPatterns = DefinitionIO.Read( Global.GetResourceReader(   BiopatMLFilePath ) );

			FeatureList Matches = nuclearLocalizationSignal.Search( 1, nuclearLocalizationSignal.Length, MyPatterns.Pattern );

            //There should be 9 matches
            Assert.AreEqual(9, Matches.Count);

            Match matched = (Match)Matches[0];
            Assert.AreEqual("nngnnnnngnnnnnn", matched.Letters());

            matched = (Match)Matches[8];
            Assert.AreEqual("nngnnggnnnnngnn", matched.Letters());
        }

        /// <summary>
        /// Test Iteration element. 
        /// Using 2 characters to try iteration of 3 cycle.
        /// </summary>
        [TestMethod]
        public void TestStructuredPattern_Iteration()
        {
            BiopatMLFilePath = "BioPaperTestData/StructuredPattern/Iteration.xml";

            Sequence seq = new Sequence(AlphabetType.DNA, "TTGAGAGATTTGCGCATC");
			MyPatterns = DefinitionIO.Read( Global.GetResourceReader(   BiopatMLFilePath ) );

			FeatureList Matches = seq.Search( 1, seq.Length, MyPatterns.Pattern );

            //There should be 3 matches
            //In this scenario only GAGA or GAGAGA is acceptable.
            Assert.AreEqual(2, Matches.Count);

            Match matched = (Match)Matches[0];
            Assert.AreEqual("gagaga", matched.Letters());

            matched = (Match)Matches[1];
            Assert.AreEqual("gaga", matched.Letters());
        }
    }
}
