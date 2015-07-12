using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.Research.ScientificWorkflow;

namespace MQuter_DataProductLib
{
    public partial class ListToProductModel : Activity
    {

        public static DependencyProperty ObjectProperty =
            DependencyProperty.Register("ObjectModel", typeof(Object),
            typeof(ListToProductModel));

        [RequiredInputParam]
        [Name("Unknown Object Model")]
        [Description(@"The desired BioPatML search model.")]
        public Object ObjectModel
        {
            get { return ((Object)(base.GetValue(ListToProductModel.ObjectProperty))); }
            set { base.SetValue(ListToProductModel.ObjectProperty, value); }
        }


        public static DependencyProperty BytesProperty =
            DependencyProperty.Register(("Matched"),
            typeof(Byte[]), typeof(ListToProductModel));

        [OutputParam]
        [Name("BioPatML Matches")]
        [Description(@"Match models containing search results of the desired sequences")]
        public Byte[] BytesModel
        {
            get { return ((Byte[])(base.GetValue(ListToProductModel.BytesProperty))); }
            set { base.SetValue(ListToProductModel.BytesProperty, value); }
        }

        //serialize to byte[]
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            

            return ActivityExecutionStatus.Closed;
        }
    }
}
