using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TridentConnector.DataContracts
{
    [DataContract]
    public class Parameter
    {
        [DataMember]
        public String DataType { get; set; }

        [DataMember]
        public String Description { get; set; }

        [DataMember]
        public bool IsInputParam { get; set; }

        [DataMember]
        public String Label { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public bool Compulsary { get; set; }

        /// <summary>
        /// Default Constructor 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="IsInputParam"></param>
        public Parameter(String Name, bool IsInputParam)
        {
            this.Name = Name;
            this.IsInputParam = IsInputParam;
        }

        public Parameter(String Name, bool IsInputParam, String Description)
            : this(Name, IsInputParam)
        {
            this.Description = Description;
        }

        public Parameter(String Name, bool IsInputParam, String Description, String Label)
            : this(Name, IsInputParam, Description)
        {
            this.Label = Label;
        }
    }
}
