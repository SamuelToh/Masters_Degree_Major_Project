using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    /// This abstract class provides methods for complex patterns, which are composed
    /// of multiple sub-patterns.
    /// </summary>
    public abstract class PatternComplex : Pattern
    {
        #region -- Automatic Properties --
        /// <summary>
        /// List of the sub-pattern the complex pattern is composed of 
        /// </summary>
        public PatternList Patterns { get; internal set; }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// Default constructor.
        /// 
        /// Not important as this class can never be created. 
        /// However all lower level patterns will bypass this constructor
        /// </summary>
        /// <param name="name">Name of the pattern.</param>
        internal PatternComplex(String name)
            : base(name) 
        {
            Patterns = new PatternList();
        }
        #endregion

        #region -- Properties --
        /// <summary>
        /// Gets a pattern by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IPattern this[int index]
        {
            get { return Patterns[index]; }
        }

        /// <summary>
        /// Gets a specified pattern by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IPattern this[string name]
        {
            get { return Patterns[name]; }
        }

        /// <summary>
        /// Returns the total number of patterns within ComplexPattern
        /// </summary>
        public int Count
        {
            get { return Patterns.Count; }
        }

        #endregion
    }
}
