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
    /// A profile element describes a pattern with a gap to a preceding pattern or
    /// profile element, if there is one. It is mainly a container to aggregate
    /// the gap information and a pattern.
    /// <para></para>
    /// The gap is described by a minimum and
    /// a maximum length and refers to a position (START, END, ...) of the preceding 
    /// profile element (this is called alignment). 
    /// Profiles are built of profile elements.
    /// </summary>
    public sealed class ProfileElement
    {
        #region -- Public Constant Fields --
        /// <summary>
        /// No gap
        /// </summary>
        public const int NONE = 0; //Change Static final to const (const is already static)

        /// <summary>
        /// Gap alignment to the start of the preceding pattern match
        /// </summary>
        public const int START = 1;

        /// <summary>
        /// Gap alignment to the end of the preceding pattern match 
        /// </summary>
        public const int END = 2;

        /// <summary>
        /// Gap alignment to the center of the preceding pattern match
        /// </summary>
        public const int CENTER = 3;

        #endregion

        #region -- Properties --

        /// <summary>
        /// Minimum gap length
        /// </summary>
        public int MinGap { get; set; }

        /// <summary>
        /// Current gap length
        /// </summary>
        public int CurrGap { get; set; }

        /// <summary>
        /// Maximum gap length
        /// </summary>
        public int MaxGap { get; set; }

        /// <summary>
        /// The pattern 
        /// </summary>
        public IPattern Pattern { get; set; }

        /// <summary>
        /// Reference to the preceding profile element the gap refers to 
        /// </summary>
        public ProfileElement RefElement { get; set; }

        /// <summary>
        ///  Alignment e.g. END,START for the gap 
        /// </summary>
        public int Alignment { get; set; }

        /// <summary>
        /// Getter for the start position of the gap. This position depends on the
        /// match of the preceding pattern and the alignment.
        /// </summary>
        public int GapStart
        {
            get
            {
                Match match = RefElement.Pattern.Matched;
                switch (Alignment)
                {
                    case START: return (match.Start);
                    case END: return (match.End + 1);
                    case CENTER: return ((match.End + match.Start) / 2 + 1);
                }

                return 0;
            }
        }

        /// <summary>
        /// Resets the current gap length to the minimum length.
        /// </summary>
        public void ResetGap()
        {
            this.CurrGap = MinGap;
        }

        #endregion

        #region -- Constructor --

        /// <summary>
        /// Internal default constructor
        /// </summary>
        internal ProfileElement() { /* You are not suppose to call this */ }

        /// <summary>
        /// Creates a profile element. A profile element is a pattern with a 
        /// preceding gap to another profile element.
        /// </summary>
        /// <param name="refElement">Reference to the preceding profile element. Null if there
        /// is none. If there is gap then there must be a preceding profile element 
        /// defined!
        /// </param>
        /// <param name="alignment">
        /// Alignment the gap is based on, e.g. END, START of the 
        /// the preceding profile element or NONE if there is no preceding profile element 
        /// or no gap.
        /// </param>
        /// <param name="minGap">Minimum gap length.</param>
        /// <param name="maxGap">Maximum gap length. Must be greater than or equal to the 
        ///  minimum gap length.</param>
        /// <param name="pattern">A pattern.</param>
        public ProfileElement
            (ProfileElement refElement, int alignment, int minGap, int maxGap, IPattern pattern)
        {
            if (minGap > maxGap)
                throw new ArgumentException
                    ("Minimum gap length is greater than maximum gap length.");

            else
                if ((refElement == null || alignment == NONE)
                    && (minGap != 0 || maxGap != 0))

                    throw new ArgumentException
                        ("Missing reference or alignment to proceding pattern in gap definition.");

            this.RefElement = refElement;
            this.Alignment = alignment;
            this.MinGap = minGap;
            this.CurrGap = minGap;
            this.MaxGap = maxGap;
            this.Pattern = pattern;

        }

        #endregion
    }
}
