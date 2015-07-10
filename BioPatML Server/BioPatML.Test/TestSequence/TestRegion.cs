using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestSequence
{
    [TestClass]
    public class TestRegion
    {
        private Region region;

        [TestInitialize]
        public void SetUp()
        {
            region = new Region(3, 6, +1);
        }

        [TestMethod]
        public void TestConstructor()
        {
            Assert.AreEqual(3, region.Start);
            Assert.AreEqual(6, region.End);
            Assert.AreEqual(4, region.CenterPosition);
            Assert.AreEqual(4, region.Length);
            Assert.AreEqual(1, region.Strand);
        }


        [TestMethod]
        public void TestSet()
        {
            region = new Region(); //Reset our region back to a new obj

            Assert.AreEqual(0, region.Start);
            Assert.AreEqual(0, region.End);
            Assert.AreEqual(1, region.Length);
            Assert.AreEqual(0, region.Strand);

            region.Set(3, 6, +1);

            Assert.AreEqual(3, region.Start);
            Assert.AreEqual(6, region.End);
            Assert.AreEqual(4, region.CenterPosition);
            Assert.AreEqual(4, region.Length);
            Assert.AreEqual(1, region.Strand);
        }

        [TestMethod]
        public void TestSetGet()
        {
            region = new Region();

            region.Start = 2;
            region.End = 5;

            Assert.AreEqual(2, region.Start);
            Assert.AreEqual(5, region.End);
            Assert.AreEqual(3, region.CenterPosition);
            Assert.AreEqual(4, region.Length);
            Assert.AreEqual(0, region.Strand);

        }

        [TestMethod]
        public void TestGetters()
        {
            region = new Region(2, 6, +1);

            Assert.AreEqual(2, region.Start);
            Assert.AreEqual(6, region.End);
            Assert.AreEqual(4, region.CenterPosition);
            Assert.AreEqual(5, region.Length);
            Assert.AreEqual(1, region.Strand);
        }

        [TestMethod]
        public void TestIsInside()
        {
            region = new Region(2, 6, +1);

            Assert.AreEqual(true, region.IsInside(2));
            Assert.AreEqual(true, region.IsInside(6));
            Assert.AreEqual(true, region.IsInside(3));
            Assert.AreEqual(false, region.IsInside(1));
            Assert.AreEqual(false, region.IsInside(7));
        }

        [TestMethod]
        public void TestToString()
        {
            region = new Region(2, 6, +1);

            Assert.AreEqual("{ 2, 6, + }", region.ToString());
        }

    }
}
