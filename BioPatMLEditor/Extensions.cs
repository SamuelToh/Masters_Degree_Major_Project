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
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace BioPatMLEditor {
	public static class Extensions {

		public static void AddRange<T>( 
			this ObservableCollection<T> collection,
			IEnumerable<T> newItems
		) {
			foreach ( T newItem in newItems ) collection.Add(newItem);
		}
	}
}
