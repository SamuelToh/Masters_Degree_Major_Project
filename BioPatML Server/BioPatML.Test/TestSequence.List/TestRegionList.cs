using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestSequence.List
{
    [TestClass]
    public class TestRegionList
    {
        private RegionList<Region> list;

        [TestInitialize]
        public void SetUp()
        {
            list = new RegionList<Region>();
        }

        [TestMethod]
        public void TestConstructor()
        {
            Assert.AreEqual(0, list.Count);
            list = new RegionList<Region>("Test");
            Assert.AreEqual("Test", list.Name );
        }

        [TestMethod]
        public void TestRegion()
        {
            list.Add(new Region(1, 10, +1));
            list.Add(new Region(2, 10, +1));

            Assert.AreEqual(1, list[0].Start);
            Assert.AreEqual(2, list[1].Start);
            Assert.AreEqual(2, list[-1].Start);
            Assert.AreEqual(1, list[2].Start);
        }

        [TestMethod]
        public void TestMinMaxLength()
        {
            // TODO Need SequenceList 
        }
    }
}
