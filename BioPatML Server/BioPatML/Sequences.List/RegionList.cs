using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/***************************************************************************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Sequences.List
{
    /// <summary>
    ///  This class implements a list of {@link Region}s and serves as a base class
    ///  for other lists as {@link SequenceList} and {<see> FeatureList </see>}.
    /// </summary>
    public class RegionList : AnnotatedList
    {
        #region -- Public Constructors --
        /// <summary>
        ///  Creates an empty region list.
        /// </summary>
        public RegionList() : base(){}

        /// <summary>
        ///  Creates a region list with the given name.
        /// </summary>
        /// <param name="name"></param>
        public RegionList(String name)
            : base (name)
        { /* No implementation required */ }
        #endregion

        #region -- Public Methods --

        /// <summary>
        ///  This method creates a new empty region list. 
        /// </summary>
        /// <param name="name"> Name of the list to create. Can be null. </param>
        /// <returns> Returns an region list casted as an annotated list. </returns>
        protected override AnnotatedList Create(String name)
        {
            return (new RegionList(name));
        }

        /// <summary>
        /// Gets a region by its specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public new Region this[int index]
        {
            get
            {
                return (Region)Get(index);
            }
        }

        /// <summary>
        ///  Calculates the minimum length of all regions in the list.
        /// </summary>
        /// <returns> Returns the minimum length. </returns>
        public int MinLength()
        {
            int min = int.MaxValue;
            int length = 0;

            for (int i = 0; i < Count; i++)
            {
                length = this[i].Length;

                if (length < min)
                    min = length;
            }

            return (min);
        }

        /// <summary>
        ///   Calculates the maximum length of all regions in the list.
        /// </summary>
        /// <returns> Returns the maximum length. </returns>
        public int MaxLength()
        {
            int max = int.MinValue;
            int length = 0;

            for (int i = 0; i < Count; i++)
            {
                length = this[i].Length;

                if (length > max)
                    max = length;
            }

            return (max);
        }

        /// <summary>
        ///  Calculates the average length of all regions in the list.
        /// </summary>
        /// <returns> Returns the average length. </returns>
        public double AverageLength()
        {
            int sum = 0;
            int size = Count;

            for (int i = 0; i < size; i++)
                sum += this[i].Length;

            return (size > 0 ? ((double)sum) / size : 0);
        }

        #endregion
    }
}
