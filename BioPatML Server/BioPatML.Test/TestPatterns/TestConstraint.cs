using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DB = System.Diagnostics.Debug;using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Common.XML;
using QUT.Bio.BioPatML.Sequences;
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
    public class TestConstraint
    {

        /** test for match at sequence start */
        [TestMethod]
        public void TestMatchStart()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "atgcatgc");
            Series series = new SeriesBest("test", 1.0);
            series.Add(new Constraint("test", "START", +1));
            series.Add(new Motif("motif", AlphabetType.DNA, "nnnn", 0.0));

            Match match = series.Match(seq, 2);
            Assert.AreEqual("tgca", match.Letters());
            match = series.Match(seq, 1);
            Assert.AreEqual(null, match);

            match = seq.SearchBest(0, 0, series);
            Assert.AreEqual("tgca", match.Letters());
        }

        /** test for match at sequence end */
        [TestMethod]
        public void TestMatchEnd()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "atgcatgc");
            Series series = new SeriesBest("test", 1.0);
            series.Add(new Motif("motif", AlphabetType.DNA, "nnnn", 1.0));
            series.Add(new Constraint("test", "END", -1));

            Match match = series.Match(seq, 4);
            Assert.AreEqual("catg", match.Letters());

            match = series.Match(seq, 1);
            Assert.AreEqual(null, match);
            
			match = series.Match(seq, 5);
            Assert.AreEqual(null, match);

            match = seq.SearchBest(0, 0, series);
            Assert.AreEqual("catg", match.Letters());
        }

        /** test for match at sequence center */
        [TestMethod]
        public void TestMatchCenter()
        {
            Sequence seq = new Sequence(AlphabetType.DNA, "atgcatg");
            Series series = new SeriesBest("test", 1.0);
            series.Add(new Constraint("test", "CENTER", -1));
            series.Add(new Motif("motif", AlphabetType.DNA, "nnnn", 1.0));

            Match match = series.Match(seq, 3);
            Assert.AreEqual("gcat", match.Letters());
            match = series.Match(seq, 2);
            Assert.AreEqual(null, match);
            match = series.Match(seq, 4);
            Assert.AreEqual(null, match);

            match = seq.SearchBest(0, 0, series);
            Assert.AreEqual("gcat", match.Letters());
        }

        [TestMethod]
        public void TestRead()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(  "BioPatMLXML/Constraint.xml" ) );
            Constraint pattern = (Constraint)((Series)definition.Pattern).Patterns[1];

            //Assert.AreEqual(1.0, pattern.Threshold); there is no threshold attr for constraint
            Assert.AreEqual("constraint", pattern.Name);
            Assert.AreEqual(0.9, pattern.Impact, 1e-3);
        }

        [TestMethod]
        public void TestToXml()
        {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(  "BioPatMLXML/Constraint.xml" ) );

			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
			Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );

			Constraint pattern = (Constraint) ( (Series) def2.Pattern ).Patterns[1];

            //Assert.AreEqual(1.0, pattern.Threshold); there is no threshold attr for constraint
            Assert.AreEqual("constraint", pattern.Name);
            Assert.AreEqual(0.9, pattern.Impact, 1e-3);
        }
  
    }
}
