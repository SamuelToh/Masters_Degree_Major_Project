using System;
using System.Collections.Generic;
using System.ComponentModel; //For background worker 
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.IO;
using System.Text;
using BioPatMLEditor.ServiceReference1;
using BioPatMLEditor.MBFParserControl.ParsingLogic;

namespace BioPatMLEditor.MBFParserControl
{
    /// <summary>
    /// This control stores all of our available parser widgets, it also acts like
    /// an instream memory database to store all parsed sequences.
    /// </summary>
    public partial class MBFParserBox : UserControl
    {
        /// <summary>
        /// A collection of parsed sequences
        /// </summary>
        public List<SequenceContract> ParsedSequences { get; set; }

        /// <summary>
        /// A background worker for performing background task so our UI remains
        /// responsive during this period of time.
        /// </summary>
        private BackgroundWorker bw;

        /// <summary>
        /// The default Constructor
        /// </summary>
        public MBFParserBox()
        {
            InitializeComponent();
            ParsedSequences = new List<SequenceContract>();
            this.GenbankDropperCavas.Drop += new DragEventHandler(Genbank_Drop);
            this.Faster_Dropper.Drop += new DragEventHandler(Faster_Dropper_Drop);
            listBoxParsers.ItemsSource = new Widgets();
        }


        /// <summary>
        /// This function converts a list of dragged data objects into a collection
        /// of files.
        /// </summary>
        /// <param name="e">The drag event that contains a list of dragged items.</param>
        /// <returns>An array of dropped files.</returns>
        private FileInfo[] GetDropFiles(DragEventArgs e)
        {
            if (e.Data == null) { return null; }
            IDataObject dataObject = e.Data as IDataObject;
            FileInfo[] files = dataObject.GetData(DataFormats.FileDrop) as FileInfo[];
            return files;
        }


        #region Delegates for our UI Component

        //our delegate used for updating the UI
        public delegate void UpdateGBProgressDelegate(string status);

        //our delegate used for updating the UI
        public delegate void UpdateFastaProgressDelegate(string status);

        /// <summary>
        /// Called by the delegates to update the genbank progress bar text
        /// </summary>
        /// <param name="message">The message to be passed into the progress bar.</param>
        public void UpdateGBProgressText(string message)
        {
            genbankBusyPB.BusyContent = message;
        }

        /// <summary>
        /// Same as above except this one is for our fasta progress bar
        /// </summary>
        /// <param name="message">The message to be passed onto the progress bar.</param>
        public void UpdateFastaProgressText(string message)
        {
            FastaBusyPB1.BusyContent = message;
        }

        #endregion

        #region Animation Codes

        /// <summary>
        /// Invoke the scaling animation when a parser is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is StackPanel)
            {
                if (this.listBoxParsers.SelectedIndex > -1)
                {
                    ListBox parsers = this.listBoxParsers;
                    Widget selcItem = parsers.SelectedItem as Widget;

                    if (selcItem.WidgetName.Equals("MBF Genbank File Parser"))
                        ToggleWidget.Begin();

                    else if
                        (selcItem.WidgetName.Equals("MBF Fasta File Parser  "))
                        FastaToggle.Begin();
                }
            }
            
        }

        /// <summary>
        /// Toggle animation when any [Back] button was clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            ToggleWidgetBack.Begin();
        }

        #endregion
    }
}
