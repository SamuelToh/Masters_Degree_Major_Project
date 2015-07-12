using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Globalization;
using TridentAPI;
using ManagementStudioServices;
using ManagementStudioServices.Interfaces;
using SR = Microsoft.Research.DataLayer;
using DM = Microsoft.Research.ScientificWorkflow.TridentModel;
using Microsoft.Research.DataLayer.ServiceModel;
using Microsoft.Research.ScientificWorkflow.TridentComposer;
using SVC = Microsoft.Research.ScientificWorkflow;
using Microsoft.Research.ScientificWorkflow.TridentUtilities;
using Microsoft.Research.ScientificWorkflow.ManagementStudioModels;
using Microsoft.Research.ScientificWorkflow.ManagementStudioControls;
using Microsoft.Research.ScientificWorkflow.ManagementStudioControls.Interfaces;
using TridentConnector.DataContracts;


namespace TridentConnector
{
    public class TridentManager
    {
        #region Declaration

        /// <summary>
        /// The registry connection.
        /// </summary>
        public SR.Connection registryConnection;

        /// <summary>
        /// The user
        /// </summary>
        public SR.User User;

        /// <summary>
        /// The connection manager.
        /// </summary>
        public SR.ConnectionManager connMgr;

        /// <summary>
        /// The workflow mananger service.
        /// </summary>
        IWorkflowManagerService WorklfowService;

        /// <summary>
        /// The registry manager service.
        /// </summary>
        private IRegistryManagerService RegistryService;

        /// <summary>
        /// This is used to access the UI Designer Class.
        /// </summary>
        private SVC.UIDesigner.UIHelper TridentHelper { get; set; }

        private InstanceAPI tridentAPI;

        #endregion

        #region Constructor

        static TridentManager()
        {
            //Initialize the registry
            SR.SR_Connection.Init();
        }

        /// <summary>
        /// Default Constructor, we initialize all the backbone connectors here.
        /// See InitializeConnectors() for more details.
        /// </summary>
        public TridentManager() { InitializeConnectors(); }

        #endregion

        #region Private Methods

        private void InitializeConnectors()
        {
            InitializeRegistry();
            AuthenticateConnector();
            TridentHelper = new SVC.UIDesigner.UIHelper(this.registryConnection);
            WorklfowService = new WorkflowManagerService(this.registryConnection);
            RegistryService = new RegistryManagerService(this.registryConnection);
            CategoriesComposer.CreateUser(TridentAuthentication.LoggedUserInUserName, registryConnection);
            this.User = TridentAuthentication.LoggedInUser; //put else where so we don't auth multiple times?
            tridentAPI = new InstanceAPI(registryConnection);
        }

        /// <summary>
        /// Authenticates the connector.
        /// </summary>
        private void AuthenticateConnector()
        {
            TridentAuthentication authendicate = new TridentAuthentication(this.registryConnection);
            authendicate.AuthenticateUser();
        }

