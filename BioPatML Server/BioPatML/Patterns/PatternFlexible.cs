using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
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
    ///  Abstract class describing patterns of flexible length. 
    /// </summary>
    public abstract class PatternFlexible : Pattern
    {
        #region -- Automatic Properties -- 
        /// <summary>
        /// Minimum length 
        /// </summary>
        public int MinLength { get; internal set; }
        
        /// <summary>
        /// Maximum length  
        /// </summary>
        public int MaxLength { get; internal set; }

        /// <summary>
        /// Length increment  
        /// </summary>
        public double IncLength { get; internal set; }

        /// <summary>
        /// Current length 
        /// </summary>
        private double Length { get; set; }

        /// <summary>
        /// increment for search position 
        /// </summary>
        private int increment;

        #endregion

        #region -- Constructor / Helper method (Set) --

        /// <summary>
        /// A constructor that will never be called because this is an abstract class
        /// </summary>
        /// <param name="name">Name of the pattern.</param>
        internal PatternFlexible(string name)
            : base(name) { IncLength = 1.0; }

        /// <summary>
        /// Sets the minimum and maximum length and length increment of the pattern.
        /// </summary>
        /// <param name="minLength">Minimum length of the pattern.</param>
        /// <param name="maxLength">Maximum length of the pattern.</param>
        /// <param name="incLength">Length increment for the pattern.</param>
        protected void Set(int minLength, int maxLength, double incLength)
        {
            if (minLength < 0)
                throw new ArgumentOutOfRangeException("Minimum length cannot be negative!");
            if (maxLength < minLength)
                throw new ArgumentOutOfRangeException("Maximum length smaller than minimum length!");
            if (incLength <= 0)
                throw new ArgumentOutOfRangeException("Length increment must be greater than zero!");

            MinLength = minLength;
            MaxLength = maxLength;
            IncLength = incLength;
            Length = minLength;
        }

        #endregion

        #region -- Properties --

        /// <summary>
        /// Gets the flexible length of the pattern. This is difference between
        /// the maximum and the minimum length of the pattern. 
        /// </summary>
        public int Flexiblity
        {
            get { return MaxLength - MinLength; }
        }

        /// <summary>
        /// Gets the length increment.
        /// </summary>
        public override int Increment
        {
            get
            {
                return increment;
            }
        }

        #endregion

        #region -- Protected Method --
        /// <summary>
        /// Increments the length.
        /// </summary>
        /// <returns>the old length rounded to an integer.</returns>
        protected int NextLength()
        {
            int oldLength = (int)(Length + 0.5);
            Length += IncLength;

            if (oldLength >= MaxLength)
            {
                increment = 1;
                Length = MinLength;
            }
            else
                increment = 0;

            return Math.Min(oldLength, MaxLength);
        }

        #endregion

        #region -- BioPatML XML Read Component --
        /// <summary>
        /// Reads the parameters and populate the attributes for this pattern.
        /// </summary>
        /// <param name="node">Any Pattern node that extends pattern flexible </param>
        /// <param name="definition">The container encapsulating this pattern</param>
        public override void ReadNode(XmlNode node, Definition definition)
        {

            PatternName = (XMLHelper.GetAttrValueString(node, "name"));

            Set(XMLHelper.GetAttrValInt       (node, "minimum"),
                    XMLHelper.GetAttrValInt   (node, "maximum"),
                    XMLHelper.GetAttrValDouble(node, "increment"));
        }

        #endregion
    }
}
