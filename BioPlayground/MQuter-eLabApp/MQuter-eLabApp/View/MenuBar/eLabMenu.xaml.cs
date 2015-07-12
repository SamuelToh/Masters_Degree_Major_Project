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
using System.Windows.Media.Imaging;
#region Testing purposes
using System.Collections.ObjectModel;
using MQuter_eLabApp.ViewModel;
using MQuter_eLabApp.ViewModel.BioWFMLResultModel;
using MQuter_eLabApp.Events;
#endregion

namespace MQuter_eLabApp.View.MenuBar
{
    public partial class eLabMenu : UserControl
    {
        public eLabMenu()
        {
            InitializeComponent();
        }

        public delegate void TestEventHandler(object sender, EventArgs e);

        public event TestEventHandler test;

        // Invoke the Changed event; called whenever activities on canvas changes:
        protected virtual void OnHideResultPanel(EventArgs e)
        {
            if (test != null)
                test(this, e);
        }

        private void FishEyeMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ResultCollections collections = new ResultCollections();

            collections.resultCollection = new ObservableCollection<ResultSet>();



            ObservableCollection<string> propertiesName = new ObservableCollection<string>();

            propertiesName.Add("HitStart");
            propertiesName.Add("HitEnd");
            propertiesName.Add("HitCharacters");
            propertiesName.Add("HitLength");
            propertiesName.Add("HitSimilarity");

            collections.propertyNames = propertiesName ;

            ResultSet r = new ResultSet();
            ResultSet r2 = new ResultSet();
            ResultSet r3 = new ResultSet();

            r.dataValues = new ObservableCollection<string>();
            r2.dataValues = new ObservableCollection<string>();
            r3.dataValues = new ObservableCollection<string>();


            string value1 = "1";
            string value2 = "3";
            string value3 = "tcg";
            string value4 = "3";
            string value5 = "1.0";

            r.dataValues.Add(value1);
            r.dataValues.Add(value2);
            r.dataValues.Add(value3);
            r.dataValues.Add(value4);
            r.dataValues.Add(value5);

            string value6 = "2";
            string value7 = "4";
            string value8 = "tta";
            string value9 = "2";
            string value10 = "0.5";

            r2.dataValues.Add(value6);
            r2.dataValues.Add(value7);
            r2.dataValues.Add(value8);
            r2.dataValues.Add( value9);
            r2.dataValues.Add(value10);
            

            string value11 = "45";
            string value12 = "55";
            string value13 = "tttggaac";
            string value14 = "10";
            string value15 = "0.8";

            r3.dataValues.Add(value11);
            r3.dataValues.Add(value12);
            r3.dataValues.Add(value13);
            r3.dataValues.Add(value14);
            r3.dataValues.Add(value15);

            collections.resultCollection.Add(r);
            collections.resultCollection.Add(r2);
            collections.resultCollection.Add(r3);


            OnHideResultPanel(new ResultSetEventArgs(collections));
            ///Testing purpose
           /* ActivityModel model = new ActivityModel()
            {
                 ActivityClass = "Demo Class",
                 Description = "Here descripts the class",
                 //Id = new Guid("SimpleId"),
                 DisplayLabel = "MBF Library",
            };

            List<ParamterModel> inputParameters = new List<ParamterModel>();
            List<ParamterModel> outputParameters = new List<ParamterModel>();

            inputParameters.Add(new ParamterModel()
                {
                    DataType = "System.String",
                    Description = "The alphabets",
                    IsInputParam = true,
                    Label = "the label",
                    Name = "The MBF Param 1"
                }
             );

            inputParameters.Add(new ParamterModel()
            {
                DataType = "System.bool",
                Description = "True or false",
                IsInputParam = true,
                Label = "the boolean",
                Name = "The MBF Param 2"
            }
             );

            outputParameters.Add(new ParamterModel()
            {
                DataType = "System.String",
                Description = "output",
                IsInputParam = false,
                Label = "Output1",
                Name = "The MBF output 1"
            }
            );

            outputParameters.Add(new ParamterModel()
            {
                DataType = "System.Int",
                Description = "The numbers",
                IsInputParam = false,
                Label = "the num label",
                Name = "The MBF out 2"
            }
            );

            model.InputParam = inputParameters;
            model.OutboundParam = outputParameters;

                ActivityComponent component = new ActivityComponent()
                {
                     DataContext = model
                };

                ActivityEditor editor = new ActivityEditor(component, component);
            editor.Show();*/
        }

     
    }
}
