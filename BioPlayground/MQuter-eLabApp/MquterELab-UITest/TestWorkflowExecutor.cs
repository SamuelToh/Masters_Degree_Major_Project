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
using MQuter_eLabApp.Model;
using MQuter_eLabApp.ViewModel;
using MQuter_eLabApp.ViewModel.WorkflowModels;
using System.Collections.ObjectModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace MquterELab_UITest
{
    [TestClass]
    public class TestWorkflowExecutor
    {
        private ActivityModel StartNode = null;
        private ActivityModel Node_GenbankParser, Node_BioPatMLParser, Node_BioPatSeeker, Node_HitsViewer; 

        [TestCleanup]
        public void CleanUp()
        {
            Node_GenbankParser = null;
            Node_BioPatMLParser = null;
            Node_BioPatSeeker = null; 
            Node_HitsViewer = null;
            StartNode = null;
        }

        [TestInitialize]
        public void Setup()
        {
            StartNode = CreateActivityModel("StartNode_0","StartNode", true);
            Node_GenbankParser = CreateActivityModel("GenbankParser_1", "MBF.Workflow.GenBankParserActivity", false);
            Node_BioPatMLParser = CreateActivityModel("BioPatMLParser_2", "BioPatMLActivities.BioPatMLParser", false);
            Node_BioPatSeeker = CreateActivityModel("BioPatSeeker_3", "BioPatMLActivities.BioPatSeeker", false);
            Node_HitsViewer = CreateActivityModel("HitsViewer_4", "BioPatMLActivities.HitsViewer", false);
        }

        private ActivityModel CreateActivityModel
                                (string identifier, string className, bool isStart)
        {
            return new ActivityModel()
            {
                UniqueName = identifier,
                ActivityClass = className,
                IsStartFlow = isStart
            };
        }

        [TestMethod]
        public void TestSimpleExecution()
        {
            //string xmlContent = 
               // "<?xml version='1.0' encoding='utf-16'?>    <BioWFML>      <Workflow name='eLabDemoWorkflow-godbless' description='Auto generated wf'>        <BeginFlow>          <FlowElement level='0'>            <Sequential>              <Activity clsname='Microsoft.Research.ScientificWorkflow.Activities.WriteToConsole' id='WriteToConsole_0'>                <InputValue name='InputText' value='helloWorld' />              </Activity>            </Sequential>          </FlowElement>        </BeginFlow>      </Workflow>    </BioWFML>";

            workflowsvc.WorkflowServiceClient service = new workflowsvc.WorkflowServiceClient();
            //service.SaveANDExecuteWFAsync(xmlContent);
        }

        /// <summary>
        /// BioPatML Activity #1
        /// </summary>
        [TestMethod]
        public void TestExecuteWorkflow()
        {
            string xmlContent =
                "<?xml version='1.0' encoding='utf-16'?> " +
                "    <BioWFML> " +
                "      <Workflow name='TestBioPatMLSample1' description='Auto generated wf'> " +
                "        <BeginFlow> " +
                "          <FlowElement level='0'> " +
                "            <Parallel> " +
                "              <Activity clsname='BioPatMLActivities.BioPatMLParser' id='BioPatMLParser_3'> " +
                "                <InputValue name='BioPatFilePath' value='D:\\TestData\\BioPatML\\Motif.xml' /> " +
                "              </Activity> " +
                "              <Activity clsname='MBF.Workflow.GenBankParserActivity' id='GenBankParserActivity_4'> " +
                "                <InputValue name='InputFile' value='D:\\TestData\\GenBank\\AE001582.gbk' /> " +
                "              </Activity> " +
                "            </Parallel> " +
                "          </FlowElement> " +
                "          <FlowElement level='1'> " +
                "            <Sequential> " +
                "              <Activity clsname='BioPatMLActivities.BioPatSeeker' id='BioPatSeeker_1'> " +
                "                <InputValue name='ListMBFSequence' linkFrom='GenBankParserActivity_4.SequenceList' /> " +
                "                <InputValue name='PatternModel' linkFrom='BioPatMLParser_3.PatternDefinition' /> " +
                "              </Activity> " +
                "            </Sequential> " +
                "          </FlowElement> " +
                "          <FlowElement level='2'> " +
                "            <Sequential> " +
                "              <Activity clsname='BioPatMLActivities.HitsViewer' id='HitsViewer_2'> " +
                "                <InputValue name='ListHits' linkFrom='BioPatSeeker_1.Hits' /> " +
                "                <InputValue name='Index' value='1' /> " +
                "                <OutputValue name='MatchDetails' dataValue='true' /> " +
                "              </Activity> " +
                "            </Sequential> " +
                "          </FlowElement> " +
                "       </BeginFlow> " +
                "      </Workflow> " +
                "    </BioWFML> ";

            workflowsvc.WorkflowServiceClient service = new workflowsvc.WorkflowServiceClient();
            //service.SaveANDExecuteWFAsync(xmlContent);
            
        }

        
    }
}
