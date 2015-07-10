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
    ///  This class describes a list of sequences.
    /// </summary>
    public class SequenceList : RegionList
    {
        #region -- Public Constructors --
        /// <summary>
        ///  Creates an empty sequence list.
        /// </summary>
        public SequenceList()
            :base()
        { /* No implementation */ }

        /// <summary>
        ///  Creates an empty sequence list with the given name.
        /// </summary>
        /// <param name="name"></param>
        public SequenceList(String name)
            : base(name)
        { /* No implementation */ }

        #endregion

        #region -- Public Methods --

        /// <summary>
        ///  This method creates a new empty sequence list. 
        /// </summary>
        /// <param name="name"> Name of the list to create. Can be null. </param>
        /// <returns> Returns an sequence list casted as an annotated list. </returns>
        protected override AnnotatedList Create(String name)
        {
            return (new SequenceList(name));
        }

        /// <summary>
        /// Gets a sequence by its specified position
        /// </summary>
        /// <param name="index">index position of sequence in this list</param>
        /// <returns></returns>
        public new Sequence this[int index]
        {
            get
            {
                return ((Sequence)Get(index));
            }
        }

        /// <summary>
        /// Gets the sequence from list by supplying a specified name
        /// </summary>
        /// <param name="name"> name of sequence </param>
        /// <returns></returns>
        public Sequence this[string name]
        {
            get
            {
                return ((Sequence)Get(name));
            }
        }
 

      

        /// <summary>
        /// Creates a feature list with all features of the sequences of the list
        /// which match the given feature name .
        /// </summary>
        /// <param name="featureListName">
        /// Name of the feature lists which contain the features to extract.
        /// </param>
        /// <param name="featureName">Feature name.</param>
        /// <returns>
        /// Returns a feature list with all features which name matches the
        /// given feature name over all sequences of the sequence list.
        /// 
        /// </returns>
        public FeatureList Features(String featureListName, String featureName)
        {
            FeatureList featureList = new FeatureList(featureListName);

            for (int i = 0; i < Count; i++)
            {
                FeatureList list = this[i].Features(featureListName);
                if (list != null)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        Feature feature = list.Feature(j);
                        if (feature.AnnotationValue("Name").Equals(featureName))
                            featureList.Add(feature);
                    }
                }
            }
            return (featureList);
        }

        #endregion
    }
}
