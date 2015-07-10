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
    /// Test the DNA alphabet class
    /// </summary>
    [TestClass]
    public class TestAlphabetDNA
    {
        private Alphabet alphabet;

        [TestInitialize]
        public void SetUp()
        {
            alphabet = DnaAlphabet.Instance();
        }

        /// <summary>
        /// Tests the alphabet constructor
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            Alphabet alphabet2 = DnaAlphabet.Instance();
            Assert.AreEqual("DNA", alphabet.Name);
            Assert.AreEqual(true, alphabet == alphabet2);
            Assert.AreEqual(true, alphabet is DnaAlphabet);
        }

        /// <summary>
        /// Tests the validator for letters
        /// </summary>
        [TestMethod]
        public void TestIsValid()
        {
            Assert.AreEqual(true, alphabet.IsValid('A'));
            Assert.AreEqual(true, alphabet.IsValid('a'));
            Assert.AreEqual(false, alphabet.IsValid('ä'));
        }

        /// <summary>
        // Test getter for alphabet symbols
        /// </summary>
        [TestMethod]
        public void TestGet()
        {
            Assert.AreEqual("Adenine", alphabet['A'].Name);
            Assert.AreEqual("Guanine", alphabet['g'].Name);
            Assert.AreEqual("Gap", alphabet['-'].Name);

            Assert.AreEqual("Ade or Cyt", alphabet['m'].Name);
            Assert.AreEqual("Ade or Cyt or Gua", alphabet['v'].Name);

            Assert.AreEqual("Any Nucleotide", alphabet['ä'].Name);
        }

        /// <summary>
        /// Tests the equals method
        /// </summary>
        [TestMethod]
        public void TestEquals()
        {
            Assert.AreEqual(true, alphabet['a'].Equals(alphabet['A']));
            Assert.AreEqual(true, alphabet['A'].Equals(alphabet['a']));
            Assert.AreEqual(false, alphabet['a'].Equals(alphabet['g']));

            Assert.AreEqual(true, alphabet['g'].Equals(alphabet['G']));
            Assert.AreEqual(true, alphabet['G'].Equals(alphabet['g']));
            Assert.AreEqual(false, alphabet['G'].Equals(alphabet['a']));

            Assert.AreEqual(true, alphabet['.'].Equals(alphabet['.']));
            Assert.AreEqual(false, alphabet['.'].Equals(alphabet['g']));

            Assert.AreEqual(true, alphabet['-'].Equals(alphabet['-']));
            Assert.AreEqual(false, alphabet['-'].Equals(alphabet['g']));

            Assert.AreEqual(true, alphabet['m'].Equals(alphabet['a']));
            Assert.AreEqual(true, alphabet['m'].Equals(alphabet['c']));
            Assert.AreEqual(false, alphabet['m'].Equals(alphabet['g']));

            Assert.AreEqual(true, alphabet['b'].Equals(alphabet['c']));
            Assert.AreEqual(true, alphabet['b'].Equals(alphabet['g']));
            Assert.AreEqual(true, alphabet['b'].Equals(alphabet['t']));
            Assert.AreEqual(false, alphabet['b'].Equals(alphabet['a']));

            Assert.AreEqual(false, alphabet['-'].Equals(alphabet['n']));
            Assert.AreEqual(true, alphabet['n'].Equals(alphabet['x']));
  
        }

        /// <summary>
        /// Tests the complement of some alphabet symbols
        /// </summary>
        [TestMethod]
        public void TestComplement()
        {
            Assert.AreEqual(alphabet['t'], alphabet['a'].Complement);
            Assert.AreEqual(alphabet['a'], alphabet['t'].Complement);
            Assert.AreEqual(alphabet['g'], alphabet['c'].Complement);
            Assert.AreEqual(alphabet['c'], alphabet['g'].Complement);

            Assert.AreEqual(alphabet['m'], alphabet['k'].Complement);
            Assert.AreEqual(alphabet['r'], alphabet['y'].Complement);
        }

        /// <summary>
        /// test the getter for size of the alphabet
        /// </summary>
        [TestMethod]
        public void TestCount()
        {
            Assert.AreEqual(4, alphabet.Count(false));
            Assert.AreEqual(18, alphabet.Count(true));
        }

    }
}
