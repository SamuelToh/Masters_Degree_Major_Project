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
using System.Collections.ObjectModel;

namespace BioPatMLEditor.MBFParserControl.ParsingLogic
{
    /// <summary>
    /// Base class for all the available file parsers.
    /// </summary>
    public sealed class Widget
    {
        /// <summary>
        /// Name of the widget to be displayed by editor
        /// </summary>
        public string WidgetName { get; set; }

        /// <summary>
        /// The image for the widget, if any.
        /// </summary>
        public string WidgetImgPath { get; set; }

        /// <summary>
        /// The constructor taking only the parameter name.
        /// </summary>
        /// <param name="name">The name of the widget.</param>
        public Widget(string name)
        {
            WidgetName = name;
        }

        /// <summary>
        /// Default Constructor, for every parser a name and an image of the parser has to be supplied
        /// </summary>
        /// <param name="name">Name of widget</param>
        /// <param name="imgPath">The source of widget image</param>
        public Widget(string name, string imgPath)
        {
            WidgetName = name;
            WidgetImgPath = imgPath;
        }
    }
        /// <summary>
        /// Collections of all supported widgets
        /// </summary>
        public class Widgets : ObservableCollection<Widget>
        {
            /// <summary>
            /// Default constructor
            /// </summary>
            public Widgets() 
            {
                //Create supported widget         ImgResources/genbankParser.png               
                Add(new Widget("MBF Genbank File Parser", @"ImgResources/BioPatML.png"));
                Add(new Widget("MBF Fasta File Parser  ", @"ImgResources/genbankParser.png"));
            }
        }
    
}
