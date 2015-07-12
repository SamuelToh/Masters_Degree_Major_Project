using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.ObjectModel;
using Microsoft.Research.ScientificWorkflow.TridentUtilities;
using Microsoft.Research.ScientificWorkflow.TridentComposer;
using SR = Microsoft.Research.DataLayer;
using DM = Microsoft.Research.ScientificWorkflow.TridentModel;

namespace TridentConnector
{
    public class TridentModelBuilder
    {
        /// <summary>
        /// The registry connection.
        /// </summary>
        private SR.Connection registryConnection;
        private SR.User user;

        ActivityComposer activityComposer;

        public TridentModelBuilder(SR.Connection registryConn, SR.User user)
        {
            this.registryConnection = registryConn;
            activityComposer = new ActivityComposer(registryConnection, false);
            this.user = user;
        }

        /// <summary>
        /// This method commits a category search request on the Trident database and pulls
        /// all the available workflow cat.
        /// </summary>
        /// <returns>A collection of workflow category models</returns>
        public Collection<DM.WorkflowCategoryModel> GetWorkFlowCategories()
        {
            return CategoriesComposer.ReadWorkflowCategoriesFromRegistry(registryConnection, this.user, true);
        }


        public DM.BaseCompositeModel GetWorkflowRoot(ref int counter)
        {
            //return activityComposer.GetActivityModel("System.Workflow.Activities.SequentialWorkflowActivity");
            DM.ActivityMetadata rootMeta = RetrieveActivityByName("System.Workflow.Activities.SequentialWorkflowActivity");
            return FetchModelByActivityMetadata(rootMeta, ref counter) as DM.BaseCompositeModel;
        }

        public DM.BaseCompositeModel GetSequentialBaseCompositeModel(int counter)
        {
            DM.ActivityMetadata SeqMeta = RetrieveActivityByName("System.Workflow.Activities.SequenceActivity");
            return FetchModelByActivityMetadata(SeqMeta, ref counter) as DM.BaseCompositeModel;
        }

        /// <summary>
        /// Retrieves a composite parallel model from Trident database.
        /// </summary>
        /// <param name="counter">an unique counter associate with the entire workflow by far</param>
        /// <returns>A parallel trident based model</returns>
        public DM.BaseCompositeModel GetParallelBaseCompositeModel(ref int counter)
        {
            DM.ActivityMetadata parallelMeta = RetrieveActivityByName("System.Workflow.Activities.ParallelActivity");
            return FetchModelByActivityMetadata(parallelMeta, ref counter) as DM.BaseCompositeModel;
        }

        /// <summary>
        /// Checks whether the given model is a parallel composite model
        /// </summary>
        /// <param name="model">Target model that is sent for checking</param>
        /// <returns></returns>
        public bool IsParallelBasedModel(DM.BaseModel model)
        {
            return 
                (model is DM.BaseCompositeModel) && (model.ActivityClass == "System.Workflow.Activities.ParallelActivity") ? true : false;
        }

        private DM.ActivityMetadata RetrieveActivityByName(string className)
        {
            Collection<SR.Activity> activities = SR.Activity.CommonSearches.GetActivitiesByName(className, registryConnection);

            if (activities.Count > 0)
            {
                //If trident still has our old copies of DLL sitting in the database
                if (activities.Count > 1)
                {
                    var actualActivity = from a in activities where a.IsDeleted == false select a;
                    return new DM.ActivityMetadata(actualActivity.ElementAt(0));
                }

                return new DM.ActivityMetadata(activities[0]);
            }

            throw new DllNotFoundException("Could not find the expected activity DLL.");
        }


        private DM.BaseModel FetchModelByActivityMetadata(DM.ActivityMetadata metadata, ref int uniqueCount)
        {
            DM.BaseModel model = null;
            switch (metadata.ActivityClass)
            {
                case "TridentBasicActivities.Activities.ForActivity":
                    //model = this.FetchForActivityModelByMeta(metadata, uniqueCount);
                    //DM.ModelFactory.AssignForActivityModelAttributes(model as DM.ForActivityCompositeModel, parentModel);
                    break;

                case "TridentBasicActivities.Activities.Replicator":
                    //model = this.FetchReplicatorActivityModelByMeta(metadata, uniqueCount);
                    //DM.ModelFactory.AssignReplicatorActivityModelAttributes(model as ReplicatorActivityCompositeModel, parentModel);
                    break;

                case "TridentBasicActivities.Activities.IFElse":
                    //model = this.FetchIfElseCompositeActivityModelByMeta(metadata, uniqueCount);
                    //DM.ModelFactory.AssignIfElseActivityModelAttributes(model as IfElseActivityCompositeModel, parentModel);
                    break;

                default:
                    if (metadata.IsComposite)
                    {
                        model = this.FetchCompositeActivityModelByMeta(metadata, ref uniqueCount);
                        //AssignCompositeActivityModelAttributes(model as DM.CompositeActivityModel, parentModel);
                    }
                    else
                    {
                        model = this.FetchSimpleActivityModelByMeta(metadata, ref uniqueCount);
                        //DM.ModelFactory.AssignSimpleActivityModelAttributes(model as SimpleActivityModel, parentModel);
                    }

                    break;
            }

            return model;
        }

        private DM.SimpleActivityModel FetchSimpleActivityModelByMeta(DM.ActivityMetadata metaData, ref int uniqueCount)
        {
            return new DM.SimpleActivityModel(metaData, uniqueCount++.ToString());
        }

        /// <summary>
        /// Entry point to grab an activity by name
        /// </summary>
        /// <param name="activityClass"></param>
        /// <param name="uniqueCount"></param>
        /// <param name="parentModel"></param>
        /// <returns></returns>
        public DM.BaseModel FetchModelByActivityClass(string activityClass, ref int uniqueCount)
        {
            DM.BaseModel model = null;
            DM.ActivityMetadata metaData = this.RetrieveActivityByName(activityClass);
            if (metaData != null)
            {
                model = this.FetchModelByActivityMetadata(metaData, ref uniqueCount);
            }

            return model;
        }

        private DM.CompositeActivityModel FetchCompositeActivityModelByMeta(DM.ActivityMetadata metaData, ref int uniqueCount)
        {
            DM.CompositeActivityModel compositeModel = new DM.CompositeActivityModel(metaData, uniqueCount++.ToString());
            if (compositeModel.ActivityClass.Equals("System.Workflow.Activities.ParallelActivity", StringComparison.OrdinalIgnoreCase))
            {
                DM.CompositeActivityModel childSequenceActivity1 = this.FetchModelByActivityClass("System.Workflow.Activities.SequenceActivity",
                    ref uniqueCount) as DM.CompositeActivityModel;

                DM.CompositeActivityModel test = childSequenceActivity1 as DM.CompositeActivityModel;

                childSequenceActivity1.ParentModel = compositeModel;
                
                DM.CompositeActivityModel childSequenceActivity2 = this.FetchModelByActivityClass("System.Workflow.Activities.SequenceActivity",
                    ref uniqueCount) as DM.CompositeActivityModel;
                childSequenceActivity2.ParentModel = compositeModel;
                
                compositeModel.Children.Add(childSequenceActivity1);
                compositeModel.Children.Add(childSequenceActivity2);
            }

            return compositeModel;
        }
    }
}
