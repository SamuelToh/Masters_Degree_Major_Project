using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Symbols.Indexer;
/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestIndexer
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class TestIndexerDirect
    {
        // private ILog log;
        private IndexerDirect indexer;

		//[TestFixtureSetUp]
		//public void FixtureSetUp()
		//{
		//    BasicConfigurator.Configure();
		//    log = LogManager.GetLogger(this.ToString());
		//}

        [TestInitialize]
        public void SetUp()
        {
            indexer = new IndexerDirect(2);
        }

        [TestMethod]
        public void TestTransform()
        {
            Assert.AreEqual(2, indexer.Transform(0));
            Assert.AreEqual(0, indexer.Transform(-2));
            Assert.AreEqual(3, indexer.Transform(1));
        }

        [TestCleanup]
        public void TearDown()
        {
            this.indexer = null; 
        }
    }
}
