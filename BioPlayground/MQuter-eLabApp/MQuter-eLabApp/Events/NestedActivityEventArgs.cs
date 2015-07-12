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

namespace MQuter_eLabApp.Events
{
    /// <summary>
    /// The argument container class that contains information about the nesting event.
    /// Typically the event is fired When a child object is added into its parent
    /// </summary>
    public class NestedActivityEventArgs : EventArgs
    {
        /// <summary>
        /// The identifier for the main container that will contain this nested object
        /// </summary>
        public string NewParentIdentifier { get; set; }

        /// <summary>
        /// Identifier for the nested object
        /// </summary>
        public string NestedEleIdentifier { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="parentIdentifier">he identifier for the main container that will contain this nested object</param>
        /// <param name="nestedName">Identifier for the nested object</param>
        public NestedActivityEventArgs
            (string parentIdentifier, string nestedName)
        {
            this.NewParentIdentifier = parentIdentifier;
            this.NestedEleIdentifier = nestedName;
        }
    }
}
