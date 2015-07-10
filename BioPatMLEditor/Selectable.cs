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
using System.ComponentModel;

namespace BioPatMLEditor {

	public abstract class Selectable : ISelectable {
		public event PropertyChangedEventHandler  PropertyChanged;

		private bool selected;

		/// <summary> Get or set the selected status iof this object.
		/// <para>Setting this value to a new value causes the PropertyChanged event.</para></summary>

		public bool Selected {
			get {
				return selected;
			}
			set {
				if ( value != selected ) {
					selected = value;
					if ( PropertyChanged != null ) {
						PropertyChanged( this, new PropertyChangedEventArgs( "Selected" ) );
					}
				}
			}
		}

		public abstract string Name {
			get;
		}
	}
}
