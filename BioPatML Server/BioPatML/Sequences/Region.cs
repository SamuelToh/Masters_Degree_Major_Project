using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

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
    /// This class describes a region. A region is defined by it start and end
    /// position and the strand.
    /// </summary>
    [Serializable()]
    public class Region
    {
        #region Private fields

        /// <summary>
        /// Start position of the region. Always refers to the forward strand and
        ///starts with one.
        /// </summary>
        private int start;

        /// <summary>
        /// End position of the region 
        /// </summary>
        private int end;

        /// <summary>
        /// Length of the region in base pairs
        /// </summary>
        public int Length { get; internal set; }

        /// <summary>
        /// Strand the region refers to. +1 = forward strand, -1 = backward strand,
        /// 0 = n.a. or unknown. 
        /// </summary>
        public int Strand { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        ///  Creates an empty reagion. Start and end position are zero.
        /// </summary>
        public Region()
        {
            Set(0, 0, 0);
        }

        /// <summary>
        ///  Creates a region.
        /// </summary>
        /// <param name="start">Start position of the region. Always refers to the 
        ///  forward strand and starts with one.</param>
        /// <param name="end">End position of the region. Always refers to the 
        ///  forward strand and starts with one.</param>
        /// <param name="strand">  Strand the region describes. +1 = forward strand, 
        /// -1 = backward strand, 0 = n.a. or unknown. </param>
        public Region(int start, int end, int strand)
        {
            Set(start, end, strand);
        }

        #endregion

        #region -- Properties of a Region --

        /// <summary>
        ///  Getter - for the start position (counting from a sequence) of a region
        ///  Setter - Sets the start position of a region.
        /// </summary>
        public int Start
        {
            get { return this.start; }
            set
            {
                this.start = value;
                Length = end - start + 1;
            }
        }

        /// <summary>
        ///  Setter - Sets the end position of a region.
        ///  Getter - for the end position of a region. The first position within a
        ///  equence is one. The end position is the last position in the region,
        ///  which means the end position is inklusive.
        /// </summary>
        public int End
        {
            get { return this.end; }
            set
            {
                this.end = value;
                Length = end - start + 1;
            }
        }


        /// <summary>
        ///  Getter for the center position of a region. Note, that this in not
        ///  the middle of the sequence but the mean of start() and end() position
        ///  for the sequence (relative to the main sequence).
        ///  
        /// Returns the center position of a region. If the length of the
        /// region is an even number the next position to the left of the true
        /// center (which is a rational) is returned.
        /// </summary>
        public int CenterPosition
        {
            get
            {
                return
                    (Start + (End - Start) / 2);
            }
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        ///  Sets the data of a region.
        /// </summary>
        /// <param name="start"> 
        /// Start position of the region. Always refers to the
        /// forward strand and starts with one. 
        /// </param>
        /// <param name="end">  
        /// End position of the region. Always refers to the 
        /// forward strand and starts with one.
        /// </param>
        /// <param name="strand">
        ///  Strand the region refers to. +1 = forward strand, 
        ///  -1 = backward strand, 0 = n.a. or unknown.
        /// </param>
        public void Set(int start, int end, int strand)
        {
            this.start  = start;
            this.end    = end;
            Length = end - start + 1;
            Strand = strand;
        }

        /// <summary>
        ///  Tests the given position is inside the region.
        /// </summary>
        /// <param name="position">  Position (starts with one). </param>
        /// <returns> true: if the given position is inside the region, false otherwise. </returns>
        public bool IsInside(int position)
        {
            return (position >= Start
                 && position <= End);
        }

        /// <summary>
        ///  Returns a string representation of a region which displays start position,
        ///  end position and strand of the region.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append( "{ " + Start + ", " + End + ", ");
            sb.Append( Strand < 0 ? "- }" : "+ }");

            return sb.ToString();
        }

        #endregion
    }
}
