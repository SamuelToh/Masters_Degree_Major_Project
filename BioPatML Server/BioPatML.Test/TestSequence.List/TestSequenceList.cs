using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Statistic;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Sequences.Annotations;


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
    public class TestSequenceList
    {
        [TestMethod]
        /** Tests the constructor of a sequence list */
        public void TestConstructor()
        {
            SequenceList list = new SequenceList();
            Assert.AreEqual(0, list.Count);
            list.Add(new Sequence(AlphabetType.DNA, "atcg"));
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        /** Test getter for sequences  */
        public void TestSequence()
        {
            SequenceList list = new SequenceList();
            Sequence seq1 = new Sequence(AlphabetType.DNA, "atcg");
            Sequence seq2 = new Sequence(AlphabetType.DNA, "gtca");
            Sequence seq3 = new Sequence(AlphabetType.DNA, "tttt");
            seq2.Annotations.Add( new Annotation("Name", "seq2") );
            seq3.Annotations.Add( new Annotation("Name", "seq3") );
            list.Add(seq1);
            list.Add(seq2);
            list.Add(seq3);
            Assert.AreEqual(seq1, list[0]);
            Assert.AreEqual(seq2, list[1]);
            Assert.AreEqual(seq1, list[3]);
            Assert.AreEqual(seq3, list[-1]);
        }


        [TestMethod]
        /** Test getter for features according to a feature name */
        public void TestFeatures()
        {
            SequenceList list = new SequenceList();
            Sequence seq1 = new Sequence(AlphabetType.DNA, "atcg");
            Sequence seq2 = new Sequence(AlphabetType.DNA, "gtca");
            seq1.AddFeatures(new FeatureList("Testlist"));
            seq1.FeatureLists["Testlist"].Add(new Feature("Test", 1, 2, +1));
            seq2.AddFeatures(new FeatureList("Testlist"));
            seq2.FeatureLists["Testlist"].Add(new Feature("Test", 2, 3, +1));
            list.Add(seq1);
            list.Add(seq2);

            FeatureList flist = list.Features("Testlist", "Test");
            Assert.AreEqual(2, flist.Count);
            Assert.AreEqual("at", flist[0].Letters());
            Assert.AreEqual("tc", flist[1].Letters());
        }

       
    }
}
