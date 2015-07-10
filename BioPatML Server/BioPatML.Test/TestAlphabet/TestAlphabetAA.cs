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
    /// Test for the amino acid alphabet.
    /// </summary>
    [TestClass]
    public class TestAlphabetAA
    {
        private Alphabet alphabet;

        [TestInitialize]
        public void SetUp()
        {
            alphabet = ProteinAlphabet.Instance();
        }

        /// <summary>
        /// Tests the alphabet constructor
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            Alphabet alphabet2 = ProteinAlphabet.Instance();
            Assert.AreEqual("AA", alphabet.Name);
            Assert.AreEqual(true, alphabet == alphabet2);
            Assert.AreEqual(true, alphabet is ProteinAlphabet);
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
            Assert.AreEqual("Alanine(A)", alphabet['a'].Name);
            Assert.AreEqual("Proline(P)", alphabet['p'].Name);
            Assert.AreEqual("Stop", alphabet['*'].Name);

            Assert.AreEqual("Asparagine or asparatic acid", alphabet['B'].Name);
            Assert.AreEqual("Glutamine or glutamic acid", alphabet['Z'].Name);

            Assert.AreEqual("Any amino acid", alphabet['ä'].Name);

        }

        /// <summary>
        /// Tests the equals method
        /// </summary>
        [TestMethod]
        public void TestEqual()
        {
            Assert.AreEqual(true, alphabet['a'].Equals(alphabet['A']));
            Assert.AreEqual(true, alphabet['A'].Equals(alphabet['a']));
            Assert.AreEqual(false, alphabet['a'].Equals(alphabet['p']));

            Assert.AreEqual(true, alphabet['g'].Equals(alphabet['G']));
            Assert.AreEqual(true, alphabet['G'].Equals(alphabet['g']));
            Assert.AreEqual(false, alphabet['G'].Equals(alphabet['p']));

            Assert.AreEqual(true, alphabet['*'].Equals(alphabet['*']));
            Assert.AreEqual(false, alphabet['*'].Equals(alphabet['p']));

            Assert.AreEqual(true, alphabet['B'].Equals(alphabet['N']));
            Assert.AreEqual(true, alphabet['B'].Equals(alphabet['D']));
            Assert.AreEqual(false, alphabet['B'].Equals(alphabet['E']));

            Assert.AreEqual(true, alphabet['z'].Equals(alphabet['q']));
            Assert.AreEqual(true, alphabet['z'].Equals(alphabet['e']));
            Assert.AreEqual(false, alphabet['z'].Equals(alphabet['d']));

            Assert.AreEqual(true, alphabet['X'].Equals(alphabet['A']));
            Assert.AreEqual(true, alphabet['-'].Equals(alphabet['X']));
        }

        /// <summary>
        /// Test the getter for size of the alphabet
        /// </summary>
        [TestMethod]
        public void TestCount()
        {
            Assert.AreEqual(20, alphabet.Count(false));
            Assert.AreEqual(24, alphabet.Count(true));
        }

    }
}
