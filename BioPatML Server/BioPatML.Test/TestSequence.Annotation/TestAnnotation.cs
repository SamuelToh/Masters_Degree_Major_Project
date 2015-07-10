using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences.Annotations;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestSequence.TestAnnotation
{
	[TestClass]
    public class TestAnnotation
    {

        private const double testDelta = 1e-1;

        [TestMethod]
        public void TestConstructors()
        {
            Annotation annotation = new Annotation ( "name", "val" );
			Assert.AreEqual("name", annotation.Name);
            Assert.AreEqual("val", annotation.Value);

            annotation = new Annotation ("name", (Object)("val"));

            Assert.AreEqual("name", annotation.Name);
            Assert.AreEqual("val", annotation.Value);


            annotation = new Annotation ("name", 10);

            Assert.AreEqual("name", annotation.Name);
            Assert.AreEqual(10, annotation.Value);

            annotation = new Annotation ("name", 10.1);
           
            Assert.AreEqual("name", annotation.Name);
            Assert.AreEqual(10.1, (Double) annotation.Value, testDelta);
        }

        [TestMethod]
        public void TestSet()
        {
            Annotation annotation = new Annotation ( "name", "val" );
            annotation.Value = "HelloWorld";
            Assert.AreEqual("HelloWorld", annotation.Value);

            char[] value = { 'h', 'e', 'l', 'l', 'o'};

            annotation.Value = new String(value);
            Assert.AreEqual("hello", annotation.Value);

            annotation.Value = 10.5;
            Assert.AreEqual(10.5, (double) annotation.Value, testDelta);

        }

        [TestMethod]
        public void TestToString()
        {
            Annotation annotation = new Annotation ( "name", "val" );
            Assert.AreEqual("name='val'\n", annotation.ToString());
        }
         
    }
}
