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
    /// This class describes a symbol array accessor which reads the symbols in a 
    /// circular way.
    /// </summary>
    [TestClass]
    public class TestAccessorCircular
    {
        private Alphabet alpha;
        private SymbolArray symbols;
        private Indexer indexer, indexerReverse;
        private IAccessor accessor;

        [TestInitialize]
        public void SetUp()
        {
            alpha = DnaAlphabet.Instance();
            symbols = new SymbolArray(alpha, "actgactg");
            indexer = new IndexerDirect(2);
            indexerReverse = new IndexerReverse(2,symbols.Length);
            accessor = new AccessorCircular(this.indexer, 4, symbols);
        }

        /// <summary>
        /// Tests the getter for a symbol
        /// </summary>
        [TestMethod]
        public void TestSymbolAt()
        {
            Assert.AreEqual(alpha['c'], accessor.SymbolAt(-1));
            Assert.AreEqual(alpha['t'], accessor.SymbolAt(0));
            Assert.AreEqual(alpha['g'], accessor.SymbolAt(1));
            Assert.AreEqual(alpha['a'], accessor.SymbolAt(2));
            Assert.AreEqual(alpha['c'], accessor.SymbolAt(3));
            Assert.AreEqual(alpha['t'], accessor.SymbolAt(4));

        }

        /// <summary>
        /// Test the reverse getting of symbols
        /// </summary>
        [TestMethod]
        public void TestReverseSymbolAt()
        {
            accessor = new AccessorCircular
                (this.indexerReverse, 4, symbols);

            Assert.AreEqual(alpha['t'], accessor.SymbolAt(-1));
            Assert.AreEqual(alpha['c'], accessor.SymbolAt(0));
            Assert.AreEqual(alpha['a'], accessor.SymbolAt(1));
            Assert.AreEqual(alpha['g'], accessor.SymbolAt(2));
            Assert.AreEqual(alpha['t'], accessor.SymbolAt(3));
            Assert.AreEqual(alpha['c'], accessor.SymbolAt(4));
        }
    }
}
