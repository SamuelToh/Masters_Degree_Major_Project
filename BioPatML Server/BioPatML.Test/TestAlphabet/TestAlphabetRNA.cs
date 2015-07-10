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
    /// NUnit test for the RNA alphabet.
    /// </summary>
    [TestClass]
    public class TestAlphabetRNA
    {
        private Alphabet alphabet;

        [TestInitialize]
        public void SetUp()
        {
            alphabet = RnaAlphabet.Instance();
        }

        /// <summary>
        /// Tests the alphabet constructor
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            Alphabet alphabet2 = RnaAlphabet.Instance();
            Assert.AreEqual("RNA", alphabet.Name);
            Assert.AreEqual(true, alphabet == alphabet2);
            Assert.AreEqual(true, alphabet is RnaAlphabet);
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
        /// test getter for alphabet symbols
        /// </summary>
        [TestMethod]
        public void TestGet()
        {
            Assert.AreEqual("Adenine", alphabet['A'].Name);
            Assert.AreEqual("Uracil", alphabet['u'].Name);
            Assert.AreEqual("Gap", alphabet['-'].Name);

            Assert.AreEqual("Ade or Cyt", alphabet['m'].Name);
            Assert.AreEqual("Ade or Cyt or Ura", alphabet['h'].Name);

            Assert.AreEqual("Any Nucleotide", alphabet['ä'].Name);
        }

        /// <summary>
        /// Tests the equals method
        /// </summary>
        [TestMethod]
        public void TestEquals()
        {
            Assert.AreEqual(true, alphabet['u'].Equals(alphabet['U']));
            Assert.AreEqual(true, alphabet['U'].Equals(alphabet['u']));
            Assert.AreEqual(false, alphabet['u'].Equals(alphabet['a']));

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
            Assert.AreEqual(true, alphabet['b'].Equals(alphabet['u']));
            Assert.AreEqual(false, alphabet['b'].Equals(alphabet['t']));

            Assert.AreEqual(false, alphabet['-'].Equals(alphabet['n']));
            Assert.AreEqual(true, alphabet['n'].Equals(alphabet['x']));
        }

        /// <summary>
        /// Tests the Complement of some alphabet symbols 
        /// </summary>
        [TestMethod]
        public void TestComplement()
        {
            Assert.AreEqual(alphabet['u'], alphabet['a'].Complement);
            Assert.AreEqual(alphabet['a'], alphabet['u'].Complement);
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
            Assert.AreEqual(17, alphabet.Count(true));
        }

    }
}
