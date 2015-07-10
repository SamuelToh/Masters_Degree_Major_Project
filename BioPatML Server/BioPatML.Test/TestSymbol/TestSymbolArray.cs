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
    public class TestSymbolArray
    {
        private Alphabet alpha = DnaAlphabet.Instance();

        [TestMethod]
        /** Tests the construction of symbol arrays */
        public void TestConstructor() {
            Symbol[] symbols = {alpha['A'], alpha['G']};
            SymbolArray symbolArray;
    
            symbolArray = new SymbolArray(alpha, symbols);
            Assert.AreEqual(2, symbolArray.Length);
            Assert.AreEqual(alpha, symbolArray.Alphabet);
    
            symbolArray = new SymbolArray(alpha, "AG");
            Assert.AreEqual(2, symbolArray.Length);
            Assert.AreEqual(alpha, symbolArray.Alphabet); //Query for alphabet type;

        }

        [TestMethod]
        /** Tests the Getter for symbols */
        public void TestSymbolAt()
        {
            SymbolArray symbolArray;
            Symbol[] symbols = { alpha['A'], alpha['C'], null };

            symbolArray = new SymbolArray(alpha, symbols);
            Assert.AreEqual(alpha['A'], symbolArray.SymbolAt(0));
            Assert.AreEqual(alpha['C'], symbolArray.SymbolAt(1));
            Assert.AreEqual(null, symbolArray.SymbolAt(2));

            symbolArray = new SymbolArray(alpha, "ACü");
            Assert.AreEqual(alpha['A'], symbolArray.SymbolAt(0));
            Assert.AreEqual(alpha['C'], symbolArray.SymbolAt(1));
            Assert.AreEqual(alpha['N'], symbolArray.SymbolAt(2));  // default letter
        }
    }
}
