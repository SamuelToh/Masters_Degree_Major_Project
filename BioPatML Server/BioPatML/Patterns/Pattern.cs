using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Common.XML;

/*****************| Queensland University Of Technology |********************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Patterns
{
    /// <summary>
    /// This abstract class serves as base class for patterns and provides only
    /// setter and getter for the pattern name.
    /// </summary>
    public abstract class Pattern : IPattern
    {
        #region -- Automatic Properties --
        /// <summary>
        /// The minimum required similarity threshold for a match
        /// </summary>
        public virtual double Threshold { get; set; }

        #endregion 

        #region -- Private fields --
        /// <summary>
        /// Name of pattern
        /// </summary>
        private string name;

        /// <summary>
        /// Counter for patterns to create unique pattern names 
        /// </summary>
        private static int counter = 0;

        /// <summary>
        /// The match object of a pattern
        /// </summary>
        private Match match;

        #endregion

        #region -- Constructors

        //internal Pattern(string name) { PatternName = name; }

        /// <summary>
        /// Construct a pattern with a given name usually passed from child class
        /// </summary>
        /// <param name="name"> name of element</param>
        public Pattern(string name)
        {
            match = new QUT.Bio.BioPatML.Patterns.Match(this);
            Threshold = 0.0;
            PatternName = name;
            Impact = 1.0; //start it with a default value
        }

        #endregion

        #region -- Properties --

        /// <summary>
        /// Properties name of pattern 
        /// </summary>
        public String PatternName 
        {
            get { return name; }

            set 
            {
                if (value != null)
                    name = value;
            }
        }

        /// <summary>
        /// Sets/ Gets the impact of a pattern. This a weight is taken into account
        /// when the overall similarity of a structured pattern, consisting of
        /// other patterns, is calculated.
        /// <para></para>
        /// Note the given param value for Impact weight must be between zero and one.
        /// </summary>
        public virtual double Impact
        {
            get { return match.Impact; }
            set { match.Impact = value; }
        }

        /// <summary>
        /// A static variable used keep track of the unique Id
        /// </summary>
        private static int Counter
        {
            get { return counter++; }
        }

       /// <summary>
       ///  A static variable used for generating unique Id     
       /// </summary>
       public static int Id
       {
           get { return Counter; }
       }

       /// <summary>
       /// Gets the internal match object of the pattern
       /// </summary>
       /// <returns>
       /// Returns a reference to the internal match object of the pattern.
       /// </returns>
        public virtual Match Matched
        {
            get
            {
                return match;
            }
            
        }

        /// <summary>
        /// Getter for the position increment after matching a pattern. Some pattern
        /// can match several times with different length at the same position. In
        /// this case the increment is zero until all matches are performed. For some
        /// patterns an increment greater than one can be performed, e.g. string
        /// searching with the Boyer-Moore algorithm. 
        /// </summary>
        /// <returns>
        /// Returns the increment of the search position.
        /// </returns>
        public virtual int Increment
        {
            get
            {
                return (1);
            }
        }

        #endregion

        #region -- BioPatML XML Read Component --
        /// <summary>
        /// 
        /// Reads a pattern from a starting specified node. This method
        /// recursivly calls the reading methods of the different patterns.
        /// </summary>
        /// <param name="node">Node of the pattern the reading starts with.</param>
        /// <param name="definition">Pattern definition which pattern list will be extended 
        /// with the pattern and all its sub-patterns read.
        /// </param>
        /// <returns>The read pattern or null if there is no pattern to read.</returns>
        /// <exception cref="System.SystemException">Thrown when unknown pattern was found</exception>
        public static IPattern ReadPattern
                    (XmlNode node, Definition definition)
        {
            while (node != null
                    && node.NodeType != XmlNodeType.Element)

                node = node.NextSibling; //Iterate thru the list of nodes until it is an element

            IPattern pattern = null;
            String mode = XMLHelper.GetAttrValueString(node, "mode");

            switch (node.Name)
            {
                case "Any":
                    pattern = new Any();
                    break;

                case "Alignment":
                    pattern = new Alignment();
                    break;

                case "Composition" :
                    pattern = new Composition();
                    break;

                case "Constraint":
                    pattern = new Constraint();
                    break;

                case "Iteration":
                    pattern = new Iteration();
                    break;

                case "Logic":
                    pattern = new Logic();
                    break;

                case "Motif":
                    pattern = new Motif();
                    break;

                case "PWM":
                    pattern = new PWM();
                    break;

                case "Regex":
                    pattern = new RegularExp();
                    break;

                case "Prosite":
                    pattern = new Prosite();
                    break;

                case "Block":
                    pattern = new Block();
                    break;

                case "Gap":
                    pattern = new Gap();
                    break;

                case "Repeat":
                    pattern = new Repeat();
                    break;

                case "Series":
                    pattern = mode.Equals("ALL") ? 
                        pattern = new SeriesAll() : pattern = new SeriesBest();
                    break;

                case "Set":
                    pattern = mode.Equals("ALL") ?
                       pattern = new SetAll() : pattern = new SetBest();
                    break;

                case "Void":
                    pattern = new VoidPattern();
                    break;

                case "Use":
                    pattern = new Use();
                    break;

                throw new SystemException
                    ("Unknown pattern found: " + node.Name);
 
            }

 
            pattern.ReadNode(node, definition);      // read the node data and initialize the pattern
            definition.Patterns.Add
                            (0, pattern); //Always adding the element to last index
 
            return pattern;
        }

        #endregion

        #region -- Not implemented overridable methods --

        #region IPattern Members

        /// <summary>
                /// 
                /// </summary>
                /// <param name="node"></param>
                /// <param name="definition"></param>
                public virtual void ReadNode(XmlNode node, Definition definition)
                {
                    throw new NotImplementedException();
                }

        #endregion

            #region IMatcher Members

                /// <summary>
                /// See IMatcher interface
                /// </summary>
                /// <param name="sequence"></param>
                /// <param name="position"></param>
                /// <returns></returns>
                public virtual Match Match(Sequence sequence, int position)
                {
                    throw new NotImplementedException();
                }

        #endregion

        #endregion
    }
}
