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
    public class TestAnnotatedList
    {
        private AnnotatedList<Sequence> list = null;

        [TestInitialize]
        public void SetUp()
        {
            list = new AnnotatedList<Sequence>("test");
        }

        [TestMethod]
        public void TestSortByAnnotation()
        {
            // TODO: Complete these unit tests.
        }


    }
}
