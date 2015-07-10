using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Common.Structures;
using System.Xml;
using QUT.Bio.BioPatML.Common.XML;
using QUT.Bio.BioPatML.Sequences;

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
    /// This class defines a gap pattern. This pattern matches any sequence
    /// within a specified length interval and can have a similiarity score
    /// based on a length distribution. 
    /// </summary>
    public sealed class Gap : PatternFlexible
    {
        #region -- Automatic Properties --

        /// <summary>
        /// Gap similarities by its length distribution.
        /// External party can not modify this variable only internal member
        /// has permission to do so.
        /// </summary>
        private double[] GapSimArr { get; set; }

        #endregion

        #region -- Constructors -- 

        /// <summary>
        /// Default constructor used to build an empty Gap with a unique name
        /// </summary>
        internal Gap() : base("Gap" + Id.ToString()) { /* No implementation */ }

        /// <summary>
        /// Constructs a gap pattern of variable length.
        /// </summary>
        /// <param name="name">Name of Gap Pattern</param>
        /// <param name="minLength">Minimum length of the gap (Can be negative).</param>
        /// <param name="maxLength">Maximum length of the gap. </param>
        /// <param name="incLength">Length increment for the gap.</param>
        /// <param name="weights">
        /// Weights for different lengths of the gap. The first weight
        /// is the weight for a gap of minLength. Additional weights are for extended
        /// gap lengths. The weights are automatically scaled to [0..1].
        /// </param>
        /// <param name="threshold">Threshold for the gap.</param>
        /// <exception cref="System.ArgumentException">Thrown only when 
        /// the maximum length is not bigger than the minimum length.</exception>
        public Gap
            (string name, int minLength, int maxLength, 
             double incLength, double[] weights, double threshold)
            : base(name)
        {
            base.Set(minLength, maxLength, incLength);
            Weights = (weights);
            Threshold = threshold;
        }

        /// <summary>
        /// Constructs a gap pattern of variable length. Same as 
        /// Gap(String, int, int, double, double, double[])
        /// but with fixed length increment of 1.
        /// </summary>
        /// <param name="name">Name of the pattern.</param>
        /// <param name="minLength">Minimum length of the gap.</param>
        /// <param name="maxLength">Maximum length of the gap.</param>
        /// <exception cref="System.ArgumentException">Thrown only when 
        /// the maximum length is not bigger than the minimum length.</exception>
        public Gap(String name, int minLength, int maxLength)
            : base(name)
        {
            base.Set(minLength, maxLength, 1.0);
            Threshold = 0.0;
            Weights = (null);
        }  

        /// <summary>
        /// Constructs a gap pattern of variable length. Same as 
        /// constructor Gap(int, int, double[]) but without weights.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="minLength">Minimum length of the gap</param>
        /// <param name="maxLength">Maximum length of the gap.</param>
        /// <param name="incLength"></param>
        /// <exception cref="System.ArgumentException">Thrown only when 
        /// the maximum length is not bigger than the minimum length.</exception>
        public Gap(string name, int minLength, int maxLength, double incLength)
            : base(name)
        {
            base.Set(minLength, maxLength, incLength);
            Threshold = 0.0;
            Weights = (null);
        }

        #endregion

        #region -- Public Methods -- 

        /// <summary>
        /// Sets the gap weights. All weights must be greater or equal to zero.
        /// Weights internally automatically scaled to the interval [0..1]. 
        /// <para></para>
        /// A weight vector with constant values is transformed to a
        /// weight vector with all elements set to one.
        /// </summary>
        public double[] Weights//(double[] weights)
        {
            internal get { return GapSimArr; }
            set
            {
                if (value != null)
                {
                    double min = value[SArray.MinIndex(value)];
                    double max = value[SArray.MaxIndex(value)];
                    int len = value.Length;

                    if (len == 0)
                        throw new ArgumentException
                            ("Invalid numer of weights!");

                    GapSimArr = new double[len];

                    for (int i = 0; i < len; i++)
                        GapSimArr[i] = max == min ? 1.0 : (value[i] - min) / (max - min);


                }
                else
                    GapSimArr = null;
            }
        }

        /// <summary>
        /// Returns the gap similarity score according to the given length.
        /// </summary>
        /// <param name="length">Current gap length.</param>
        /// <returns>Returns the similarity score for this gap length.</returns>
        public double TabulateGapSim(int length)
        {
            if (GapSimArr == null)
                return 1.0;

            else 
                if (length <= 0)
                    return GapSimArr[0];

            else
                    if (length >= GapSimArr.Length)
                        return GapSimArr[GapSimArr.Length - 1];

            return GapSimArr[length];
        }

        #endregion

        #region -- IMatcher Members --
        /// <summary>
        /// Implementation of the pattern interface. A gap pattern matches any sequence.
        /// <see cref="QUT.Bio.BioPatML.Patterns.IMatcher">IMatcher interface</see>.
        /// </summary>
        /// <param name="sequence">Sequence to compare with.</param>
        /// <param name="position">Matching position.</param>
        /// <returns></returns>
        public override Match Match
            (Sequence sequence, int position)
        {
           //New matching code 
            int length = NextLength();
            Matched.Set(sequence, position, length, sequence.Strand, TabulateGapSim(length - MinLength));
            return Matched;
        }

        #endregion

        #region -- BioPatML XML Read Component -- 

        /// <summary>
        /// Implementation of the pattern interface.
        /// Reads in the Gap node and populate the attributes accordingly.
        /// <see cref="QUT.Bio.BioPatML.Patterns.IPattern">IPattern</see>
        /// </summary>
        /// <param name="node"></param>
        /// <param name="definition"></param>
        public override void ReadNode(XmlNode node, Definition definition)
        {
            base.ReadNode(node, definition);
            Threshold = XMLHelper.GetAttrValDouble(node, "threshold");
            Impact    = XMLHelper.GetAttrValDouble(node, "impact");
            String weightstr = XMLHelper.GetTextContent(node, "Weights");

            Weights = (weightstr == null ? null : PrimitiveParse.StringToDoubleArray(weightstr));

        }

        #endregion
    }
}
