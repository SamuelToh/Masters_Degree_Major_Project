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
    /// This class is used to contain information when linkage are drawn from a
    /// source to another.
    /// </summary>
    public class ConnectionEventArgs : EventArgs
    {
        /// <summary>
        /// The identifier for the source Activity (commonly also referred to as inbound
        /// activity in this project)
        /// </summary>
        private string parentName;

        /// <summary>
        /// The identifier for outbound activity
        /// </summary>
        private string childName;

        /// <summary>
        /// Signifies whether the event is an addition or removal of connection line 
        /// </summary>
        private bool isAdding;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="fromIdentifier">The unique name for the source activity</param>
        /// <param name="toIdentifier">And the distinct name for outbound activity</param>
        public ConnectionEventArgs(string parentIdent, string childIdent, bool isAdding)
        {
            this.parentName = parentIdent;
            this.childName  = childIdent;
            this.isAdding = isAdding;
        }

        /// <summary>
        /// Unique identifier for the parent
        /// </summary>
        public string ParentName { get { return parentName; } set { parentName = value; } }
        
        /// <summary>
        /// Similiar to above but for its child
        /// </summary>
        public string ChildName  { get { return childName;  } set { childName = value;  } }
        
        /// <summary>
        /// Basically tells the canvas whether we are removing or adding a link between the child and parent
        /// </summary>
        public bool   IsAdding   { get { return isAdding;   } set { isAdding = value;   } }
    }
}
