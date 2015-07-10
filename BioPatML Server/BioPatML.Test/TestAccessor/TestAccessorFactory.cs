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
    /// NUnit test for the accessor factory.
    /// </summary>
    [TestClass]
    public class TestAccessorFactory
    {
        /// <summary>
        /// Tests the creation of accessors
        /// </summary>
        [TestMethod]
        public void TestInstance()
        {
            SymbolArray symbols = new SymbolArray(DnaAlphabet.Instance(), "ACTG");


            Assert.AreEqual
                (true, AccessorFactory.Instance
                        (AccessorFactory.LIN_DIR, 0, 0, 0, symbols) is AccessorLinear);

            Assert.AreEqual
                (true, AccessorFactory.Instance
                        (AccessorFactory.CIRC_DIR, 0, 0, 0, symbols) is AccessorCircular);

            Assert.AreEqual
                (true, AccessorFactory.Instance
                        (AccessorFactory.TRANS_DIR, 0, 0, 0, symbols) is AccessorTransparent);


            Assert.AreEqual
                (true, AccessorFactory.Instance
                        (AccessorFactory.LIN_REV_COMP, 0, 0, 0, symbols) is AccessorComplement);
        }
    }
}
