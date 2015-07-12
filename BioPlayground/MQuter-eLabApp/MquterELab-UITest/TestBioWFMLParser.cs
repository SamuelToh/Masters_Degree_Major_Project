using System;
using System.Net;
using System.Windows;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MQuter_eLabApp.Model;
using MQuter_eLabApp.ViewModel;
using MQuter_eLabApp.ViewModel.WorkflowModels;

using System.Xml;

namespace MquterELab_UITest
{
    [TestClass]
    public class TestBioWFMLParser
    {
        private ActivityModel StartNode = null;
        private ActivityModel Node1, Node2, Node3, Node4, Node5;
        private WorkflowManager workflowMang = null;

        [TestCleanup]
        public void CleanUp()
        {
            Node1 = null; Node2 = null; Node3 = null; Node4 = null; Node5 = null;
            StartNode = null;
        }

        [TestInitialize]
        public void Setup()
        {
            workflowMang = new WorkflowManager();
            StartNode = CreateActivityModel("StartNode_0", "StartNode", true);
            Node1 = CreateActivityModel("Node_1", "MBF.Workflow.FastaParserActivity", false);
            Node2 = CreateActivityModel("Node_2", "Microsoft.Research.ScientificWorkflow.Activities.WriteToConsole", false);
            Node3 = CreateActivityModel("Node_3", "Microsoft.Research.ScientificWorkflow.Activities.FileWriter", false);
            Node4 = CreateActivityModel("Node_4", "Microsoft.Research.ScientificWorkflow.Activities.DataProductReaderActivity", false);
            Node5 = CreateActivityModel("Node_5", "MBF.Workflow.DnaTranscriptionActivity", false);
        }

        private ActivityModel CreateActivityModel(string identifier, string className, bool isStart)
        {
            return new ActivityModel()
            {
                UniqueName = identifier,
                ActivityClass = className,
                IsStartFlow = isStart
            };
        }


        [TestMethod]
        public void TestParseDoubleSequential()
        {
            workflowMang.AddActivities("start_0", StartNode);
            workflowMang.AddActivities("sequential_1", Node1);
            workflowMang.AddActivities("sequential_2", Node2);

            workflowMang.AddActivityLink("start_0", "sequential_1");
            workflowMang.AddActivityLink("sequential_1", "sequential_2");

            string xmlContent = workflowMang.GetWorkflowXML();

            workflowsvc.WorkflowServiceClient service = new workflowsvc.WorkflowServiceClient();
            //service.SaveANDExecuteWFAsync(xmlContent);
        }


        [TestMethod]
        public void TestParseSequentialInParallel()
        {
            workflowMang.AddActivities("start_0", StartNode);
            workflowMang.AddActivities("sequential_1", Node1);
            workflowMang.AddActivities("sequential_2", Node2);

            workflowMang.AddActivityLink("start_0", "sequential_1");
            workflowMang.AddActivityLink("start_0", "sequential_2");

            string xmlContent = workflowMang.GetWorkflowXML();
            //System.Diagnostics.Debug.WriteLine(xmlContent);

            workflowsvc.WorkflowServiceClient service = new workflowsvc.WorkflowServiceClient();
            //service.SaveANDExecuteWFAsync(xmlContent);
        }

        /// <summary>
        /// Test Scenario: 3 
        /// double nested parallels in a parallel model.
        /// 
        /// Parallel Model A --> Parallel Model B + a Sequential 
        /// Parallel Model B --> another nested with a sequential model
        /// </summary>
        [TestMethod]
        public void TestParseNestedParallels()
        {
            workflowMang.AddActivities("start_0", StartNode);
            workflowMang.AddActivities("Item_1", Node1);
            workflowMang.AddActivities("Item_2", Node2);
            workflowMang.AddActivities("Item_3", Node3);
            workflowMang.AddActivities("Item_4", Node4);
            workflowMang.AddActivities("Item_5", Node5);

            //First parallel
            workflowMang.AddActivityLink("start_0", "Item_1");
            workflowMang.AddActivityLink("start_0", "Item_2");
            //Nested Parallel
            workflowMang.AddActivityLink("Item_2", "Item_3");
            workflowMang.AddActivityLink("Item_2", "Item_4");

            workflowMang.AddActivityLink("Item_1", "Item_5");

            string xmlContent = workflowMang.GetWorkflowXML();
            // System.Diagnostics.Debug.WriteLine(xmlContent);
            workflowsvc.WorkflowServiceClient service = new workflowsvc.WorkflowServiceClient();
            //service.SaveANDExecuteWFAsync(xmlContent);
        }


