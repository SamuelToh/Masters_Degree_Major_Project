using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Statistic;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestSatistic
{
    [TestClass]
    public class TestHistogramSymbol
    {
        private HistogramSymbol histo;
        private Alphabet alpha;

        [TestInitialize]
        public void Setup()
        {
            histo = new HistogramSymbol();
            alpha = AlphabetFactory.Instance(AlphabetType.DNA);
        }

        [TestMethod]
        /** Tests the adding and Getting for the histogram */
        public void TestAddGet()
        {
            histo.Add(alpha['a'], 1);
            histo.Add(alpha['c'], 3);
            histo.Add(alpha['a'], 1);
            histo.Add(alpha['t'], 2);

            Assert.AreEqual(2, histo.HistoValue(alpha['a']));
            Assert.AreEqual(3, histo.HistoValue(alpha['c']));
            Assert.AreEqual(2, histo.HistoValue(alpha['t']));
            Assert.AreEqual(0, histo.HistoValue(alpha['g']));
        }

        [TestMethod]
        /** Tests the adding of a sequence to the histogram */
        public void TestAddSequence()
        {
            histo.Add(new Sequence(AlphabetType.DNA, "actgactaca"));

            Assert.AreEqual(4, histo.HistoValue(alpha['a']));
            Assert.AreEqual(3, histo.HistoValue(alpha['c']));
            Assert.AreEqual(2, histo.HistoValue(alpha['t']));
            Assert.AreEqual(1, histo.HistoValue(alpha['g']));
        }

        [TestMethod]
        /** Tests the adding of a sequence list to the histogram */
        public void TestAddSequenceList()
        {
            SequenceList list = new SequenceList();
            list.Add(new Sequence(AlphabetType.DNA, "actga"));
            list.Add(new Sequence(AlphabetType.DNA, "ctaca"));

            histo.Add(list);

            Assert.AreEqual(4, histo.HistoValue(alpha['a']));
            Assert.AreEqual(3, histo.HistoValue(alpha['c']));
            Assert.AreEqual(2, histo.HistoValue(alpha['t']));
            Assert.AreEqual(1, histo.HistoValue(alpha['g']));
        }

        [TestMethod]
        /** Tests the adding of a histogram to the histogram */
        public void TestAddHistogram()
        {
            HistogramSymbol histo2 = new HistogramSymbol();
            histo.Add(new Sequence(AlphabetType.DNA, "gactt"));
            histo2.Add(new Sequence(AlphabetType.DNA, "act"));
            histo.Add(histo2);

            Assert.AreEqual(2, histo.HistoValue(alpha['a']));
            Assert.AreEqual(2, histo.HistoValue(alpha['c']));
            Assert.AreEqual(3, histo.HistoValue(alpha['t']));
            Assert.AreEqual(1, histo.HistoValue(alpha['g']));
        }

        
        [TestMethod]
        /** Tests the subtraction of a histogram from a histogram */
        public void TestSubtractHistogram()
        {
            HistogramSymbol histo2 = new HistogramSymbol();
            histo.Add(new Sequence(AlphabetType.DNA, "gactt"));
            histo2.Add(new Sequence(AlphabetType.DNA, "act"));
            histo.Substract(histo2);

            Assert.AreEqual(0, histo.HistoValue(alpha['a']));
            Assert.AreEqual(0, histo.HistoValue(alpha['c']));
            Assert.AreEqual(1, histo.HistoValue(alpha['t']));
            Assert.AreEqual(1, histo.HistoValue(alpha['g']));
        }

        [TestMethod]
        /** Tests the calculation of the sum over all counts */
        public void TestSum()
        {
            histo.Add(new Sequence(AlphabetType.DNA, "actgactaca"));
            Assert.AreEqual(10, histo.Sum);
        }

        [TestMethod]
        /** Tests the finding of the maximum value within the histogram */
        public void TestMax()
        {
            histo.Add(new Sequence(AlphabetType.DNA, "actgactaca"));
            Assert.AreEqual(4, histo.Max);
        }

        [TestMethod]
        /** Tests the calculation of the rel. frequency */
        public void TestFrequency()
        {
            histo.Add(new Sequence(AlphabetType.DNA, "aaaaacccgg"));
            Assert.AreEqual(0.5, histo.Frequency(alpha['a']), 1e-3);
            Assert.AreEqual(0.3, histo.Frequency(alpha['c']), 1e-3);
            Assert.AreEqual(0.2, histo.Frequency(alpha['g']), 1e-3);
            Assert.AreEqual(0.0, histo.Frequency(alpha['t']), 1e-3);
        }

        [TestMethod]
        /** Tests the handling of bin indicies */
        public void TestBinIndex()
        {
            histo.Add(alpha['a']);
            histo.Add(alpha['c']);
            histo.Add(alpha['a']);
            histo.Add(alpha['t']);

            Assert.AreEqual(3, histo.Count);
            Assert.AreEqual(2, histo.HistoValue(0));
            Assert.AreEqual(1, histo.HistoValue(1));
            Assert.AreEqual(1, histo.HistoValue(2));

            Assert.AreEqual(0, histo.CalSymIndex(alpha['a']));
            Assert.AreEqual(1, histo.CalSymIndex(alpha['c']));
            Assert.AreEqual(2, histo.CalSymIndex(alpha['t']));
            Assert.AreEqual(-1, histo.CalSymIndex(alpha['g']));
        }

        [TestMethod]
        /** Tests the Getting of Symbols */
        public void TestGetSymbol()
        {
            histo.Add(alpha['a']);
            histo.Add(alpha['c']);
            histo.Add(alpha['a']);
            histo.Add(alpha['t']);

            Assert.AreEqual(alpha['a'], histo[0]);
            Assert.AreEqual(alpha['c'], histo[1]);
            Assert.AreEqual(alpha['t'], histo[2]);
        }

        [TestMethod]
        /** Tests the removal of all histogram entries */
        public void TestClear()
        {
            histo.Add(alpha['a']);
            histo.Add(alpha['c']);
            histo.Clear();
            Assert.AreEqual(0, histo.Sum);
            Assert.AreEqual(0, histo.Count);
            Assert.AreEqual(0, histo.HistoValue(alpha['a']));
        }

        [TestMethod]
        /** Tests the conversion to a string */
        public void TestToString()
        {
            histo.Add(alpha['a'], 1);
            histo.Add(alpha['c'], 2);
            histo.Add(alpha['t'], 3);

            Assert.AreEqual("a:1 c:2 t:3 ", histo.ToString());
        }

    }
}
