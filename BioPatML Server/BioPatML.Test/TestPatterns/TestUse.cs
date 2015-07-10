using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DB = System.Diagnostics.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Common.XML;
using QUT.Bio.BioPatML.Patterns;
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
namespace TestBioPatML.TestPatterns {
	[TestClass]
	public class TestUse {
		/** Test the constructor */
		[TestMethod]
		public void TestConstructor () {
			Definition container = new Definition();
			container.Definitions.Add( new Definition( "Def", new Any( "Any", 1, 3, 1 ) ) );
			Use use = new Use { Name = "Use" };
			use.ReferTo( container, "Def" );
			Assert.AreEqual( "Use", use.Name );
			Assert.AreEqual( "Def", use.ReferencedDefinition.Name );
		}

		/** Tests the matching of a use pattern */
		[TestMethod]
		public void TestMatch ()
        {
			Definition def = new Definition();
			def.Definitions.Add( new Definition("Def", new Motif("Motif", AlphabetType.DNA, "atg", 0.0)) );
            Sequence seq = new Sequence(AlphabetType.DNA, "atgc");
            Use use = new Use{ Name = "Use" };
			use.ReferTo( def, "Def" );
            Match match = use.Match(seq, 1);
            Assert.AreEqual(use, match.MatchPattern);
            Assert.AreEqual(1, match.Start);
            Assert.AreEqual(3, match.Length);
            Assert.AreEqual(1, match.Strand);
            Assert.AreEqual(1.0, match.Similarity, 1e-2);
        }

		/** Tests the reading of a use pattern from an XML document */
		[TestMethod]
		public void TestRead () {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader( "BioPatMLXML/Use.xml" ) );
			Use pattern = (Use) ( (Series) definition.Pattern ).Patterns[0];

			Assert.AreEqual( "promoter", definition.Name );
			Assert.AreEqual( "useTag", pattern.Name );
			Assert.AreEqual( "-35element", pattern.ReferencedDefinition.Name );
		}

		/** Tests the reading of a use pattern from an XML document */
		[TestMethod]
		public void TestToXml () {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader( "BioPatMLXML/Use.xml" ) );

			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
			Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );

			Use pattern = (Use) ( (Series) def2.Pattern ).Patterns[0];

			Assert.AreEqual( "promoter", def2.Name );
			Assert.AreEqual( "useTag", pattern.Name );
			Assert.AreEqual( "-35element", pattern.ReferencedDefinition.Name );
		}
	}
}
