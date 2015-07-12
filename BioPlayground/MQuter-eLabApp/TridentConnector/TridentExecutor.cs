using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using TridentAPI;
using SR = Microsoft.Research.DataLayer;
using DM = Microsoft.Research.ScientificWorkflow.TridentModel;
using Util = Microsoft.Research.ScientificWorkflow.TridentUtilities;
using System.Net.Mail;

namespace TridentConnector
{
    public class TridentExecutor
    {
        /// <summary>
        /// The user
        /// </summary>
        private SR.User user;

        private InstanceAPI tridentApi;

        /// <summary>
        /// The registry connection.
        /// </summary>
        private SR.Connection registryConnection;

        public TridentExecutor(TridentManager manager)
        {
            this.registryConnection = manager.registryConnection;
            this.user = manager.User;

            //// Initializing the trident api object
            tridentApi = new InstanceAPI(registryConnection);
        }


        public Guid CreateJob(string name, string workflowName, Guid workflowId, string machineName)
        {
            try
            {
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(machineName) && !string.IsNullOrEmpty(workflowName))
                {
                    // Retrieving the activity
                    List<SR.Activity> requiredActivity = new List<SR.Activity>();
                    SR.Activity.ISearch activitySearch = SR.Activity.ISearch.Create();
                    SR.Activity.ISearch.ISearchClause searchCondition = SR.Activity.ISearch.Name(SR.StringField.Condition.Equals, workflowName);
                    activitySearch.Query = searchCondition;

                    requiredActivity = SR.Activity.Search(activitySearch, registryConnection);
                    SR.Activity actualActivity = null;

                    foreach (SR.Activity activity in requiredActivity)
                    {
                        if ((activity as SR.IObject).ID == workflowId)
                        {
                            actualActivity = activity;
                            break;
                        }

                    }

                    List<SR.Machine> requiredMachine = new List<SR.Machine>();
                    SR.Machine.ISearch machineSearch = SR.Machine.ISearch.Create();
                    SR.Machine.ISearch.ISearchClause machineCondition = SR.Machine.ISearch.Name(SR.StringField.Condition.Equals, machineName);
                    machineSearch.Query = machineCondition;
                    requiredMachine = SR.Machine.Search(machineSearch, registryConnection);
                    SR.Job jobInstance = tridentApi.CreateJob(name, requiredMachine[0], actualActivity);
                    return (jobInstance as SR.IObject).ID;
                    
                }
            }
            catch (Exception ex)
            {
                Util.TridentErrorHandler.HandleUIException(ex);
                throw ex;
            }

            throw new Exception("Failed to create job please check parameters (especially machine name).");
        }

        private SR.Job GetJob(Guid jobId, string jobName)
        {
            if (!string.IsNullOrEmpty(jobName))
            {
                List<SR.Job> requiredJobs = new List<SR.Job>();
                SR.Job.ISearch jobSearch = SR.Job.ISearch.Create();
                SR.Job.ISearch.ISearchClause jobCondition = SR.Job.ISearch.Name(SR.StringField.Condition.Equals, jobName);
                jobSearch.Query = jobCondition;
                requiredJobs = SR.Job.Search(jobSearch, registryConnection);
                SR.Job actualJob = null;

                // Getting the appropriate job
                foreach (SR.Job currentJob in requiredJobs)
                {
                    if ((currentJob as SR.IObject).ID == jobId)
                    {
                        actualJob = currentJob;
                    }
                }
                //actualJob.Status = SR.JobStatus.
                return actualJob;
            }

            throw new ArgumentException("Job name cannot be null.");
        }

        public string GetWorkflowExecutionStatus(Guid jobId, string jobName)
        {
            return GetJob(jobId, jobName).Status.ToString();
        }


