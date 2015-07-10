using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Sequences.Annotations;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Symbols.Accessor;
using QUT.Bio.BioPatML.Symbols;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Patterns;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Sequences
{
    /// <summary>
    /// Part of the sequence class
    /// <para></para>
    /// Consisting of all the helper methods.
    /// </summary>
    partial class Sequence
    {
        #region -- Sequence Annotations Helper Methods --

        /// <summary>
        /// Adds a list of annotations to the already existing annotations of the
        /// sequence. 
        /// </summary>
        /// <param name="annotationList"> List of annotations to add. </param>
        public void AddAnnotations(AnnotationList annotationList)
        {
            if (annotationList != null)
            {
                AnnotationList a = Annotations();

                for (int i = 0; i < annotationList.Count; i++)
                    a.Add(annotationList[i]);
            }
        }

        /// <summary>
        /// Query method if the sequence has annotations or not. This method is more
        /// efficent than calling  "Annotations().Count > 0" because no empty
        /// annotation list will be created if there are no annotations.
        /// </summary>
        /// <returns>
        /// Returns true if the sequence has at least one annotation and false otherwise.
        /// </returns>
        public bool HasAnnotations()
        {
            if (ListAnnotations == null)
                return false;
            return (ListAnnotations.Count > 0);
        }

        /// <summary>
        /// Gets the annotation value of the given annotation name. If annotation not found a null
        /// will be returned.
        /// </summary>
        /// <param name="annotationName"> Name of the annotation. </param>
        /// <returns> Returns the value of the annotation as a string or null if no
        /// annotion with the given name exits. 
        /// </returns>
        public String AnnotationValue(String annotationName)
        {
            if (ListAnnotations == null)
                return (null);

            Annotation ann = ListAnnotations[annotationName];

            return (ann == null ? null : ann.AnnotationValue.ToString());
        }


        /// <summary>
        /// Getter for the list of annotations attached to the sequence. As soon as
        /// this method is called an empty annotation list will be attached to the
        /// sequence if none is existing before.
        /// </summary>
        /// <returns></returns>
        public AnnotationList Annotations()
        {
            if (ListAnnotations == null)
                ListAnnotations = new AnnotationList();;


            return (ListAnnotations);
        }

        /// <summary>
        /// Gets the annotation object with the specified name.
        /// </summary>
        /// <param name="annotationName"> Name of the annotation. </param>
        /// <returns> Returns an annotation or null if no annotation with the given
        /// name is in the annotation list attached to the sequence.
        /// </returns>
        public Annotation Annotations(String annotationName)
        {
            if (ListAnnotations == null)
                return null;

            return (ListAnnotations[annotationName]);
        }

        #endregion -- Sequence Annotations Helper Methods --

        #region -- Sequence Feature Helper Methods --

        /// <summary>
        /// Adds a feature list to the sequence. This attaches the feature list and
        /// all it's features to the sequence. Features which were attached to a
        /// different sequence before are then attached to the current sequence!
        /// </summary>
        /// <param name="featureList"> Feature list. </param>
        public void AddFeatures(FeatureList featureList)
        {
            Features().Add(featureList);
            featureList.AttachSequence(this);
        }

        /// <summary>
        /// Adds a list of feature lists to the sequence by a already created annotatedList
        /// as well.
        /// </summary>
        /// <param name="featureLists"> List of feature lists. </param>
        public void AddFeatures(AnnotatedList featureLists)
        {
            for (int i = 0; i < featureLists.Count; i++)
                AddFeatures((FeatureList)featureLists.Get(i));
        }

        /// <summary>
        /// Query method if the sequence has features or not. This method is more
        /// efficent than calling  "features().Count > 0" because no empty
        /// feature list will be created if there are no features.
        /// </summary>
        /// <returns> 
        /// Returns true if the sequence has at least one feature and
        /// false otherwise.
        /// </returns>
        public bool HasFeatures()
        {
            if (ListFeatures == null)
                return (false);

            return (ListFeatures.Count > 0);
        }

        /// <summary>
        /// Gets a list of features attached to the sequence. If there
        /// is no list attached to sequence yet the method will create an empty one
        /// and attach it to the sequence.
        /// </summary>
        /// <returns> Returns the list of feature lists attached to the sequence. </returns>
        public AnnotatedList Features()
        {
            if (ListFeatures == null)
                ListFeatures = new AnnotatedList();

            return ListFeatures;
        }

        /// <summary>
        /// Gets the whole selected (by index) feature collections.
        /// </summary>
        /// <param name="index"> Index of the feature list. </param>
        /// <returns> Returns the feature list for the given index. </returns>
        public FeatureList Features(int index)
        {
            return ((FeatureList)Features().Get(index));
        }

        /// <summary>
        /// Gets the feature list by name.
        /// </summary>
        /// <param name="name">  Name of the feature list. </param>
        /// <returns> Returns the feature list with the given name or null if no such 
        /// list exits. 
        /// </returns>
        public FeatureList Features(String name)
        {
            return ((FeatureList)Features().Get(name));
        }


        #endregion -- Sequence Feature Helper Methods --

        #region -- Sequence Extractor Helper Methods --

        /// <summary>
        /// Extracts a subsequence using the given start and end positons.
        /// Positions can be smaller than one and bigger than the length of the
        /// sequence but the start position has always to be smaller (or equal) than 
        /// the end position.
        /// </summary>
        /// <param name="start"> Start position (first position is one). </param>
        /// <param name="end"> End position (inclusive). </param>
        /// <returns> Returns the extracted subsequence. </returns>
        public Sequence Extract(int start, int end)
        {
            return new Sequence(this, start, end, Strand);
        }

        /// <summary>
        /// Extracts a subsequence with the given length and the given offset
        /// from the start, end or center of the sequence. Note that the offset can
        /// be negative and can refer to positions outside of the sequence boundary.
        /// </summary>
        /// <param name="offset"> Offset (zero means no offset). This is the distance
        /// to the start, end or center of the sequence (see direction). The offset
        /// can be negative.
        /// </param>
        /// <param name="length"> Length of the sequence to extract. Must be greater than zero. </param>
        /// <param name="direction"> Direction and reference of the extraction: +1 = from the start, 
        /// -1 = from the end, 0 = from the center. </param>
        /// <returns> Returns the extracted subsequence. </returns>
        public Sequence Extract(int offset, int length, int direction)
        {
            if (direction > 0)
                return new Sequence
                            (this, 1 + offset, offset + length, Strand);

            else
                if (direction < 0)
                    return new Sequence
                        (this, End - offset - length + 1, End - offset, Strand);


            offset += 1 + (Length - length) / 2;

            return
                new Sequence
                        (this, offset, offset + length - 1, Strand);

        }

        #endregion  -- Sequence Extractor Helper Methods --

        #region -- Other standard Overriding Methods, ToString Equals --

        /// <summary>
        /// Compares if the sequence is equal to the given object. The object can
        /// be sequence, an instance of a class derived from the sequence class
        /// or a string. In every case the content (the sequence of letters) must
        /// be equal according to the alphabet of the sequence.
        /// </summary>
        /// <param name="obj"> Object to compare with. </param>
        /// <returns> Returns true if the sequence and the object are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return (false);

            if (this == obj)
                return true;

            if (obj is Sequence)
            {
                Sequence charSequence = (Sequence)obj;

                if (charSequence.Length != Length)
                    return false;


                for (int i = 0; i < Length; i++)
                    if (!SymbolAt(i).Equals(charSequence.SymbolAt(i)))
                        return false;

                return true;

            }

            if (obj is string || obj is String)
            {
                String strSequences = obj.ToString();

                if (strSequences.Length != Length)
                    return false;

                BioPatML.Alphabets.Alphabet alphabet = Alphabet;

                for (int i = 0; i < Length; i++)
                    if (!SymbolAt(i).Equals(alphabet[strSequences[i]]))
                        return false;

                return true;

            }

            return (false); //object is not a letter sequence of any kind
        }

        /// <summary>
        ///  Creates a string representation of the sequence. 
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return ("[" + Name + "," + this.Alphabet.Name + "," + Start + "," + End + "," + Length +
                   "," + Strand + "," + Offset + "," + Position() + "] " + Letters());
        }

        /// <summary>
        /// Returns the Hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion -- Other overriding Minor Methods --
    }
}
