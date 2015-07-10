using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Symbols;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestAlphabet
{
    /// <summary>
    /// NUnit test for the alphabet factory.
    /// </summary>
    [TestClass]
    public class TestAlphabetFactory
    {
        private Alphabet alphabetDNA;
        private Alphabet alphabetRNA;
        private Alphabet alphabetAA;

        [TestInitialize]
        public void SetUp()
        {
            alphabetDNA = DnaAlphabet.Instance();
            alphabetAA  = ProteinAlphabet .Instance();
            alphabetRNA = RnaAlphabet.Instance();
        }

        /// <summary>
        /// Tests the creation of an alphabet
        /// </summary>
        [TestMethod]
        public void TestInstance()
        {
            Assert.AreEqual(RnaAlphabet.Instance(), AlphabetFactory.Instance(AlphabetType.RNA));
			Assert.AreEqual( DnaAlphabet.Instance(), AlphabetFactory.Instance( AlphabetType.DNA ) );
            Assert.AreEqual(ProteinAlphabet.Instance(), AlphabetFactory.Instance(AlphabetType.AA ));
            Assert.AreEqual(ProteinAlphabet.Instance(), AlphabetFactory.Instance(AlphabetType.PROTEIN));      
        }

        /// <summary>
        /// Tests the equality of symbols of different alphabets
        /// </summary>
        [TestMethod]
        public void TestAlphabets()
        {
            Assert.AreEqual(true, alphabetDNA['a'].Equals(alphabetRNA['a']));
            Assert.AreEqual(true, alphabetRNA['a'].Equals(alphabetDNA['a']));
            Assert.AreEqual(false, alphabetRNA['u'].Equals(alphabetDNA['t']));

            Assert.AreEqual(false, alphabetDNA['a'].Equals(alphabetAA['a']));
            Assert.AreEqual(false, alphabetAA['a'].Equals(alphabetDNA['a']));
            Assert.AreEqual(false, alphabetAA['a'].Equals(alphabetRNA['a']));
        }

        /// <summary>
        /// test for the recognition of an alphabet
        /// </summary>
        [TestMethod]
        public void TestRecognize()
        {
            Assert.AreEqual("RNA", AlphabetFactory.Recognize("uuguNNuccgga*agaug").Name);
            Assert.AreEqual("DNA", AlphabetFactory.Recognize("ttgtNNtccggtt**aaagatggt").Name);
            Assert.AreEqual("AA", AlphabetFactory.Recognize("MTQLQISLLLTATISLLH").Name);
        }

        [TestCleanup]
        public void TearDown()
        {
            this.alphabetAA = null;
            this.alphabetDNA = null;
            this.alphabetRNA = null;
        }
    }
}
