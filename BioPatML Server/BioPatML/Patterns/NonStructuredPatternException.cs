using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QUT.Bio.BioPatML.Patterns {
	/// <summary> Exception used to indicate that a pattern does not support child patterns. </summary>

	public class NonStructuredPatternException : Exception {

		/// <summary> Create a new NonStructuredPatternExcception object. </summary>
		
		public NonStructuredPatternException () : base( "Non-structured pattern: child patterns not supported." ) { }
	}
}
