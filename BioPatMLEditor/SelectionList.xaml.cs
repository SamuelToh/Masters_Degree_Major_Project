using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BioPatMLEditor {
	public partial class SelectionList : UserControl {
		public SelectionList () {
			InitializeComponent();
		}

		/// <summary> Get or set the selection mode of this list. </summary>
		
		public SelectionMode SelectionMode {
			get { return listbox.SelectionMode; }
			set { listbox.SelectionMode = value; }
		}

		/// <summary> Gets a reference to the list of selectable objects. </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>

		public ObservableCollection<T> GetCollection<T> () where T : ISelectable {
			return listbox.ItemsSource as ObservableCollection<T>;
		}

		/// <summary> Sets the list of selectable objects. </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>

		public void SetCollection<T> ( ObservableCollection<T> collection ) where T : ISelectable {
			listbox.ItemsSource = collection;
		}

		/// <summary> Feed selection information through to the selectable objects in the collection. </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void SelectionChanged ( object sender, SelectionChangedEventArgs e ) {
			//foreach ( var selectedItem in e.AddedItems ) {
			//    ((ISelectable) selectedItem).Selected = true;
			//}
			//foreach ( var selectedItem in e.RemovedItems ) {
			//    ((ISelectable) selectedItem).Selected = false;
			//}

			listbox.SelectedItem = null;
		}

	}
}
