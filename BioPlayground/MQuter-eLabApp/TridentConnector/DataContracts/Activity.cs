using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TridentConnector.DataContracts
{
    [DataContract]
    public class Activity
    {
        public enum ActivityType
        {
            Standard = 0,
            ForLoop = 1
        };

        [DataMember]
        public ActivityType ActivityTypes { get; set; }

        [DataMember]
        public String ActivityClass { get; set; }

        [DataMember]
        public String Description { get; set; }

        [DataMember]
        public String DisplayLabel { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public List<Parameter> InputParam;

        [DataMember]
        public List<Parameter> OutputParam;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="Id"></param>
        public Activity(Guid Id)
        {
            this.Id = Id;
            this.InputParam = new List<Parameter>();
            this.OutputParam = new List<Parameter>();
        }

        public Activity(Guid Id, String Description)
            : this(Id)
        {
            this.Description = Description;
        }

        public Activity(Guid Id, String Description, String DisplayLabel)
            : this(Id, Description)
        {
            this.DisplayLabel = DisplayLabel;
        }

        public Activity(Guid Id, String Description, String DisplayLabel, String ActivityClass)
            : this(Id, Description, DisplayLabel)
        {
            this.ActivityClass = ActivityClass;
        }

    }
}
