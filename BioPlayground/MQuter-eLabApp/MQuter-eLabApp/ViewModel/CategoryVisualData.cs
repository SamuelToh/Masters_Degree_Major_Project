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
using System.Collections.ObjectModel;
using MQuter_eLabApp.TridentEmulatorSvc;

namespace MQuter_eLabApp.ViewModel
{
    public class CategoryVisualData
    {
        /// <summary>
        /// The host that contains all the Category models 
        /// </summary>
        private readonly List<CategoryModel> _items = new List<CategoryModel>();

        #region Constructors

        public CategoryVisualData() { }

        public CategoryVisualData
            (Collection<Category> Categories)
        {
            foreach (Category c in Categories)

                _items.Add(new CategoryModel()
                {
                    Activities = ExtractActivities(c),
                    Id = c.Id,
                    Name = c.Name,
                    Label = c.Label,
                    CatDescription = c.Description,
                    ImgSource = "/MQuter-eLabApp;component/Resources/Images/MainLogo.png"
                });
            
        }

        #endregion Constructors

        #region Setters & Getters 

        /// <summary>
        /// Getter for CategoryModels
        /// </summary>
        public List<CategoryModel> Items
        {
            get { return _items; }
        }

        #endregion Setters & Getters

        #region Private Methods

        private List<ActivityModel> ExtractActivities(Category c)
        {
            if (c.Activities != null)
            {
                List<ActivityModel> models = new List<ActivityModel>();

                foreach (Activity a in c.Activities)
                {
                    ActivityModel.ActivityType theType = ActivityModel.ActivityType.Standard;

                    if (a.ActivityTypes == Activity.ActivityType.ForLoop)
                    {
                        theType = ActivityModel.ActivityType.ForLoop;
                    }

                    models.Add(new ActivityModel()
                                {
                                    ActivityClass = a.ActivityClass,
                                    ActivityClassification = theType,
                                    Description = a.Description,
                                    DisplayLabel = a.DisplayLabel,
                                    Id = a.Id,
                                    InputParam = ExtractParam(a, true),
                                    OutboundParam = ExtractParam(a, false)
                                });
                }
                return models;
            }

            return null;
        }

        private List<ParameterModel> ExtractParam(Activity a, bool isInput)
        {
            #region Validation

            if ((isInput) &&
                (a.InputParam == null))
                    return null;

            else
                if ((!isInput) &&
                    (a.OutputParam == null))
                    return null;

            #endregion

            Collection<Parameter> parameters = isInput ? a.InputParam : a.OutputParam;
            List<ParameterModel> models = new List<ParameterModel>();

            foreach (Parameter p in parameters)
                models.Add(new ParameterModel(p.Compulsary)
                {
                    Name = p.Name,
                    DataType = p.DataType,
                    Description = p.Description,
                    IsInputParam = p.IsInputParam,
                    Label = p.Label
                });

            foreach (ParameterModel p in models)
                p.SiblingModels = models.ToArray();

            return models;
        }

        #endregion Private Methods
    }
}
