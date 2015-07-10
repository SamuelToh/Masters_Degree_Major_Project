using System.Windows;
using System.Windows.Controls;
using BioPatMLEditor.PatternControls.PatternModels;

namespace BioPatMLEditor
{
	public partial class PatternTemplatesPanel : UserControl
	{

		Patterns patterns = new Patterns();

		public PatternTemplatesPanel()
		{
			// Required to initialize variables
			InitializeComponent();
			BuildPatternMenus();

		}

		/// <summary>
		/// Constructs the pattern list on the right panel
		/// </summary>
		public void BuildPatternMenus () {
			Patterns p = new Patterns();
			//Regional Patterns
			p.CreateRegionalPatterns();
			listBoxRegionalPatternMenu.ItemsSource = p;

			//Recursive ones
			p = new Patterns();
			p.CreateRecursivePatterns();
			listBoxRecursivePatternMenu.ItemsSource = p;

			//Structured
			p = new Patterns();
			p.CreateStructuredPatterns();
			listBoxStructuredPatternMenu.ItemsSource = p;

			//Special patterns
			p = new Patterns();
			p.CreateSpecialPatterns();
			listBoxSpecialPatternMenu.ItemsSource = p;
		}

		private void resizeRight_DragDelta ( object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e ) {
			LayoutRoot.Width = LayoutRoot.ActualWidth + e.HorizontalChange;
		}

		private void resizeDown_DragDelta ( object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e ) {
			LayoutRoot.Height = LayoutRoot.ActualHeight + e.VerticalChange;
		}

		private void CloseBtn_Click ( object sender, System.Windows.RoutedEventArgs e ) {
			MainPage mainPage = Application.Current.RootVisual as MainPage;
			mainPage.patternTemplatesPanelCheckBox.IsChecked = false;
		}

	}
}