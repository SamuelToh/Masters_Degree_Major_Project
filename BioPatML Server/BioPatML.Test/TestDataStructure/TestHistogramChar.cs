using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Common.Structures;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestDataStructure
{
    /// <summary>
    /// Test HistogramChar class
    /// </summary>
    [TestClass]
    public class TestHistogramChar
    {
        private HistogramChar histo;

        [TestInitialize]
        public void SetUp()
        {
            histo = new HistogramChar();
        }

        [TestMethod]
        public void TestInc()
        {
            histo.Inc('a', 2);
            histo.Inc('c', 2);
            histo.Inc('a');

            Assert.AreEqual(3, histo.Get('a'));
            Assert.AreEqual(0.6, histo.GetRelative('a'), 1e-3);

            Assert.AreEqual(2, histo.Get('c'));
            Assert.AreEqual(0.4, histo.GetRelative('c'), 1e-3);

            Assert.AreEqual(0, histo.Get('g'));
            Assert.AreEqual(0.0, histo.GetRelative('g'), 1e-3);
        }

        [TestMethod]
        public void TestIncSequence()
        {
            histo.Inc("atatag");
            Assert.AreEqual(3, histo.Get('a'));
            Assert.AreEqual(2, histo.Get('t'));
            Assert.AreEqual(0, histo.Get('c'));
            Assert.AreEqual(1, histo.Get('g'));
        }

        [TestMethod]
        public void TestSum()
        {
            char[] charactersSequence = { 'a', 't', 'a', 't', 'a', 'g' };
            histo.Inc(new String(charactersSequence));
            Assert.AreEqual(6, histo.Sum);
        }

        [TestMethod]
        public void TestCounter()
        {
            char[] charactersSequence = { 'a', 't', 'a', 't', 'a', 'g' };
            histo.Inc(new String(charactersSequence));

            Assert.AreEqual(3, histo.Count);
        }

        [TestCleanup]
        public void TearDown()
        {
            this.histo = null;
        }
    }
}
