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
using System.ComponentModel; //For background worker 
using BioPatMLEditor.BioPatMLDataRepo;
using System.Collections.Generic;

namespace BioPatMLEditor.PatternControls
{
    /// <summary>
    /// A point of note this class is different from Pattern Repository Retriver
    /// This class is only called when we first starts the application, what it does
    /// is to call a background worker to load in all the available patterns
    /// from our repository and display their pattern name details on our treeview.
    /// </summary>
    public partial class PatternRepository
    {
        /// <summary>
        /// Our worker for performing Async call to our BioPatML Data repo service
        /// </summary>
        private BackgroundWorker bw = new BackgroundWorker();

        /// <summary>
        /// This method is invoked by the UI when there is a need to populate the 
        /// "available pattern treeview"
        /// </summary>
        public void GetRepositoryStructure()
        {
            //Initialisation
            txtLoading.Visibility = System.Windows.Visibility.Visible;
            BlinkingAction.RepeatBehavior = RepeatBehavior.Forever;
            BlinkingAction.Begin();
            
            //Background worker do work method
            bw.DoWork += delegate(object s, DoWorkEventArgs args)
            {
                bool done = false;
                List<DefinitionPatternInfo> resultTray = new List<DefinitionPatternInfo>();
                BioPatMLDataRepo.BioPatMLDataServiceClient repository = new BioPatMLDataServiceClient();
                repository.GetALLDefPatternInfoCompleted += delegate
                    (object sender, GetALLDefPatternInfoCompletedEventArgs e)
                {
                    //Do something
                    resultTray = e.Result as List<DefinitionPatternInfo>;
                    done = true;
                };

                repository.GetALLDefPatternInfoAsync();

                while (!done) { System.Threading.Thread.Sleep(2000); };

                repository.CloseAsync(); //close the service
                repository = null; //clean up resources
                args.Result = resultTray;

            };

            bw.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
            {
                patterns = args.Result as List<DefinitionPatternInfo>;
                txtLoading.Visibility = System.Windows.Visibility.Collapsed;
                cpiedUnits = RepositoryUnit.CreateRepositoryData(patterns).Copy();
                ConstructTreeView(RepositoryUnit.CreateRepositoryData(patterns));
                BlinkingAction.Stop();
            };

            bw.RunWorkerAsync();
            
        }

        /// <summary>
        /// Rebuilds the repository tree view structure
        /// </summary>
        /// <param name="dataNode">/test/</param>
        public void ConstructTreeView(RepositoryUnit dataNode)
        {
            TreeOfLife.Items.Clear();
            FillTree(TreeOfLife.Items, dataNode);
        }

    }
}
