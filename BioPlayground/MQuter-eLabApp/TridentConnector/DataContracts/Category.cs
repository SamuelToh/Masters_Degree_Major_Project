using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace TridentConnector.DataContracts
{
    [DataContract]
    public class Category
    {
        [DataMember]
        public List<Activity> Activities { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public String Label { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public String Description { get; set; }

        public Category() { }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Name"></param>
        public Category(Guid Id, String Name)
        {
            this.Id = Id;
            this.Name = Name;
        }

        public Category(Guid Id, String Name, String Label)
            : this(Id, Name)
        {
            this.Label = Label;
        }

        public Category
                (Guid Id, String Name, String Label, List<Activity> Activities)
            : this(Id, Name, Label)
        {
            this.Activities = Activities;
        }
    }
}
