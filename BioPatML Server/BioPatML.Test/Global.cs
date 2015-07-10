using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;

namespace BioPatML.Test {
	public class Global {
		private readonly static Global instance = new Global();

		private Global () {
#if ! SILVERLIGHT
			Application.ResourceAssembly = GetType().Assembly;
#endif
		}

		/// <summary> Gets the uri of a resource included in this assembly. </summary>
		/// <param name="fileName">The name of the resource, which is assumed to reside in a top-level folder called Resources.</param>
		/// <returns></returns>

		public static Uri GetResourceUri ( string fileName ) {
			return instance.ResourceUri( fileName );
		}

		/// <summary> Gets a new StreamReader attached a resource included in this assembly. </summary>
		/// <param name="fileName">The name of the resource, which is assumed to reside in a top-level folder called Resources.</param>
		/// <returns></returns>

		public static StreamReader GetResourceReader ( string fileName ) {
			return new StreamReader( Application.GetResourceStream(
				instance.ResourceUri( fileName )
			).Stream );
		}

		/// <summary> Constructs a uri for a resource file located in the "Resources" folder. </summary>
		/// <param name="fileName">the name of the resource file.</param>
		/// <returns></returns>

		private Uri ResourceUri ( string fileName ) {
			string assemblyName = GetType().Assembly.FullName;
			string shortName = assemblyName.Substring( 0, assemblyName.IndexOf( ',' ) );
			string uriString = String.Format( "{0};component/Resources/{1}", shortName, fileName );
			// string uriString = String.Format( "pack://application:,,,/{0};component/Resources/{1}", shortName, fileName );
			return new Uri( uriString, UriKind.Relative );
		}

		/// <summary> Loads the contents of a resource file into a string. </summary>
		/// <param name="fileName">the name of the resource to be loaded.</param>
		/// <returns></returns>

		private string GetResource ( string fileName ) {
			Uri uri = ResourceUri( fileName );

			using ( StreamReader r = new StreamReader( Application.GetResourceStream( uri ).Stream ) ) {
				return r.ReadToEnd();
			}
		}

	}
}
