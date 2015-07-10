using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = System.Diagnostics.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Symbols;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Sequences.Annotations;
using QUT.Bio.BioPatML.Patterns;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace TestBioPatML.TestSequence {
	[TestClass]
	public class TestSequence {
		[TestMethod]
		public void TestConstructor () {
			Sequence seq = new Sequence( AlphabetType.AA, "ArTGü" );
			Assert.AreEqual( 5, seq.Length );
			Assert.AreEqual( "AA", seq.Alphabet.Name );
			Assert.AreEqual( "ARTGX", seq.Letters() );

			seq = new Sequence( AlphabetType.RNA, "AcUGz" );
			Assert.AreEqual( "acugn", seq.Letters() );

			seq = new Sequence( AlphabetType.RNA, seq );
			Assert.AreEqual( "acugn", seq.Letters() );

			seq = new Sequence( AlphabetType.UNKNOWN, "actgactg" );
			Assert.AreEqual( "actgactg", seq.Letters() );
			Assert.AreEqual( AlphabetFactory.Instance( AlphabetType.DNA ), seq.Alphabet );

			seq = new Sequence( AlphabetType.UNKNOWN, "AMCMKQQRTAYWY" );
			Assert.AreEqual( "AMCMKQQRTAYWY", seq.Letters() );
			Assert.AreEqual( AlphabetFactory.Instance( AlphabetType.PROTEIN ), seq.Alphabet );

			Alphabet alpha = DnaAlphabet.Instance();
			Symbol[] symbols = { alpha['A'], alpha['C'], alpha['T'] };
			seq = new Sequence( alpha, symbols, false );
			symbols[0] = alpha['G']; //shouldnt be able to change the symbol
			Assert.AreEqual( "act", seq.Letters() );
		}

		[TestMethod]
		public void TestHasAnnotations () {
			Sequence seq = new Sequence( AlphabetType.DNA, "ACTG" );
			Assert.AreEqual( false, seq.HasAnnotations() );

			seq.Annotations.Add( new Annotation( "name", "value" ) );
			Assert.AreEqual( true, seq.HasAnnotations() );
		}

		[TestMethod]
		public void TestAddGetAnnotations () {
			Sequence seq = new Sequence( AlphabetType.DNA, "ACTG" );
			AnnotationList a1 = seq.Annotations;
			Assert.AreEqual( 0, a1.Count );

			AnnotationList a2 = new AnnotationList();
			a2.Add( new Annotation( "name1", "value1" ) );
			a2.Add( new Annotation( "name2", "value2" ) );
			seq.Annotations.AddRange( a2 );
			Assert.AreEqual( "name1", seq.Annotations[0].Name );
			Assert.AreEqual( "value2", seq.Annotations[1].Value );
			Assert.AreEqual( "name1", seq.Annotations[ "name1" ].Name );
			Assert.AreEqual( "value2", seq.Annotations.Value( "name2" ) );
			Assert.AreEqual( null, seq.Annotations.Value( "name3" ) );
			Assert.AreEqual( null, seq.Annotations[ "name3" ] );
		}

		[TestMethod]
		public void TestHasFeatures () {
			Sequence seq = new Sequence( AlphabetType.DNA, "ACTG" );
			Assert.AreEqual( false, seq.HasFeatures() );
			seq.AddFeatures( new FeatureList( "test1" ) );
			Assert.AreEqual( true, seq.HasFeatures() );
		}

		[TestMethod]
		public void TestAddGetFeature () {
			Sequence seq = new Sequence( AlphabetType.DNA, "ACTG" );
			FeatureList flist1 = new FeatureList( "test1" );
			FeatureList flist2 = new FeatureList( "test2" );

			Assert.AreEqual( 0, seq.FeatureLists.Count );
			seq.AddFeatures( flist1 );
			seq.AddFeatures( flist2 );
			Assert.AreEqual( 2, seq.FeatureLists.Count );
			Assert.AreEqual( flist1, seq.FeatureLists[ 0 ] );
			Assert.AreEqual( flist1, seq.FeatureLists[ "test1" ] );

			Assert.AreEqual( flist2, seq.FeatureLists[ 1 ] );
			Assert.AreEqual( flist2, seq.FeatureLists[ "test2" ] );
		}

		[TestMethod]
		public void TestAddFeatureLists () {
			Sequence seq = new Sequence( AlphabetType.DNA, "ACTG" );
			FeatureList flist1 = new FeatureList( "test1" );
			FeatureList flist2 = new FeatureList( "test2" );
			AnnotatedList<FeatureList> flists = new AnnotatedList<FeatureList>();
			flists.Add( flist1 );
			flists.Add( flist2 );

			seq.AddFeatures( flists );
			Assert.AreEqual( 2, seq.FeatureLists.Count );
		}

		[TestMethod]
		public void TestSetGetName () {
			Sequence seq = new Sequence( AlphabetType.DNA, "actg" );
			Assert.AreEqual( null, seq.Name );

			seq.Name = "test";
			Assert.AreEqual( "test", seq.Name );
		}

		[TestMethod]
		public void TestIsCircular () {
			Assert.AreEqual( false, ( new Sequence( AlphabetType.DNA, "act" ) ).IsCircular() );
			Assert.AreEqual( true, ( new Sequence( AlphabetType.DNA, "act", true ) ).IsCircular() );
			Assert.AreEqual( false, ( new Sequence( AlphabetType.DNA, "act", false ) ).IsCircular() );
		}

		[TestMethod]
		public void TestPosition () {
			Sequence seq = new Sequence( AlphabetType.DNA, "actg" );
			Assert.AreEqual( 1, seq.Position() );
		}

		[TestMethod]
		public void TestConvertPosition () {
			Sequence seq = new Sequence( AlphabetType.DNA, "actgat" );

			Sequence sub1 = new Sequence( seq, 2, 4, +1 );
			Assert.AreEqual( 2, seq.Position( sub1, 1 ) );
			Assert.AreEqual( 1, sub1.Position( seq, 2 ) );

			Sequence sub2 = new Sequence( sub1, 1, 3, +1 );
			Assert.AreEqual( 2, seq.Position( sub2, 1 ) );
			Assert.AreEqual( 1, sub1.Position( seq, 2 ) );
			Assert.AreEqual( 1, sub1.Position( sub1, 1 ) );
			Assert.AreEqual( 1, sub1.Position( sub2, 1 ) );


			sub2 = new Sequence( sub1, 1, 3, -1 );
			Assert.AreEqual( 4, seq.Position( sub2, 1 ) );
			Assert.AreEqual( 1, sub2.Position( seq, 4 ) );
			Assert.AreEqual( 3, sub2.Position( sub1, 1 ) );
			Assert.AreEqual( 1, sub1.Position( sub2, 3 ) );

			sub1 = new Sequence( seq, 2, 4, -1 );
			Assert.AreEqual( 4, seq.Position( sub1, 1 ) );
			Assert.AreEqual( 1, sub1.Position( seq, 4 ) );

			sub2 = new Sequence( sub1, 1, 3, +1 );
			Assert.AreEqual( 2, seq.Position( sub2, 1 ) );
			Assert.AreEqual( 1, sub2.Position( seq, 2 ) );
			Assert.AreEqual( 3, sub2.Position( sub1, 1 ) );
			Assert.AreEqual( 1, sub1.Position( sub2, 3 ) );

			sub2 = new Sequence( sub1, 1, 3, -1 );
			Assert.AreEqual( 4, seq.Position( sub2, 1 ) );
			Assert.AreEqual( 1, sub2.Position( seq, 4 ) );
			Assert.AreEqual( 1, sub2.Position( sub1, 1 ) );
			Assert.AreEqual( 1, sub1.Position( sub2, 1 ) );
		}

		[TestMethod]
		public void TestGet () {
			Sequence seq = new Sequence( AlphabetType.AA, "artg", true );

			Assert.AreEqual( 'T', seq.GetSymbol( -5 ).Letter );
			Assert.AreEqual( 'T', seq.GetSymbol( -1 ).Letter );
			Assert.AreEqual( 'G', seq.GetSymbol( 0 ).Letter );


			Assert.AreEqual( 'A', seq.GetSymbol( 1 ).Letter );
			Assert.AreEqual( 'R', seq.GetSymbol( 2 ).Letter );
			Assert.AreEqual( 'T', seq.GetSymbol( 3 ).Letter );


			Assert.AreEqual( 'G', seq.GetSymbol( 4 ).Letter );
			Assert.AreEqual( 'A', seq.GetSymbol( 5 ).Letter );
			Assert.AreEqual( 'A', seq.GetSymbol( 9 ).Letter );
		}

		[TestMethod]
		public void TestExtract () {
			Sequence seq = new Sequence( AlphabetType.AA, "artg", true );

			Assert.AreEqual( "ARTG", seq.Extract( 1, 4 ).Letters() );
			Assert.AreEqual( "RT", seq.Extract( 2, 3 ).Letters() );
			Assert.AreEqual( "GARTGA", seq.Extract( 0, 5 ).Letters() );
		}


		[TestMethod]
		public void TestExtract2 () {
			Sequence seq = new Sequence( AlphabetType.AA, "artg", true );

			Assert.AreEqual( "AR", seq.Extract( 0, 2, +1 ).Letters() );
			Assert.AreEqual( "RT", seq.Extract( 1, 2, +1 ).Letters() );
			Assert.AreEqual( "GA", seq.Extract( -1, 2, +1 ).Letters() );

			Assert.AreEqual( "TG", seq.Extract( 0, 2, -1 ).Letters() );
			Assert.AreEqual( "RT", seq.Extract( 1, 2, -1 ).Letters() );
			Assert.AreEqual( "GA", seq.Extract( -1, 2, -1 ).Letters() );

			Assert.AreEqual( "RT", seq.Extract( 0, 2, 0 ).Letters() );
			Assert.AreEqual( "AR", seq.Extract( -1, 2, 0 ).Letters() );
			Assert.AreEqual( "TG", seq.Extract( +1, 2, 0 ).Letters() );

			Assert.AreEqual( "ART", seq.Extract( 0, 3, 0 ).Letters() );
			Assert.AreEqual( "R", seq.Extract( 0, 1, 0 ).Letters() );

			seq = new Sequence( AlphabetType.AA, "artga", true );

			Assert.AreEqual( "T", seq.Extract( 0, 1, 0 ).Letters() );
			Assert.AreEqual( "RTG", seq.Extract( 0, 3, 0 ).Letters() );
			Assert.AreEqual( "ARTG", seq.Extract( 0, 4, 0 ).Letters() );
		}

		[TestMethod]
		public void TestEquals () {
			Sequence seq = new Sequence( AlphabetType.DNA, "aCtg" );
			Assert.AreEqual( false, seq.Equals( null ) );
			Assert.AreEqual( false, seq.Equals( new Sequence( AlphabetType.DNA, "act" ) ) );

			Assert.AreEqual( false, seq.Equals( new Sequence( AlphabetType.DNA, "actga" ) ) );
			Assert.AreEqual( true, seq.Equals( new Sequence( AlphabetType.DNA, "actg" ) ) );
			Assert.AreEqual( true, seq.Equals( new Sequence( AlphabetType.DNA, "anxg" ) ) );
			Assert.AreEqual( true, seq.Equals( "aCtG" ) );

			Assert.AreEqual( true, seq.Equals( "ncTX" ) );
			Assert.AreEqual( false, seq.Equals( "act" ) );
			Assert.AreEqual( false, seq.Equals( "actgc" ) );
			Assert.AreEqual( false, seq.Equals( "actw" ) );
		}

		[TestMethod]
		public void TestMismatches () {
			Sequence seq = new Sequence( AlphabetType.DNA, "actg" );
			Assert.AreEqual( 0, seq.Mismatches( 2, new Sequence( AlphabetType.DNA, "ctg" ) ) );
			Assert.AreEqual( 1, seq.Mismatches( 2, new Sequence( AlphabetType.DNA, "cta" ) ) );
			Assert.AreEqual( 3, seq.Mismatches( 1, new Sequence( AlphabetType.DNA, "ctg" ) ) );
		}

		[TestMethod]
		public void TestSearch () {
			Sequence seq   = new Sequence( AlphabetType.DNA, "acttacttagttaac" );
			Motif  motif = new Motif( "motif1", AlphabetType.DNA, "act", 0.5 );

			FeatureList list = seq.Search( 0, 0, motif );
			Assert.AreEqual( 3, list.Count() );
			Assert.AreEqual( 1, list[ 0 ].Start );
			Assert.AreEqual( 1.0, ( (Match) list[ 0 ] ).Similarity, 1e-1 );
			Assert.AreEqual( 5, list[ 1 ].Start );
			Assert.AreEqual( 1.0, ( (Match) list[ 1 ] ).Similarity, 1e-1 );
			Assert.AreEqual( 9, list[ 2 ].Start );
			Assert.AreEqual( 0.6, ( (Match) list[ 2 ] ).Similarity, 1e-1 );

			list = seq.Search( 11, seq.Length + 1, motif );
			Assert.AreEqual( 1, list.Count() );
			Assert.AreEqual( 14, list[ 0 ].Start );
			Assert.AreEqual( 0.6, ( (Match) list[ 0 ] ).Similarity, 1e-1 );


		}

		[TestMethod]
		/** Test for best match searching within a sequence */
		public void TestSearchBest () {
			Sequence seq = new Sequence( AlphabetType.DNA, "acttacttagttaatt" );
			Motif motif = new Motif( "motif1", AlphabetType.DNA, "taga", 0.0 );
			Match match = seq.SearchBest( 1, -1, motif );
			Assert.AreEqual( "tagt", match.Letters() );
			Assert.AreEqual( 8, match.Start );
			Assert.AreEqual( 4, match.Length );
			Assert.AreEqual( 0.75, match.Similarity, 1e-3 );
			motif = new Motif( "motif2", AlphabetType.DNA, "aatt", 0.0 );
			match = seq.SearchBest( 1, -1, motif );
			Assert.AreEqual( 13, match.Start );
			match = seq.SearchBest( 1, seq.Length, motif );
			Assert.AreEqual( 13, match.Start );
			match = seq.SearchBest( 1, seq.Length - 1, motif );
			Assert.AreEqual( 1, match.Start );
		}

		//[TestMethod]
		/** Test the creation of a reverse sequence */
		public void TestReverse () {
			/*
			Sequence seq = new Sequence(AlphabetType.DNA, "actg", true);
			Assert.AreEqual("gtca", seq.Re().Letters());
			Assert.AreEqual("actg", seq.reverse().reverse().Letters());
			Assert.AreEqual("cagt", seq.reverse().complement().Letters());
			Sequence sub = new Sequence(seq, 2, 5, +1);
			Assert.AreEqual("agtc", sub.reverse().Letters());
			sub = new Sequence(seq, 1, 4, -1);
			Assert.AreEqual("tgac", sub.reverse().Letters());
			Assert.AreEqual("cagt", sub.reverse().reverse().Letters());
			sub = new Sequence(seq, 2, 5, -1);
			Assert.AreEqual("gact", sub.reverse().Letters());
			Assert.AreEqual("tcag", sub.reverse().reverse().Letters());
			Sequence sub2 = new Sequence(sub, 1, 4, -1);
			Assert.AreEqual("gact", sub2.reverse().Letters());
			Assert.AreEqual("tcag", sub2.reverse().reverse().Letters());*/
		}

		/** Test of the string representation */
		public void TestLetters () {
			Sequence seq = new Sequence( AlphabetType.AA, "ARTGARTG" );
			Assert.AreEqual( "ARTGARTG", seq.Letters() );
		}

		/** Test of the string representation for sections */
		public void TestLetters2 () {
			Sequence seq = new Sequence( AlphabetType.AA, "ARTG", true );
			Assert.AreEqual( "ARTG", seq.Letters( 1, 4 ) );
			Assert.AreEqual( "ARTGA", seq.Letters( 1, 5 ) );
			Assert.AreEqual( "RT", seq.Letters( 2, 3 ) );
			Assert.AreEqual( "GAR", seq.Letters( 4, 6 ) );
		}


	}
}
