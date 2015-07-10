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
    /// This abstract class provides basic functionality for profiles. A profile is a 
    /// sequence of patterns described by weight matrices, consensus patterns, 
    /// regular expression, motifs, ... which are usually separated by gaps.<para></para>
    /// This class handles only the administrative aspects but does not implement
    /// any matching strategies between a profile and a sequence.
    /// Patterns are stored as <see cref="QUT.Bio.BioPatML.Patterns.ProfileElement">
    /// ProfileElement </see> which are an aggregation of
    /// a pattern and its preceding gap.
    /// </summary>
    public abstract class Profile : PatternComplex
    {
        #region -- Protected Fields --
        
        /// <summary>
        /// List of patterns and gaps
        /// </summary>
        protected List<ProfileElement>
            patternList = new List<ProfileElement>();

        #endregion

        #region -- Constructors -- 
        /// <summary>
        /// The default constructor for profile
        /// </summary>
        /// <param name="name"></param>
        public Profile(String name) : base(name) { }

        #endregion

        #region -- Properties --

        /// <summary>
        /// <see cref="QUT.Bio.BioPatML.Patterns.IPattern">IPattern #Increment()</see>
        /// </summary>
        /// <returns></returns>
        public override int Increment
        {
            get
            {
                int maxInc = -int.MaxValue;
                int gapSum = 0;

                for (int i = 0; i < Count; i++)
                {
                    ProfileElement element = this[i];
                    gapSum += element.MaxGap;
                    int inc = element.Pattern.Increment - gapSum;

                    if (inc > maxInc)
                        maxInc = inc;
                }

                return (Math.Max(1, maxInc));
            }
        }

        /// <summary>
        /// Gets the number of patterns/elements of the profile.
        /// </summary>
        public new  int Count
        {
            get { return patternList.Count; }
        }

        /// <summary>
        /// Gets a profile element based on the given index.
        /// <para></para>
        /// Note that all patterns in a profile are stored 
        /// as <see cref="QUT.Bio.BioPatML.Patterns.ProfileElement">ProfileElement</see> 
        /// which is a gap description followed by the genuine
        /// pattern.
        /// </summary>
        /// <param name="index">Index within pattern list.</param>
        /// <returns>Returns a profile element or null if the index is invalid.</returns>
        public new ProfileElement this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    return (null);

                return (patternList[index]);
            }
        }

        #endregion

        #region -- Public Methods --

        #region [Add Methods] - For adding a pattern description into the profile

        /// <summary>
        ///
        /// Adds a pattern description, e.g. a sequence, a weight matrix to the 
        /// profile. This method assumes that the pattern follows the preceding
        /// pattern in the profile (if there is one) and that there is no gap
        /// between the patterns.
        /// 
        /// </summary>
        /// <param name="pattern">Any object which implements the pattern interface.</param>
        /// <returns>Returns a reference to the added {ProfileElement}.</returns>
        public ProfileElement Add(IPattern pattern)
        {
            return (Add(this[Count - 1], ProfileElement.END, 0, 0, pattern));
        }

        /// <summary>
        /// Adds a pattern preceeded by a gap of variable length to the profile. This
        /// pattern can't be the first pattern of the profile because the gap is
        /// defined between the given pattern and a preceding pattern!
        /// </summary>
        /// <param name="alignment">Alignment the gap is based on, e.g. END, START of the
        /// the preceding profile element or NONE if there is no preceding profile element 
        /// or no gap.
        /// </param>
        /// <param name="minGap">Minimum gap length.</param>
        /// <param name="maxGap">Maximum gap length. Must be greater than or equal to the 
        /// minimum gap length.</param>
        /// <param name="pattern">Any object which implements the pattern interface.</param>
        /// <returns>Returns a reference to the added { ProfileElement}.</returns>
        public ProfileElement Add
            (int alignment, int minGap, int maxGap, IPattern pattern)
        {
            if (Count == 0)
                throw new ArgumentException
                    ("First pattern of a profile must not have a gap!");

            return
                (Add(this[Count - 1], alignment, minGap, maxGap, pattern));
        }

        /// <summary>
        /// Adds a pattern preceeded by a gap of variable length to the profile. This
        /// pattern can't be the first pattern of the profile because the gap is
        /// defined between the given pattern and a preceding pattern! The gap
        /// is assumed to start at the end of the preceding pattern.
        /// Use #add(IPattern) to add the first ungapped pattern to the profile.
        /// </summary>
        /// <param name="minGap">Minimum gap length.</param>
        /// <param name="maxGap">Maximum gap length. Must be greater than or equal to the 
        /// inimum gap length.</param>
        /// <param name="pattern">Any object which implements the pattern interface.</param>
        /// <returns>a reference to the added {@link ProfileElement}.</returns>
        public ProfileElement Add(int minGap, int maxGap, IPattern pattern)
        {
            return (Add(this[Count - 1], ProfileElement.END, minGap, maxGap, pattern));
        }


        /// <summary>
        /// 
        /// Adds a pattern preceeded by a gap of variable length to the profile.
        /// </summary>
        /// <param name="refElement">
        /// refElement Reference to the preceding profile element. Null if there
        /// is none. If there is gap then there must be a preceding profile element 
        /// defined!
        /// </param>
        /// <param name="alignment">Alignment the gap is based on, e.g. END, START of the 
        /// the preceding profile element or NONE if there is no preceding profile 
        /// element or no gap.</param>
        /// <param name="minGap">Minimum gap length.</param>
        /// <param name="maxGap">Maximum gap length. Must be greater than or equal to the 
        /// minimum gap length.</param>
        /// <param name="pattern">Any object which implements the pattern interface.</param>
        /// <returns></returns>
        public ProfileElement Add
            (ProfileElement refElement, int alignment, int minGap, int maxGap, IPattern pattern)
        {
            ProfileElement element = new ProfileElement
                                            (refElement, alignment, minGap, maxGap, pattern);

            patternList.Add(element);
            Matched.Add(pattern.Matched);
            return (element);
        }

        /// <summary>
        ///  Adds a pattern preceeded by a gap of variable length to the profile.
        /// </summary>
        /// <param name="elementIndex">
        /// Index to a preceding profile element. If the index
        /// smaller than zero it is assumed that there is no preceding profile element.
        /// In this case minGap and maxGap must be zero!
        /// Please note that the same pattern can not be added twice except it it
        /// a copy (with its own internal match object).
        /// </param>
        /// <param name="alignment">
        /// Alignment the gap is based on, e.g. END, START of the 
        /// the preceding profile element or NONE if there is no preceding profile element 
        /// or no gap.
        /// </param>
        /// <param name="minGap"> The min length</param>
        /// <param name="maxGap">
        /// Maximum gap length. Must be greater than or equal to the 
        /// minimum gap length.
        /// </param>
        /// <param name="pattern">Any object which implements the pattern interface.</param>
        /// <returns>Returns a reference to the added {ProfileElement}.</returns>
        public ProfileElement Add
            (int elementIndex, int alignment, int minGap, int maxGap, IPattern pattern)
        {
            if (IndexOf(pattern) >= 0)
                throw new ArgumentException
                    ("The same pattern can not be added twice to a profile!");

            ProfileElement element = new ProfileElement
                                            (this[elementIndex], alignment, minGap, maxGap, pattern);

            patternList.Add(element);
            Matched.Add(pattern.Matched);
            return (element);
        }

        #endregion

        /// <summary>
        /// Getter for a pattern. 
        /// </summary>
        /// <param name="index">Your requested index</param>
        /// <returns>Returns the number of patterns/elements.</returns>
        public IPattern Pattern(int index)
        {
            return (this[index].Pattern);
        }

        /// <summary>
        /// Getter for the index of the given profile element.
        /// </summary>
        /// <param name="element">Profile element.</param>
        /// <returns>
        /// Index of the given profile element within the profile or -1 if
        /// the profile element is not part of the profile.
        /// </returns>
        public int IndexOf(ProfileElement element)
        {
            return (patternList.IndexOf(element));
        }

        /// <summary>
        /// Getter for the index of the given pattern.
        /// </summary>
        /// <param name="pattern">Pattern reference.</param>
        /// <returns>
        /// Index of the given pattern within the profile or -1 if the
        /// pattern is not part of the profile.
        /// </returns>
        public int IndexOf(IPattern pattern)
        {
            for (int i = 0; i < Count; i++)
                if (this[i].Pattern == pattern)
                    return (i);

            return -1; //not found
        }

        #endregion

        #region -- Protected Methods --

        /// <summary>
        /// Converts a string with the alignment to an integer constant.
        /// </summary>
        /// <param name="str">String with alignment mode: END, START, CENTER.</param>
        /// <returns>Returns an alignment constant.</returns>
        /// <see>ProfileElement</see>
        protected int StringToAlignment(String str)
        {
            if (str == null)
                return (ProfileElement.NONE);

            else
                if (str.Equals("START"))
                    return (ProfileElement.START);

            else
                if (str.Equals("END"))
                    return (ProfileElement.END);

            else
                if (str.Equals("CENTER"))
                    return (ProfileElement.CENTER);

            return ProfileElement.NONE;
        }

        #endregion -- Protected Methods --

        #region -- BioPatML XML Read Component -- 
        /// <summary>
        /// Reads the parameters and populate the attributes for this pattern.
        /// 
        /// Hides the ReadNode method for PatternComplex
        /// </summary>
        /// <param name="node">Profile Pattern node</param>
        /// <param name="definition">The container encapsulating this pattern</param>
        public new void ReadNode(XmlNode node, Definition definition)
        {
            Dictionary<String, ProfileElement> map = new Dictionary<string, ProfileElement>();

            PatternName = XMLHelper.GetAttrValueString(node, "name");
            Threshold = XMLHelper.GetAttrValDouble(node, "threshold");
            Impact = XMLHelper.GetAttrValDouble(node, "impact");

            node = node.FirstChild;

            while (node != null)
            {
                if (node.Name.Equals("Region"))
                {
                    String name = XMLHelper.GetAttrValueString(node, "name");
                    String reference = XMLHelper.GetAttrValueString(node, "reference");
                    int minGap = XMLHelper.GetAttrValInt(node, "minGap");
                    int maxGap = XMLHelper.GetAttrValInt(node, "maxGap");
                    String align = XMLHelper.GetAttrValueString(node, "alignment");

                    //Has confusing method Pattern
                    IPattern pattern = QUT.Bio.BioPatML.Patterns.Pattern.ReadPattern
                                                                    (node.FirstChild, definition);

                    ProfileElement refElement = null;
                    if (reference != null)
                    {
                        refElement = map[reference];
                        if (refElement == null)    
                            throw new ArgumentException
                                ("Unknown reference : " + reference);
                        
                    }

                    refElement = Add(refElement, StringToAlignment(align), minGap, maxGap, pattern);
                    map.Add(name, refElement);
                }

                node = node.NextSibling;
            }

        }

        #endregion
    }
}
