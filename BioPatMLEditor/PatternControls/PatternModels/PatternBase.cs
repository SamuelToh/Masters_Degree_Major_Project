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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace BioPatMLEditor.PatternControls.PatternModels
{
    /// <summary>
    /// This is the base of all models. Contains all the binable information
    /// needed for binding data onto the AttributeEditor Child form.
    /// </summary>
    [Bindable(true)]
    public  class PatternBase : IPattern
    {
        /// <summary>
        /// The indentation needed when writing out XML as string
        /// </summary>
        protected string Indentation = "    ";

        /// <summary>
        /// The head node of this pattern (this variable can be null if he is
        /// the master pattern).
        /// </summary>
        protected IPattern ParentNode = null;

        /// <summary>
        /// The next lower node pointing to him.
        /// </summary>
        protected IPattern ChildNode = null;

        /// <summary>
        /// The type name of this pattern e.g. Any Motif or Logic
        /// A convinent way of knowing what this pattern actually is.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public string BioPatternName { get; set; }
      
        [Display(Name = "Name",
            Description = "The name you want this component to be called.",
            Order = 2)]
        [Bindable(false)]
        public String PatternName { get; set; }

        [Display(Description = "How accurate you want the match to be performed upon. 1 is the max you can go.",
            Order = 4)]
        [Range(0.0, 1.0, ErrorMessage = "The impact value can only be between 0.0 to 1.0.")]
        public double Impact { get; set; }

        /// <summary>
        /// The default constructor of building a pattern.
        /// </summary>
        /// <param name="bioPatternName">The Type name</param>
        /// <param name="patternName">Name of this component</param>
        /// <param name="impact">the default impact value</param>
        public PatternBase(String bioPatternName, String patternName, double impact)
        {
            BioPatternName = bioPatternName;
            Impact = impact;
        }

        /// <summary>
        /// To be overriden by child classes.
        /// </summary>
        /// <returns></returns>
        public virtual string ElementXML()
        {
            return "";
        }

    }

    /// <summary>
    /// A collection of BioPatML patterns to be used for data binding.
    /// </summary>
    public class Patterns : ObservableCollection<PatternBase>
    {
        /// <summary>
        /// Default constructor 
        /// </summary>
        public Patterns() { }

        /// <summary>
        /// This method populates all regional patterns onto the observable collection
        /// </summary>
        public void CreateRegionalPatterns()
        {
            Add(new Any());
            Add(new Gap());
            Add(new Composition());
        }

        /// <summary>
        /// Same as above, creates recursive patterns.
        /// </summary>
        public void CreateRecursivePatterns()
        {
            Add(new Block());
            Add(new RegularExp());
            Add(new Motif());
            Add(new Prosite());
            Add(new PWM());

        }

        /// <summary>
        /// Same as above, Structured patterns.
        /// </summary>
        public void CreateStructuredPatterns()
        {
            Add(new Set());
            Add(new Series());
            Add(new Iteration());
            Add(new Repeat());
            Add(new Logic());
        }

        /// <summary>
        /// Same as above, special patterns.
        /// </summary>
        public void CreateSpecialPatterns()
        {
            Add(new Alignment());
            Add(new Constraint());
            Add(new Void());
        }
    }
}