using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DB = System.Diagnostics.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Symbols;
using QUT.Bio.BioPatML.Common.XML;
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
	public class TestPWM {
		private Alphabet alpha;

		[TestInitialize]
		public void SetUp () {
			alpha = AlphabetFactory.Instance( AlphabetType.DNA );
		}

		/** Tests the simple constructor */
		[TestMethod]
		public void TestConstructor () {
			PWM pwm = new PWM( "test", alpha, 0.0 );
			double[] w1 = { 1.0, 2.0, 3.0 };
			double[] w2 = { 2.0, 4.0, 6.0 };
			pwm.Add( 'a', w1 );
			pwm.Add( 'c', w2 );
			Assert.AreEqual( 3, pwm.WeightedVectorLength );
			Assert.AreEqual( 2, pwm.SymbolNumber );
		}

		[TestMethod]
		/** Tests the calc. of a PWM on base of a motif */
		public void TestConstructorMotif () {
			PWM pwm = new PWM( "PWM1", alpha, "A[TG]", 0.0 );
			Assert.AreEqual( 100.0, pwm.Get( 'a', 0 ), 1e-3 );
			Assert.AreEqual( 1.0, pwm.Get( 'c', 0 ), 1e-3 );
			Assert.AreEqual( 1.0, pwm.Get( 't', 0 ), 1e-3 );
			Assert.AreEqual( 1.0, pwm.Get( 'g', 0 ), 1e-3 );
			Assert.AreEqual( 1.0, pwm.Get( 'a', 1 ), 1e-3 );
			Assert.AreEqual( 1.0, pwm.Get( 'c', 1 ), 1e-3 );
			Assert.AreEqual( 100.0, pwm.Get( 't', 1 ), 1e-3 );
			Assert.AreEqual( 100.0, pwm.Get( 'g', 1 ), 1e-3 );
		}

		[TestMethod]
		/** Tests the adding and getting of weights */
		public void TestAddGet () {
			PWM pwm = new PWM( "test", alpha, 0.0 );
			double[] w1 = { 1.0, 2.0, 3.0 };
			double[] w2 = { 2.0, 4.0, 6.0 };
			pwm.Add( 'a', w1 );
			pwm.Add( alpha['c'], w2 );
			Assert.AreEqual( w1, pwm.Get( alpha['a'] ) );
			Assert.AreEqual( null, pwm.Get( alpha['z'] ) );
			Assert.AreEqual( w2, pwm.Get( 'c' ) );
			Assert.AreEqual( 3, pwm.WeightedVectorLength );
			Assert.AreEqual( 2, pwm.SymbolNumber );
			Assert.AreEqual( 1.0, pwm.Get( 'a', 0 ), 1e-3 );
			Assert.AreEqual( 2.0, pwm.Get( 'a', 1 ), 1e-3 );
			Assert.AreEqual( 3.0, pwm.Get( 'a', 2 ), 1e-3 );
			Assert.AreEqual( 2.0, pwm.Get( 'c', 0 ), 1e-3 );
			Assert.AreEqual( 4.0, pwm.Get( 'c', 1 ), 1e-3 );
			Assert.AreEqual( 6.0, pwm.Get( 'c', 2 ), 1e-3 );

			Assert.AreEqual( 1.0, pwm.Get( 't', 0 ), 1e-3 );
			Assert.AreEqual( 2.0, pwm.Get( 't', 1 ), 1e-3 );
			Assert.AreEqual( 3.0, pwm.Get( 't', 2 ), 1e-3 );
		}

		[TestMethod]
		/** Tests the adding of weights as strings */
		public void TestAddString () {
			PWM pwm = new PWM( "test", alpha, 0.0 );

			pwm.Add( 'a', "1 2 3" );
			pwm.Add( alpha['c'], "2 4 6" );
			Assert.AreEqual( 3, pwm.WeightedVectorLength );
			Assert.AreEqual( 2, pwm.SymbolNumber );
			Assert.AreEqual( 1.0, pwm.Get( 'a', 0 ), 1e-3 );
			Assert.AreEqual( 2.0, pwm.Get( 'a', 1 ), 1e-3 );
			Assert.AreEqual( 3.0, pwm.Get( 'a', 2 ), 1e-3 );
			Assert.AreEqual( 2.0, pwm.Get( 'c', 0 ), 1e-3 );
			Assert.AreEqual( 4.0, pwm.Get( 'c', 1 ), 1e-3 );
			Assert.AreEqual( 6.0, pwm.Get( 'c', 2 ), 1e-3 );
		}

		[TestMethod]
		/** Tests the setting and getting of weights */
		public void TestSetGet () {
			PWM pwm = new PWM( "test", alpha, 0.0 );
			pwm.Add( alpha['a'], "0 0" );
			pwm.Set( alpha['a'], 0, 1 );
			pwm.Set( 'a', 1, 2 );
			Assert.AreEqual( 1.0, pwm.Get( 'a', 0 ), 1e-3 );
			Assert.AreEqual( 2.0, pwm.Get( 'a', 1 ), 1e-3 );

			Assert.AreEqual( 1.0, pwm.Get( 't', 0 ), 1e-3 );
			Assert.AreEqual( 2.0, pwm.Get( 't', 1 ), 1e-3 );
		}

		[TestMethod]
		/** Tests the matching  */
		public void TestMatch () {
			PWM pwm = new PWM( "test", alpha, 0.5 );
			pwm.Add( 'a', "1 0 1 0" );
			pwm.Add( 'c', "0 1 0 1" );
			Sequence seq = new Sequence( AlphabetType.DNA, "aaaacac" );
			FeatureList matches = seq.Search( 0, 0, pwm );
			Assert.AreEqual( 3, matches.Count );
			Assert.AreEqual( "aaaa", matches[ 0 ].Letters() );
			Assert.AreEqual( 0.5, ( (Match) matches[ 0 ] ).Similarity, 1e-3 );
			Assert.AreEqual( "aaac", matches[ 1 ].Letters() );
			Assert.AreEqual( 0.75, ( (Match) matches[ 1 ] ).Similarity, 1e-3 );
			Assert.AreEqual( "acac", matches[ 2 ].Letters() );
			Assert.AreEqual( 1.0, ( (Match) matches[ 2 ] ).Similarity, 1e-3 );
		}


		[TestMethod]
		/** Tests the creation of a sorting index for the rows of a PWM */
		public void TestSortingIndex () {
			PWM pwm = new PWM( "test", alpha, 0.0 );
			pwm.Add( 'a', "1 3 2" );
			pwm.Add( 'g', "2 2 1" );
			pwm.Add( 'c', "3 1 3" );
			Symbol[] index = pwm.SortingIndex( 0 );
			Assert.AreEqual( 'a', index[0].Letter );
			Assert.AreEqual( 'g', index[1].Letter );
			Assert.AreEqual( 'c', index[2].Letter );
			index = pwm.SortingIndex( 1 );
			Assert.AreEqual( 'c', index[0].Letter );
			Assert.AreEqual( 'g', index[1].Letter );
			Assert.AreEqual( 'a', index[2].Letter );
			index = pwm.SortingIndex( 2 );
			Assert.AreEqual( 'g', index[0].Letter );
			Assert.AreEqual( 'a', index[1].Letter );
			Assert.AreEqual( 'c', index[2].Letter );
		}

		[TestMethod]
		/** Tests the updating of the consensus and the anti-consensus. */
		public void TestUpdateConsensus () {
			PWM pwm = new PWM( "test", alpha, 0.0 );
			pwm.Add( 'a', "1 0 1" );
			Assert.AreEqual( "aaa", pwm.Consensus.Letters() );
			Assert.AreEqual( "aaa", pwm.AntiConsensus.Letters() );
			pwm.Add( 'c', "0 1 0" );
			Assert.AreEqual( "aca", pwm.Consensus.Letters() );
			Assert.AreEqual( "cac", pwm.AntiConsensus.Letters() );
			pwm.Add( 'g', "-1 1 2" );
			Assert.AreEqual( "acg", pwm.Consensus.Letters() );
			Assert.AreEqual( "gac", pwm.AntiConsensus.Letters() );
		}

		[TestMethod]
		/** Tests the calculation of the max. score of the PWM */
		public void TestUpdateMinMax () {
			PWM pwm = new PWM( "test", alpha, 0.0 );
			pwm.Add( 'a', "2 4 3" );
			pwm.Add( 'g', "1 1 1" );
			pwm.Add( 'c', "1 3 6" );
			Assert.AreEqual( 12.0, pwm.MaxScore, 1e-3 );
			Assert.AreEqual( 3.0, pwm.MinScore, 1e-3 );
			Assert.AreEqual( 9.0, pwm.RangeScore, 1e-3 );
		}

		[TestMethod]
		/** Tests the creation of the consensus and the anti-consensus. */
		public void TestConsensus () {
			PWM pwm = new PWM( "test", alpha, 0.0 );
			pwm.Add( 'a', "1 0 1" );
			pwm.Add( 'c', "0 1 0" );
			Assert.AreEqual( "aca", pwm.Consensus.Letters() );
			Assert.AreEqual( "cac", pwm.AntiConsensus.Letters() );
		}

		[TestMethod]
		/** test the creation of a sup PWM */
		public void TestSubPWM () {
			PWM pwm = new PWM( "test", alpha, 0.0 );
			pwm.Add( 'a', "1 2 3 4" );
			pwm.Add( 'c', "5 6 7 8" );
			PWM sub = pwm.SubPWM( "test", 1, 2 );
			Assert.AreEqual( 2, sub.Get( 'a', 0 ), 1e-3 );
			Assert.AreEqual( 3, sub.Get( 'a', 1 ), 1e-3 );
			Assert.AreEqual( 6, sub.Get( 'c', 0 ), 1e-3 );
			Assert.AreEqual( 7, sub.Get( 'c', 1 ), 1e-3 );
		}

		[TestMethod]
		/** Tests the reading of a PWM from an XML document*/
		public void TestRead () {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader(   "BioPatMLXML/PWM.xml" ) );
			PWM pattern = (PWM) definition.Pattern;

			Assert.AreEqual( "pwm", definition.Name );
			Assert.AreEqual( "PWM", pattern.Name );
			Assert.AreEqual( 0.7, pattern.Threshold );
			Assert.AreEqual( 0.9, pattern.Impact );
			Assert.AreEqual( 3, pattern.WeightedVectorLength );
			Assert.AreEqual( 2, pattern.SymbolNumber );
			Assert.AreEqual( 1.00, pattern.Get( 'a', 0 ) );
			Assert.AreEqual( 2.00, pattern.Get( 'a', 1 ) );
			Assert.AreEqual( 3.00, pattern.Get( 'a', 2 ) );
			Assert.AreEqual( 4.00, pattern.Get( 'c', 0 ) );
			Assert.AreEqual( 5.00, pattern.Get( 'c', 1 ) );
			Assert.AreEqual( 6.00, pattern.Get( 'c', 2 ) );

		}

		[TestMethod]
		/** Tests the reading of a PWM from an XML document*/
		public void TestToXml () {
			Definition definition = DefinitionIO.Read( Global.GetResourceReader( "BioPatMLXML/PWM.xml" ) );

			Assert.IsTrue( definition.ToXml().ToString().IndexOf( "name=\"auto-" ) < 0 );
			Definition def2 = DefinitionIO.Read( DefinitionIO.Write( definition ) );

			PWM pattern = (PWM) def2.Pattern;

			Assert.AreEqual( "pwm", def2.Name );
			Assert.AreEqual( "PWM", pattern.Name );
			Assert.AreEqual( 0.7, pattern.Threshold );
			Assert.AreEqual( 0.9, pattern.Impact );
			Assert.AreEqual( 3, pattern.WeightedVectorLength );
			Assert.AreEqual( 2, pattern.SymbolNumber );
			Assert.AreEqual( 1.00, pattern.Get( 'a', 0 ) );
			Assert.AreEqual( 2.00, pattern.Get( 'a', 1 ) );
			Assert.AreEqual( 3.00, pattern.Get( 'a', 2 ) );
			Assert.AreEqual( 4.00, pattern.Get( 'c', 0 ) );
			Assert.AreEqual( 5.00, pattern.Get( 'c', 1 ) );
			Assert.AreEqual( 6.00, pattern.Get( 'c', 2 ) );

		}
	}
}
