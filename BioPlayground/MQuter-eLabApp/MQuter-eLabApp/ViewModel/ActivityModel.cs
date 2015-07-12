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
using System.Xml.Serialization;

namespace MQuter_eLabApp.ViewModel
{
    public class ActivityModel
    {
        public enum ActivityType
        {
            Standard = 0,
            ForLoop = 1
        };

        #region Properties

        public ActivityType ActivityClassification { get; set; }

        public String ActivityClass { get; set; }

        public String UniqueName { get; set; }

        public String Description { get; set; }

        public String DisplayLabel { get; set; }

        public bool IsStartFlow { get; set; }

        public bool IsEndFlow { get; set; }

        public Guid Id { get; set; }

        public List<ParameterModel> InputParam { get; set; }

        public List<ParameterModel> OutboundParam { get; set;}

        public ActivityModel()
        {
            InputParam = new List<ParameterModel>();
            OutboundParam = new List<ParameterModel>();
        }

        public ActivityModel CloneModel()
        {
            return new ActivityModel
            {
                ActivityClass = this.ActivityClass,
                ActivityClassification = this.ActivityClassification,
                Description = this.Description,
                DisplayLabel = this.DisplayLabel,
                Id = this.Id,
                InputParam = CopyParameters(InputParam, null),
                OutboundParam = CopyParameters(OutboundParam, null)
            };
        }

        public ActivityModel CloneModel(string uniqueName)
        {
            return new ActivityModel
            {
                ActivityClass = this.ActivityClass,
                ActivityClassification = this.ActivityClassification,
                Description = this.Description,
                DisplayLabel = this.DisplayLabel,
                Id = this.Id,
                IsEndFlow = this.IsEndFlow,
                IsStartFlow = this.IsStartFlow,
                UniqueName = uniqueName,
                InputParam = CopyParameters(InputParam, uniqueName),
                OutboundParam = CopyParameters(OutboundParam, uniqueName)
            };
        }

        private List<ParameterModel> CopyParameters(List<ParameterModel> paramCollection, string parentName)
        {
            List<ParameterModel> parameters = new List<ParameterModel>();

            foreach (ParameterModel param in paramCollection)
            {
                ParameterModel clonedModel = null;

                if (param.IsInputParam)
                    clonedModel = new ParamInputModel() { IsMandatory = (param.isMandatory) };
                else
                    clonedModel = new ParamOutputModel();

                AssignParamValues(param, clonedModel, parentName);

                parameters.Add(clonedModel);
            }

            AssignSiblingsValue(parameters);

            return parameters;
        }

        private void AssignSiblingsValue(List<ParameterModel> models)
        {
            for (int i = 0; i < models.Count; i++)
            {
                models[i].SiblingModels = models.ToArray();
            }
        }

        private void AssignParamValues(ParameterModel p, ParameterModel cloned, String parentName)
        {
            cloned.DataType = p.DataType;
            cloned.Description = p.Description;
            cloned.IsInputParam = p.IsInputParam;
            cloned.Label = p.Label;
            cloned.Name = p.Name;
            cloned.ParentName = parentName == null ? null : parentName;
            //cloned.Value = p.Value;
        }

        #endregion Properties
    }
}
