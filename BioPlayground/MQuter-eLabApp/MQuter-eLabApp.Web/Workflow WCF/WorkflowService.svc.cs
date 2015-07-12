using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.ObjectModel;
using TridentConnector;
using TridentConnector.DataContracts;

namespace MQuter_eLabApp.Web.Workflow_WCF
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WorkflowService
    {
        /// <summary>
        /// The manager handles all the essential operations needed for communication 
        /// between the service and Trident Database.
        /// </summary>
        private TridentManager manager = new TridentManager();

       
        [OperationContract]
        public void DoWork()
        {
            // Add your operation implementation here
            return;
        }

        /// <summary>
        /// Returns all the available Activities in our Trident Database
        /// </summary>
        [OperationContract]
        public Collection<Category> RetrieveActivities()
        {
            return manager.GetALLActivities();
        }

        [OperationContract]
        public Guid SaveWorkFlow(string xmlcontent)
        {
            //TridentConnector.BioWFMLParser parser = new BioWFMLParser(manager.
            BioWFMLParser parser = new BioWFMLParser();
            return parser.ParseWorkflowLanguage(xmlcontent);
        }

        [OperationContract]
        public Guid ExecuteWorkflow(string workflowName, Guid workflowId)
        {
            TridentExecutor executor = new TridentExecutor(manager);
            return executor.CreateJob("Bioplayground2010", workflowName, workflowId, "FITKGBL03");
            //return executor.CreateJob("Bioplayground2010", workflowName, workflowId, "Tendious-PC");
        }

        [OperationContract]
        public string HelloWorld(string workflowName, Guid workflowId)
        {
            TridentExecutor executor = new TridentExecutor(manager);
            return "echo";
        }

        [OperationContract]
        public string GetJobStatus(string jobName, Guid jobId)
        {
            TridentExecutor executor = new TridentExecutor(manager);
            return executor.GetWorkflowExecutionStatus(jobId, "Bioplayground2010");
        }

        [OperationContract]
        public Collection<string> GetJobOuput(string jobName, Guid jobId)
        {
            TridentExecutor executor = new TridentExecutor(manager);
            return executor.GetWorkflowOutputs(jobId, "Bioplayground2010");
        }

    }
}
