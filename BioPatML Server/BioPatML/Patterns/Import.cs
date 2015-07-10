/*
  <xsd:complexType  name="Import">
    <xsd:attribute name="uri" type="xsd:string"  use="required"/>
  </xsd:complexType>
 */

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using QUT.Bio.BioPatML.Common.XML;

namespace QUT.Bio.BioPatML.Patterns {
	/// <summary> Import is used to include another definition as part of the current definition. </summary>

	public class Import : Definition {

		/// <summary> Creates a new Import directive. </summary>
		/// <param name="uri"> The address of the imported definition. </param>

		public Import ( string uri )
			: base( uri ) {
			// TODO: is this class complete? should we not be trying to load the included defintiion?
		}

		/// <summary> Loads an Import object from an xml element. </summary>
		/// <param name="element">A non-null xml element presumed to contain an import definition.</param>
		/// <returns>An Import directive referencing a document at the supplied address. </returns>

		public new static Import Parse ( XElement element ) {
			if ( element.Name != "Import" ) {
				throw new ArgumentException( string.Format( "Expecting Import declaration but encountered {0}", element.Name ) );
			}

			string uri = element.String( "uri" );

			if ( uri == null ) {
				throw new ArgumentException( "Import directive requires a uri attribute." );
			}

			return new Import( uri );
		}

		/// <summary> Saves the contents of this object in an xml element.
		/// </summary>
		/// <returns>An xml element containign the content of this object.</returns>

		public override XElement ToXml () {
			return new XElement( "Import", new XAttribute( "uri", base.Name ) );
		}
	}
}