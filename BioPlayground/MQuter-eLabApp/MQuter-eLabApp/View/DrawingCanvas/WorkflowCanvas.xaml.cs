using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using MQuter_eLabApp.Model;
using MQuter_eLabApp.Events;
using MQuter_eLabApp.ViewModel.BioWFMLResultModel;
using MQuter_eLabApp.View.Panels;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel; //For background worker 

namespace MQuter_eLabApp.View.DrawingCanvas
{
    public partial class WorkflowCanvas : UserControl
    {
        private WorkflowManager wfManager = new WorkflowManager();
        private AnnotationManager annManager;
        //private bool isAnnotating = false;

        public delegate void HasResultEventHandler(object sender, EventArgs e);

        public event HasResultEventHandler HasResultSet;

        // Invoke the Changed event; called whenever activities on canvas changes:
        protected virtual void OnHasNewResults(EventArgs e)
        {
            if (HasResultSet != null)
                HasResultSet(this, e);
        }

        public WorkflowCanvas()
        {
            InitializeComponent();
            annManager = new AnnotationManager(WorkflowContainer);
            Loaded += new RoutedEventHandler(WorkflowCanvas_Loaded);
            //testbtn.Visibility = System.Windows.Visibility.Collapsed;
        }

        void WorkflowCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            WorkflowDropManager.ActivityChanged 
                += new WorkflowController.ActivityChangedEventHandler
                    (ActivitiesChanged);

            WorkflowDropManager.ConnectionChanged
                += new WorkflowController.ActivityLinkageEventHandler
                    (ConnectionChanged);

            WorkflowDropManager.ActivityParentChanged
                += new WorkflowController.ActivityNestingEventHandler
                    (ActivityParentChanged);
        }


        public void TriggerSelectionState()
        {
            annManager.IsSelectingAnnotation = true;
        }

        public void DrawAnnotation(AnnotationEventArgs args)
        {
            annManager.AnnotationSubject = args.AnnotationContainer; //set subject for annotation
        }

        private void ActivityParentChanged(object sender, EventArgs e)
        {
            NestedActivityEventArgs nestedInfo = e as NestedActivityEventArgs;

            wfManager.ChangeActivityParent
                (nestedInfo.NewParentIdentifier, nestedInfo.NestedEleIdentifier);
        }

        // This will be called whenever there are changes to the activities:
        private void ActivitiesChanged(object sender, EventArgs e)
        {
            ActivityEventArgs args = e as ActivityEventArgs;

            try
            {
                if (args.IsAddition)
                
                    wfManager.AddActivities(args.Identifier, args.DataModel);
                
                else
                    wfManager.RemoveActivities(args.Identifier);

            }
            catch (Exception exception)
            {
                ReportException(exception);
            }
        }

        // invoked whenever a connection line drawn successfully
        private void ConnectionChanged(object sender, EventArgs e)
        {
            ConnectionEventArgs args = e as ConnectionEventArgs;

            try
            {
                if (args.IsAdding)
                    wfManager.AddActivityLink
                                (args.ParentName, args.ChildName);
                else
                    wfManager.RemoveActivityLink
                                (args.ParentName, args.ChildName);
            }
            catch (Exception exception)
            {
                ReportException(exception);
            }
        }

        private void ReportException(Exception e)
        {
            string errorMsg = e.InnerException != null ? e.InnerException.Message : e.Message;

            ExceptionPanel exPanel = new ExceptionPanel();
            exPanel.MessageBox.Text = errorMsg;
            exPanel.Show();
            //MessageBox.Show(errorMsg, e.GetType().Name, MessageBoxButton.OK);
        }

        private void WorkflowContainer_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas item = sender as Canvas;
            Point  myLocation = e.GetPosition(item);
            WorkflowDropManager.MseLocation = myLocation; //Update the mouse location

