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

namespace MQuter_eLabApp.ViewModel.WorkflowModels
{
    public class ForLoopFlowModel : StandardFlowModel
    {
        private List<StandardFlowModel> _nested;

        public ForLoopFlowModel(ActivityModel activity)
            : base(activity)
        {
            _nested = new List<StandardFlowModel>();
        }

        public List<StandardFlowModel> NestedModels
        {
            get { return this._nested; }
        }


        public override void SerializeToXML(XmlWriter writer)
        {
            writer.WriteStartElement("Loop");

            foreach (ParamInputModel param in Activity.InputParam)
            {
                string paramValue = param.Value != null ? param.Value.ToString() : param.ValueStr == null ? param.ValueStr : null;

                if (paramValue != null)
                {
                    writer.WriteStartElement(param.IsInputParam ? "InboundParam" : "OutboundParam");
                    writer.WriteAttributeString("name", param.Name);
                    writer.WriteAttributeString("type", param.DataType);
                    writer.WriteStartElement("Value");
                    writer.WriteValue(paramValue); //todo
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }


            writer.WriteAttributeString("cycles", "0");
            writer.WriteAttributeString("increment", "1");

            foreach (StandardFlowModel activity in NestedModels)
            {
                activity.SerializeToXML(writer);
            }

            writer.WriteEndElement();
        }
    }
}
