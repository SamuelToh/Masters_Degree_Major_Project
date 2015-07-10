using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Patterns;
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
    public class TestAny
    {
        /** Test the constructor */
        [TestMethod]
        public void TestConstructorAny()
        {
            Any any = new Any("Any", 1, 3, 3.14);
            Assert.AreEqual("Any", any.Name);
            Assert.AreEqual(1, any.MinLength);
            Assert.AreEqual(3, any.MaxLength);
            Assert.AreEqual(3.14, any.IncLength);
        }  

        /** Tests the matching  */
        [TestMethod]
        public void TestMatch()
        {
            Any any = new Any("", 1, 3, 1);
            Sequence seq = new Sequence(AlphabetType.DNA, "actg");
            Match match = any.Match(seq, 2);

            Assert.AreEqual("c", match.Letters());
            Assert.AreEqual(0, any.Increment);

            any.Match(seq, 2);
            Assert.AreEqual("ct", match.Letters());
            Assert.AreEqual(0, any.Increment);

            any.Match(seq, 2);
            Assert.AreEqual("ctg", match.Letters());
            Assert.AreEqual(1, any.Increment);

            any.Match(seq, 2);
            Assert.AreEqual("c", match.Letters());
            Assert.AreEqual(0, any.Increment);

        }

        /** Tests the matching with increment  */
        [TestMethod]
        public void TestMatchIncrement1()
        {
            Any any = new Any("Any", 1, 4, 2.0);
            Sequence seq = new Sequence(AlphabetType.DNA, "actg");
            Match match = any.Match(seq, 1);
            Assert.AreEqual("a", match.Letters());
            Assert.AreEqual(0, any.Increment);
            any.Match(seq, 1);
            Assert.AreEqual("act", match.Letters());
            Assert.AreEqual(0, any.Increment);
            any.Match(seq, 1);
            Assert.AreEqual("actg", match.Letters());
            Assert.AreEqual(1, any.Increment);
            any.Match(seq, 1);
            Assert.AreEqual("a", match.Letters());
            Assert.AreEqual(0, any.Increment);
        }

        /** Tests the matching with increment  */
        [TestMethod]
        public void TestMatchIncrement2()
        {
            Any any = new Any("Any", 1, 3, 0.4);
            Sequence seq = new Sequence(AlphabetType.DNA, "actg");
            Match match = any.Match(seq, 1);
            Assert.AreEqual("a", match.Letters());
            Assert.AreEqual(0, any.Increment);
            any.Match(seq, 1);
            Assert.AreEqual("a", match.Letters());
            Assert.AreEqual(0, any.Increment);
            any.Match(seq, 1);
            Assert.AreEqual("ac", match.Letters());
            Assert.AreEqual(0, any.Increment);
            any.Match(seq, 1);
            Assert.AreEqual("ac", match.Letters());
            Assert.AreEqual(0, any.Increment);
            any.Match(seq, 1);
            Assert.AreEqual("act", match.Letters());
            Assert.AreEqual(1, any.Increment);
            any.Match(seq, 1);
            Assert.AreEqual("a", match.Letters());
            Assert.AreEqual(0, any.Increment);
        }      

        /** Tests the reading of an Any pattern from an XML document */
        [TestMethod]
        public void TestRead()
        {
            Definition definition = DefinitionIO.Read( Global.GetResourceReader( "BioPatMLXML/Any.xml" ) );
            Any pattern = (Any)definition.Pattern;

            Assert.AreEqual("Any", definition.Name);
            Assert.AreEqual("any", pattern.Name);
            Assert.AreEqual(6, pattern.MinLength);
            Assert.AreEqual(8, pattern.MaxLength);
            Assert.AreEqual(1.1, pattern.IncLength);
            Assert.AreEqual(0.9, pattern.Impact);
        }

        /** Tests the reading of an Any pattern from an XML document */
        [TestMethod]
        public void TestToXml()
        {
            Definition definition = DefinitionIO.Read( Global.GetResourceReader( "BioPatMLXML/Any.xml" ) );
            Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );

			Any pattern = (Any) def2.Pattern;

            Assert.AreEqual("Any", definition.Name);
            Assert.AreEqual("any", pattern.Name);
            Assert.AreEqual(6, pattern.MinLength);
            Assert.AreEqual(8, pattern.MaxLength);
            Assert.AreEqual(1.1, pattern.IncLength);
            Assert.AreEqual(0.9, pattern.Impact);

			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
		}
    }
}
