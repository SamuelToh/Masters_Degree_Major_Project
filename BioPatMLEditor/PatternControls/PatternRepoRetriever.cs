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
using System.Xml;
using System.IO;
using System.Text;
using System.ComponentModel; //For background worker 
using BioPatMLEditor.BioPatMLDataRepo;
using BioPatMLEditor.PatternControls.PatternModels;

namespace BioPatMLEditor.PatternControls
{
    /// <summary>
    /// This class consists of methods for retrieving of the desire BioPatML language
    /// through our repository web service and method which converts raw BioPatML language to
    /// the editor's pattern model.
    /// </summary>
    public partial class PatternBuilderPanel
    {
        /// <summary>
        /// Our worker for performing Async call to our BioPatML Data repo service
        /// </summary>
        private BackgroundWorker bw = new BackgroundWorker();

        /// <summary>
        /// Fires the BioPatML Repository Web service to grab our desire BioPatML pattern.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        public void GetWebPatternDetail(String id, String type)
        {
            #region Code for the blue "Loading" visual effect

            txtLoading.Visibility = System.Windows.Visibility.Visible;
            BlinkLoading.RepeatBehavior = RepeatBehavior.Forever;
            BlinkLoading.Begin();

            DefinitionInfo definition = new DefinitionInfo()
            {
                MainPatternId = id,
                MainPatternType = type
            };
            #endregion Code for the blue "Loading" visual effect

            #region Code for the background worker to invoke our BioPatML repository service
            bw.DoWork += delegate(object s, DoWorkEventArgs args)
            {
                #region Initialize all the essential variables 

                DefinitionInfo target = (DefinitionInfo)args.Argument;
                bool done = false; 
                string result = string.Empty; 
                BioPatMLDataRepo.BioPatMLDataServiceClient repository = new BioPatMLDataServiceClient();
                
                #endregion

                repository.GetPatternDetailInfoCompleted += delegate
                    (object sender, GetPatternDetailInfoCompletedEventArgs e)
                {
                    //When finish retrieving data we assign the BioPatML String to the result variable
                    result = e.Result.ToString();
                    done = true;
                };

                //Fires the web service and pass in a definition which contains the pattern Id and its type
                repository.GetPatternDetailInfoAsync(target);

                while (!done) { System.Threading.Thread.Sleep(2000); };

                repository.CloseAsync(); //close the service
                repository = null; //clean up resources
                args.Result = result;
            };
            #endregion #region Code for the background worker to invoke our BioPatML repository service

            //When worker has finished its job
            //we want him to remake our repository treeview n end all visual effects
            bw.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
            {
                ReconstructTreeView(currBioPatMLStr = args.Result.ToString());
                #region End the blinking loading  effect
                txtLoading.Visibility = System.Windows.Visibility.Collapsed;
                BlinkLoading.Stop();
                #endregion
            };

            bw.RunWorkerAsync(definition);
        }

        internal void ReconstructTreeView(String patternStr)
        {
            this.CleanTreeView();
            StreamReader sreader = new StreamReader (new MemoryStream (Encoding.UTF8.GetBytes (patternStr)));
            XmlReader reader = XmlReader.Create(sreader);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        ConstructPattern(reader);
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        Console.WriteLine(reader.Value);
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        Console.Write("</" + reader.Name);
                        Console.WriteLine(">");
                        break;
                }

            }

        }

        /// <summary>
        /// TODO: Now the conversion only supports Motif / Prosite and PWM models
        /// We need to add in the remaining models here...
        /// </summary>
        /// <param name="reader"></param>
        private void ConstructPattern(XmlReader reader)
        {
            switch (reader.Name)
            {
                case "Motif":
                    this.PatternViewTree.Items.Add(CreateMotif(reader));
                    break;

                case "Prosite" :
                    this.PatternViewTree.Items.Add(CreateProsite(reader));
                    break;

                case "PWM" :
                    this.PatternViewTree.Items.Add(CreatePWM(reader));
                    break;
            }
        }

        /// <summary>
        /// This method reconstruct the given raw BioPatML language into our Editor's 
        /// PWM model. 
        /// </summary>
        /// <param name="reader">The Xml reader which contains the BioPatML language</param>
        /// <returns>The editor's model object</returns>
        private PatternBase CreatePWM(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                PWM pwm = new PWM()
                {
                    PatternName = reader.GetAttribute("name").ToString(),
                    Impact = Double.Parse(reader.GetAttribute("impact").ToString()),
                    Threshold = Double.Parse(reader.GetAttribute("threshold").ToString()),
                    Alphabet = Motif.GetEnumAlpha(reader.GetAttribute("alphabet").ToString())

                };

                bool flagStop = false;

                //Contiune process for rows of PWM
                while (!flagStop)
                {
                    reader.Read();

                    switch (reader.NodeType)
                    {

                        case XmlNodeType.Element:
                            if (reader.Name == "Row")
                            {
                                PWMRow row = new PWMRow()
                                {
                                    RowKey = reader.GetAttribute("letter")[0]
                                };

                                reader.Read(); //Skip to text doc
                                row.RowValue = reader.Value;
                                pwm.RowElements.Add(row);
                            }
                            break;

                        case XmlNodeType.EndElement:
                            if (reader.Name == "PWM")
                            {
                                flagStop = true ; //get out of PWM creation
                            }
                            break;
                    }
                }

                return pwm;
            }

            return new PWM();
        }

        /// <summary>
        /// Similar to the method above except this one converts Prosite model
        /// </summary>
        /// <param name="reader">The Xml reader which contains the BioPatML language</param>
        /// <returns>The editor's model object</returns>
        private PatternBase CreateProsite(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                return new Prosite()
                {
                    PatternName = reader.GetAttribute("name").ToString(),
                    SearchPattern= reader.GetAttribute("prosite").ToString(),
                    Impact = Double.Parse(reader.GetAttribute("impact").ToString()),
                    Threshold= Double.Parse(reader.GetAttribute("threshold").ToString()),
                    Alphabet = Motif.GetEnumAlpha(reader.GetAttribute("alphabet").ToString())

                };
            }

            return new Prosite();
        }

        /// <summary>
        /// And this one the Motif model
        /// </summary>
        /// <param name="reader">The Xml reader which contains the BioPatML language</param>
        /// <returns>The editor's model object</returns>
        private PatternBase CreateMotif(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                return new Motif()
                {
                    PatternName = reader.GetAttribute("name").ToString(),
                    SearchPattern  = reader.GetAttribute("motif").ToString(),
                    Impact = Double.Parse(reader.GetAttribute("impact").ToString()),
                    Threshold =  Double.Parse(reader.GetAttribute("threshold").ToString()),
                    Alphabet = Motif.GetEnumAlpha(reader.GetAttribute("alphabet").ToString())
                    
                };
            }

            return new Motif();
        }

    }
}
