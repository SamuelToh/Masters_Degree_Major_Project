using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using QUT.Bio.BioPatML.Patterns;
using System.Diagnostics;

namespace BioPatMLEditor {
	/// <summary> Eventually this class should be able to manage a list of patterns, loaded from 
	/// disk or downloaded from somewhere. They should be able to be selected, sorted, removed etc.
	/// </summary>
	
	public class PatternManager {
		private static PatternManager instance = new PatternManager();

		private PatternManager() {}

		/** <summary> Gets a reference to the singleton instance. </summary>*/

		public static PatternManager Instance {
			get { return instance; }
		}

		/** <summary> Get or set the "current pattern". </summary> */

		public Definition SelectedDefinition {
			// TODO: this should be generalised.

			get {
				return selectedDefinition;
			}
			set {
				Debug.Assert( value != null, "Definition may not be null." );
				selectedDefinition = value;
			}
		}

		private Definition selectedDefinition = new Definition();
	}
}
