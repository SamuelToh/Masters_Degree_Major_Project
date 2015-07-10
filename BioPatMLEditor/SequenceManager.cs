using System.Collections.Generic;
using System.IO;
using System.Linq;
using QUT.Bio.BioPatML.Readers;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;
using System.Collections.ObjectModel;
using System.Windows;
using QUT.Bio.Util;
using System.Windows.Threading;

namespace BioPatMLEditor {
	public class SequenceManager {

		public class Wrapper : Selectable {
			private Sequence sequence;
			private FileInfo fileInfo;

			public Wrapper ( Sequence sequence, FileInfo fileInfo ) {
				this.sequence = sequence;
				this.fileInfo = fileInfo;
			}

			public override string Name {
				get { return sequence.Name; }
			}

			public FileInfo FileInfo {
				get {
					return fileInfo;
				}
			}

			public Sequence Sequence {
				get {
					return sequence;
				}
			}
		}

		private static SequenceManager instance = new SequenceManager();

		public static SequenceManager Instance {
			get {
				return instance;
			}
		}

		private readonly ObservableCollection<Wrapper> sequences = new ObservableCollection<Wrapper>();

		public ObservableCollection<Wrapper> Sequences {
			get {
				return sequences;
			}
		}

		public Dispatcher Dispatcher { get; set; }

		private enum FileType {
			Unsupported, GenBank, Fasta
		}

		public void Load ( 
			IEnumerable<FileInfo> files 
		) {
			//TODO SJ check file start format
			foreach ( FileInfo file in files ) {
				FileType fileType = FileType.GenBank;

				using ( Stream stream = file.OpenRead() ) {
					using ( StreamReader reader = new StreamReader( stream ) ) {
						while ( !reader.EndOfStream ) {
							string line = reader.ReadLine();

							if ( line.StartsWith( ">" ) ) {
								fileType = FileType.Fasta;
								break;
							}

							if ( line.StartsWith( "LOCUS" ) ) {
								fileType = FileType.GenBank;
							}
						}
					}
				}

				switch ( fileType ) {
					case FileType.Fasta:
						LoadFasta( file );
						break;
					case FileType.GenBank:
						LoadGenbank( file );
						break;
					default:
						Alert.Show( "Unsupported file type: {0}", file.Name );
						break;
				}
			}
		}

		/// <summary> Load a sequence of sequences from a genbank file.</summary>
		/// <param name="connection"> BioPatML WCF Connection </param>
		/// <param name="file">The desire genbank file</param>
		/// <returns></returns>

		public void LoadGenbank (
			FileInfo file
		) {
			foreach ( var wrapper in sequences ) {
				if ( file.ToString().Equals( wrapper.FileInfo.ToString() ) ) return;
			}

			using ( Stream stream = file.OpenRead() ) {
				string content = string.Empty;

				using ( StreamReader streamReader = new StreamReader( stream ) ) {
					BioPatMBF_Reader reader = new BioPatMBF_Reader();
					ReadSequences( file, streamReader, reader );
				}
			}
		}

		private void ReadSequences ( 
			FileInfo file, 
			StreamReader streamReader, 
			ReaderBase reader 
		) {
			foreach ( var seq in reader.Read( streamReader ) ) {
				if ( Dispatcher == null ){
					sequences.Add( new Wrapper( seq, file ) { Selected = true } );
				}
				else {
					Dispatcher.BeginInvoke( () => {
						sequences.Add( new Wrapper( seq, file ) { Selected = true } );
					} );
				}
			}
		}

		public void LoadFasta (
			FileInfo file
		) {
			foreach ( var wrapper in sequences ) {
				if ( file.ToString().Equals( wrapper.FileInfo.ToString() ) ) return;
			}

			using ( Stream stream = file.OpenRead() ) {
				string content = string.Empty;

				using ( StreamReader streamReader = new StreamReader( stream ) ) {
					BioFastaReader reader = new BioFastaReader();
					ReadSequences( file, streamReader, reader );
				}
			}
		}

		public IEnumerable<Sequence> SelectedItems {
			get {
				return from w in sequences where w.Selected select w.Sequence;
			}
		}

		public void RemoveSelectedItems () {
			for ( int i = sequences.Count - 1; i >= 0; i-- ) {
				if ( sequences[i].Selected ) {
					sequences.RemoveAt( i );
				}
			}
		}
	}
}
