using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Sequences;
using DB = System.Diagnostics.Debug;
using QUT.Bio.BioPatML.Alphabets;


namespace BioPatML.Test {
	[TestClass]
	public class AlignmentTest {
		private Sequence seq;
		private Series series;
		private Motif motif1;
		private Motif motif2;

		[TestInitialize]
		public void SetUp () {
			seq = new Sequence( AlphabetType.DNA, "atgcatgc" );
			series = new SeriesBest( "series", 1.0 );
			motif1 = new Motif( "motif1", AlphabetType.DNA, "tgc", 1.0 );
			motif2 = new Motif( "motif2", AlphabetType.DNA, "gca", 1.0 );
		}

		[TestMethod]
		public void TestMatchStart () {
			Alignment alignment = new Alignment( "cursor", motif1, AlignmentPosition.START, +1 );
			series.Add( motif1 );
			series.Add( alignment );
			series.Add( motif2 );

			Match match = seq.SearchBest( 0, 0, series );
			DB.Assert( "tgca" == match.Letters() );
		}

		[TestMethod]
		public void TestMatchEnd () {
			Alignment alignment = new Alignment( "cursor", motif1, AlignmentPosition.END, -2 );
			series.Add( motif1 );
			series.Add( alignment );
			series.Add( motif2 );

			Match match = seq.SearchBest( 0, 0, series );
			DB.Assert( "tgca" == match.Letters() );
		}

		/** test for match at the center */
		public void TestMatchCenter () {
			Alignment alignment = new Alignment( "cursor", motif1, AlignmentPosition.CENTER, 0 );
			series.Add( motif1 );
			series.Add( alignment );
			series.Add( motif2 );

			Match match = seq.SearchBest( 0, 0, series );
			DB.Assert( "tgca" == match.Letters() );
		}

		[TestMethod] 

		public void TestRead () {
			string document = @"<BioPatML
    xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
    xsi:noNamespaceSchemaLocation='BioPatML.xsd'>
	<Definition name='Alignment'>
		<Series>
			<Motif name='motif1' alphabet='DNA' motif='tgc' />
			<Alignment name='alignment' pattern='motif1' offset='-2' impact='0.9' position='END' />
			<Motif name='motif2' alphabet='DNA' motif='gca' />
		</Series>
	</Definition>
</BioPatML>";

			Definition definition = Reader.Parse( 
				new StringReader( document ) 
			);
			
			DB.Assert( "Alignment" == definition.Name );
			Alignment alignment = (Alignment) definition.Patterns[1];

			DB.Assert( "alignment" == alignment.Name );
			DB.Assert( "motif1" == alignment.Pattern.Name );
			DB.Assert( AlignmentPosition.END == alignment.Position );
			DB.Assert( -2 == alignment.Offset );
			DB.Assert( 0.9 == alignment.Impact );
		}

	}
}
