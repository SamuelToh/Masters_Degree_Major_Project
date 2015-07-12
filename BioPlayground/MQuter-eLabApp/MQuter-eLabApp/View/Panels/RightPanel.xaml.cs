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
using MQuter_eLabApp.ViewModel;
using System.Collections.ObjectModel;

namespace MQuter_eLabApp.View.Panels
{
    /// <summary>
    /// this class should go to the view section
    /// </summary>
    public partial class RightPanel : UserControl
    {
        private CategoryVisualData viewModel = new CategoryVisualData();

        private bool FlagFlipToFront { get; set; }

        private bool FlagDisplayCategory { get; set; }

        #region Constructors

        public RightPanel()
        {
            InitializeComponent();
            RunTridentFrActivity();
            FlagFlipToFront = true;
            FlagDisplayCategory = true;
            this.Loaded += new RoutedEventHandler(RightPanel_Loaded);
        }

        void RightPanel_Loaded(object sender, RoutedEventArgs e)
        {
            OnLoad.RepeatBehavior = RepeatBehavior.Forever;
            OnLoad.Begin();
        }

        #endregion Constructors

        private void RunTridentFrActivity()
        {
            TridentEmulatorSvc.WorkflowServiceClient trident = new TridentEmulatorSvc.WorkflowServiceClient();
            trident.RetrieveActivitiesCompleted += new EventHandler<TridentEmulatorSvc.RetrieveActivitiesCompletedEventArgs>(trident_RetrieveActivitiesCompleted);
            trident.RetrieveActivitiesAsync();      
        }

        private void Build3DCategoryPane()
        {
            this._3DFlowPanel.DataContext = new _3DActivityVisualizationData(viewModel.Items);
            this._3DFlowPanel.SectionChanged += new SStuff.FlowControls.FlowItemsControlBase.SelectedItemChangedEventHandler(_3DFlowPanel_SectionChanged);
        }

        void _3DFlowPanel_SectionChanged(int selectedItemIndex, EventArgs e)
        {
            listActivity.DataContext = viewModel.Items[selectedItemIndex];
            lblHeader.Content = viewModel.Items[selectedItemIndex].Name;
            DisplayPanelContent(false);
            AnimateFlipPanel(FlagFlipToFront);
        }

        private CategoryModel BuildToolkitCat()
        {
            List<ActivityModel> toolkitActivities = new List<ActivityModel>();

            toolkitActivities.Add(new ActivityModel()
            {
                ActivityClass = "WorkflowStartClass",
                ActivityClassification = ActivityModel.ActivityType.Standard,
                DisplayLabel = "Start",
                IsStartFlow = true,
                Description = "Marks the beginning of a workflow"
            });

            toolkitActivities.Add(new ActivityModel()
            {
                ActivityClass = "EndClass",
                ActivityClassification = ActivityModel.ActivityType.Standard,
                DisplayLabel = "End",
                IsEndFlow = true,
                Description = "All workflow ends here. Note: all workflows must have a start and an End."
            });

            toolkitActivities.Add(new ActivityModel()
            {
                ActivityClass = "DataOuputWindowClass",
                ActivityClassification = ActivityModel.ActivityType.Standard,
                DisplayLabel = "OutputWindow",
                Description = "Construct your own dynamic output window here."
            });

            CategoryModel model = new CategoryModel()
            {
                ImgSource = "/MQuter-eLabApp;component/Resources/Images/MainLogo.png",
                Name = "Workflow Toolkit",
                Label = "Workflow Toolkit",
                Activities = toolkitActivities,
                CatDescription = "This category has all the essential tools for workflow construction."
            };

            return model;
        }

        private void BuildCategoryPane()
        {
            //here we build the self defined category [Workflow Toolkit] that has Start & End
            viewModel.Items.Add(BuildToolkitCat());

            for (int i = viewModel.Items.Count - 1; i >= 0; i--)
            {
                CategoryModel model = viewModel.Items[i];

                Button btn = new Button()
                {
                    Name = i.ToString(),
                    Width = 270,
                    Height = 75.0,
                    Content = model.Name,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.Orange),
                    Style = Application.Current.Resources["CategoryBtnStyle"] as Style,
                    DataContext = model,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left
                };

                btn.Click += new RoutedEventHandler(btn_Click);
                this.FrntSPActivityCat.Children.Add(btn);
            }

            DisplayPanelContent(true);
        }

      
        private void AnimateFlipPanel(bool toFront)
        {
            if (toFront)
            {
                flipToFront.AutoReverse = false;
                flipToFront.Begin();
                FlagFlipToFront = false; //prepare next round
            }

            else
            {
                flipToBack.AutoReverse = false;
                flipToBack.Begin();
                FlagFlipToFront = true; //prepare next round
                
            }
        }

        private void DisplayPanelContent(bool showCategory)
        {
            if (showCategory)
            {
                btnBack.Visibility = Visibility.Collapsed;
                btnActCat2.Visibility = Visibility.Visible;
                FrntSPActivityCat.Visibility = Visibility.Visible;
                BckSPActivities.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                btnBack.Visibility = Visibility.Visible;
                btnActCat2.Visibility = Visibility.Collapsed;
                FrntSPActivityCat.Visibility = Visibility.Collapsed;
                BckSPActivities.Visibility = System.Windows.Visibility.Visible;
            }
        }


        void trident_RetrieveActivitiesCompleted(object sender, TridentEmulatorSvc.RetrieveActivitiesCompletedEventArgs e)
        {
            OnLoad.Stop();
            OnCategoryDataLoaded.AutoReverse = false;
            OnCategoryDataLoaded.Begin();

            try
            {
                viewModel = new CategoryVisualData
                                    (e.Result as Collection<TridentEmulatorSvc.Category>);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.InnerException.Message);
            }

           
            BuildCategoryPane();
            Build3DCategoryPane();
            AnimateFlipPanel(this.FlagFlipToFront);
        }

        #region Event Handlers

        /// <summary>
        /// TODO: If there is no activity within a category there seems to be a bug
        /// which kills the panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int selectedValue = Convert.ToInt16(btn.Name);
            //Change the selected item of shortcut panel and let its event change the category
            this._3DFlowPanel.ChangeSelectedItem(selectedValue);

        }
        
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            listActivity.DataContext = null;
            lblHeader.Content = null;
            DisplayPanelContent(true);
            AnimateFlipPanel(this.FlagFlipToFront);
        }

        #endregion
    }
}
