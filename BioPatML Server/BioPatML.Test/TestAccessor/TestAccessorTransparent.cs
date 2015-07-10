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
    /// NUnit test for the transparent symbol array accessor.
    /// </summary>
    [TestClass]
    public class TestAccessorTransparent
    {
        private Alphabet alpha;
        private SymbolArray symbols;
        private Indexer indexer;
        private IAccessor accessor;

        [TestInitialize]
        public void SetUp()
        {
            alpha = DnaAlphabet.Instance();
            symbols = new SymbolArray(alpha, "tatgacgt");
            this.indexer = new IndexerDirect(2);
            accessor = new AccessorTransparent(indexer, 4, symbols);
        }

        /// <summary>
        /// Tests the getter for a symbol
        /// </summary>
        [TestMethod]
        public void TestSymbolsAt()
        {
            Assert.AreEqual(alpha['a'], accessor.SymbolAt(-1));
            Assert.AreEqual(alpha['t'], accessor.SymbolAt(0));
            Assert.AreEqual(alpha['g'], accessor.SymbolAt(1));

            Assert.AreEqual(alpha['a'], accessor.SymbolAt(2));
            Assert.AreEqual(alpha['c'], accessor.SymbolAt(3));
            Assert.AreEqual(alpha['g'], accessor.SymbolAt(4));
        }

        /// <summary>
        /// Reverse manner
        /// </summary>
        [TestMethod]
        public void TestReverseSymbolsAt()
        {
            this.indexer = new IndexerReverse(2, symbols.Length);
            this.accessor = new AccessorTransparent
                                    (this.indexer, 4, symbols);

            Assert.AreEqual(alpha['g'], accessor.SymbolAt(-1));
            Assert.AreEqual(alpha['c'], accessor.SymbolAt(0));
            Assert.AreEqual(alpha['a'], accessor.SymbolAt(1));

            Assert.AreEqual(alpha['g'], accessor.SymbolAt(2));
            Assert.AreEqual(alpha['t'], accessor.SymbolAt(3));
            Assert.AreEqual(alpha['a'], accessor.SymbolAt(4));
        }

    }
}
