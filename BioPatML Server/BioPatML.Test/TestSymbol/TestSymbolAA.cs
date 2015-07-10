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
    public class TestSymbolAA
    {
        private ProteinAlphabet alpha = ProteinAlphabet.Instance();
        //private QUT.Bio.BioPatML.Symbols.Symbol s = new QUT.Bio.BioPatML.Symbols.Symbol('A', AlphabetType.DNA, "ss");
        //private BioPatML.Structures.MapChar mc = new BioPatML.Structures.MapChar();

        [TestMethod]
        public void TestProperties()
        {
            Assert.AreEqual("TFFFTTFF", ((SymbolAA)alpha['A']).Properties);
        }

         /** Tests Test for hydrophobicity */
        [TestMethod]
        public void TestIsHydrophobic() {
           Assert.AreEqual(true, ((SymbolAA)alpha['V']).IsHydrophobic);
           Assert.AreEqual(true, ((SymbolAA)alpha['L']).IsHydrophobic);
           Assert.AreEqual(false, ((SymbolAA)alpha['N']).IsHydrophobic);
           Assert.AreEqual(false, ((SymbolAA)alpha['E']).IsHydrophobic);
        }

        [TestMethod]
        /** Tests Test for polar amino acids */
        public void TestIsPolar() {
           Assert.AreEqual(true, ((SymbolAA)alpha['K']).IsPolar);
           Assert.AreEqual(true, ((SymbolAA)alpha['R']).IsPolar);
           Assert.AreEqual(false, ((SymbolAA)alpha['I']).IsPolar);
           Assert.AreEqual(false, ((SymbolAA)alpha['V']).IsPolar);
        }

        [TestMethod]
        /** Tests Test for positively charged amino acids */
        public void TestIsPositive() {
          Assert.AreEqual(true, ((SymbolAA)alpha['K']).IsPositive);
          Assert.AreEqual(true, ((SymbolAA)alpha['R']).IsPositive);
          Assert.AreEqual(false, ((SymbolAA)alpha['D']).IsPositive);
          Assert.AreEqual(false, ((SymbolAA)alpha['E']).IsPositive);
        }

        [TestMethod]
        /** Tests Test for negatively charged residues */
        public void TestIsNegative()
        {
            Assert.AreEqual(true, ((SymbolAA)alpha['D']).IsNegative);
            Assert.AreEqual(true, ((SymbolAA)alpha['E']).IsNegative);
            Assert.AreEqual(false, ((SymbolAA)alpha['K']).IsNegative);
            Assert.AreEqual(false, ((SymbolAA)alpha['R']).IsNegative);
        }

        [TestMethod]
        /** Tests Test for charged amino acids */
        public void TestIsCharged() {
            Assert.AreEqual(true, ((SymbolAA)alpha['D']).IsCharged);
            Assert.AreEqual(true, ((SymbolAA)alpha['E']).IsCharged);
            Assert.AreEqual(true, ((SymbolAA)alpha['K']).IsCharged);
            Assert.AreEqual(true, ((SymbolAA)alpha['R']).IsCharged);
            Assert.AreEqual(false, ((SymbolAA)alpha['G']).IsCharged);
            Assert.AreEqual(false, ((SymbolAA)alpha['P']).IsCharged);
        }

        [TestMethod]
        /** Tests Test for small amino acids */
        public void TestIsSmall() {
            Assert.AreEqual(true, ((SymbolAA)alpha['G']).IsSmall);
            Assert.AreEqual(true, ((SymbolAA)alpha['P']).IsSmall);
            Assert.AreEqual(false, ((SymbolAA)alpha['W']).IsSmall);
            Assert.AreEqual(false, ((SymbolAA)alpha['Y']).IsSmall);
        }

        [TestMethod]
        /** Tests Test for tiny amino acids */
        public void TestIsTiny() {
            Assert.AreEqual(true, ((SymbolAA)alpha['G']).IsTiny);
            Assert.AreEqual(true, ((SymbolAA)alpha['A']).IsTiny);
            Assert.AreEqual(false, ((SymbolAA)alpha['W']).IsTiny);
            Assert.AreEqual(false, ((SymbolAA)alpha['Y']).IsTiny);
        }
        
        [TestMethod]
        /** Tests Test for aromatic amino acids */
        public void TestIsAromatic() {
            Assert.AreEqual(true, ((SymbolAA)alpha['Y']).IsAromatic);
            Assert.AreEqual(true, ((SymbolAA)alpha['W']).IsAromatic);
            Assert.AreEqual(false, ((SymbolAA)alpha['Q']).IsAromatic);
            Assert.AreEqual(false, ((SymbolAA)alpha['D']).IsAromatic);
        }

        [TestMethod]
        /** Tests Test for aliphatic amino acids */
        public void TestIsAliphatic() {
            Assert.AreEqual(true, ((SymbolAA)alpha['I']).IsAliphatic);
            Assert.AreEqual(true, ((SymbolAA)alpha['V']).IsAliphatic);
            Assert.AreEqual(false, ((SymbolAA)alpha['W']).IsAliphatic);
            Assert.AreEqual(false, ((SymbolAA)alpha['Y']).IsAliphatic);
        }
    }
}
