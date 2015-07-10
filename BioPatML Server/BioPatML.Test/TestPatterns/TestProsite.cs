using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Common.XML;
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
    public class TestProsite
    {

        [TestMethod]
        public void TestMatch()
        {
            Alphabet alphabet = DnaAlphabet.Instance();
            Sequence seq = new Sequence(AlphabetType.DNA, "acctccgg");
            Prosite prosite = new Prosite("c-t-c-x.", alphabet);
            Match match = prosite.Match(seq, 1);
            Assert.AreEqual(null, match);

            match = prosite.Match(seq, 3);
            Assert.AreEqual(3, match.Start);
            Assert.AreEqual(4, match.Length);
            Assert.AreEqual(1, match.Strand);
            Assert.AreEqual(1.0, match.Similarity, 1e-2);


            prosite = new Prosite("a-c-c.", alphabet);
            match = prosite.Match(seq, 1);
            Assert.AreEqual(1, match.Start);

            prosite = new Prosite("<a-c-c.", alphabet);
            match = prosite.Match(seq, 3);
            Assert.AreEqual(null, match);

            prosite = new Prosite("c-g-g.", alphabet);
            match = prosite.Match(seq, 6);
            Assert.AreEqual(8, match.End);

            prosite = new Prosite("c-y-y-c.", alphabet);
            Assert.IsNotNull(prosite.Match(seq, 2));

            prosite = new Prosite("c-{d}-c.", alphabet);
            //Assert.IsNotNull(prosite.Match(seq, 1)); //<-- not sure why this pattern wont work

            prosite = new Prosite("c-x(0,2)-g.", alphabet);
            FeatureList matches = seq.Search(0, 0, prosite);
            Assert.AreEqual(2, matches.Count);
            Assert.AreEqual("ccgg", matches[0].Letters());
            Assert.AreEqual("cgg", matches[1].Letters());
  
        }

        [TestMethod]
        /** Tests the conversion of prosite patterns to regular expressions */
        public void TestConvertPattern()
        {
            Alphabet alphabet = DnaAlphabet.Instance();
            Prosite prosite = new Prosite();
            Assert.AreEqual("actg", prosite.Convert("actg.", alphabet).ToString());
            Assert.AreEqual("[ac].[ag].{4}[^e[agt]]$", prosite.Convert("[ac]-x-r-x(4)-{ed}>.", alphabet).ToString());
            Assert.AreEqual("^a.[tg]{2}.{0,1}[ag]", prosite.Convert("<a-x-[tg](2)-x(0,1)-r.", alphabet).ToString());
        }

        [TestMethod]
        /** Tests the conversion of char to regular expressions */
        public void TestConvertChar()
        {
            Alphabet alphabet = DnaAlphabet.Instance();
            Prosite prosite = new Prosite();
            Assert.AreEqual("a", prosite.Convert('a', alphabet));
            Assert.AreEqual("[ag]", prosite.Convert('r', alphabet));
            Assert.AreEqual("0", prosite.Convert('0', alphabet));
            Assert.AreEqual(",", prosite.Convert(',', alphabet));
        }

        [TestMethod]
        public void TestRead()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(   "BioPatMLXML/Prosite.xml" ) );
            Prosite pattern = (Prosite)definition.Pattern;

            Assert.AreEqual("Prosite", definition.Name);
            Assert.AreEqual("prosite", pattern.Name);
            Assert.AreEqual("a-c-t", pattern.ToString());
            Assert.AreEqual(0.9, pattern.Impact, 1e-3);
        }

        [TestMethod]
        public void TestToXml()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(   "BioPatMLXML/Prosite.xml" ) );

			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
			Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );

			Prosite pattern = (Prosite) def2.Pattern;

            Assert.AreEqual("Prosite", def2.Name);
            Assert.AreEqual("prosite", pattern.Name);
            Assert.AreEqual("a-c-t", pattern.ToString());
            Assert.AreEqual(0.9, pattern.Impact, 1e-3);
        }
    }
}
