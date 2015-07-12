using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using MQuter_eLabApp.ViewModel;

namespace MQuter_eLabApp.View.Editors
{
    public partial class ParamValueEditor : ChildWindow
    {

        public delegate void RefreshEventHandler(object sender, EventArgs e);

        public event RefreshEventHandler OnRefresh;

        // Invoke the Changed event; called whenever activities on canvas changes:
        protected virtual void RefreshWindow(EventArgs e)
        {
            if (OnRefresh != null)
                OnRefresh(this, e);
        }

        public ParamValueEditor()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(ParamValueEditor_Loaded);
        }

        void ParamValueEditor_Loaded(object sender, RoutedEventArgs e)
        {
            ParamDataForm.CommandButtonsVisibility =
                DataFormCommandButtonsVisibility.Edit |
                DataFormCommandButtonsVisibility.Navigation |
                DataFormCommandButtonsVisibility.Commit;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ContentReaderTool_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ParamDataForm.Visibility = Visibility.Collapsed;
            MagicBox.Visibility = Visibility.Visible;
        }

        private void ParamDataForm_Drop(object sender, DragEventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();
            FileInfo[] files = this.GetDropFiles(e);

            if (files.Equals(null))
                return; /* nothing was dropped */
            else if (files.Count() > 1)
                return;
            else if (!(ParamDataForm.CurrentItem is ParamInputModel))
                return;

            bw.DoWork += delegate(object s, DoWorkEventArgs args)
            {
                //Cast the argument into files
                FileInfo[] workfiles = (FileInfo[])args.Argument;
                string content = string.Empty;

                foreach (FileInfo file in workfiles)
                {
                    using (Stream stream = file.OpenRead())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            content = reader.ReadToEnd();
                        }
                    }
                }

                args.Result = content;
            };

            bw.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
            {
                ParamInputModel param = ParamDataForm.CurrentItem as ParamInputModel;
                param.ValueStr = args.Result as string;
                RefreshWindow(new EventArgs());
                DialogResult = true;
            };

            bw.RunWorkerAsync(files); //Activate Background worker
        }

        private FileInfo[] GetDropFiles(DragEventArgs e)
        {
            if (e.Data == null) { return null; }
            IDataObject dataObject = e.Data as IDataObject;
            FileInfo[] files = dataObject.GetData(DataFormats.FileDrop) as FileInfo[];
            return files;
        }
    }
}

