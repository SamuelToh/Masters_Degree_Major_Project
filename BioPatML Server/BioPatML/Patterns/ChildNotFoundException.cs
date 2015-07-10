using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QUT.Bio.BioPatML.Patterns {

	/// <summary> Exception used to represent "child pattern not found" </summary>

	public class ChildNotFoundException : Exception {

		/// <summary> Initialise a new ChildNotFoundException. </summary>
		/// <param name="name"></param>

		public ChildNotFoundException ( string name )
			: base( string.Format( "Child pattern with name '{0}' not found.", name ) ) { }
	}
}
