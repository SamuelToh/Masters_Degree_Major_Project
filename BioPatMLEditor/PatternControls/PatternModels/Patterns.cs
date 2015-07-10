using System.Collections.ObjectModel;

namespace BioPatMLEditor.PatternControls.PatternModels {

	/// <summary> A collection of BioPatML patterns to be used for data binding. </summary>
	
	public class Patterns : ObservableCollection<IPatternElement> {

		/// <summary> Adds a list of "regional" patterns to the collection </summary>
		
		public void CreateRegionalPatterns () {
			Add( new Any() );
			Add( new Gap() );
			Add( new Composition() );
		}

		/// <summary> Adds a list of "recursive patterns" to the collection. </summary>
		
		public void CreateRecursivePatterns () {
			Add( new Block() );
			Add( new RegularExp() );
			Add( new Motif() );
			Add( new Prosite() );
			Add( new PWM() );
		}

		/// <summary> Adds a list of "structured patterns" to the collection. </summary>
		
		public void CreateStructuredPatterns () {
			Add( new Set() );
			Add( new Series() );
			Add( new Iteration() );
			Add( new Repeat() );
			Add( new Logic() );
		}

		/// <summary> Adds a list of "special patterns" to the collection. </summary>
		
		public void CreateSpecialPatterns () {
			Add( new Alignment() );
			Add( new Constraint() );
			Add( new Void() );
		}
	}
}