        [TestMethod]
        public void TestReadBasicParamValuesFrmXML()
        {
            ParamInputModel inputParam1 = new ParamInputModel()
            {
                Name = "InputFile",
                DataType = "System.String",
                ValueStr = "C:\\TestDocument.txt"
            };

            ParamInputModel inputParam2 = new ParamInputModel()
            {
                Name = "InputText",
                DataType = "System.String",
                ValueStr = "HelloWorld"
            };

            Node1.InputParam.Add(inputParam1);
            Node2.InputParam.Add(inputParam2);

            workflowMang.AddActivities("start_0", StartNode);
            workflowMang.AddActivities("Item_1", Node1);
            workflowMang.AddActivities("Item_2", Node2);

            //First parallel
            workflowMang.AddActivityLink("start_0", "Item_1");
            workflowMang.AddActivityLink("start_0", "Item_2");

            System.Diagnostics.Debug.WriteLine("Test Case - direct inputvalue for param");
            string xmlContent = workflowMang.GetWorkflowXML();
            System.Diagnostics.Debug.WriteLine(xmlContent);

            workflowsvc.WorkflowServiceClient service = new workflowsvc.WorkflowServiceClient();
            //service.SaveANDExecuteWFAsync(xmlContent);

        }

        [TestMethod]
        public void TestReadAdvParamBinderFrmXML()
        {
            ParamBinder binder = new ParamBinder("Node_1", "SequenceList");
            ParamInputModel inputParam1 = new ParamInputModel()
            {
                Name = "InputText",
                DataType = "System.String",
                Value = binder
            };

            Node2.InputParam.Add(inputParam1);

            workflowMang.AddActivities("start_0", StartNode);
            workflowMang.AddActivities("Item_1", Node1);
            workflowMang.AddActivities("Item_2", Node2);

            //double sequential activities 
            workflowMang.AddActivityLink("start_0", "Item_1");
            workflowMang.AddActivityLink("Item_1", "Item_2");

            string xmlContent = workflowMang.GetWorkflowXML();

            System.Diagnostics.Debug.WriteLine("Test Case - Adv linkage param value");
            System.Diagnostics.Debug.WriteLine(xmlContent);

            workflowsvc.WorkflowServiceClient service = new workflowsvc.WorkflowServiceClient();
            //service.SaveANDExecuteWFAsync(xmlContent);

        }

        [TestMethod]
        public void TestFinalizeProduct()
        {
            ParamOutputModel outputParam1 = new ParamOutputModel()
            {
                Name = "SequenceList",
                DataType = "System.Collection.Generic.IList<Sequence>",
                AsDataOutput = true
            };

            Node1.OutboundParam.Add(outputParam1);

            workflowMang.AddActivities("start_0", StartNode);
            workflowMang.AddActivities("Item_1", Node1);
            workflowMang.AddActivities("Item_2", Node2);

            //double sequential activities 
            workflowMang.AddActivityLink("start_0", "Item_1");
            workflowMang.AddActivityLink("Item_1", "Item_2");

            string xmlContent = workflowMang.GetWorkflowXML();

            
            System.Diagnostics.Debug.WriteLine("Test highlighting output param as data product in trident");
            System.Diagnostics.Debug.WriteLine(xmlContent);

            workflowsvc.WorkflowServiceClient service = new workflowsvc.WorkflowServiceClient();
            //service.SaveANDExecuteWFAsync(xmlContent);
        }

    }
}
