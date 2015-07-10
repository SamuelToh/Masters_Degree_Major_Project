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
    ///   A feature list is a list of features, typically attached to a sequence. 
    ///   A list of gene locations in a genome for example.
    /// </summary>
    public sealed class FeatureList : SequenceList
    {
        #region Private Field

        /** Reference to the sequence the feature list is attached to */
        private Sequence sequence;

        #endregion

        #region -- Public Constructors --

        /// <summary>
        ///  Creates an empty feature list.
        /// </summary>
        public FeatureList() { }

        /// <summary>
        ///  Creates an empty feature list with the given name.
        /// </summary>
        /// <param name="name"> Name of the list, e.g. "Promotor locations".
        /// <see>AnnotatedList#AnnotatedList(String)</see></param>
        public FeatureList(String name)
        {
            Annotations()
                .Add("Name", name);
        }

        #endregion

        #region -- Protected Methods --

        /// <summary>
        ///  This method creates a new empty feature list. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override AnnotatedList Create(string name)
        {
            //Check on this method...
            return base.Create(name);
        }

      
        #endregion

        #region -- Public Methods --
        /// <summary>
        ///  Attaches the feature list to the given sequence. This method is 
        ///  automatically called when the feature list is added to a sequence. It
        ///  sets the sequence for all features within the feature list which were 
        ///  attached to the former sequence of the feature list. This means
        ///  features which were attached to a different sequence before remain
        ///  unchanged.
        /// </summary>
        /// <param name="sequence">Sequence</param>
        public void AttachSequence(Sequence sequence)
        {
            Feature feature;
            for (int i = 0; i < Count; i++)
            {
                feature = this.Feature(i);

                if (feature.BaseSequence == this.sequence)
                    feature.SetSequence(sequence);
            }

            this.sequence = sequence;
        }

        /// <summary>
        /// Gets for a feature by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Feature Feature(int index)
        {
            return ((Feature)Get(index));
        }

        /// <summary>
        /// Getter for a feature. Just a wrapper for AnnotatedList Get(String) method
        /// that casts to a feature.
        /// </summary>
        /// <param name="name">Name of the feature. </param>
        /// <returns> Returns the feature for the given name or null if no such
        /// feature exists.</returns>
        public Feature Feature(String name)
        {
            return ((Feature)Get(name));
        }

        /// <summary>
        /// Adds a feature to the list. The feature is assigend to the same sequence
        /// the feature list belongs to. If the feature list is not attached to a
        /// sequence the assignment of the feature will not be changed.
        /// </summary>
        /// <param name="feature"> Reference to a feature to add. </param>
        public void Add(Feature feature)
        {
            Add(feature, sequence != null);
        }

        /// <summary>
        ///  Adds a feature at a specific index position to the list.
        ///  <see> #add(Feature) </see>
        /// </summary>
        /// <param name="index"> Index position for insertion.</param>
        /// <param name="feature"> Reference to a feature to add.</param>
        public void Add(int index, Feature feature)
        {
            Add(index, feature, sequence != null);
        }

        /// <summary>
        ///  Adds a feature to the list.
        /// </summary>
        /// <param name="feature"> Reference to a feature to add. </param>
        /// <param name="setSequence"> true: sets the sequence the feature is attached to 
        /// to the sequence the feature list is attached to; false: the attachement
        /// of the feature will not be changed. </param>
        public void Add(Feature feature, bool setSequence)
        {
            base.Add(feature);
            if (setSequence)
                feature.SetSequence(sequence);
        }

        /// <summary>
        ///  Adds a feature at a specific index position to the list.
        ///  <see>#add(Feature, boolean)</see>
        /// </summary>
        /// <param name="index"> Index position for insertion. </param>
        /// <param name="feature"> Reference to a feature to add. </param>
        /// <param name="setSequence">sets the sequence the feature is attached to
        /// to the sequence the feature list is attached to; false: the attachement
        /// of the feature will not be changed.
        /// </param>
        public void Add(int index, Feature feature, bool setSequence)
        {
            base.Insert(index, feature);

            if(setSequence)
                feature.SetSequence(sequence);
        }

        /// <summary>
        ///  Adds the features of the given list to the list.
        /// </summary>
        /// <param name="featureList"> A  feature list.</param>
        /// <param name="setSequence"> <see> #add(Feature, boolean </see></param>
        public void Append(FeatureList featureList, bool setSequence)
        {
            for (int i = 0; i < featureList.Count; i++)
                Add(featureList.Feature(i), setSequence);
        }

        /// <summary>
        ///  Finds the first feature which contains the given position.
        /// </summary>
        ///<param name="sequence"> Sequence the given position refers to.</param>
        ///<param name="position"> Position (starts with one).</param>
        ///<returns> Returns the first feature which covers the given position or
        /// null if none of the features in the list contain the given position. 
        ///</returns>
        public Feature Inside(Sequence sequence, int position)
        {
            for (int i = 0; i < Count; i++)
                if (Feature(i).IsInside(sequence, position))
                    return (Feature(i));

            return (null);
        }

        /// <summary>
        ///  Creates a string representation of a feature list.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Count; i++)
                sb.Append(Feature(i).ToString());
            return (sb.ToString());
        }

        #endregion
    }
}
