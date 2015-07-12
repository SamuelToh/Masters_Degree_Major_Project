namespace MQuter_eLabApp.Model
{
    #region Directives
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Runtime.Serialization;
    using MQuter_eLabApp.ViewModel;
    using MQuter_eLabApp.ViewModel.WorkflowModels;
    using System.Linq;
    using System.Text;
    using System.IO;
    #endregion

    /// <summary>
    /// Maintains the structural of workflow as activities get added/removed off the canvas
    /// </summary>
    public class WorkflowManager
    {
        /// <summary>
        /// Current workflow structure are kept in here.
        /// </summary>
        private MQuterWorkflow workflow;

        /// <summary>
        /// All the available Activities are kept in this dict. for reference
        /// </summary>
        internal Dictionary<String, StandardFlowModel> activityDict;

        /// <summary>
        /// Default constructor
        /// </summary>
        public WorkflowManager()
        {
            activityDict = new Dictionary<string, StandardFlowModel>();
        }

        #region Methods concerning Adding / Removal of activities and linkages

        public void AddActivities(String identifier, ActivityModel activity)
        {
            #region Validation

            if ((identifier == null))
                throw new ArgumentNullException
                            ("The activity identifier (unique name) cannot be null.");

            else if (activity == null)
                throw new ArgumentNullException
                            ("Workflow Manager could not add null activity.");

            #endregion

            StandardFlowModel activityTray = 
                activity.ActivityClassification ==  ActivityModel.ActivityType.ForLoop 
                ? new ForLoopFlowModel(activity) : new StandardFlowModel(activity);

            this.activityDict.Add(identifier, activityTray);
        }

        public void ChangeActivityParent(string newParentIdentifier, string activityName)
        {
            StandardFlowModel activityTray = activityDict[activityName];
            this.activityDict.Remove(activityName);
            ForLoopFlowModel newParentTray = activityDict[newParentIdentifier] as ForLoopFlowModel;
            newParentTray.NestedModels.Add(activityTray);
        }

        public int CountActivitiesInMemory
        {
            get { return activityDict.Keys.Count;  }
        }

        public void RemoveActivities(String identifier)
        {
            this.activityDict.Remove(identifier);
        }

        public void AddActivityLink(String parentIdent, String childIdent)
        {
            #region Validation

            if ((parentIdent == null))
                throw new ArgumentNullException
                            ("Parent identifier name is null.");

            else if (childIdent == null)
                throw new ArgumentNullException
                            ("Child identifier name is null.");

            else if (!activityDict.ContainsKey(parentIdent))
                throw new KeyNotFoundException
                            ("Parent model not found!");

            else
                if (!activityDict.ContainsKey(childIdent))
                        throw new KeyNotFoundException
                                  ("Child model not found!");

            #endregion

            //Validate that there cannot be more than 1 Start and End links

            StandardFlowModel source = activityDict[parentIdent];
            StandardFlowModel target = activityDict[childIdent];

            source.Children.Add(target);
        }

        public void RemoveActivityLink(String parentIdentf, String childIdentf)
        {
            #region Validation
            if ((parentIdentf == null))
                throw new ArgumentNullException
                            ("Parent identifier name is null.");

            else if (childIdentf == null)
                throw new ArgumentNullException
                            ("Child identifier name is null.");

            else if (!activityDict.ContainsKey(parentIdentf))
                throw new KeyNotFoundException
                            ("Parent model not found!");

            else if (!activityDict.ContainsKey(childIdentf)) { return; }
                //throw new KeyNotFoundException
                            //("Child model not found!");
            #endregion

            StandardFlowModel source = activityDict[parentIdentf];
            StandardFlowModel target = activityDict[childIdentf];

            source.Children.Remove(target);
        }

        #endregion

        public MQuterWorkflow WorkflowStructure
        {
            get { return this.workflow; }
        }

        /// <summary>
        /// This method basically takes all the activity within the dictionary
        /// segment them into their appropriate level and then sort them into
        /// the different flow container; can be sequential, parallel and 
        /// nested parallel models.
        /// Then XML are built through XML writer object and a stringBuilder
        /// afterwhich these xml syntax are transformed into string and returned.
        /// </summary>
        /// <returns></returns>
        public String GetWorkflowXML()
        {
            workflow = new MQuterWorkflow();
            QueueActivitiesIntoOrder();
       
            StringBuilder output = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(output, ws))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("BioWFML"); //Root Element
                workflow.WriteXml(writer);
                writer.WriteEndElement();
            }

            return output.ToString();
        }

        /// <summary>
        /// Here we transverse to the deepest level of our workflow
        /// and from there building each level's item by reverse direction.
        /// The final outcome would be several levels with each
        /// level filled with the respective nodes.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="model"></param>
        /// <param name="groups"></param>
        private void GroupItemByLevel(int level, StandardFlowModel model, Dictionary<int, List<StandardFlowModel>> groups)
        {
            int currLevel = level++;
            foreach (StandardFlowModel item in model.Children)
            {
                GroupItemByLevel(level, item, groups);

                if (item.Activity.IsEndFlow)
                    break; // dont include the ending node

                //Start groupping activity here
                if (groups.Keys.Contains(currLevel))
                {
                    
                    List<StandardFlowModel> itemList = groups[currLevel];

                    if(!itemList.Contains(item))
                        groups[currLevel].Add(item);
                }
                else
                {
                    List<StandardFlowModel> itemList = new List<StandardFlowModel>();
                    itemList.Add(item);
                    groups.Add(currLevel, itemList);
                }   
            }
        }

        private void QueueActivitiesIntoOrder()
        {
            #region Select the Start Model
            StandardFlowModel StartModel = null;
            StandardFlowModel EndModel = null;
            
            try
            {
                StartModel = 
                    (from keyvalue in activityDict
                     where (keyvalue.Value.Activity.IsStartFlow == true)
                     select keyvalue.Value).Single();

                EndModel =
                    (from keyvalue in activityDict
                     where (keyvalue.Value.Activity.IsEndFlow == true)
                     select keyvalue.Value).Single();

                if (StartModel == null)
                    throw new ArgumentException
                            ("Please make sure you have a Start Model that are linked up in the workflow.");

                else if (EndModel == null)
                    throw new ArgumentException
                            ("Could not locate End Model in the workflow make sure there is one before proceeding.");
    
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            #endregion

            #region Sort all activities into their respective Level

            Dictionary<int, List<StandardFlowModel>> groups = new Dictionary<int, List<StandardFlowModel>>();
            GroupItemByLevel(0, StartModel, groups); //Transverse to the deepest node and group them

            #endregion

            #region Iterate through each level of collections and build their flow (sequential/parallel)
            
            int currLevel = 0;

            for (int i = 0; i < groups.Keys.Count; i++)
            {
                groups
                    .Where(lv => lv.Key == currLevel)
                    .Select(lv => lv.Value)
                    .ToList()
                    .ForEach(delegate(List<StandardFlowModel> model)
                    {
                        #region For each level, build the models into flow of sequential / parallel trays
                        System.Diagnostics.Debug.WriteLine("Curr Level > " + model[0].Activity.ActivityClass);
                        if (model.Count == 1)
                        {
                            //Simplest scenario of all, only an activity exist so a sequential tray is only needed
                            StandardFlowModel activity = model[0];
                            SequentialWorkflowModel sequential = new SequentialWorkflowModel(activity);
                            workflow.Root.Insert(currLevel, sequential);
                        }
                        else if (model.Count == 2)
                        {
                            //2 activities for this step thus a Parallel is sufficient
                            StandardFlowModel activity1 = model[0];
                            StandardFlowModel activity2 = model[1];
                            ParallelWorkflowModel parallel = new ParallelWorkflowModel(activity1, activity2);
                            workflow.Root.Insert(currLevel, parallel);
                        }
                        else if (model.Count > 2)
                        {
                            //Complex scenario where there are more than 2 activities
                            //Here we need several parallel models and a sequential model
                            ParallelWorkflowModel complexType = BuildParallelModels(model);
                            workflow.Root.Insert(currLevel, complexType);
                        }
                        else
                            throw new ArgumentOutOfRangeException
                                        ("Unexpected error ocurred while parsing the workflow model.");
                        #endregion
                    });
                currLevel++;
            }
            
            #endregion
        }

        private ParallelWorkflowModel BuildParallelModels(List<StandardFlowModel> model)
        {
            ParallelWorkflowModel currParallelModel = new ParallelWorkflowModel(null, null);
            ParallelWorkflowModel head = currParallelModel;
            ParallelWorkflowModel nextParallelModel, prevParallelModel;
            int iterator = 0, totalModels = model.Count;

            do
            {
                StandardFlowModel childModel = model[iterator++];

                nextParallelModel = new ParallelWorkflowModel(null, null);

                currParallelModel.ActivityModel1 = childModel;
                currParallelModel.ActivityModel2 = nextParallelModel;
                totalModels--; //model.Remove(childModel);

                //Prepare for next iteration
                prevParallelModel = currParallelModel;
                currParallelModel = nextParallelModel;
            } while (totalModels > 1);
                //} while (model.Count > 1);

            StandardFlowModel sequentialChild = model[0]; // The last element
            //SequentialWorkflowModel sequential = new SequentialWorkflowModel(sequentialChild);
            //nextParallelModel.ActivityModel2 = sequentialChild;
            prevParallelModel.ActivityModel2 = sequentialChild;
            currParallelModel = null;

            return head;
        }
    }
}
