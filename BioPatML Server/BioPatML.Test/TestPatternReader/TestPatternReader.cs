using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DB = System.Diagnostics.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Patterns;
using BioPatML.Test;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestPatternReader {
	[TestClass]
	public class TestPatternReader {
		/// <summary>
		/// Testing a very very bad scenario... malformed xml file being feed into BioPatML reader
		/// </summary>
		
		[TestMethod]
		[ExpectedException( typeof( ArgumentException ) )]
		
		public void TestReadBadXML () {
			DefinitionIO.Read( Global.GetResourceReader(   "BioPatMLXML/BadMotif.xml" ) );
		}

		/// <summary>
		/// Test reading a good motif biopatml document. This Motif file is the same one
		/// used in TestPattern package, TestMotif class.
		/// </summary>
		
		[TestMethod]
		
		public void TestReadGoodXML () {
			Definition d  = DefinitionIO.Read( Global.GetResourceReader(   "BioPatMLXML/Motif.xml" ) );
			Assert.AreEqual( "Motif", d.Name );
		}

		/// <summary>
		/// Test reading a good motif biopatml document. This Series file is the same one
		/// used in TestPattern package, TestSeries class.
		/// </summary>
		[TestMethod]
		public void TestReadGoodXML_SeriesALL () {
			Definition d  = DefinitionIO.Read( Global.GetResourceReader(   "BioPatMLXML/SeriesAll.xml" ) );
			Assert.AreEqual( 4, d.Patterns.Length );
		}

		/// <summary>
		/// Test reading biology pattern 'any' in a pure string form. This Any pattern is the same
		/// one used in TestPattern Package, TestAny
		/// </summary>
		[TestMethod]
		public void TestReadPattern_XMLDocument () {
			string Any =
                        "<?xml version='1.0'?> " +
							"<BioPatML xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' " +
									   "xsi:noNamespaceSchemaLocation='BioPatML.xsd'> " +
								"<Definition name='Any'> " +
									"<Any  name = 'any' minimum = '6' maximum = '8' increment = '1.1' impact = '0.9' /> " +
								"</Definition> " +
							"</BioPatML>";

			Definition d = DefinitionIO.Read( new System.IO.StringReader( Any ) );

			Assert.AreEqual( "Any", d.Name );
		}
	}
}
