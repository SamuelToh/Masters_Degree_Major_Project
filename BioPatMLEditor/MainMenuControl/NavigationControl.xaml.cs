using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace BioPatMLEditor.MainMenuControl
{
    /// <summary>
    /// This UC encapsulates all the navigation element for the BioPatML Editor
    /// </summary>
    public partial class NavigationControl : Page
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public NavigationControl()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
