using System;

namespace QUT.Bio.BioPatML {
	/// <summary> Provides a basic automattically numbered naming facility for 
	/// anonymous elements that require a name;
	/// </summary>
	public static class AutoName {
		private static int counter;

		private static readonly string prefix = "auto-";

		/// <summary> Generates a new searil id string. </summary>

		private static string Next {
			get {
				return prefix + ( counter++ );
			}
		}

		/// <summary> Returns true iff the supplied string begins with the auto serial id prefix. </summary>
		/// <param name="objectName">A string to assess.</param>
		/// <returns> Returns true iff the supplied string begins with the auto serial id prefix. </returns>

		public static bool IsAnonymous ( string objectName ) {
			return objectName.StartsWith( prefix );
		}

		/// <summary> Assigns a value to the supplied string variable.
		/// If the supplied value is null or empty, an automatically generated serial Id
		/// is created.
		/// </summary>
		/// <param name="name">The variable (typically an element name in BioPatML).</param>
		/// <param name="value">The value to be assigned, if not null or empty. Use null or empty to generate a serial id.</param>

		public static void SetName ( ref string name, string value ) {
			if ( string.IsNullOrEmpty( value ) ) {
				if ( name == null ) {
					name = AutoName.Next;
				}
			}
			else {
				name = value;
			}
		}
	}
}