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
using BioPatMLEditor.PatternControls.PatternModels;
using BioPatMLEditor.PatternControls.ContextMenu;
using BioPatMLEditor.PatternControls.PatternAttrEditor;
using System.IO;
using System.Collections.ObjectModel;

namespace BioPatMLEditor.PatternControls
{
    public partial class PatternBuilderPanel : UserControl
    {
        /// <summary>
        /// A pattern container containning all the dragable pattern objects
        /// </summary>
        Patterns patterns = new Patterns();

        /// <summary>
        /// a reference to our other control panel's textbox to show pattern has being
        /// implemented. 
        /// </summary>
        public TextBox refTxtCurrPattern { get; set; }

        /// <summary>
        /// A reference to our Pattern repository UC (This variable is optional) The
        /// Pattern builder UC can go without this variable (pattern reading from repository
        /// is not needed)
        /// </summary>
        public PatternRepository refPatternRepoUC { get; set; }

        /// <summary>
        /// The already implemented BioPatML XML.
        /// </summary>
        public String implementedBioPatML { get; set; }

        /// <summary>
        /// This is slightly different from the one above, 
        /// the current working on biopatml that hasn't being implemented by the user.
        /// </summary>
        internal String currBioPatMLStr { get; set; }

        /// <summary>
        /// This might not be needed anymore...
        /// </summary>
        private bool flagLoadFrmRepo { get; set; }

        /// <summary>
        /// States that our tree view is dirty
        /// </summary>
        private bool flagTreeIsDirty { get; set; }

        /// <summary>
        /// There should always only have 1 instance of child window
        /// </summary>
        AttributeEditor PatternPanel = new AttributeEditor();

        public PatternBuilderPanel()
        {
            InitializeComponent();
            BuildPatternMenus();

            PatternViewTree.LayoutUpdated += new EventHandler(PatternViewTree_LayoutUpdated);
            PatternViewTree.MouseRightButtonDown += new MouseButtonEventHandler(PatternViewTree_MouseRightButtonDown);
            PatternViewTree.MouseRightButtonUp += new MouseButtonEventHandler(PatternViewTree_MouseRightButtonUp);
            //PatternPanel.Closed += new EventHandler(patternPanel_Closed);
            flagTreeIsDirty = false;
            flagLoadFrmRepo = false;
        }


        void PatternViewTree_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainMenu menu = new MainMenu(this);
            menu.Show(e.GetPosition(LayoutRoot), 230, 0);
        }

