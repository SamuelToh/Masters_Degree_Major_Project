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
        #region -- Automatic Properties --
        /// <summary>
        /// List of patterns
        /// </summary>
        private List<IPattern> list = new List<IPattern>();

        /// <summary>
        /// Maps pattern names to patterns
        /// </summary>
        private Dictionary<string, IPattern> dictionary = new Dictionary<string, IPattern>();

        #endregion

        #region ICollection<IPattern> Members

        /// <summary>
        /// Adds a pattern to the list. The name of the pattern must be unique.
        /// No two patterns with the same name can be stored in the list.
        /// </summary>
        /// <param name="item">Pattern to add.</param>
        /// <exception cref="System.ArgumentException">Thrown when duplicate name pattern was found</exception>
        public void Add(IPattern item)
        {
            Add(list.Count, item.PatternName, item);
        }

        /// <summary>
        /// Adds a pattern to the list. The name of the pattern must be unique.
        /// No two patterns with the same name can be stored in the list.
        /// </summary>
        /// <param name="index">Index position where the pattern is added to the list.</param>
        /// <param name="pattern">Pattern to add.</param>
        /// <exception cref="System.ArgumentException">Thrown when duplicate name pattern was found</exception>
        public void Add(int index, IPattern pattern)
        {
            Add(index, pattern.PatternName, pattern);
        }

        /// <summary>
        /// Adds a pattern to the list. The name of the pattern must be unique.
        /// No two patterns with the same name can be stored in the list.
        /// 
        /// </summary>
        /// <param name="index">Index position where the pattern is added to the list.</param>
        /// <param name="name">Name of the pattern.</param>
        /// <param name="pattern">Pattern to add.</param>
        /// <exception cref="System.ArgumentException">Thrown when duplicate name pattern was found</exception>
        private void Add(int index, string name, IPattern pattern)
        {
            if (dictionary.ContainsKey(name))
                throw new ArgumentException("Duplicate pattern name: " + name);

            dictionary.Add(name, pattern);
            list.Insert(index, pattern);
        }

        /// <summary>
        /// Retrieves the pattern using a key that is mapped to the pattern
        /// </summary>
        /// <param name="key">Key mapped to the pattern</param>
        /// <returns>The desire pattern object</returns>
        public IPattern this[string key]
        {
            get { return dictionary[key]; }
        }

        /// <summary>
        /// Gets the pattern by the specified index
        /// </summary>
        /// <param name="index">location of pattern</param>
        /// <returns>The desired pattern object</returns>
        public IPattern this[int index]
        {
            get { return list[index]; }
        }

        /// <summary>
        /// Clear our dictionary and list.
        /// </summary>
        public void Clear()
        {
            list = new List<IPattern>();
            dictionary  = new Dictionary<string, IPattern>();
        }

        /// <summary>
        /// Returns the number of patterns in this list
        /// </summary>
        public int Count
        {
            get { return list.Count; }
        }

        #endregion

        #region -- Not implemented methods --
        /// <summary>
        /// No implementation
        /// </summary>
        /// <exception cref="System.NotImplementedException">When called</exception>
        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// No implementation
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">When called</exception>
        public bool Remove(IPattern item)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// No implementation
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">When called</exception>
        public bool Contains(IPattern item)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// No implementation
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        /// <exception cref="System.NotImplementedException">When called</exception>
        public void CopyTo(IPattern[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
