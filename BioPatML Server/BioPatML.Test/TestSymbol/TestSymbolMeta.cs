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
    public class TestSymbolMeta
    {
        [TestMethod]
        public void TestAdd()
        {
            SymbolMeta sym = new SymbolMeta('X', "XXX", "Unknown");
            Assert.AreEqual(0, sym.SymbolNumber);
            //sym.remove(new Symbol('a', "adenine"));
            sym.Add(new Symbol('a', "Ade", "Adenine"));
            sym.Add(new Symbol('u', "Ura", "Adenine"));
            Assert.AreEqual(2, sym.SymbolNumber);
            Assert.AreEqual("Adenine", sym[0].Name);
            Assert.AreEqual("Adenine", sym[1].Name);
        }

        [TestMethod]
        /** Tests the removal of symbols from the symbol set */
        public void TestRemove()
        {
            SymbolMeta sym = new SymbolMeta('X', "XXX", "Unknown");
            Symbol sym1 = new Symbol('a', "Ade", "Adenine");
            Symbol sym2 = new Symbol('u', "Ura", "Uracil");

            sym.Remove(sym1);
            sym.Add(sym1);
            sym.Add(sym2);
            
            
            Assert.AreEqual(2, sym.SymbolNumber);

            sym.Remove(sym1);
            Assert.AreEqual(1, sym.SymbolNumber);
            Assert.AreEqual(sym2, sym[0]);
        }

        [TestMethod]
        /** Tests the replacement of symbols within the symbol set */
        public void TestReplace()
        {
            SymbolMeta sym = new SymbolMeta('X', "XXX", "Unknown");
            Symbol sym1 = new Symbol('a', "Ade", "Adenine");
            Symbol sym2 = new Symbol('u', "Ura", "Uracil");
            Symbol sym3 = new Symbol('t', "Thy", "Thymine");

            sym.Add(sym1);
            sym.Add(sym2);
            Assert.AreEqual(2, sym.SymbolNumber);

            sym.Replace(sym2, sym3);
            Assert.AreEqual(2, sym.SymbolNumber);
            Assert.AreEqual(sym3, sym[1]);
        }

        [TestMethod]
        /** Tests the comparision with a meta symbol */
        public void TestEquals()
        {
            SymbolMeta sym = new SymbolMeta('X', "XXX", "Unknown");
            Symbol sym2 = new Symbol('a', "Ade", "Adenine");
            Symbol sym3 = new Symbol('u', "Ura", "Uracil");

            Assert.AreEqual(true, sym.Equals(sym2));
            Assert.AreEqual(true, sym2.Equals(sym));

            sym.Add(sym2);
            Assert.AreEqual(true, sym.Equals(sym2));
            Assert.AreEqual(true, sym2.Equals(sym));
            Assert.AreEqual(false, sym.Equals(sym3));
            Assert.AreEqual(false, sym3.Equals(sym));

            sym.Add(sym3);
            Assert.AreEqual(true, sym.Equals(sym));
            Assert.AreEqual(true, sym.Equals(sym2));
            Assert.AreEqual(true, sym.Equals(sym3));
        }

        [TestMethod]
        /** Tests the letter representation of the symbol set */
        public void TestLetters()
        {
            SymbolMeta sym = new SymbolMeta('X', "XXX", "Unknown");
            sym.Add(new Symbol('a', "Ade", "Adenine"));
            sym.Add(new Symbol('u', "Ura", "Adenine"));

            Assert.AreEqual("au", sym.Letters);
        }  
    }
}
