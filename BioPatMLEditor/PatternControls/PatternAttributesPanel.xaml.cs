using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BioPatMLEditor.PatternControls.PatternModels;

namespace BioPatMLEditor.PatternControls {
	public partial class PatternAttributesPanel : UserControl {

		public static PatternAttributesPanel Instance {
			get {
				return ( (MainPage) Application.Current.RootVisual ).patternAttributesPanel;
			}
		}

		/// <summary>
		/// The data source for our dataForm...
		/// </summary>
		internal PagedCollectionView pcv { get; set; }

		/// <summary>
		/// Default constructor nothing much here
		/// </summary>
		public PatternAttributesPanel () {
			InitializeComponent();
		}
		/// <summary>
		/// Re-initialize all components in this child UI.
		/// This method also checks whether or not the selected pattern is a complex type. 
		/// If yes, the data form will render a special tag to accomodate the complex type settings.
		/// </summary>
		/// <param name="p">The selected Pattern</param>
		public void RebuildPanel ( PatternBase p ) {
			this.PnlPatternSetter.CommandButtonsVisibility = DataFormCommandButtonsVisibility.Commit;
			this.PnlPatternSetter.CurrentItem = p;
			MainView.IsSelected = true;

			if ( p == null ) {
				SubElementView.Visibility = System.Windows.Visibility.Collapsed;
			}

			else if ( p is PWM ) {
				pcv = new PagedCollectionView( ( p as PWM ).RowElements );
				DataContext = pcv;
				SubElementView.Visibility = System.Windows.Visibility.Visible;
				SubElementView.Header = "PWM row elements";
				SubPanelDF.Header = "Add a PWM row here.";
			}

			else if ( p is BioPatMLEditor.PatternControls.PatternModels.Block ) {
				pcv = new PagedCollectionView( ( p as BioPatMLEditor.PatternControls.PatternModels.Block ).children );
				DataContext = pcv;
				SubElementView.Visibility = System.Windows.Visibility.Visible;
				SubPanelDF.Header = "Add your Block(s) here.";
				SubElementView.Header = "Block sequences";
			}

			else if ( p is Composition ) {
				pcv = new PagedCollectionView( ( p as Composition ).children );
				DataContext = pcv;
				SubElementView.Visibility = System.Windows.Visibility.Visible;
				SubPanelDF.Header = "Add your Symbol & Weight here.";
				SubElementView.Header = "Symbol Weights";
			}

			else if ( p is Repeat ) {
				pcv = new PagedCollectionView( ( p as Repeat ).RepeatPairs );
				DataContext = pcv;
				SubElementView.Visibility = System.Windows.Visibility.Visible;
				SubElementView.Header = "Pairings";
				SubPanelDF.Header = "Add your pairings here.";
			}

			else {
				//Patterns with no sub elements
				SubElementView.Visibility = System.Windows.Visibility.Collapsed;
			}
		}

		private void resizeRight_DragDelta ( object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e ) {
			LayoutRoot.Width += e.HorizontalChange;
			PnlPatternSetter.Width += e.HorizontalChange;
			//ElementGrid.Width += e.HorizontalChange;
			//SubPanelDF.Width = ElementGrid.Width;
		}

		private void resizeDown_DragDelta ( object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e ) {
			LayoutRoot.Height += e.VerticalChange;
			PnlPatternSetter.Height += e.VerticalChange;
			//ElementGrid.Height += e.VerticalChange;
			//SubPanelDF.Height += e.VerticalChange;
		}

		public void Clear () {
			RebuildPanel( null );
		}

		private void CloseBtn_Click ( object sender, System.Windows.RoutedEventArgs e ) {
			MainPage mainPage = Application.Current.RootVisual as MainPage;
			mainPage.patternAttributesPanelCheckBox.IsChecked = false;
		}

	}
}
