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
using MQuter_eLabApp.Events;
using MQuter_eLabApp.ViewModel.BioWFMLResultModel;
using System.Xml.Linq;

namespace MQuter_eLabApp
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            AnnotationToolBar0.AddAnnotation +=
                new View.MenuBar.AnnotationToolbar.AnnoateEventHandler
                    (AnnotationEvent);

            AnnotationToolBar0.SelectAnnotation += 
                new View.MenuBar.AnnotationToolbar.SelectEventHandler
                    (SelectAnnotation);

            WorkflowCanvas0.HasResultSet += 
                new View.DrawingCanvas.WorkflowCanvas.HasResultEventHandler
                    (WorkflowCanvas0_HasResultSet);

            resultPanel.ResultHider += 
                new View.Panels.ResultPanel.HideResultEventHandler
                    (resultPanel_ResultHider);

            this.eLabMenuBar.test += new View.MenuBar.eLabMenu.TestEventHandler(eLabMenuBar_test);
        }

        void eLabMenuBar_test(object sender, EventArgs e)
        {
            WorkflowCanvas0_HasResultSet(null, e);
        }

        void resultPanel_ResultHider(object sender, EventArgs e)
        {
            HideResult.AutoReverse = false;
            HideResult.Begin();
        }

        void WorkflowCanvas0_HasResultSet(object sender, EventArgs e)
        {
            ResultSetEventArgs resultSetArgs =  e as ResultSetEventArgs;
            ResultCollections collections = resultSetArgs.results;

            resultPanel.listProperties.ItemsSource = collections.propertyNames;
            resultPanel.CreateDataGrid(collections);


            ShowResult.AutoReverse = false;
            ShowResult.Begin();
        }

        private void SelectAnnotation(object sender)
        {
            WorkflowCanvas0.TriggerSelectionState();
        }

        private void AnnotationEvent(object sender, EventArgs e)
        {
            WorkflowCanvas0.DrawAnnotation(e as AnnotationEventArgs);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool isFullScreen = Application.Current.Host.Content.IsFullScreen;

            if(isFullScreen)
                Application.Current.Host.Content.IsFullScreen = false;

            else
                Application.Current.Host.Content.IsFullScreen = true;
        }

    }

}
