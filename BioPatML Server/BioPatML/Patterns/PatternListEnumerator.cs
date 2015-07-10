using System;
using System.Collections.Generic;
using System.Collections;
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
    /// Implements a list of patterns that can be addressed by name as well.
    /// Pattern lists are used by complex patterns.
    /// </summary>
    public sealed partial class PatternList : ICollection<IPattern>, IEnumerable<IPattern>
    {
        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new PatternListIEnumerator(this);
        }

        #endregion

        #region IEnumerable<IPattern> Members

        IEnumerator<IPattern> IEnumerable<IPattern>.GetEnumerator()
        {
            return new PatternListIEnumerator(this);
        }

        #endregion
    }

    /// <summary>
    /// The enumerator class makes the browsing of patterns possible.
    /// </summary>
    public class PatternListIEnumerator : IEnumerator<IPattern>
    {
        private PatternList listPattern;
        private int index = -1;

        #region -- Constructor --

        /// <summary>
        /// Building a enumerator base on the given list of patterns
        /// </summary>
        /// <param name="list">The patternList that this enumerator is built based on</param>
        public PatternListIEnumerator(PatternList list)
        {
            this.listPattern = list;
        }

        #endregion

        #region -- IDisposable Members --

        /// <summary>
        /// Method to dispose on all objects implementing IDispose interface.
        /// </summary>
        public void Dispose() { }

        #endregion

        #region -- Public Members --

        /// <summary>
        /// Returns the current letter of this sequence base on the index value
        /// </summary>
        public IPattern Current
        {
            get
            {
                if (index < 0)
                    return null;

                return listPattern[index];
            }
        }

        #endregion

        #region -- IEnumerator Members --

        /// <summary>
        /// Get the current pattern
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get { return listPattern[index]; }
        }

        /// <summary>
        /// Iterate to the next pattern.
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (index < (listPattern.Count - 1))
            {
                index++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reset the iteration back to first pattern of list
        /// </summary>
        public void Reset()
        {
            index = -1;
        }

        #endregion
    }
}
