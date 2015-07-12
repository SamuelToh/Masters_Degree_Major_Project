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
    public class ParallelWorkflowModel : BaseWorkflowModel
    {
        public ParallelWorkflowModel() { }

        public ParallelWorkflowModel(IWorkflowItem activity1, IWorkflowItem activity2)
        {
            _activity = new IWorkflowItem[2]; //Sequential model only has an activity
            _activity[0] = activity1;
            _activity[1] = activity2;
        }

        public IWorkflowItem ActivityModel2
        {
            get { return this._activity[1]; }
            set { this._activity[1] = value; }
        }
    }
}
