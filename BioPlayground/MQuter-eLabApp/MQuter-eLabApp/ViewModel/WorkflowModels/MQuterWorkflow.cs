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
using System.Xml;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MQuter_eLabApp.ViewModel.WorkflowModels
{
    [XmlRoot("BioWFML")]
    public class MQuterWorkflow : IXmlSerializable
    {
        const string ROOT_NAME = "BioWFML";

        /// <summary>
        /// entry point of the root
        /// </summary>
        //private BaseWorkflowModel entryPtr;

        /// <summary>
        /// Workflow model in sequential order 
        /// the lowest index is the entry level, the highest is the end point.
        /// </summary>
        private Collection<BaseWorkflowModel> root;

        //private BaseWorkflowModel endPtr;

        public MQuterWorkflow()
        {
            root = new Collection<BaseWorkflowModel>();
        }

        //[XmlElement("Flow")]
        public Collection<BaseWorkflowModel> Root
        {
            get
            {
                return this.root;
            }
            set
            {
                root = value;
            }
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            //WRite the header information for this workflow
            writer.WriteStartElement("Workflow");
            writer.WriteAttributeString("name", "BioPlaygroundWF");
            writer.WriteAttributeString("description", "Auto generated wf");

            writer.WriteStartElement("BeginFlow");

            int level = 0;

            //Please note here, each root represents a different level
            foreach (BaseWorkflowModel model in root)
            {
                writer.WriteStartElement("FlowElement");
                writer.WriteAttributeString("level", level++.ToString());

                model.WriteXml(writer);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}
