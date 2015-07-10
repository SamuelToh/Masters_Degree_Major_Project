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
using System.Collections.Generic;
using System.Xml.Linq;
using QUT.Bio.BioPatML.Common.XML;

namespace QUT.Bio.BioPatML.Patterns {

	/// <summary> Each definition may contain a list of parameters, represented by this class. </summary>

	public class ParameterList: List<Parameter> {

		/// <summary> Populates this list from a sequence of xml elements. </summary>
		/// <param name="elements"></param>
		/// <returns></returns>

		public ParameterList Parse ( IEnumerable<XElement> elements ) {
			foreach ( XElement child in elements ) {
				Parameter p = new Parameter();
				p.Parse( child );
				Add( p );
			}

			return this;
		}

		/// <summary> Serialize this object to an xml element.
		/// </summary>
		/// <returns></returns>

		public XElement ToXml () {
			XElement result = new XElement( "Parameters" );

			foreach ( var p in this ) {
				result.Add( p.Xml );
			}

			return result;
		}
	}
}