using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace BioPatMLEditor.PatternControls.PatternModels {
	public class SimpleElement : INotifyPropertyChanged, IPatternElement {
		private string elementName;
		private string cosmeticElementName;

		/// <summary> Create a new simple element. </summary>
		/// <param name="elementName">The element name to be used when generating xml. </param>
		
		public SimpleElement ( string elementName, string cosmeticElementName ) {
			this.elementName = elementName;
			this.cosmeticElementName = cosmeticElementName;
		}

		/// <summary> Create a new simple element. </summary>
		/// <param name="elementName">The element name to be used when generating xml. </param>
		
		public SimpleElement ( string elementName ) : this( elementName, elementName ) {
		}

		/// <summary> Get he name of the xml element that will be produces by ToXml. </summary>

		[Display( AutoGenerateField = false )]
		
		public string ElementName { get { return elementName; } }

		[Display(AutoGenerateField = false)]
		
		public string CosmeticElementName {
			get {
				return cosmeticElementName;
			}
		}

		protected void NotifyPropertyChanged ( string PropertyName ) {
			if ( null != PropertyChanged )
				PropertyChanged( this, new PropertyChangedEventArgs( PropertyName ) );
		}

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary> Produces xml that represents basic pattern information, along with 
		/// a collection of additional attributes. </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		
		public virtual XElement ToXml (
		    params object [] attributes
		) {
		    return new XElement( elementName, attributes );
		}

		public virtual void LoadFromTree ( IEnumerable<TreeViewItem> childViewItems ) {
			// Simple elements have no contents: do nothing.
			// TODO: check for errors?
		}
	}
}
