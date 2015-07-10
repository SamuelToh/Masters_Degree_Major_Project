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

namespace BioPatMLEditor {
	public class Status {
		public static void Show ( string message ) {
			MainPage mainPage = Application.Current.RootVisual as MainPage;
			mainPage.updateStatus( message );
			mainPage.statusTxtBorder.Visibility = System.Windows.Visibility.Visible;
		}

		public static void Hide () {
			MainPage mainPage = Application.Current.RootVisual as MainPage;
			mainPage.statusTxtBorder.Visibility = System.Windows.Visibility.Collapsed;
		}
	}
}
