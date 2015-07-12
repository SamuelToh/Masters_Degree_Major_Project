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
using System.Collections.Generic;
using MQuter_eLabApp.ViewModel;

namespace MQuter_eLabApp.ViewModel.WorkflowModels
{

    public class StandardFlowModel : IDisposable, IWorkflowItem
    {
        /// <summary>
        /// The activity representing this model
        /// </summary>
        private ActivityModel activity;

        /// <summary>
        /// A list of connected sub flow activities
        /// </summary>
        private List<StandardFlowModel> children;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public StandardFlowModel() 
        {
            children = new List<StandardFlowModel>();
        }

        public StandardFlowModel(ActivityModel activity)
            : this()
        {
            this.activity = activity;
        }

        public ActivityModel Activity
        {
            get { return this.activity; }
            set{ this.activity = value; }
        }

        public List<StandardFlowModel> Children
        {
            get { return this.children; }
        }

        #region IDisposable

        /// <summary>
        /// IDisposable interface
        /// </summary>
        public void Dispose()
        {
            activity = null;
            children = null;
        }

        #endregion

        protected void WriteParameterXML(XmlWriter writer, List<ParameterModel> parameters)
        {
            foreach (ParameterModel param in parameters)
            {
                if (param is ParamInputModel)
                    WriteInputParam(writer, param as ParamInputModel);
                
                else if 
                    (param is ParamOutputModel)
                        WriteOutputParam(writer, param as ParamOutputModel);
            }
        }

        private void WriteOutputParam(XmlWriter writer, ParamOutputModel param)
        {
            if (param.AsDataOutput)
            {
                writer.WriteStartElement("OutputValue");
                writer.WriteAttributeString("name", param.Name);
                writer.WriteAttributeString("dataValue", "true");
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// If this param has a binder object we will write it out first otherwise
        /// we will just write its original input value.
        /// </summary>
        /// <param name="writer">writer object</param>
        /// <param name="param">the subject</param>
        private void WriteInputParam(XmlWriter writer, ParamInputModel param)
        {
            string parameterValue =
                param.ValueStr  != null ? param.ValueStr : null;

            string parameterBinderValue = 
                param.Value     != null ? param.Value.ToString() : null;

            if (parameterValue != null || parameterBinderValue != null)
            {
                writer.WriteStartElement("InputValue");
                writer.WriteAttributeString("name", param.Name);

                if (parameterBinderValue != null)
                    writer.WriteAttributeString("linkFrom", param.Value.ToString());
                    
                else
                    writer.WriteAttributeString("value", param.ValueStr);


                writer.WriteEndElement();
            }
        }

        #region IXmlSerializable Interface

        /// <summary>
        /// IXmlSerializable interface
        /// </summary>
        /// <param name="writer"></param>
        public virtual void SerializeToXML(XmlWriter writer)
        {
            writer.WriteStartElement("Activity");
            writer.WriteAttributeString("clsname", Activity.ActivityClass);
            writer.WriteAttributeString("id", Activity.UniqueName);

            WriteParameterXML(writer, Activity.InputParam);
            WriteParameterXML(writer, Activity.OutboundParam);

            writer.WriteEndElement();
        }

        #endregion

        #region Unimplemented IXmlSerializable Methods

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
            SerializeToXML(writer);
        }

        #endregion
    }
}
