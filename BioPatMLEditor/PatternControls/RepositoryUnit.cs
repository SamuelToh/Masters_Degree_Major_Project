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
using BioPatMLEditor.BioPatMLDataRepo;

namespace BioPatMLEditor.PatternControls
{
    public class RepositoryUnit
    {
        public string Name { get; set; }

        public int Counter { get; set; }

        public int PatternId { get; set; }

        public bool IsLeaf { get; set; }

        public RepositoryUnit Parent { get; set; }

        public List<RepositoryUnit> Children { get; set; }

        public string ItemId { get; set; }

        public int Depth { get { return Parent == null ? 0 : Parent.Depth + 1; } }


        public RepositoryUnit()
        {
            Name = String.Empty;
            Parent = null;
            Counter = 1;
            Children = new List<RepositoryUnit>();
        }

        public RepositoryUnit CreateChild(string name, string type, int id)
        {
            RepositoryUnit child = new RepositoryUnit() 
             {
                PatternId = id,
                //if is a leaf, issue a normal name to it otherwise put a counter beside it
                Name =  name , 
                Parent = this,
                //Check if we have a valid Id. If no, we know it is not a leaf
                IsLeaf = id > 0 ? true : false 
             };

            //child.Name = id > 0 ? name : name + " [" + (Counter++) + "] ";
            Children.Add(child);

            if (child.IsLeaf)
            {
                this.Name = Name + " [" + (Counter++) + "] ";
                //Create a unique id for our tree view item
                child.ItemId = child.PatternId + "|" + type + "|" + child.Name;
            }
            
            return child;
        }

        /// <summary>
        /// silverlight library does not supports ICloneable interface hence we
        /// are implementing our own deep copy api here. This method copies 
        /// all the references of this object and returns it. 
        /// </summary>
        /// <returns></returns>
        public RepositoryUnit Copy()
        {
            //we might need to chck for possible nulls here
            RepositoryUnit duplicate = new RepositoryUnit()
            {
                Name = this.Name,
                Counter = this.Counter,
                IsLeaf = this.IsLeaf,
                PatternId = this.PatternId,
                ItemId = this.ItemId,
                Parent = this.Parent
            };

            foreach (RepositoryUnit unit in this.Children)
                duplicate.Children.Add(unit.Copy());

            return duplicate;
        }

