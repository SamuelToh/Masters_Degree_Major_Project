using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections.ObjectModel;
using SR = Microsoft.Research.DataLayer;
using DM = Microsoft.Research.ScientificWorkflow.TridentModel;
using Util = Microsoft.Research.ScientificWorkflow.TridentUtilities;

namespace TridentConnector
{
    public class BioWFMLParser
    {
        /// <summary>
        /// The user
        /// </summary>
        private SR.User User;

        private TridentManager manager = null;
        //private TridentExecutor tridentRunner = null;
        private TridentModelBuilder tridentModeller;
        private int uniqueWFCounter = 0;
        private Dictionary<string, DM.BaseModel> activityDict;
        DM.TridentWorkflowModel workflow = new DM.TridentWorkflowModel();

        private int UniqueCounter
        {
            get { return uniqueWFCounter++; }
        }

        /// <summary>
        /// The registry connection.
        /// </summary>
        private SR.Connection registryConnection;

        /// <summary>
        /// Default Constructor which includes setting up the trident registry and
        /// initializing a new trident based workflow model.
        /// </summary>
        public BioWFMLParser()
        {
            manager = new TridentManager();
            activityDict = new Dictionary<string, DM.BaseModel>();
            this.registryConnection = manager.registryConnection;
            this.User = manager.User;
            //tridentRunner = new TridentExecutor(registryConnection, this.User);
            tridentModeller = new TridentModelBuilder(registryConnection, this.User);
            InitializeWorkflowModel();
        }

        /// <summary>
        /// This method fills up the compulsary field of a trident workflow model.
        /// </summary>
        private void InitializeWorkflowModel()
        {
            workflow.Category = tridentModeller.GetWorkFlowCategories()[0];
            workflow.Description = "Automatic generated description";
            workflow.Name = "eLabWorkflowD-BLESS";
        }

        /// <summary>
        /// This method fills up the compulsary field of a trident workflow model.
        /// </summary>
        private void InitializeWorkflowModel(XmlReader reader)
        {
            workflow.Category = tridentModeller.GetWorkFlowCategories()[0];
            workflow.Description = ReadAttrValue("description", reader);
            workflow.Name = ReadAttrValue("name", reader);
        }

        /// <summary>
        /// Reads the BioWFML language and saves its workflow model into Trident Database.
        /// </summary>
        /// <param name="xmlContent">BioWFML</param>
        public Guid ParseWorkflowLanguage(string xmlContent)
        {
            bool hasRootEle = false;

            using (XmlReader reader = XmlReader.Create(new StringReader(xmlContent)))
            {
                while (reader.Read())
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                if (reader.Name == "BioWFML") { hasRootEle = true; }
                                else if (!hasRootEle) { throw new XmlException("Root element not found / is not the right standard."); }
                                else { DecipherElement(reader); }

                                break;
                            }
                    }

                try
                {
                    manager.CommitWorkflowToTridentDB(workflow);
                }
                catch (Exception ex)
                {
                    Util.TridentErrorHandler.HandleUIException(ex);
                    throw ex;
                }

