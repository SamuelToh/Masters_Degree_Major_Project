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
    /// Test for Mapchar class
    /// </summary>
    [TestClass]
    public class TestMapChar
    {
        private MapChar map;

        [TestInitialize]
        public void SetUp()
        {
            map = new MapChar();
        }

        /// <summary>
        /// Test Constructor
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            map.Put('1', "test1");
            map.Put('2', "test2");
            map.Put('3', "test3");
            map.Put('1', "test1");

            Assert.AreEqual(3, map.Size);
        }

        [TestMethod]
        public void TestPutGet()
        {
            map.Put('1', "test1");
            map.Put('2', "test2");
            map.Put('3', "test3");

            //log.Debug(map.Get(1));
            Assert.AreEqual("test1", map.Get('1'));
            Assert.AreEqual("test2", map.Get('2'));
            Assert.AreEqual("test3", map.Get('3'));
            
            Assert.AreEqual("test1", map.Get(0));
            Assert.AreEqual("test2", map.Get(1));
            Assert.AreEqual("test3", map.Get(2));
        }

        [TestMethod]
        public void TestGetKey()
        {
            map.Put('1', "test1");
            map.Put('2', "test2");
            map.Put('3', "test3");
            map.Put('1', "test1");

            Assert.AreEqual('1', map.GetKey(0));
            Assert.AreEqual('2', map.GetKey(1));
            Assert.AreEqual('3', map.GetKey(2));
        }

        [TestMethod]
        public void TestContainKey()
        {
            map.Put('1', "test1");
            map.Put('2', "test2");

            Assert.AreEqual(true, map.ContainsKey('1'));
            Assert.AreEqual(true, map.ContainsKey('2'));
            Assert.AreEqual(false, map.ContainsKey('3'));
        }

        [TestMethod]
        public void TestContains()
        {
            map.Put('1', "test1");
            map.Put('2', "test2");

            char[] testName = { 't', 'e', 's', 't', '1' };

            Assert.AreEqual(true, map.Contains("test1"));
            Assert.AreEqual(true, map.Contains("test2"));
            Assert.AreEqual(true, map.Contains(new String(testName)));
            Assert.AreEqual(false, map.Contains("test3"));
        }

        [TestMethod]
        public void testRemove()
        {
            map.Put('1', "test1");
            map.Put('2', "test2");
            map.Put('3', "test3");

            map.Remove('4');
            Assert.AreEqual(3, map.Size);

            map.Remove('2');
            Assert.AreEqual(2, map.Size);
            Assert.AreEqual("test1", map.Get('1'));
            Assert.AreEqual(null, map.Get('2'));
            Assert.AreEqual("test3", map.Get('3'));

        }

        [TestMethod]
        public void testClear()
        {
            map.Put('1', "test1");
            map.Put('2', "test2");

            map.Clear();
            Assert.AreEqual(0, map.Size);
            Assert.AreEqual(null, map.Get('1'));
        }

        [TestMethod]
        public void TestRehash()
        {
            for (char ch = 'A'; ch < 'A' + 100; ch++)
                map.Put(ch, "test" + ch);

            for (char ch = 'A'; ch < 'A' + 100; ch++)
                Assert.AreEqual("test" + ch, map.Get(ch));

        }


        [TestMethod]
        public void TestLiterable()
        {
            //Not implemented Yet
        }


        [TestCleanup]
        public void TearDown()
        {
            this.map = null;
        }


    }
}
