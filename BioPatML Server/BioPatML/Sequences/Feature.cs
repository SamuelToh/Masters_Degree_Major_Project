using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Sequences.Annotations;
using QUT.Bio.BioPatML.Symbols.Accessor;

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
    /// This class describes a feature. A feature stores information about
    /// a region of a sequence and is described by a region, a list of
    /// properties and a reference to the sequence the feature is related to.
    /// <para></para>
    /// Note that a feature extends the sequence class and all methods which
    /// are available for sequences are therefore also applicable to 
    /// features.
    /// </summary>
    public class Feature : Sequence
    {
        #region -- Constructors --
        /// <summary>
        /// Creates an empty feature.
        /// </summary>
        internal Feature() : base()
        { /* No implementation */ }

        /// <summary>
        ///  Creates a feature with the given name. A feature contains information
        ///  about a section of a sequence, e.g. locus of a certain gene.
        /// </summary>
        /// <param name="name"> Name of the feature, e.g. name of a gene. </param>
        /// <param name="start">
        /// Start position of the feature within the sequence. 
        /// The first symbol in a sequence has start one and the start
        /// refers to the forward strand.
        /// </param>
        /// <param name="end">
        /// End position of the feature within the sequence. First position
        /// is one. End position must be bigger than the start position.
        /// </param>
        /// <param name="strand">
        /// Strand the feature belongs to. +1 = forward strand, 
        /// -1 = backward strand, 0 = n.a. or unknown. 
        /// </param>
        public Feature
            (String name, int start, int end, int strand)
            : base (start, end, strand)
        {
            if (name != null)
                Annotations().Add("Name", String.Intern(name));
        }

        /// <summary>
        ///  Creates a feature based on a given sequence. The name of this feature
        ///  is set to the name of the sequence. The main sequence of the sequence
        ///  is also set to the main sequence and the base sequence is
        ///  the sequence itself. 
        ///  <para></para>
        ///  The Main and based sequence can be quite confusing, they are both different items.
        ///  A main sequence is the long original sequence while the base sequence is the
        ///  short region of the main sequence.
        ///  <para></para> Please see 
        /// <see cref="QUT.Bio.BioPatML.Sequences.Sequence">
        /// Sequence
        /// </see> to understand the overall data structure
        /// </summary>
        /// <param name="name"> Name of the feature. </param>
        /// <param name="sequence"> 
        /// Sequence the feature is based on. Note that a feature can be 
        /// based on another feature since a feature is derived from the sequence class.
        /// </param>
        /// <param name="start">
        /// Start position of the feature within the sequence. 
        /// The first symbol in a sequence has start one and the start
        /// refers to the forward strand.
        /// </param>
        /// <param name="end">
        /// End position of the feature within the sequence. First position
        /// is one. End position must be bigger than the start position.
        /// </param>
        /// <param name="strand">
        /// Strand the feature belongs to. +1 = forward strand,
        /// -1 = backward strand, 0 = n.a. or unknown. 
        /// </param>
        public Feature
            (String name, Sequence sequence, int start, int end, int strand)
            : base(sequence, start, end, strand)
        {
            Annotations().Add("Name", name);
        }

        /// <summary>
        ///  Creates a feature. The name of the feature is automatically set to the name
        ///  of the sequence.
        /// </summary>
        /// <param name="sequence"> Sequence the feature is based on. </param>
        /// <param name="start"> Start position of the feature within the sequence. </param>
        /// <param name="end"> End position of the feature within the sequence. </param>
        /// <param name="strand"> Strand the feature belongs to. </param>
        public Feature
            (Sequence sequence, int start, int end, int strand)
            : base(sequence, start, end, strand)
        {
            Annotations().Add("Name", sequence.Name);
        }

        /// <summary>
        ///  Creates a feature of two joined sequences.
        /// </summary>
        /// <param name="sequence1"> First sequence. </param>
        /// <param name="sequence2"> Second sequence. </param>
        public Feature(Sequence sequence1, Sequence sequence2)
            : base(sequence1, sequence2)
        { /* No implementation */ }

  
#endregion

        #region -- Public Methods --
        
        /// <summary>
        ///  Setter for the sequence the feature is attached/refers to. This method is 
        ///  automatically called when the feature is added to a feature list. 
        /// </summary>
        /// <param name="sequence"> Sequence </param>
        public void SetSequence(Sequence sequence)
        {
            if (sequence != null)
            {
                if(Start == 0 && End == 0 && Strand == 0)
                    throw new ArgumentOutOfRangeException
                        ("No start and end defined \n start =" + Start + " end = " + End +
                            " strand = " + Strand );
                
                InitSubsequence(Strand < 0 ? AccessorFactory.TRANS_REV_COMP :
                                                    AccessorFactory.TRANS_DIR, sequence, Start, End);

            }
        }

        /// <summary>
        /// Calculates the distance between the start positions of two features. If the 
        /// second feature has a smaller position than the current feature it is assumed 
        /// that the sequence is cyclic and the distance is calculated the other way
        /// around. Therefore the distance is always positive. 
        /// Note that the feature MUST be attached to a sequence! Otherwise a null
        /// pointer exception will occur.
        /// </summary>
        /// <param name="feature"> Second feature. </param>
        /// <returns> 
        /// Returns the distance between the two features (feature.start -
        /// this.start if feature.start >= this.start).
        /// </returns>
        public int DistanceStartStart(Feature feature)
        {
            if (feature.Start < this.Start)
                return (base.Length - this.Start + feature.Start);

            return (feature.Start - this.Start);
        }

        /// <summary>
        /// Calculates the distance between the end positions of two features. If the 
        /// second feature has a smaller position than the current feature it is assumed
        /// that the sequence is cyclic and the distance is calculated the other way
        /// around. Therefore the distance is always positive. 
        /// Note that the feature MUST be attached to a sequence! Otherwise a null
        /// pointer exception will occur.
        /// </summary>
        /// <param name="feature"> Second feature. </param>
        /// <returns>
        /// Returns the distance between the two features (feature.end -
        /// this.end if feature.end >= this.end).
        /// </returns>
        public int DistanceEndEnd(Feature feature)
        {
            if (feature.End < this.End)
                return (this.BaseSequence.Length - this.End + feature.End);

            return (feature.End - this.End);
        }

        /// <summary>
        ///  Calculates the distance between the start position of the current feature
        ///  and the end position of the second feature. If the  second feature has a 
        ///  smaller end position than the current feature it is assumed 
        ///  that the sequence is cyclic and the distance is calculated the other way
        ///  around. Therefore the distance is always positive. 
        ///  Note that the feature MUST be attached to a sequence! Otherwise a null
        ///  pointer exception will occur.
        /// </summary>
        /// <param name="feature"> Second feature. </param>
        /// <returns>
        /// Returns the distance between the two features (feature.end -
        /// this.start if feature.end >= this.start).
        /// </returns>
        public int DistanceStartEnd(Feature feature)
        {
            if (feature.End < this.Start)
                return (this.BaseSequence.Length - this.Start + feature.End);

            return (feature.End - this.Start);
        }

        /// <summary>
        /// Calculates the distance between the end position of the current feature
        /// and the start position of the second feature. If the  second feature has a 
        /// smaller start position than the current feature it is assumed 
        /// that the sequence is cyclic and the distance is calculated the other way
        /// around. Therefore the distance is always positive. 
        /// Note that the feature MUST be attached to a sequence! Otherwise a null
        /// pointer exception will occur.
        /// </summary>
        /// <param name="feature"> Second feature. </param>
        /// <returns> Returns the distance between the two features (feature.start -
        /// this.end if feature.start >= this.end). 
        /// </returns>
        public int DistanceEndStart(Feature feature)
        {
            if (feature.Start < this.End)
                return (this.BaseSequence.Length - this.End + feature.Start);

            return (feature.Start - this.End);
        }

        /// <summary>
        /// Determines if the region of the current feature overlaps with the region
        /// of the given feature.
        /// </summary>
        /// <param name="feature"> A feature. </param>
        /// <returns>
        /// True, if the regions of the features are overlapping. False,
        /// otherwise.
        /// </returns>
        public bool IsOverlapping(Feature feature)
        {
            if (feature.End >= this.Start
                        && feature.End <= this.End)
                return true;

            if (feature.Start >= this.Start
                        && feature.Start <= this.End)
                return true;

            if (feature.Start <= this.Start
                        && feature.End >= this.End)
                return true;


            return false;
        }

        /// <summary>
        /// Tests if the position within the given sequence is inside the feature.
        /// Note that the region of a feature refers to the sequence the feature
        /// is attached to (if there is any). This is taken into account when
        /// testing the given position. 
        /// </summary>
        /// <param name="sequence"> Sequence the given position refers to. </param>
        /// <param name="position"> Position (starts with one). </param>
        /// <returns> True: if the given position is inside the feature region, 
        /// false otherwise. </returns>
        public bool IsInside(Sequence sequence, int position)
        {
            if (this.BaseSequence != null)
                position = BaseSequence.Position(sequence, position);

            return base.IsInside(position);
        }

        /// <summary>
        ///  Calculates the upstream distance to the given feature. Attention:  The
        ///  calculated distance can be negative if the features are overlapping. 
        ///  The method takes into account circular sequences and features 
        ///  on both strands.
        /// </summary>
        /// <param name="feature">
        ///  Feature the upstream distance is calculated to. If the
        ///  feature is not in the upstream region to the end of the sequnce but the
        ///  sequence is circular, the distance is calculated accross the sequence 
        ///  boundary. For linear sequences the distance to the corresponding sequence 
        ///  terminus is calculated. 
        /// </param>
        /// <returns></returns>
        public int DistanceUpstream(Feature feature)
        {
            int dist = (Strand > 0 ? Start - feature.End : feature.Start - End) - 1;

            if ((feature.Start - Start) * Strand > 0)
            {
                if (BaseSequence.IsCircular())
                    dist = BaseSequence.Length + dist;

                else
                    dist = Strand > 0 ? Start - 1 : BaseSequence.Length - End;
            }

            return dist;

        }

        /// <summary>
        /// Calculates the downstream distance to the given feature. Attention:  The
        /// calculated distance can be negative if the features are overlapping. 
        /// The method takes into account circular sequences and features 
        /// on both strands.
        /// </summary>
        /// <param name="feature">
        /// Feature the downstream distance is calculated to. If the
        /// feature is not in the downstream region to the end of the sequnce but the
        /// sequence is circular, the distance is calculated accross the sequence 
        /// boundary. For linear sequences the distance to the corresponding sequence 
        /// terminus is calculated. 
        /// </param>
        /// <returns>
        /// Returns the downstream distance to the given feature.
        /// </returns>
        public int DistanceDownstream(Feature feature)
        {
            int dist = (Strand > 0 ? feature.Start - End : Start - feature.End) - 1;

            if ((End - feature.End) * Strand > 0)
            {
                if (BaseSequence.IsCircular())
                    dist = BaseSequence.Length + dist;

                else
                    dist = Strand > 0 ? BaseSequence.Length - End : Start - 1;
            }

            return dist;
        }

        /// <summary>
        /// Creates a string representation of a feature.
        /// </summary>
        /// <returns> Representation of a feature. </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name + ": " + Letters() + "'\n");
            sb.Append("    { " + Start + ", " + End + ", ");
            sb.Append(Strand < 0 ? "- } \n" : "+ }\n");

            AnnotationList annotations = Annotations();

            for (int i = 0; i < annotations.Count; i++)
            {
                Annotation a = annotations[i];
                sb.Append("    " + a.Name + "=" + a.AnnotationValue + "\n");
            }

            return sb.ToString();
        }

        #endregion
    }
}
