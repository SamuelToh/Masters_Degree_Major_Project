using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public class TestPaperSpecialCases
    {
        /// <summary>
        /// All these test files are taken from MBF test suite
        /// </summary>
        private int SearchPosition { get; set; } //The position we wan to start search with
        private Definition MyPatterns;
        private string BiopatMLFilePath = string.Empty;

        [TestInitialize]
        public void SetUp()
        {
            this.BiopatMLFilePath = string.Empty;
            this.MyPatterns = null;
            SearchPosition = 1;
        }

        /// <summary>
        /// Testing a special element void with the combination of 2 motifs, iteration and gap elements.
        /// </summary>
        [TestMethod]
        public void TestSpecialPattern_Void()
        {
            BiopatMLFilePath = "BioPaperTestData/SpecialPattern/Void.xml";

            Sequence voidTestSequence = new Sequence(AlphabetType.AA, "CCAMMMKAACCMACCTEFCCCMKTCECCMKKTAACCKTCCMKKEC");

			MyPatterns = DefinitionIO.Read( Global.GetResourceReader(   BiopatMLFilePath ) );

			FeatureList Matches = voidTestSequence.Search( 1, voidTestSequence.Length, MyPatterns.Pattern );

            //There should be 8 matches
            Assert.AreEqual(8, Matches.Count);

            Match matched = (Match)Matches[0];

            //The overall matched
            Assert.AreEqual("CC", matched.Letters());
            // 2  matches however most of them are void
            Assert.AreEqual(2, matched.SubMatches.Count);
            Assert.AreEqual("CC", matched.SubMatches[0].Letters());
        }

        /// <summary>
        /// Test special element constraint. It should only match at the position of the sequence
        /// that is specified by position and offset param of the pattern.
        /// 
        /// </summary>
        [TestMethod]
        public void TestSpecialPattern_Constraint()
        {
            BiopatMLFilePath = "BioPaperTestData/SpecialPattern/Constraint.xml";

            Sequence voidTestSequence = new Sequence(AlphabetType.AA, "CCAMMMKAACCMACCTEFCCCMKTCECCMKKTAACCKTCCMKKEC");

			MyPatterns = DefinitionIO.Read( Global.GetResourceReader(   BiopatMLFilePath ) );

			 FeatureList Matches = voidTestSequence.Search( 1, voidTestSequence.Length, MyPatterns.Pattern );

            //There should be 1 match
            Assert.AreEqual(1, Matches.Count);

            Match matched = (Match)Matches[0];

            //The overall matched
            Assert.AreEqual("KK", matched.Letters());
            Assert.AreEqual(2, matched.SubMatches.Count);

        }
    }
}
