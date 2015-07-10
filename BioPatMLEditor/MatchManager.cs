using System.Collections.Generic;
using System.IO;
using QUT.Bio.BioPatML.Readers;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Patterns;
using System;
using System.Collections.ObjectModel;

namespace BioPatMLEditor {
	public class MatchManager {
		private static MatchManager instance = new MatchManager();

		public static MatchManager Instance {
			get {
				return instance;
			}
		}

		private readonly ObservableCollection<Match> matches = new ObservableCollection<Match>();

		public ObservableCollection<Match> Matches {
			get {
				return matches;
			}
		}
	}
}
