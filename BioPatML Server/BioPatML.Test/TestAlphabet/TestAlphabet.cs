using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Symbols;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestAlphabet
{
    /// <summary>
    /// NUnit tests for the alphabet base
    /// </summary>
    [TestClass]
    public class TestAlphabet
    {
		class TestAlphabetClass : Alphabet {
			public TestAlphabetClass () : base( "test" ) { }

			private static Symbol defaultSymbol = new Symbol( 'a', "Ade", "Adenine" );

			public override Symbol DefaultSymbol {
				get { return defaultSymbol; }
			}
		}

        private Alphabet alphabet, alphabet2;
        private Symbol sym1, sym2, sym3;

        [TestInitialize]
        public void SetUp()
        {
            alphabet = new TestAlphabetClass();
			alphabet2 = new TestAlphabetClass();

            sym1 = new Symbol('a', "Ade", "Adenine");
            sym2 = new Symbol('u', "Ura", "Uracil");
            sym3 = new SymbolMeta('-', "GAP", "Gap");
        }

        /// <summary>
        /// Test the constructor
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            Assert.AreEqual("test", alphabet.Name);
        }

        /// <summary>
        /// Test counting the number of symbols
        /// </summary>
        [TestMethod]
        public void TestCount()
        {
            Assert.AreEqual(0, alphabet.Count(true));
            Assert.AreEqual(0, alphabet.Count(false));

            alphabet.Add(sym1);
            alphabet.Add(sym2);
            alphabet.Add(sym3);

            Assert.AreEqual(3, alphabet.Count(true));
            Assert.AreEqual(2, alphabet.Count(false));

        }

        /// <summary>
        /// Test validity of symbols in a specified alphabet
        /// </summary>
        [TestMethod]
        public void TestIsValid()
        {
            alphabet.Add(sym1);

            Assert.AreEqual(false, alphabet.IsValid('A'));
            Assert.AreEqual(true, alphabet.IsValid('a'));
            Assert.AreEqual(false, alphabet.IsValid('b'));

            alphabet2.Add(sym1);
            alphabet2.Add(sym2);

            Assert.AreEqual(true, alphabet.IsValid(alphabet2['a']));
            Assert.AreEqual(false, alphabet.IsValid(alphabet2['u']));
        }

        /// <summary>
        /// Try adding and getting alphabet
        /// </summary>
        [TestMethod]
		[ExpectedException( typeof( AssertFailedException ) )]
        public void TestAddGet()
        {
            Assert.AreEqual(0, alphabet.Count(true));

            alphabet.Add(sym1);
            alphabet.Add(sym2);
            alphabet.Add(sym3);

            Assert.AreEqual(3, alphabet.Count(true));
            Assert.AreEqual(2, alphabet.Count(false));
            Assert.AreEqual(sym1, alphabet[0, false]);
            Assert.AreEqual(sym2, alphabet[1, false]);

            Assert.AreEqual(sym1, alphabet[0, true]);
            Assert.AreEqual(sym2, alphabet[1, true]);
            Assert.AreEqual(sym3, alphabet[2, true]);

            Assert.AreEqual(sym1, alphabet['a']);
            Assert.AreEqual(sym2, alphabet['u']);
            Assert.AreEqual(sym3, alphabet['-']);

            Assert.AreEqual('g', alphabet['g']);  //expecting exception thrown as g dont exist
        }

        /// <summary>
        /// Test default alphabet
        /// </summary>
        [TestMethod]
        public void TestSetGetDefault()
        {
            Assert.AreEqual(sym1, alphabet.DefaultSymbol);
            Assert.AreEqual(alphabet.DefaultSymbol, alphabet['ü']);
        }

        /// <summary>
        /// Test adding the symbols
        /// </summary>
        [TestMethod]
        public void TestSymbols()
        {
            alphabet.Add(sym1);
            alphabet.Add(sym2);
            alphabet.Add(sym3);

            int index = 0;

            foreach (Symbol syml in alphabet) 
                Assert.AreEqual(alphabet[index++, true], syml);

            index = 0;

            foreach (Symbol syml in alphabet)
                Assert.AreEqual(alphabet[index++, false], syml); 
           
        }

        /// <summary>
        /// Test removing the symbols
        /// </summary>
        [TestMethod]
        public void TestRemove()
        {
            alphabet.Add(sym1);
            alphabet.Add(sym2);
            Console.WriteLine(alphabet[0, true].Name);
            Console.WriteLine(alphabet[1, true].Name); 
            Assert.AreEqual(2, alphabet.Count(true));

            alphabet.Remove('a');
            Assert.AreEqual(1, alphabet.Count(true));
            
            Assert.AreEqual("Uracil", alphabet[0, true].Name);
        }

        [TestCleanup]
        public void TearDown()
        {
            this.alphabet = null;
            this.alphabet2 = null;
            this.sym1 = null;
            this.sym2 = null;
            this.sym3 = null;
        }

    }
}