                return workflow.Id;
            }
        }

        private void DecipherElement(XmlReader reader)
        {
            switch (reader.Name)
            {
                case "FlowElement": { ReadFlowElement(reader); break; }
                case "Workflow"   : { InitializeWorkflowModel(reader); break; }
                case "BeginFlow"  : { workflow.Root = tridentModeller.GetWorkflowRoot(ref this.uniqueWFCounter); break; } 
            }
        }

        private void ReadFlowElement(XmlReader reader)
        {
            DM.BaseCompositeModel activityTray = null;
            DM.BaseModel currActivity = null;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: 
                        {
                            switch (reader.Name)
                            {
                                case "Sequential":
                                    {
                                        activityTray = workflow.Root;
                                        break;
                                    }
                                case "Parallel":
                                    {
                                        DM.BaseCompositeModel parallelModel = GetParallelModel();

                                        if ((activityTray != null) && tridentModeller.IsParallelBasedModel(activityTray.ParentModel))
                                        {
                                            parallelModel.ParentModel = activityTray;
                                            activityTray.Children.Add(parallelModel);
                                        }
                                        else
                                        {
                                            parallelModel.ParentModel = workflow.Root;
                                            workflow.Root.Children.Add(parallelModel);
                                        }

                                        activityTray = parallelModel;
                                        break;
                                    }
                                case "Activity":
                                    {
                                        DM.BaseModel activityModel = GetActivityModel(ReadAttrValue("clsname", reader));
                                        AddActivityRefToDict(ReadAttrValue("id", reader), activityModel);
                                        currActivity = activityModel;

                                        if (tridentModeller.IsParallelBasedModel(activityTray))
                                        {
                                            DM.CompositeActivityModel composite1 = activityTray.Children[0] as DM.CompositeActivityModel;
                                            activityModel.ParentModel = composite1;
                                            composite1.Children.Add(activityModel);
                                            activityTray = activityTray.Children[1] as DM.CompositeActivityModel;
                                        } else {
                                            activityModel.ParentModel = activityTray;
                                            activityTray.Children.Add(activityModel);
                                        }

                                        break;
                                    }

                                case "InputValue":
                                    {
                                        if (currActivity != null)
                                        {
                                            DM.ParameterDescriptionModel
                                                tridentParam = GetParameterModelFrmCollection
                                                                    (currActivity.InputParameters, ReadAttrValue("name", reader));
                                            
                                            if ((tridentParam != null) 
                                                        && ReadAttrValue("value", reader) != null)
                                            
                                                    tridentParam.Value = ReadAttrValue("value", reader);
                                            
                                            else if 
                                                ((tridentParam != null) 
                                                        && ReadAttrValue("linkFrom", reader) != null)
                                            {

                                                string binderValue = ReadAttrValue("linkFrom", reader);

                                                tridentParam.CreateBindings
                                                                (this.activityDict[GetBioWFMLItemUniqueIdentifier(binderValue)].UniqueId,
                                                                  GetBioWFMLItemPropertyName(binderValue));
                                            }
                                        }
                                        break;
                                    }

                                case "OutputValue":
                                    {
                                        DM.ParameterDescriptionModel
                                                tridentParam = GetParameterModelFrmCollection
                                                                    (currActivity.OutputParameters, ReadAttrValue("name", reader));

                                        if ((tridentParam != null)
                                                       && ReadAttrValue("dataValue", reader) == "true")
                                        {
                                            //tridentParam.IsMandatory = true;
                                            tridentParam.IsMandatoryForWorkflow = true;
                                            tridentParam.IsFinal = true;
                                            tridentParam.OutputType = "text/plain";
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                    case XmlNodeType.EndElement: 
                        { 
                            if (reader.Name.Equals("FlowElement")) { return; }
                            continue;
                        }
                }
            }
        }

        private string GetBioWFMLItemPropertyName(string binderValue)
        {
            string[] parts = binderValue.Split('.');

            if (parts.Length < 2)
                throw new ArgumentException
                    ("The binder value does not follow the BioWFML standard.");

            return parts[parts.Length - 1]; //return the tail value
        }

        private string GetBioWFMLItemUniqueIdentifier(string binderValue)
        {
            string[] parts = binderValue.Split('.');

            if (parts.Length < 2)
                throw new ArgumentException
                    ("The binder value does not follow the BioWFML standard.");

            return parts[parts.Length - 2]; //return only the header value.
        }

        private void AddActivityRefToDict(string activityId, DM.BaseModel tridentModel)
        {
            this.activityDict.Add(activityId, tridentModel);
        }

        private DM.ParameterDescriptionModel GetParameterModelFrmCollection
                        (Collection<DM.ParameterDescriptionModel> paramModels, string paramIdentifier)
        {
            var paramModel = from p in paramModels where p.Name == paramIdentifier select p;
            if (paramModel.Count() > 1)
            {
                throw new ArgumentException("There are duplicated param.");
            }
            else if (paramModel.Count() < 1)
            {
                throw new ArgumentException("Input parameter don't exist.");
            }

            return paramModel.ElementAt(0);
        }

        private string ReadAttrValue(string attrName, XmlReader reader)
        {
            reader.MoveToAttribute(attrName);

            return reader.GetAttribute(attrName) == null ? null : reader.Value;
        }
        
        /// <summary>
        /// Retrieves the desire activity model from Trident Database.
        /// Basic models are typically those activities while complex type are
        /// the composition model (parallel / sequential).
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private DM.BaseModel GetActivityModel(string className)
        {
            return
                tridentModeller.FetchModelByActivityClass
                                    (className, ref this.uniqueWFCounter);
        }
        
        /// <summary>
        /// Retrieves a parallel data model from Trident data base
        /// </summary>
        /// <returns></returns>
        private DM.BaseCompositeModel GetParallelModel()
        {
            return tridentModeller.GetParallelBaseCompositeModel(ref this.uniqueWFCounter);
        }
    }
}
