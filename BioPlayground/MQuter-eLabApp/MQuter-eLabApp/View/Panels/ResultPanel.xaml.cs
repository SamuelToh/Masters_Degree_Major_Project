using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Xml.Linq;
using MQuter_eLabApp._3rdParty_Helper;
using System.Collections.ObjectModel;
using MQuter_eLabApp.ViewModel.BioWFMLResultModel;

namespace MQuter_eLabApp.View.Panels
{
    public partial class ResultPanel : UserControl
    {
        public delegate void HideResultEventHandler(object sender, EventArgs e);

        public event HideResultEventHandler ResultHider;

        public ResultCollections Collection { get; private set; }

        // Invoke the Changed event; called whenever activities on canvas changes:
        protected virtual void OnHideResultPanel(EventArgs e)
        {
            if (ResultHider != null)
                ResultHider(this, e);
        }

        public ResultPanel()
        {
            InitializeComponent();
        }

        public void CreateDataGrid(ResultCollections collections)
        {
            Collection = collections;
            this.ResultGrid.Columns.Clear();

            for (var x = 0; x < Collection.propertyNames.Count; x++)
            {
                this.ResultGrid.Columns.Add(
                        new DataGridTextColumn
                        {
                            Header = Collection.propertyNames[x],
                            Binding = new Binding(Collection.propertyNames[x])
                        });
            }

            this.ResultGrid.ItemsSource = GenerateData().ToDataSource();
        }

        public IEnumerable<IDictionary> GenerateData()
        {
            //For each row...
            for (var i = 0; i < Collection.resultCollection.Count; i++)
            {
                var dict = new Dictionary<string, object>();
                //And for each column in row
                for (var x = 0; x < Collection.propertyNames.Count; x++)
                {
                    string key = Collection.propertyNames[x].Trim();
                    object value = Collection.resultCollection[i].dataValues[x];

                    dict[key] = value;
                }
                
                yield return dict;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                object selectedValue = ((CheckBox)sender).DataContext;

                if (selectedValue == null) { return; }

                for (int i = 0; i < ResultGrid.Columns.Count; i++)
                {
                    if (ResultGrid.Columns[i].Header.ToString() == selectedValue.ToString())
                    {
                        ResultGrid.Columns[i].Visibility = System.Windows.Visibility.Visible;
                        break;
                    }
                }

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResultHider(this, null);
        }


        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                object selectedValue = ((CheckBox)sender).DataContext;

                if (selectedValue == null) { return; }

                for (int i = 0; i < ResultGrid.Columns.Count; i++)
                {
                    if (ResultGrid.Columns[i].Header.ToString() == selectedValue.ToString())
                    {
                        ResultGrid.Columns[i].Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    }
                }
            }
        }
    }
}
