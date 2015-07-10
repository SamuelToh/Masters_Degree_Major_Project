using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Symbols.Accessor;
using QUT.Bio.BioPatML.Symbols;
using QUT.Bio.BioPatML.Symbols.Indexer;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestAccessor
{
    /// <summary>
    /// NUnit test for the complement symbol array accessor.
    /// </summary>
    [TestClass]
    public class TestAccessorComplement
    {
        private Alphabet alpha;
        private SymbolArray symbols;
        private AccessorBase accessor;
        private Indexer indexer = new IndexerReverse(0, 4);

        [TestInitialize]
        public void SetUp()
        {
            alpha = DnaAlphabet.Instance();
            symbols = new SymbolArray(alpha, "actg");
            accessor = new AccessorComplement(symbols);
        }

        /// <summary>
        /// Test constructor of accessor complement
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            Assert.AreEqual(null, accessor.Indexer);
        }

        /// <summary>
        /// Tests the getter for a symbol
        /// </summary>
        [TestMethod]
        public void TestSymbolAt()
        {
            Assert.AreEqual(alpha['t'], accessor.SymbolAt(0));
            Assert.AreEqual(alpha['g'], accessor.SymbolAt(1));
            Assert.AreEqual(alpha['a'], accessor.SymbolAt(2));
            Assert.AreEqual(alpha['c'], accessor.SymbolAt(3));
        }

        /// <summary>
        /// Reversed complement
        /// </summary>
        [TestMethod]
        public void TestReverseSymbolAt()
        {
            accessor = new AccessorLinear
                            (this.indexer, 4, symbols);

            accessor = new AccessorComplement(accessor);

            Assert.AreEqual(alpha['-'], accessor.SymbolAt(-1)); //index out of bound situation
            Assert.AreEqual(alpha['-'], accessor.SymbolAt(4));
            Assert.AreEqual(alpha['c'], accessor.SymbolAt(0));
            Assert.AreEqual(alpha['a'], accessor.SymbolAt(1));
            Assert.AreEqual(alpha['g'], accessor.SymbolAt(2));
            Assert.AreEqual(alpha['t'], accessor.SymbolAt(3));
        }
    }
}
