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

namespace MQuter_eLabApp.ViewModel.WorkflowModels
{
    public class SequentialWorkflowModel : BaseWorkflowModel
    {
        public SequentialWorkflowModel() { }

        public SequentialWorkflowModel(IWorkflowItem activity1)
        {
            _activity = new IWorkflowItem[1]; //Sequential activiy 1
            ActivityModel1 = activity1;
        }
    }
}
