using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;
using QUT.Bio.Util;

namespace BioPatMLEditor.MBFParserControl {
	public partial class ParsedSequencesPanel : UserControl {
		public ParsedSequencesPanel () {
			// Required to initialize variables
			InitializeComponent();
			filesList.SetCollection( SequenceManager.Instance.Sequences );
			dragNDroptxt.Text = "Drag and drop your\nfasta or genbank file/s here.";
		}

		private void resizeRight_DragDelta ( object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e ) {
			LayoutRoot.Width = LayoutRoot.ActualWidth + e.HorizontalChange;
		}

		private void resizeDown_DragDelta ( object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e ) {
			LayoutRoot.Height = LayoutRoot.ActualHeight + e.VerticalChange;
		}

		private void filesList_KeyDown ( object sender, System.Windows.Input.KeyEventArgs e ) {
			if ( e.Key == Key.Delete
				&& filesList.GetCollection<SequenceManager.Wrapper>().Count > 0
			) {
				SequenceManager.Instance.RemoveSelectedItems();
			}
		}

		private void CloseBtn_Click ( object sender, System.Windows.RoutedEventArgs e ) {
			MainPage mainPage = Application.Current.RootVisual as MainPage;
			mainPage.sequencesPanelCheckBox.IsChecked = false;
		}

		private void DropFiles ( object sender, DragEventArgs e ) {
			FileInfo[] files = this.GetDropFiles( e );
			BackgroundWorker worker = new BackgroundWorker();

			this.fileParseStatus.Text = "Loading files...";
			this.fileLoadingPanel.Visibility = Visibility.Visible;
			this.fileCanvas.Visibility = Visibility.Visible;

			worker.DoWork += new DoWorkEventHandler( ( s2, e2 ) => {
				SequenceManager.Instance.Load( files );
			} );

			worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler( ( s3, e3 ) => {
				if ( e3.Error != null ){
					Alert.Show( "Error encountered while parsing files: {0}", e3.Error ); 
				}

				Dispatcher.BeginInvoke( () => {
					if ( SequenceManager.Instance.Sequences.Count > 0 ) { 
						titleBar.Text = "Parsed sequences";
						this.fileLoadingPanel.Visibility = Visibility.Collapsed;
					}
					this.fileCanvas.Visibility = Visibility.Collapsed;
				} );
			} );

			worker.RunWorkerAsync();
		}

		/// <summary>
		/// This function converts a list of dragged data objects into a collection
		/// of files.
		/// </summary>
		/// <param name="e">The drag event that contains a list of dragged items.</param>
		/// <returns>An array of dropped files.</returns>
		private FileInfo[] GetDropFiles ( DragEventArgs e ) {
			if ( e.Data == null ) { return null; }
			IDataObject dataObject = e.Data as IDataObject;
			FileInfo[] files = dataObject.GetData( DataFormats.FileDrop ) as FileInfo[];
			return files;
		}

	}
}