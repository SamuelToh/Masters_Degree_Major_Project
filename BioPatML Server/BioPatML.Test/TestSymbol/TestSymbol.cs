using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Symbols;
using QUT.Bio.BioPatML.Alphabets;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestSymbol
{
    [TestClass]
    public class TestSymbol
    {
        [TestMethod]
        /** Tests the construction of symbols */
        public void TestConstructor()
        {
            char[] adenine = { 'A', 'd', 'e', 'n', 'i', 'n', 'e' };
            Symbol sym1 = new Symbol('a', "Ade", "Adenine");
            Symbol sym2 = new Symbol('a', "Ade", new String(adenine));

            Assert.AreEqual('a', sym1.Letter);
            Assert.AreEqual("Adenine", sym1.Name);
            Assert.AreEqual(sym1, sym1.Complement);
            Assert.IsTrue(sym1.Name == sym2.Name);
        }

        [TestMethod]
        /** Tests the comparions of symbols */
        public void TestEquals()
        {
            char[] adenine = { 'A', 'd', 'e', 'n', 'i', 'n', 'e' };
            Symbol sym1 = new Symbol('a', "Ade", new string(adenine));
            Symbol sym2 = new Symbol('a', "Ade", "Adenine");
            Symbol sym3 = new Symbol('u', "Ura", "Uracil");

            Assert.AreEqual(true, sym1.Equals(sym2));
            Assert.AreEqual(true, sym2.Equals(sym1));
            Assert.AreEqual(false, sym1.Equals(sym3));
            Assert.AreEqual(false, sym3.Equals(sym1));
        }
    }
}
