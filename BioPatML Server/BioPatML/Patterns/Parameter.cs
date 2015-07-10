/*
  <!-- Parameters =========================================================  -->
  <xsd:complexType name="Parameters">
    <xsd:sequence>
      <xsd:element name="Parameter" type="Parameter" minOccurs="1" maxOccurs="unbounded"/>
    </xsd:sequence>
  </xsd:complexType>


  <!-- Parameter =========================================================  -->
  <xsd:complexType  name="Parameter">
    <xsd:attribute name="name"        type="xsd:string"  use="required"/>
    <xsd:attribute name="pattern"     type="xsd:string"  use="required"/>
    <xsd:attribute name="parameter"   type="xsd:string"  use="required"/>
    <xsd:attribute name="description" type="xsd:string"  default=""/>
    <xsd:attribute name="validator"   type="xsd:string"  default=""/>
  </xsd:complexType>
 */

using System;
using System.Xml.Linq;
using QUT.Bio.BioPatML.Common.XML;

namespace QUT.Bio.BioPatML.Patterns {
	
	/** <summary>A Parameter represents a member of a parameter list. </summary> */
	
	public class Parameter {
		private string name;
		private string pattern;
		private string value;
		private string description;
		private string validator;

		/// <summary> Parameterless constructor used for deserialisation. </summary>

		public Parameter () {
		}

		/// <summary> Initialises a new parameter, with name, pattern and parameter value</summary>
		/// <param name="name"></param>
		/// <param name="pattern"></param>
		/// <param name="parameter"></param>

		public Parameter ( 
			string name, 
			string pattern, 
			string parameter 
		) {
			if ( name == null || pattern == null || parameter == null ) {
				throw new ArgumentException( "Invalid Parameter." );
			}

			this.name = name;
			this.pattern = pattern;
			this.value = parameter;
		}

		/// <summary> gets the Name of this object. </summary>
		public string Name { get { return name; } }

		/// <summary> gets the Pattern of this object. </summary>
		public string Pattern { get { return pattern; } }

		/// <summary> gets the Value of this object. </summary>
		public string Value { get { return value; } }

		/// <summary> gets the Description of this object. </summary>
		public string Description { get { return description; } set { description = value; } }

		/// <summary> gets the Validator of this object. </summary>
		public string Validator { get { return validator; } set { validator = value; } }


		/// <summary> Restores the content of this parameter from an xml element. </summary>
		/// <param name="element">A non-null xml element presumed to contain a parameter definition.</param>
		/// <exception cref="System.ArgumentException">Throws an argument exception if the element is not a Parameter 
		/// or any of the name, pattern or value are missing.</exception>

		public void Parse ( XElement element ) {
			if ( element.Name != "Parameter" ) {
				throw new ArgumentException( string.Format( "Expecting Parameter but encountered {0}", element.Name ) );
			}

			string name        = element.String( "name" );
			string pattern     = element.String( "pattern" );
			string parameter   = element.String( "parameter" );
			string description = element.String( "description" );
			string validator   = element.String( "validator" );

			if ( name == null || pattern == null || parameter == null ) {
				throw new ArgumentException( "Invalid Parameter." );
			}

			this.name = name;
			this.pattern = pattern;
			this.value = parameter;
			this.description = description;
			this.validator = validator;
		}

		/// <summary> Saves the contents of this object in an xml element.
		/// </summary>
		/// <returns>An xml element containign the content of this object.</returns>

		public XElement Xml {
			get {
				XElement result = new XElement( "Parameter",
					new XAttribute( "name", name ),
					new XAttribute( "pattern", pattern ),
					new XAttribute( "parameter", value )
				);

				if ( description != null ) {
					result.Add( new XAttribute( "description", description ) );
				}

				if ( validator != null ) {
					result.Add( new XAttribute( "validator", validator ) );
				}

				return result;
			}
		}
	}
}