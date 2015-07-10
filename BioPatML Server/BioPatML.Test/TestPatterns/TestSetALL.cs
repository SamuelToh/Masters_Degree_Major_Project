using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DB = System.Diagnostics.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;
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
namespace TestBioPatML.TestPatterns {
	[TestClass]
	public class TestSetALL {
		[TestMethod]
		/** Tests if the match method finds all patterns */
		public void TestMatch () {
			Sequence seq = new Sequence( AlphabetType.DNA, "taaacc" );
			SetAll set = new SetAll( "SetAll", 0.5 );
			set.Add( new Motif( "motif", AlphabetType.DNA, "taa", 0.0 ) );
			set.Add( new Motif( "motif1", AlphabetType.DNA, "aacc", 0.0 ) );
			FeatureList matches;

			matches = seq.Search( 1, seq.Length, set );
			Assert.AreEqual( 4, matches.Count );
			Assert.AreEqual( "taa", matches[0].Letters() );
			Assert.AreEqual( 1.00, ( (Match) matches[0] ).Similarity, 1e-2 );
			Assert.AreEqual( "aaa", matches[1].Letters() );
			Assert.AreEqual( 0.66, ( (Match) matches[1] ).Similarity, 1e-2 );
			Assert.AreEqual( "aaac", matches[2].Letters() );
			Assert.AreEqual( 0.75, ( (Match) matches[2] ).Similarity, 1e-2 );
			Assert.AreEqual( "aacc", matches[3].Letters() );
			Assert.AreEqual( 1.00, ( (Match) matches[3] ).Similarity, 1e-2 );
		}

		[TestMethod]
		public void TestRead () {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(   "BioPatMLXML/SetAll.xml" ) );
			SetAll pattern = (SetAll) definition.Pattern;

			Assert.AreEqual( "SetAll", pattern.Name );
			Assert.AreEqual( 0.7, pattern.Threshold, 1e-3 );
			Assert.AreEqual( 0.9, pattern.Impact, 1e-3 );

			Assert.AreEqual( "motif1", pattern[0].Name );
			Assert.AreEqual( "motif2", pattern[1].Name );
			Assert.AreEqual( "regex1", pattern[2].Name );
		}

		[TestMethod]
		public void TestToXml () {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader( "BioPatMLXML/SetAll.xml" ) );

			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
			Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );

			SetAll pattern = (SetAll) def2.Pattern;

			Assert.AreEqual( "SetAll", pattern.Name );
			Assert.AreEqual( 0.7, pattern.Threshold, 1e-3 );
			Assert.AreEqual( 0.9, pattern.Impact, 1e-3 );

			Assert.AreEqual( "motif1", pattern[0].Name );
			Assert.AreEqual( "motif2", pattern[1].Name );
			Assert.AreEqual( "regex1", pattern[2].Name );
		}
	}
}