        void PatternViewTree_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;       
        }

        
        /// <summary>
        /// Constructs the pattern list on the right panel
        /// </summary>
        private void BuildPatternMenus()
        {
            Patterns p = new Patterns();
            //Regional Patterns
            p.CreateRegionalPatterns();
            listBoxRegionalPatternMenu.ItemsSource = p;

            //Recursive ones
            p = new Patterns();
            p.CreateRecursivePatterns();
            listBoxRecursivePatternMenu.ItemsSource = p;

            //Structured
            p = new Patterns();
            p.CreateStructuredPatterns();
            listBoxStructuredPatternMenu.ItemsSource = p;

            //Special patterns
            p = new Patterns();
            p.CreateSpecialPatterns();
            listBoxSpecialPatternMenu.ItemsSource = p;
        }

        /// <summary>
        /// Events on what happen prior to item was dropped onto the treeView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewDragDropTarget_Drop(object sender, Microsoft.Windows.DragEventArgs e)
        {
            ItemDragEventArgs id = (ItemDragEventArgs)e.Data.GetData(typeof(ItemDragEventArgs));

            if (((SelectionCollection)id.Data)[0].Item is PatternBase)
            {
                BuildPatternMenus();
            }
            else if (((SelectionCollection)id.Data)[0].Item is TreeViewItem)
            {
                
                ItemDragEventArgs args = e.Data.GetData(typeof(ItemDragEventArgs)) as ItemDragEventArgs;
                var collection = args.Data as SelectionCollection;
                var selection = collection[0] as Selection;
                var data = selection.Item as TreeViewItem;

                #region if is a valid tree view item that has property Id
                if (data.Name.Contains("|"))
                {
                    flagLoadFrmRepo = true; // We have data from Repository treeview
                    String[] dataInfo = data.Name.Split('|'); //split up our string of information
                    GetWebPatternDetail(dataInfo[0], dataInfo[1]);
                }
                #endregion
                #region else we might want to clean our tree view
                else
                {
                    flagTreeIsDirty = true;
                }
                #endregion
                //TreeViewItem
                //refPatternRepoUC.RepopulateTreeItems();
            }
            else
                //There shouldn't be foreign object within our tree view... 
                flagTreeIsDirty = true; // we should improve on this on next release

            //Repopulate the repository tree items
            refPatternRepoUC.ConstructTreeView(refPatternRepoUC.cpiedUnits.Copy());
        }

       
        private void RearrangeParentChildNodes()
        {
            #region Ensure the parent nodes has all its appropreriate child nodes

            //First collect all parent's position
            List<int> parentPosition = new List<int>();

            for (int i = 0; i < PatternViewTree.Items.Count; i++)

                if (PatternViewTree.Items[i] is StructuredPattern)
                    parentPosition.Add(i);

            if (parentPosition.Count < 1) // No structured pattern found perhaps there is only 1 main pattern
                return;
            
            //For the first parent we want him to encapsulate all sibling nodes
            StructuredPattern firstNode = PatternViewTree.Items[parentPosition[0]] as StructuredPattern;
            firstNode.ListOfChilds.Clear(); // We need to repopulate

            for (int x = 0; x < PatternViewTree.Items.Count; x++)
                if (PatternViewTree.Items[x] is PatternBase) //If the iterated item is a pattern
                    if (!PatternViewTree.Items[x].Equals(firstNode)) //And he is not the first node himself
                        firstNode.ListOfChilds.Add(PatternViewTree.Items[x] as PatternBase); //We add him into the child list of first node.

            parentPosition.RemoveAt(0); // Since we have already processed the first node, remove it.

            #region Inner structured Iteration pattern

            //To check this later.. this method is for recording iteration pattern...
            TreeViewItem tvi = PatternViewTree.ItemContainerGenerator.ContainerFromIndex(1) as TreeViewItem; 

            foreach (int position in parentPosition)
            {
                TreeViewItem parent = PatternViewTree.ItemContainerGenerator.ContainerFromIndex(position) as TreeViewItem;
                StructuredPattern parentNode = PatternViewTree.Items[position] as StructuredPattern;
                parentNode.ListOfChilds.Clear(); // We need to repopulate

                //Iterates all his child and add it into parent
                for (int x = 0; x < parent.Items.Count; x++) 
                {
                    if (parent.Items[x] is PatternBase)
                        if (!parent.Items[x].Equals(parentNode))
                            parentNode.ListOfChilds.Add(parent.Items[x] as PatternBase);
                }

            }
            #endregion

            #endregion
        }

        private bool TreeViewNodeIsValidOrder()
        {
            if (PatternViewTree.Items.Count < 3) // Either no tree item nor has one only
                return true;

            if (PatternViewTree.Items.Count > 1 && //Has at least 1 siling node in the main tree branch
                    !(PatternViewTree.Items[1] is StructuredPattern)) // AND our main item is not a structured pattern, invalid tree
            {
                #region Trim away all the siblings
                for (int i = 0; i < PatternViewTree.Items.Count; i++)
                    if (i > 1) //if not master node
                        PatternViewTree.Items.RemoveAt(i); // trim it
                #endregion
                return false;
            }

          

            //Now check all the child node's inner nodes

            return true; //valid tree
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Fix the bug where UI could not properly display the Panel
            PatternPanel = null;
            PatternPanel = new AttributeEditor(); // Renew it to avoid conflict
            PatternPanel.RebuildPanel(PatternViewTree.SelectedItem as PatternBase);
            PatternPanel.Show();
        }

        /// <summary>
        /// Handles the "Closed" event of the ChildWindow. For code efficient purpose
        /// we null our panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void patternPanel_Closed(object sender, EventArgs e)
        {
            //PatternPanel = null; //back to original state
        }

        private void TabItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RearrangeParentChildNodes();
            txtXMLPanel.Blocks.Clear();
            txtXMLPanel.Selection.Text = ConstructBioPatternString();
        }

        public string ConstructBioPatternString()
        {
            return PatternViewTree.Items.Count < 2 ?
                        "" :
                        BioPatXMLBuilder.BuildPatternXML(PatternViewTree.Items[1] as PatternBase,
                        "Definition");
        }

        private void PatternViewTree_LayoutUpdated(object sender, EventArgs e)
        {
            //First check for whether there is a repository unit being put into it
            if (flagLoadFrmRepo)
            { }

            else
                if (flagTreeIsDirty)
                    CleanTreeView();
            
            if (!TreeViewNodeIsValidOrder())
                sender = null;
        }

        private void btnImplement_Click(object sender, RoutedEventArgs e)
        {
            ImplementBioPatML();
        }

        /// <summary>
        /// Shared method for our pattern menu and button implement.
        /// </summary>
        internal void ImplementBioPatML()
        {
            implementedBioPatML = ConstructBioPatternString();
            if (refTxtCurrPattern != null)
                refTxtCurrPattern.Text = "Pattern Implemented.";
        }
        
        /// <summary>
        /// Removes all object from o ur tree
        /// </summary>
        internal void CleanTreeView()
        {
            Object template = PatternViewTree.Items[0];
            PatternViewTree.Items.Clear();
            PatternViewTree.Items.Add(template);
            flagTreeIsDirty = false;
        }
     
    }
}
