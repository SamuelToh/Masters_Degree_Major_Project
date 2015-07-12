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
using System.Collections.Generic;
using MQuter_eLabApp.Exceptions;
using MQuter_eLabApp.ViewModel;
using MQuter_eLabApp.View.Components.Activity;

namespace MQuter_eLabApp.Model
{
    public class ActivityValidator : IDisposable
    {

        public void ValidateActivities
            (List<IActivityComponent> outboundComp, IActivityComponent inboundComp)
        {
            ActivityModel inboundModel = (inboundComp.DataContext as ActivityModel);

            foreach (ParamInputModel input in inboundModel.InputParam)
            {
                if (input.IsMandatory)
                {
                    if (input.ValueStr == null)
                        throw new NullValueForMandatoryParam
                                    ("The parameter " + input.Name + " from activity "
                                        + input.ParentName + " is mandatory but no data value " +
                                        "was found.");
                    //else
                    //  if (input.ValueStr == null)
                    //    throw new ArgumentNullException
                    //          ("input is mandatory!");

                    else
                        if (input.ValueStr.Length < 1)
                            throw new NullValueForMandatoryParam("The length of string value for paramter " + input.Name + " was less than 1.");

                        else if (input.ValueStr.Contains(":\\"))
                        {
                            throw new ArgumentException
                                ("Parameter " + input.Name + " of activity " + input.ParentName +
                                " has invalid data input. File reading from local machine is not supported for this application, " +
                                    "try parsing the entire file using the file content reader.");
                        }
                        else if ((input.DataType.ToLower() == "system.boolean")
                               && (input.ValueStr.ToLower() != "true" || input.ValueStr.ToLower() != "false"))
                            throw new ArgumentException("The Parameter " + input.Name + " for activity " + input.ParentName +
                                "is boolean type and thus it only can accept either true or false as its data value.");
                }
            }
        }

        public void Dispose()
        {
            
        }
    }
}
