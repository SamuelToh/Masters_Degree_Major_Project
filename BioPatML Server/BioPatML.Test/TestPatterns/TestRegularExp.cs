using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Common.XML;
using QUT.Bio.BioPatML.Alphabets;
using BioPatML.Test;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestPatterns
{
    [TestClass]
    public class TestRegularExp
    {
        [TestMethod]
        public void TestTest()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "acctccctcccgacgg");
            RegularExp regx = new RegularExp("test","ctc.");

            Match myMatch = regx.Match(seq, 3);

            Assert.IsNotNull(myMatch);

            FeatureList matches = seq.Search(0, 0, regx);
            Assert.IsNotNull(matches);
        }

        /** test for match method from pattern interface */
        [TestMethod]
        public void TestMatch()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "acctccgg");
            RegularExp regx = new RegularExp("test","ctc.");
            Match myMatch = regx.Match(seq, 1);

            Assert.AreEqual(null, myMatch);

            myMatch = regx.Match(seq, 3);
            Assert.AreEqual(3, myMatch.Start);
            Assert.AreEqual(4, myMatch.Length);
            Assert.AreEqual(1, myMatch.Strand);
            Assert.AreEqual(1.0, myMatch.Similarity, 1e-2);

            regx = new RegularExp("test", "acc");
            myMatch = regx.Match(seq, 1);
            Assert.AreEqual(1, myMatch.Start);

            regx = new RegularExp("test", "cgg");
            myMatch = regx.Match(seq, 6);
            Assert.AreEqual(8, myMatch.End);

            //last test
            
            //ISSUE: could be star problem
            regx = new RegularExp("test", "c.*g");
            FeatureList matches = seq.Search(0, 0, regx);
            Assert.AreEqual(4, matches.Count);
            Assert.AreEqual("cctccgg", matches[0].Letters());
            Assert.AreEqual("ctccgg", matches[1].Letters());
            Assert.AreEqual("ccgg", matches[2].Letters());
            Assert.AreEqual("cgg", matches[3].Letters());


        }

        [TestMethod]
        /** test for case insensitivity of a match */
        public void TestCaseInsensitivity()
        {
            RegularExp regx;

            regx = new RegularExp("test", "act", true);
            Assert.IsNotNull(regx.Match(new Sequence(AlphabetType.DNA, "act"), 1));

            regx = new RegularExp("test", "act", true);
            Assert.IsNotNull(regx.Match(new Sequence(AlphabetType.DNA, "ACT"), 1));


            regx = new RegularExp("test", "ACT", true);
            Assert.IsNull(regx.Match(new Sequence(AlphabetType.DNA, "act"), 1));

            //---
            regx = new RegularExp("test", "Act", true);
            Assert.IsNull(regx.Match(new Sequence(AlphabetType.DNA, "ACT"), 1));

            regx = new RegularExp("test", "Act", false);
            Assert.IsNotNull(regx.Match(new Sequence(AlphabetType.DNA, "act"), 1));


            regx = new RegularExp("test", "Act", false);
            Assert.IsNotNull(regx.Match(new Sequence(AlphabetType.DNA, "ACT"), 1));
        }

        [TestMethod]
        public void TestRead()
        {

			Definition definition = DefinitionIO.Read( Global.GetResourceReader(   "BioPatMLXML/Regex.test" ) );
            RegularExp pattern = (RegularExp)definition.Pattern;

            Assert.AreEqual("Regex", definition.Name);
            Assert.AreEqual("regex", pattern.Name);
            Assert.AreEqual(0.9, pattern.Impact, 1e-3);
            Assert.AreEqual(false, pattern.IsCaseSensitive);
            Assert.AreEqual("Regex: 'regex'=act", pattern.ToString());
        }

        [TestMethod]
        public void TestToXml()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(   "BioPatMLXML/Regex.test" ) );

			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
			Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );

			RegularExp pattern = (RegularExp) def2.Pattern;

            Assert.AreEqual("Regex", def2.Name);
            Assert.AreEqual("regex", pattern.Name);
            Assert.AreEqual(0.9, pattern.Impact, 1e-3);
            Assert.AreEqual(false, pattern.IsCaseSensitive);
            Assert.AreEqual("Regex: 'regex'=act", pattern.ToString());
        }

    }
}