        /// <summary>
        /// Here we build our repository tree view collection.
        /// ***More documentation needed***
        /// </summary>
        /// <param name="DefinitingPatterns"></param>
        /// <returns></returns>
        public static RepositoryUnit CreateRepositoryData(List<DefinitionPatternInfo> DefinitingPatterns)
        {
            //We hardcode the server name for now.
            RepositoryUnit root = new RepositoryUnit() { Name = "QUT.BioPatML.Repository.server" };

            #region Initialize all BioPatML possible pattern 
            RepositoryUnit any = root.CreateChild("Any", null, -1);
            RepositoryUnit gap = root.CreateChild("Gap", null, -1);
            RepositoryUnit composite = root.CreateChild("Composite", null, -1);

            RepositoryUnit block = root.CreateChild("Block", null, -1);
            RepositoryUnit regEx = root.CreateChild("RegularExpression", null, -1);
            RepositoryUnit motif = root.CreateChild("Motif", null, -1);
            RepositoryUnit prosite = root.CreateChild("Prosite", null, -1);
            RepositoryUnit pwm = root.CreateChild("PWM", null, -1);

            RepositoryUnit set = root.CreateChild("Set", null, -1);
            RepositoryUnit series = root.CreateChild("Series", null, -1);
            RepositoryUnit iteration = root.CreateChild("Iteration", null, -1);
            RepositoryUnit repeat = root.CreateChild("Repeat", null, -1);
            RepositoryUnit logic = root.CreateChild("Logic", null, -1);

            RepositoryUnit alignment = root.CreateChild("Alignment", null, -1);
            RepositoryUnit constraint = root.CreateChild("Constraint", null, -1);
            RepositoryUnit Void = root.CreateChild("Void", null, -1);
  
            #endregion

            foreach (DefinitionPatternInfo defPattern in DefinitingPatterns)
            {
                switch (defPattern.MainPatternType)
                {
                    #region Find and Build the appropriate "Regional Pattern" 
                    case "Any":
                        {
                            any.CreateChild
                                (defPattern.MainPatternName,
                                 defPattern.MainPatternType, 
                                 Convert.ToInt16(defPattern.MainPatternId));
                            break;
                        }

                    case "Gap" :
                        {
                            gap.CreateChild
                                (defPattern.MainPatternName,
                                 defPattern.MainPatternType, 
                                 Convert.ToInt16(defPattern.MainPatternId));
                            break;
                        }

                    case "Composite":
                        {
                            composite.CreateChild
                                (defPattern.MainPatternName,
                                 defPattern.MainPatternType, 
                                 Convert.ToInt16(defPattern.MainPatternId));
                            break;
                        }
                    #endregion
                    #region Find and Build the appropriate "Recursive Pattern" 
                    case "Block":
                        {
                            block.CreateChild
                                (defPattern.MainPatternName,
                                 defPattern.MainPatternType,
                                Convert.ToInt16(defPattern.MainPatternId));
                            break;
                        }

                    case "RegularExpression":
                        {
                            regEx.CreateChild
                                (defPattern.MainPatternName,
                                 defPattern.MainPatternType,
                                 Convert.ToInt16(defPattern.MainPatternId));
                            break;
                        }

                    case "Motif":
                        {
                            motif.CreateChild
                                (defPattern.MainPatternName,
                                 defPattern.MainPatternType, 
                                 Convert.ToInt16(defPattern.MainPatternId));
                            break;
                        }

                    case "Prosite":
                        {
                            prosite.CreateChild
                                (defPattern.MainPatternName,
                                 defPattern.MainPatternType, 
                                 Convert.ToInt16(defPattern.MainPatternId));
                            break;
                        }
                    case "PWM":
                        {
                            pwm.CreateChild
                                (defPattern.MainPatternName,
                                 defPattern.MainPatternType, 
                                 Convert.ToInt16(defPattern.MainPatternId));
                            break;
                        }
                    #endregion
                    #region Find and Build the appropriate "Structured Pattern"
                    case "Set":
                        {
                            set.CreateChild
                                (defPattern.MainPatternName,
                                 defPattern.MainPatternType,
                                 Convert.ToInt16(defPattern.MainPatternId));
                            break;
                        }
                    case "Series":
                        {
                            series.CreateChild
                                (defPattern.MainPatternName,
                                 defPattern.MainPatternType, 
                                 Convert.ToInt16(defPattern.MainPatternId));
                            break;
                        }
                    case "Iteration":
                        {
                            iteration.CreateChild
                                (defPattern.MainPatternName,
                                 defPattern.MainPatternType, 
                                 Convert.ToInt16(defPattern.MainPatternId));
                            break;
                        }
                    case "Repeat":
                        {
                            repeat.CreateChild
                                (defPattern.MainPatternName,
                                 defPattern.MainPatternType, 
                                 Convert.ToInt16(defPattern.MainPatternId));
                            break;
                        }
                    case "Logic":
                        {
                            logic.CreateChild
                                (defPattern.MainPatternName,
                                 defPattern.MainPatternType, 
                                 Convert.ToInt16(defPattern.MainPatternId));
                            break;
                        }
                    #endregion
                    #region Find and Build the appropriate "Special Pattern"   
                    case "Alignment":
                        {
                            alignment.CreateChild
                                (defPattern.MainPatternName,
                                 defPattern.MainPatternType, 
                                 Convert.ToInt16(defPattern.MainPatternId));
                            break;
                        }
                    case "Constraint":
                        {
                            constraint.CreateChild
                                (defPattern.MainPatternName,
                                 defPattern.MainPatternType, 
                                 Convert.ToInt16(defPattern.MainPatternId));
                            break;
                        }
                    case "Void":
                        {
                            Void.CreateChild
                                (defPattern.MainPatternName,
                                 defPattern.MainPatternType, 
                                 Convert.ToInt16(defPattern.MainPatternId));
                            break;
                        }
                    #endregion
                }   

            }

            return root;

        }
    }
}