            if(annManager.AnnotationSubject != null) 
                annManager.MseLocation = myLocation; //update the annotation manager too
        }

        private void WorkflowContainer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas item = sender as Canvas;
            Point myLocation = e.GetPosition(item);

            if (annManager.AnnotationSubject != null 
                    && annManager.IsSelectingAnnotation == false) //if is annotating
            {
                annManager.IsAnnotating = true; //wake the manager up
            }
            /*else if //Selecting a potential annotation object, try check for potential obj
                (annManager.IsSelectingAnnotation)
            {
                annManager.MseLocation = myLocation; //prepare update coordinate if neccessary
            }*/
            annManager.startLocation = myLocation;
        }

        private void WorkflowContainer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (annManager.IsAnnotating) //if its annotating then release it
            {
                annManager.IsAnnotating = false;
                annManager.FinalizeAnnotationState();
                annManager.AnnotationSubject = null;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
           MessageBox.Show(wfManager.GetWorkflowXML());
        }

        private void WorkflowContainer_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("right clicked");
        }

        private MQuter_eLabApp.View.Panels.JobRunner runnerPanel;

        private void MenuItem1_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menu = sender as MenuItem;

            if (menu != null)
            {
                switch (menu.Header.ToString())
                {
                    case "Run Workflow":
                        {
                            try
                            {
                                if (CheckHasWorkflow())
                                {
                                    runnerPanel = new Panels.JobRunner();
                                    runnerPanel.Show();

                                    tridentService = new TridentEmulatorSvc.WorkflowServiceClient();
                                    bw = new BackgroundWorker();
                                    bw.DoWork += delegate(object s, DoWorkEventArgs args)
                                    {
                                        Dispatcher pbDispatcher = runnerPanel.StatusTxt.Dispatcher;
                                        UpdateStatusBarProgressTextDelegate update = new UpdateStatusBarProgressTextDelegate(UpdateStatusBarProgressText);
                                        

                                        Object[] arguments = args.Argument as Object[];
                                        string xmlContent = arguments[0] as string;
                                        bool done = false; // a flag to determine the state of the search process
                                        Guid jobId = new Guid();
                                        Guid workflowId = new Guid();

                                        tridentService.SaveWorkFlowCompleted += delegate (object source, TridentEmulatorSvc.SaveWorkFlowCompletedEventArgs e2)
                                        {
                                            
                                            workflowId = e2.Result;
                                            pbDispatcher.BeginInvoke(update, "Workflow Saved!");
                                            done = true;
                                        };

                                        pbDispatcher.BeginInvoke(update, "Saving Workflow...");
                                        tridentService.SaveWorkFlowAsync(xmlContent);

                                        while (!done) { System.Threading.Thread.Sleep(1000); }

                                        done = false;

                                        tridentService.ExecuteWorkflowCompleted += delegate(object sender2, TridentEmulatorSvc.ExecuteWorkflowCompletedEventArgs e3)
                                        {
                                            jobId = e3.Result;
                                            done = true;
                                        };

                                        pbDispatcher.BeginInvoke(update, "Queuing job " + workflowId + "");
                                        tridentService.ExecuteWorkflowAsync("BioPlaygroundWF", workflowId);

                                        bool isCheckingStatus = false;
                                        string jobStatus = String.Empty;
                                        while (!done) { System.Threading.Thread.Sleep(1000); }

                                        done = false;

                                        tridentService.GetJobStatusCompleted += delegate(object sender3, TridentEmulatorSvc.GetJobStatusCompletedEventArgs e4)
                                        {
                                            jobStatus = e4.Result;
                                            pbDispatcher.BeginInvoke(update, jobStatus + "...");

                                            System.Diagnostics.Debug.WriteLine("Job Status : > " + jobStatus);
                                            if (jobStatus.ToLower().Equals("completed"))
                                            {   
                                                done = true;
                                            }
                                            else if (jobStatus.ToLower().Equals("aborted"))
                                            {
                                                //this.ReportException(new Exception("Task run fail!"));
                                                done = true;
                                            }
                                            isCheckingStatus = false;
                                         };


                                        while (!done) 
                                        {
                                            System.Threading.Thread.Sleep(2000);
                                            if (!isCheckingStatus)
                                            {
                                                if (done && jobStatus == "completed" )
                                                { 
                                                    pbDispatcher.BeginInvoke(update, "Job execution success!"); 
                                                    System.Threading.Thread.Sleep(2000);
                                                    break;
                                                } 

                                                else
                                                    if
                                                (done && jobStatus == "aborted")
                                                    {
                                                        pbDispatcher.BeginInvoke(update, "Job execution fail, please check your workflow or parameters.");
                                                        System.Threading.Thread.Sleep(2000);
                                                        break;
                                                    }
                                                
                                                tridentService.GetJobStatusAsync("BioPlaygroundWF", jobId);
                                                isCheckingStatus = true;
                                            }
                                        }

                                       
                                        if (jobStatus == "completed") 
                                        {
                                            pbDispatcher.BeginInvoke(update, "Scanning for data output.");
                                            done = false; 
                                        }

                                        System.Threading.Thread.Sleep(1500);
  
                                        List<string> BioWFResultML = new List<string>();
                                        BioWFResultML.Add(jobStatus); 
                                       
                                        tridentService.GetJobOuputCompleted += delegate(object sender4, TridentEmulatorSvc.GetJobOuputCompletedEventArgs e5)
                                        {
                                            if (e5.Result != null)
                                            {
                                                pbDispatcher.BeginInvoke(update, "downloading data output...");
                                                BioWFResultML = e5.Result.ToList();
                                                args.Result = BioWFResultML[0]; //for now...
                                            }
                                            else
                                            {
                                                pbDispatcher.BeginInvoke(update, "No data output...");
                                            }

                                            System.Threading.Thread.Sleep(2500);
                                            done = true;
                                        };

                                        if (jobStatus.CompareTo("Completed") == 0)
                                        {
                                            done = false;
                                            tridentService.GetJobOuputAsync("BioPlaygroundWF", jobId);
                                        }
                                        else
                                            args.Result = BioWFResultML[0]; //try

                                        while (!done) { System.Threading.Thread.Sleep(1000); }
                                
                                    };

                                    Object[] parameters = new Object[1];
                                    parameters[0] = wfManager.GetWorkflowXML();

                                    
                                    try
                                    {
                                        bw.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
                                        {
                                            string resultString = args.Result as string;
                                            if (resultString.CompareTo("Aborted") == 0)
                                                ReportException(new Exception("Task Run Fail, please check your workflow or parameters."));
                                            else
                                            {
                                                //Inform the parent about the task completion
                                                runnerPanel.CloseMe();
                                                BioWFMLResultManager resultComponent = new BioWFMLResultManager();
                                                OnHasNewResults(new ResultSetEventArgs(resultComponent.ReadBioWFML(args.Result as string)));
                                            }
                                        };
                                        //Run the bot
                                        bw.RunWorkerAsync(parameters);
                                    }
                                    catch (Exception eee)
                                    {
                                        ReportException(eee);
                                        runnerPanel.CloseMe();
                                    }
  
                                }
                                else
                                {
                                    MessageBox.Show("No workflow found!");
                                }
                            }
                            catch (Exception ex)
                            {
                                ReportException(ex);
                                runnerPanel.CloseMe();
                            }
                            
                            break;
                        }

                    case "Save Workflow":
                        {
                            try
                            {
                                //MessageBox.Show("Ooops not supported yet, perhaps try running it directly?");
                                MessageBox.Show(wfManager.GetWorkflowXML());
                                
                            }
                            catch (Exception ee)
                            {
                                ReportException(ee);
                            }

                            break;
                        }
                }


            }
        }

        /// <summary>
        /// Delegate used for updating the UI
        /// </summary>
        /// <param name="value"></param>
        public delegate void UpdateStatusBarProgressTextDelegate(string value);

        /// <summary>
        /// This is the method that the deleagte will execute
        /// </summary>
        /// <param name="message">integer value of how much was done.</param>
        public void UpdateStatusBarProgressText(string percentDone)
        {
            //Debug.WriteLine(percentDone);
            if(runnerPanel != null)
                runnerPanel.StatusTxt.Text = percentDone;
        }


        private TridentEmulatorSvc.WorkflowServiceClient tridentService = null;
 

        /// <summary>
        /// We always only use this instance for search.
        /// </summary>
        private BackgroundWorker bw { get; set; }


        
        /// <summary>
        /// A simple check for now
        /// </summary>
        /// <returns></returns>
        private bool CheckHasWorkflow()
        {
            return wfManager.GetWorkflowXML().Length > 0 ? true : false;
        }
    }
}