        public Collection<string> GetWorkflowOutputs(Guid jobId, string jobName)
        {
            SR.Job jobInstance = GetJob(jobId, jobName);

            try
            {
                if (jobInstance != null)
                {
                    List<SR.DataProduct> myDataProducts = tridentApi.GetWorkflowOutputs(jobInstance);

                    Collection<string> BioWFResultML = new Collection<string>();
                    Collection<string> urls = new Collection<string>();

                    foreach (SR.DataProduct dp in myDataProducts)
                    {
                        if ((dp.ContentsAsString != null) &&
                                (dp.ContentsAsString.Contains("<BioWFML>")
                                    && dp.ContentsAsString.Contains("<ExecutionResult>"))
                                    && dp.ContentsAsString.Contains("</BioWFML>")
                                    && dp.ContentsAsString.Contains("</ExecutionResult>")
                                    && dp.ContentsAsString.Contains("</BioWFML>"))
                        {
                            BioWFResultML.Add(dp.ContentsAsString);
                            urls.Add(SaveELabDataProductToLocalMachine(dp.ContentsAsString));
                        }
                        //else
                          //  SaveDataProductOnToLocalMachine(dp);
                    }

                    if (BioWFResultML.Count > 0)
                        SendEmail(jobId, urls);

                    return BioWFResultML.Count > 0 ? BioWFResultML : null;
                }

                throw new KeyNotFoundException("Could not get the job for the provided Id.");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private string SaveELabDataProductToLocalMachine(string xmlContent)
        {
            //string url = System.Web.HttpRuntime.BinDirectory;
            string url = System.Web.HttpRuntime.AppDomainAppPath + "DataProduct\\";
            string fileName = (Guid.NewGuid()).ToString() + ".xml";

            TextWriter tw = new StreamWriter(url + fileName);
            tw.WriteLine(xmlContent);
            tw.Close();

            return fileName;
        }

        private void SaveDataProductOnToLocalMachine(SR.DataProduct product)
        {
            string url = System.Web.HttpRuntime.BinDirectory;
            string fileName = (Guid.NewGuid()).ToString();
            Stream stream = File.Open(url + fileName, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, product);
            stream.Close();
        }

        private void SendEmail(Guid jobId, Collection<String> URLs)
        {

            try
            {
                string serverPath = "http://bio.mquter.qut.edu.au/BioPlayground2010/DataProduct/";
                /*gmail sample*/
                MailMessage theMailMessage = new MailMessage("MQUTER-ELaboratory@QUT.Edu.Au", "yu.toh@connect.qut.edu.au"); //from, to address
                string message = "Dear user, " + Environment.NewLine +
                                        "your workflow execution was successful, you can retrieve its data product(s) at the following URL address" + Environment.NewLine +
                                        "Data Products:" + Environment.NewLine;


                StringBuilder sb = new StringBuilder(message);
                int i = 1;

                foreach (string url in URLs)
                {
                    sb.Append(i++ + ") " + serverPath + url + Environment.NewLine);
                    //sb.Append(i++ + ") " + url + Environment.NewLine);
                }

                sb.Append("Thank you for using Bioplayground workflow service.");

                //string.Format(message, sb.ToString());

                theMailMessage.Body = sb.ToString();

                //theMailMessage.Attachments.Add(new Attachment("pathToEmailAttachment"));
                theMailMessage.Subject = "Data product URLs for MQuter Workflow Id : " + jobId.ToString();

                SmtpClient theClient = new SmtpClient("smtp.gmail.com");
                theClient.UseDefaultCredentials = false;
                theClient.EnableSsl = true;
                theClient.Port = 587;
                System.Net.NetworkCredential theCredential = new System.Net.NetworkCredential("Tendious@Gmail.com", "iso92022");
                theClient.Credentials = theCredential;

                theClient.Send(theMailMessage);
            }
            catch (Exception eee)
            {
                string url = System.Web.HttpRuntime.AppDomainAppPath + "DataProduct\\";
                string fileName = "Error_"+ (Guid.NewGuid()).ToString() + ".xml";
                TextWriter tw = new StreamWriter(url + fileName);
                tw.WriteLine(eee.Message);
                tw.Close();
            }
        }
    }
}
