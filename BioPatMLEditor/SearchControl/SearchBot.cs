using System;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel; //For background worker 
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Sequences.List;
using System.Linq;
using QUT.Bio.BioPatML.Alphabets;

/*****************| Queensland  University Of Technology |*******************
 *  Author                   : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrence BuckingHam
 * 
 ***************************************************************************/
namespace BioPatMLEditor.SearchControl {
	public sealed class SearchBot {

		public SearchBot (
			ProgressBar progressBar,
			Action onSearchCompleted
		) {
			worker = new BackgroundWorker();
			this.SearchComplete += onSearchCompleted;
			this.progressBar = progressBar;
		}

		/// <summary> Executes search on a nackground thread. </summary>
		private BackgroundWorker worker;

		/// <summary> a delegate that will be called when the search finishes. </summary>
		private event Action SearchComplete;

		/// <summary> a delegate that will be called when the search finishes. </summary>
		private ProgressBar progressBar;

		/// <summary>
		/// Delegate used for updating the UI
		/// </summary>
		/// <param name="value"></param>
		public delegate void UpdateUCPanelProgressDelegate ( int value );

		/// <summary>
		/// A complex implementation for searching hits in our sequence(s)
		/// </summary>
		public void Search () {
			worker.WorkerReportsProgress = true;

			worker.DoWork += delegate( object s, DoWorkEventArgs eventArgs ) {
				int progressCounter = 0;
				IPattern pattern = PatternManager.Instance.SelectedDefinition.Pattern;

				if ( pattern == null ) return;

				foreach ( var sequence in SequenceManager.Instance.SelectedItems ) {
					FeatureList matches = null;

					if ( pattern is SeriesBest || pattern is SetBest ) {
						matches.Add( sequence.SearchBest( 1, 1, pattern ) );
					}
					else {
						matches = sequence.Search( 1, sequence.Length, pattern );
					}

					progressCounter++;

					progressBar.Dispatcher.BeginInvoke( () => {
						MatchManager.Instance.Matches.AddRange( matches.Select( m => m as Match ) );
						progressBar.Value = (int) (100.0 * progressCounter / SequenceManager.Instance.Sequences.Count);
					} );
				}
			};

			worker.RunWorkerCompleted += delegate( object s, RunWorkerCompletedEventArgs args ) {
				if ( SearchComplete != null ) SearchComplete();
			};

			MatchManager.Instance.Matches.Clear();
			worker.RunWorkerAsync();
		}
	}
}