        /// <summary>
        /// TODO: Throw the exception up one level...
        /// </summary>
        private void InitializeRegistry()
        {
            try
            {
                //NOTE HERE!!!
                //It is very important for the project folder to have the dlls taken directly
                //from the trident installation directory ("Program files\Scientific workflow\etc.dll
                //And yes... the project folder is MQuter-eLabApp.web's bin
                //right now i am referencing to another project's bin as it worked
                //previously. i will need to add the references to the project folder or change
                //the alternateproviderlocation to this (Tridentconnetor) proj folder

                //SR.ConnectionManager.AlternateProviderLocation = "C:\\Users\\Tendious\\Documents\\Visual Studio 2010\\Projects\\MQuter-eLaboratory\\MQuter-eLaboratory.Web\\bin\\";
                SR.ConnectionManager.AlternateProviderLocation = System.Web.HttpRuntime.BinDirectory; //provider; //System.Web.HttpRuntime.BinDirectory;

                connMgr = SR.ConnectionManager
                                    .CreateForAgent
                                        (SR.ConnectionManager.CancelBehavior.ThrowException,
                                         SR.ConnectionManager.Agent.WebService);

                registryConnection = connMgr.PickConnection(SR.ConnectionManagerBase.ConnectionUI.NeverShowUI);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        /// <summary>
        /// A temp method to distinguish between useful and non-useful 
        /// categories for the workflow application
        /// </summary>
        /// <returns></returns>
        private bool IsUsefulCategory(string catName)
        {
            //unless stated otherwise it is an essential category
            switch (catName.ToLower())
            {
                #region A list of useless categories to look out for.
                case "web service activities" :
                    return false;

                case "netcdf":
                    return false;

                case "database" :
                    return false;

                case "opendap" :
                    return false;

                case "chart" :
                    return false;

                case "windows workflow" :
                    return false;

                //Bio presentation take aways
                case "data":
                    return false;

                case "control flow":
                    return false;

                case "condition":
                    return false;

             
                #endregion
            }

            return true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Convert Namespace object to ActivityCategoryModel object.
        /// </summary>
        /// <param name="cat">Namespace object.</param>
        /// <returns></returns>
        private static DM.ActivityCategoryModel ConvertNamespacetoActivityCategory(SR.Namespace cat)
        {
            DM.ActivityCategoryModel catModel = new DM.ActivityCategoryModel(((SR.IObject)cat).ID.ToString());
            DM.ActivityCategoryModel catModelChild;

            catModel.Name = cat.Name;
            catModel.Description = cat.Description; //added to mke the desc appearing 

            catModel.Path = ((SR.IObject)cat).ID.ToString();

            catModel.IsEditable = cat.IsEditable;

            catModel.Id = ((SR.IObject)cat).ID;
            if (!String.IsNullOrEmpty(cat.Label))
            {
                catModel.Label = cat.Label;
            }
            else
            {
                catModel.Label = cat.Name;
            }

            if (cat.Children != null)
            {
                foreach (SR.Namespace catChild in cat.Children)
                {
                    catModelChild = ConvertNamespacetoActivityCategory(catChild);
                    catModelChild.Parent = catModel;
                    catModel.Categories.Add(catModelChild);
                }
            }

            return catModel;
        }

        /// <summary>
        /// Pretty straight forward method, it directly saves the model into
        /// trident's database.
        /// </summary>
        /// <param name="tridentWFModel">The trident model which we want to save.</param>
        public void CommitWorkflowToTridentDB(DM.TridentWorkflowModel tridentWFModel)
        {
            ActivityComposer activityComposer = new ActivityComposer(registryConnection, false);
            WorkflowComposer tridentComposer = new WorkflowComposer(registryConnection, activityComposer);
            tridentComposer.SaveWorkflowInRegistry(tridentWFModel, references, TridentAuthentication.LoggedUserInUserName);
        }

        /// <summary>
        /// Retrieves all the available activities from Trident database and then
        /// pack these activities into their respective categories. 
        /// A collection of category are returned when all is done.
        /// </summary>
        /// <returns></returns>
        public Collection<Category> GetALLActivities()
        {
            SR.Namespace activityRoot = this.RegistryService.GetActivityCategories();
            DM.ActivityCategoryModel catModel = ConvertNamespacetoActivityCategory(activityRoot);
          
            Collection<Category> resultSet = new Collection<Category>();


            foreach (DM.ActivityCategoryModel category in catModel.Categories)
            {
                #region If the category is useful to the discipline of bioinformatics, proceed.

                System.Diagnostics.Debug.WriteLine("=============================");
                System.Diagnostics.Debug.WriteLine("Category : " + category.Name);
                System.Diagnostics.Debug.WriteLine("=============================");

                if (!IsUsefulCategory(category.Name))
                {
                    continue;
                }

                #endregion

                //Copy only the information we are interested for our editor
                Category myCategory = new Category()
                {
                    Id = category.Id,
                    Name = category.Name,
                    Label = category.Label,
                    Description = category.Description,
                    Activities = new List<Activity>()
                };

                #region Here we take a deeper look into each category, for each activities within the category port over to our own data structure

                Collection<DM.ActivityMetadata> activities = CategoriesComposer.GetActivitiesFromCategory
                                                                (category.Id, this.registryConnection);

                if (activities != null)
                    foreach (DM.ActivityMetadata data in activities) //category.Activities.Add(data);
                    {
                        System.Diagnostics.Debug.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
                        System.Diagnostics.Debug.WriteLine("Activity Class --> " + data.ActivityClass);
                        System.Diagnostics.Debug.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXX");

                        Activity newActivity = new Activity(data.Id)
                        {
                            DisplayLabel = data.Name,
                            Description = data.Description,
                            ActivityClass = data.ActivityClass
                        };

                        myCategory.Activities.Add(newActivity);

                        //Check for special composition model
                        if (data.ActivityClass == "TridentBasicActivities.Activities.ForActivity")
                        {
                            newActivity.ActivityTypes = Activity.ActivityType.ForLoop;
                        }
                        else
                        {
                            newActivity.ActivityTypes = Activity.ActivityType.Standard;
                        }

                        foreach (DM.ParameterDescriptionModel param in data.InputParameters)
                        {
                            myCategory.Activities.Last()
                                .InputParam.Add(new Parameter(param.Name, true)
                                {
                                    DataType = param.DataType,
                                    Name = param.Name,
                                    Description = param.Description,
                                    Label = param.Label,
                                    Compulsary = param.IsMandatory
                                });
                        }

                        foreach (DM.ParameterDescriptionModel param in data.OutputParameters)
                        {
                            myCategory.Activities.Last()
                                .OutputParam.Add(new Parameter(param.Name, false)
                                {
                                    DataType = param.DataType,
                                    Name = param.Name,
                                    Description = param.Description,
                                    Label = param.Label
                                });
                        }
                    }
                resultSet.Add(myCategory);
                #endregion
            }

            return resultSet;
        }

    
        private Collection<DM.ActivityMetadata> currMetas = new Collection<DM.ActivityMetadata>();
        private Collection<String> suppActivityList = new Collection<string>();
        private Collection<FileReference> references = new Collection<FileReference>();

      

        
        private Collection<SR.Machine> GetExecutableMachines()
        {
             return SR.Machine.CommonSearches.GetOnlineMachines
                                                (this.registryConnection, false);
        }


        private SR.Activity GetActivity(string workflowName, Guid workflowId)
        {
            List<SR.Activity> requiredActivity = new List<SR.Activity>();
            SR.Activity.ISearch activitySearch = SR.Activity.ISearch.Create();
            SR.Activity.ISearch.ISearchClause searchCondition = SR.Activity.ISearch.Name(SR.StringField.Condition.Equals, workflowName);
            activitySearch.Query = searchCondition;
            requiredActivity = SR.Activity.Search(activitySearch, registryConnection);
            //SR.Activity actualActivity = null;
            foreach (SR.Activity activity in requiredActivity)
            {
                if ((activity as SR.IObject).ID == workflowId)
                {
                    return activity;
                }

            }

            return null;
        }

        



        private DM.ActivityMetadata RetrieveActivityByName(string className)
        {
            Collection<SR.Activity> activities = SR.Activity.CommonSearches.GetActivitiesByName(className, registryConnection);

            if (activities.Count == 1)
            {
                return new DM.ActivityMetadata(activities[0]);
            }
            else if
                (activities.Count > 1)

                throw new ArgumentException("Invalid activity as there are duplication.");

            return null;
        }



        #endregion
    }
}
