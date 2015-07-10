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
    [TestClass]
    public class TestPrimitiveParse
    {
        [TestMethod]
        public void TestStringToDoubleArray()
        {
            double[] array = PrimitiveParse.StringToDoubleArray(" 1, 2.1  3.5:-4 u 1e-1 ");
            Assert.AreEqual(1.0, array[0], 1e-3);
            Assert.AreEqual(2.1, array[1], 1e-3);
            Assert.AreEqual(3.5, array[2], 1e-3);
            Assert.AreEqual(-4.0, array[3], 1e-3);
            Assert.AreEqual(0.0, array[4], 1e-3);
            Assert.AreEqual(0.1, array[5], 1e-3);
        }

        [TestMethod]
        public void TestStringToIntArray()
        {
            int[] array = PrimitiveParse.StringToIntArray(" 1, 2: 3  4\t5 ");
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
            Assert.AreEqual(4, array[3]);
            Assert.AreEqual(5, array[4]);

        }

        [TestMethod]
        /** Tests the string conversion to double */
        public void TestStringToDouble()
        {
            Assert.AreEqual(0.5, PrimitiveParse.Atod("+.5"), 1e-5);
            Assert.AreEqual(0.5, PrimitiveParse.Atod(".5"), 1e-5);
            Assert.AreEqual(0.5, PrimitiveParse.Atod("0.5"), 1e-5);
            Assert.AreEqual(0.5, PrimitiveParse.Atod("+.5e-0"), 1e-5);
            Assert.AreEqual(0.5, PrimitiveParse.Atod("5e-1"), 1e-5);
            Assert.AreEqual(-0.5, PrimitiveParse.Atod("-.5"), 1e-5);
            Assert.AreEqual(0.5, PrimitiveParse.Atod("  0.5  "), 1e-5);
            Assert.AreEqual(0, PrimitiveParse.Atod("  bla  "), 1e-5);
            Assert.AreEqual(0, PrimitiveParse.Atod(null), 1e-5);
        }

        [TestMethod]
        /** Tests the string conversion to integer */
        public void TestStringToInt()
        {
            Assert.AreEqual(5, PrimitiveParse.Atoi("0.5e+1"));
            Assert.AreEqual(5, PrimitiveParse.Atoi("5"));
            Assert.AreEqual(5, PrimitiveParse.Atoi("5.1"));
            Assert.AreEqual(5, PrimitiveParse.Atoi("+5"));
            Assert.AreEqual(5, PrimitiveParse.Atoi("  5  "));
            Assert.AreEqual(-5, PrimitiveParse.Atoi("-5"));
        }
    }
}
