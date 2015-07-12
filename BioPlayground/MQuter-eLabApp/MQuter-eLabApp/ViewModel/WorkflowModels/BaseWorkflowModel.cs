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
using System.Xml.Serialization;

namespace MQuter_eLabApp.ViewModel.WorkflowModels
{
    public class BaseWorkflowModel : IWorkflowItem, IXmlSerializable, IDisposable
    {
        protected IWorkflowItem[] _activity;

        public IWorkflowItem[] GetActivities()
        {
            return this._activity; 
        }

        public IWorkflowItem ActivityModel1
        {
            get { return this._activity[0]; }
            set { this._activity[0] = value; }
        }


        public void WriteXml(System.Xml.XmlWriter writer)
        {

            String headerTag = null;

            if(this is SequentialWorkflowModel)
            {
                headerTag = "Sequential";
            }
            else if (this is ParallelWorkflowModel)
            {
                headerTag = "Parallel";
            }

            writer.WriteStartElement(headerTag);

            for (int i = 0; i < _activity.Length; i++)
            {
                if (GetActivities()[i] is StandardFlowModel)
                {
                    (GetActivities()[i] as StandardFlowModel).WriteXml(writer);
                }
                else if (GetActivities()[i] is BaseWorkflowModel)
                {
                    (GetActivities()[i] as BaseWorkflowModel).WriteXml(writer);
                }
            }

            writer.WriteEndElement();
        }

        #region IDisposable

        /// <summary>
        /// IDisposable interface
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < GetActivities().Length; i++)
            {
                GetActivities()[i] = null;
            }
        }

        #endregion

        #region Unimplemented IXmlSerializable Methods

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
