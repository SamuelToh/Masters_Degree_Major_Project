#define DEBUG

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
using BioPatMLEditor.BioPatMLDataRepo;

namespace BioPatMLEditor.PatternControls
{

    public partial class PatternRepository : UserControl
    {
        public string MyHeart { get; set;}
        public List<DefinitionPatternInfo> patterns;
        public RepositoryUnit cpiedUnits; 

        public PatternRepository()
        {
            InitializeComponent();
            GetRepositoryStructure(); //Call out web service to populate the BioPatML Tree
        }

      
        private void FillTree(ItemCollection itemColl, RepositoryUnit dataNode)
        {
            TreeViewItem tvi = new TreeViewItem();
            tvi.IsExpanded = true;
            
            itemColl.Add(tvi);
            //If its a potential draggable object we put his item id into it
            if (dataNode.IsLeaf)
                tvi.Name = dataNode.ItemId;

            tvi.Header = dataNode.Name;
            
            foreach (RepositoryUnit childDataNode in dataNode.Children)
            {
                FillTree(tvi.Items, childDataNode); 
            }
        }  // end method

     
    }
}
