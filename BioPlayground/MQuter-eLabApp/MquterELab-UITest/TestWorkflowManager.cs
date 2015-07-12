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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MQuter_eLabApp.Model;
using MQuter_eLabApp.ViewModel;
using MQuter_eLabApp.ViewModel.WorkflowModels;

namespace MquterELab_UITest
{
    [TestClass]
    public class TestWorkflowManager
    {
        private WorkflowManager workflowMang = new WorkflowManager();
        private ActivityModel StartNode = null;
        private ActivityModel Node1, Node2, Node3, Node4, Node5;

        [TestCleanup]
        public void CleanUp()
        {
            Node1 = null; Node2 = null; Node3 = null; Node4 = null; Node5 = null;
            StartNode = null;
        }

        [TestInitialize]
        public void Setup()
        {
            StartNode = CreateActivityModel("StartNode", true);
            Node1 = CreateActivityModel("MBF.Workflow.FastaParserActivity", false);
            Node2 = CreateActivityModel("Microsoft.Research.ScientificWorkflow.Activities.WriteToConsole", false);
            Node3 = CreateActivityModel("Microsoft.Research.ScientificWorkflow.Activities.FileWriter", false);
            Node4 = CreateActivityModel("Microsoft.Research.ScientificWorkflow.Activities.DataProductReaderActivity", false);
            Node5 = CreateActivityModel("MBF.Workflow.DnaTranscriptionActivity", false);
        }

        private ActivityModel CreateActivityModel(string className, bool isStart)
        {
            return new ActivityModel()
            {
                ActivityClass = className,
                IsStartFlow = isStart
            };
        }
        
        [TestMethod]
        public void TestAddActivities()
        {
            workflowMang.AddActivities("dummyActivity_1", Node1);
            workflowMang.AddActivities("dummyActivity_2", Node2);
            Assert.AreEqual(workflowMang.CountActivitiesInMemory, 2);
        }

        [TestMethod]
        public void TestRemoveActivity()
        {
            workflowMang.RemoveActivities("dummyActivity_2");
            Assert.AreEqual(workflowMang.CountActivitiesInMemory, 1);

            workflowMang.RemoveActivities("dummyActivity_1");
            Assert.AreEqual(workflowMang.CountActivitiesInMemory, 0);
        }

        [TestMethod]
        public void TestAddActivityLinks()
        {
            WorkflowManager workflowMang = new WorkflowManager();

            Guid MyGuidId = new Guid();
            Guid MyGuidId2 = new Guid();

            MyGuidId = Guid.NewGuid();
            MyGuidId2 = Guid.NewGuid();

            ActivityModel dummyModel1 = new ActivityModel()
            {
                ActivityClass = "DummyNode",
                Description = "For testing",
                DisplayLabel = "noLabel",
                Id = MyGuidId
            };

            ActivityModel dummyModel2 = new ActivityModel()
            {
                ActivityClass = "DummyNode2",
                Description = "For testing2",
                DisplayLabel = "noLabel2",
                Id = MyGuidId2
            };

            workflowMang.AddActivities("dummyActivity_1", dummyModel1);
            workflowMang.AddActivities("dummyActivity_2", dummyModel2);

            workflowMang.AddActivityLink("dummyActivity_1", "dummyActivity_2");

            StandardFlowModel source = workflowMang.activityDict["dummyActivity_1"];
            StandardFlowModel target = workflowMang.activityDict["dummyActivity_2"];

            Assert.AreEqual(source.Children.Count, 1);
            Assert.AreEqual(source.Children[0], target);
        }

        [TestMethod]
        public void TestGetWorkflowXML()
        {
            //A complex test scenario
            //here we build a workflow that has 
            //A Start Node connected to a sequential activity
            //Then branches out to triplet activities
            //And ends with an ending node

            WorkflowManager workflowMang = new WorkflowManager();
            Guid MyGuidId = new Guid();
            MyGuidId = Guid.NewGuid();

            ActivityModel StartNode = new ActivityModel()
            {
                ActivityClass = "StartNode",
                Id = MyGuidId,
                IsStartFlow = true
            };

            Guid MyGuidId2 = new Guid();
            MyGuidId2 = Guid.NewGuid();

            ActivityModel SequentialNode1 = new ActivityModel()
            {
                ActivityClass = "sequentialNode1",
                Id = MyGuidId2
            };

            Guid MyGuidId3 = new Guid();
            MyGuidId3 = Guid.NewGuid();

            ActivityModel TripletNode1 = new ActivityModel()
            {
                ActivityClass = "tripletNode1",
                Id = MyGuidId3
            };

            Guid MyGuidId4 = new Guid();
            MyGuidId4 = Guid.NewGuid();

            ActivityModel TripletNode2 = new ActivityModel()
            {
                ActivityClass = "tripletNode2",
                Id = MyGuidId4
            };

            Guid MyGuidId5 = new Guid();
            MyGuidId5 = Guid.NewGuid();

            ActivityModel TripletNode3 = new ActivityModel()
            {
                ActivityClass = "tripletNode3",
                Id = MyGuidId5
            };

            Guid MyGuidId6 = new Guid();
            MyGuidId6 = Guid.NewGuid();

            ActivityModel OutputWindow = new ActivityModel()
            {
                ActivityClass = "outputWindow",
                Id = MyGuidId6
            };

            workflowMang.AddActivities("start_1", StartNode);
            workflowMang.AddActivities("sequential_2", SequentialNode1);
            workflowMang.AddActivities("triplet_3", TripletNode1);
            workflowMang.AddActivities("triplet_4", TripletNode2);
            workflowMang.AddActivities("triplet_5", TripletNode3);
            workflowMang.AddActivities("outputWind_6", OutputWindow);

            workflowMang.AddActivityLink("start_1", "sequential_2");
            workflowMang.AddActivityLink("sequential_2", "triplet_3");
            workflowMang.AddActivityLink("sequential_2", "triplet_4");
            workflowMang.AddActivityLink("sequential_2", "triplet_5");
            workflowMang.AddActivityLink("triplet_3", "outputWind_6");

            //XmlReader reader = XmlReader.Create(new System.IO.Stream(workflowMang.GetWorkflowXML()));
            System.Diagnostics.Debug.WriteLine("====================================");
            //System.Diagnostics.Debug.WriteLine(workflowMang.GetWorkflowXML());
        }

      
    }
}
