using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MQuter_eLabApp.TridentEmulatorSvc;
using System.Collections.ObjectModel;

namespace MQuter_eLabApp.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class _3DActivityVisualizationData
    {
        /// <summary>
        /// The host that contains our activity models 
        /// (*Note these models are not the functional models)
        /// </summary>
        private readonly List<_3DActivityModel> _items = new List<_3DActivityModel>();

        #region Constructors 

        public _3DActivityVisualizationData()
        {
            for (int i = 0; i < 20; i++)
            {
                _items.Add(new _3DActivityModel { Value = "Item " + i });
            }
        }

        #endregion Constructors

        #region Public Methods

        public _3DActivityVisualizationData(List<CategoryModel> activities)
        {
            PopulateModels(activities);
        }

        public void PopulateModels(List<CategoryModel> activities)
        {
            Items.Clear();
            foreach (CategoryModel c in activities)
            {
                _items.Add(new _3DActivityModel() { Value = c.Name });
            }
        }

        /// <summary>
        /// Getter for our items
        /// </summary>
        public List<_3DActivityModel> Items
        {
            get { return _items; }
        }

        #endregion Public Methods
    }
}
