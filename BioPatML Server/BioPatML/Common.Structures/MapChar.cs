using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Symbols;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Common.Structures
{
    /// <summary>
    ///  This class implements a hash map for chars. The code is based on an
    ///  implementation by Justin Couch, http://code.j3d.org.
    /// </summary>
    public class MapChar 
        : IEnumerator, IEnumerable
    {
        #region -- Private Fields --

        /** Table of hash entries */
        [NonSerialized]
        private Entry[] table;

        /** Threshold for rehashing the map when to many entries are contained */
        private int threshold;

        /** load factor for the hash table */
        private double loadFactor;

        /** stores all keys (=chars) contained in the map */
        private StringBuilder keys = new StringBuilder();

        private int index = 0;

        #endregion

        #region -- Public Constructors --

        /// <summary>
        /// Constructs a new, empty hashtable with the specified initial capacity and
        /// the specified load factor.
        /// </summary>
        /// <param name="initialCapacity"> the initial capacity of the hashtable. </param>
        /// <param name="loadFactor"> the load factor of the hashtable. </param>
        public MapChar(int initialCapacity, double loadFactor)
        {
            ConstructMapChar(initialCapacity, loadFactor);
        }

        /// <summary>
        /// Constructs a new, empty hashtable with a default capacity and load factor,
        /// which is 20 and 0.75 respectively.
        /// </summary>
        public MapChar()
        {
            ConstructMapChar(20, 0.75);
        }

        /// <summary>
        ///  Constructs a new, empty hashtable with the specified initial capacity and
        ///  default load factor, which is 0.75
        /// </summary>
        /// <param name="initialCapacity">the initial capacity of the hashtable.</param>
        public MapChar(int initialCapacity)
        {
            ConstructMapChar(initialCapacity, 0.75);
        }

        #endregion

        #region -- Properties --

        /// <summary>
        /// Returns the number of keys in this hashtable.
        /// </summary>
        public int Size
        {
            get
            {
                return this.keys.Length;
            }
        }

        /// <summary>
        /// Returns the load factor of mapcharacter
        /// </summary>
        private double LoadFactor
        {
            set
            {
                this.loadFactor = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Object Current
        {
            get
            {
                return this.Get(index);
            }
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Common method used by the varies constructors to build the map
        /// </summary>
        /// <param name="initialCapacity"></param>
        /// <param name="loadFactor"></param>
        private void ConstructMapChar(int initialCapacity, double loadFactor)
        {
            if (initialCapacity < 0)
                throw new ArgumentOutOfRangeException
                    ("Illegal Capacity: " + initialCapacity);

            if (loadFactor < 0)
                throw new ArgumentOutOfRangeException
                    ("Illegal Load: " + loadFactor);

            if (initialCapacity == 0)
                initialCapacity = 1;

            this.loadFactor = loadFactor;
            table = new Entry[initialCapacity];
            threshold = (int)(initialCapacity * loadFactor);
        }

       
        /// <summary>
        ///  Tests if this hashtable maps no keys to values.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return keys.Length == 0;
        }

        /// <summary>
        /// Tests if some key maps into the specified value in this hashtable. This
        /// operation is more expensive than the <code>containsKey</code> method.
        /// <p>
        /// 
        /// Note that this method is identical in functionality to containsValue,
        /// (which is part of the Map interface in the collections framework).
        /// </p>
        /// </summary>
        /// <param name="value"> A value to search for. </param>
        /// <returns>
        /// true if and only if some key maps to the
        /// value argument in this hashtable as determined by
        /// the equals method; false otherwise.
        /// </returns>
        public bool Contains(Object value)
        {
            if (value == null)
                return false;

           
            for (int i = table.Length; i-- > 0; )

                for (Entry e = table[i]; e != null; e = e.next)
                    if (e.value.Equals(value))
                        return true;


            return false;
        }

        /// <summary>
        /// Returns true if this HashMap maps one or more keys to this value.
        /// 
        /// Note that this method is identical in functionality to contains (which
        /// predates the Map interface).
        /// </summary>
        /// <param name="value">value whose presence in this HashMap is to be tested.</param>
        /// <returns>
        /// Returns true if the hash map contains one or more keys for the
        /// given value.
        /// </returns>
        public bool ContainsValue(Object value)
        {
            return (Contains(value));
        }

        /// <summary>
        /// Tests if the specified object is a key in this hashtable.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(char key)
        {
            return (Get(key) != null);
        }

        /// <summary>
        /// Returns the value to which the specified key is mapped in this map.
        /// </summary>
        /// <param name="key">A key in the hashtable.</param>
        /// <returns>
        /// Returns the value to which the key is mapped in this hashtable;
        /// null if the key is not mapped to any value in this
        /// hashtable.
        /// </returns>
        public Object Get(char key)
        {
            int index = (key & 0x7FFFFFFF) % table.Length;


            for (Entry e = table[index]; e != null; e = e.next)
                if (e.hash == key)
                    return (e.value);


            return null;

        }

        /// <summary>
        ///  Getter for the value which is stored for the given index. Indicies are
        ///  defined by the order the values are put to the map. First index is zero and
        ///  last index is size()-1;
        /// </summary>
        /// <param name="index"> Index of a value. </param>
        /// <returns> Returns the value for the given index.S </returns>
        public Object Get(int index)
        {
            return (Get(GetKey(index)));
        }

        /// <summary>
        /// Getter for the index-th key stored in the map.
        /// </summary>
        /// <param name="index">
        /// Index of a key. The first key has index zero and last one has
        /// index size()-1.
        /// </param>
        /// <returns> Returns the key for the given index. </returns>
        public char GetKey(int index)
        {
            return (keys[index]);
        }

        /// <summary>
        /// Increases the capacity of and internally reorganizes this hashtable, in
        /// order to accommodate and access its entries more efficiently. This method
        /// is called automatically when the number of keys in the hashtable exceeds
        /// this hashtable's capacity and load factor.
        /// </summary>
        protected void Rehash()
        {
            int oldCapacity = table.Length;
            Entry[] oldMap = table;

            int newCapacity = oldCapacity * 2 + 1;
            Entry[] newMap = new Entry[newCapacity];

            threshold = (int) (newCapacity * loadFactor);
            table = newMap;

            for(int i = oldCapacity; i -- > 0;){
                for (Entry old = oldMap[i]; old != null; )
                {
                    Entry e = old;
                    old = old.next;

                    int index = (e.hash & 0xFFFFFFF) % newCapacity;
                    e.next = newMap[index];
                    newMap[index] = e;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Object Put(char key, Object value)
        {
            int index = (key & 0x7FFFFFFF) % table.Length;

            //try to find entry for this key
            for (Entry e = table[index]; e != null; e = e.next)
            {
                if (e.hash == key)
                {
                    Object oldValue = e.value;
                    e.value = value;
                    return (oldValue);
                }
            }

            //rehash map is map gets stuffed
            if (keys.Length >= threshold)
            {
                Rehash();
                index = (key & 0x7FFFFFFF) % table.Length;
            }

            //this is new key therefore store the key and create a new entry
            keys.Append(key);
            //Entry e = new Entry(key, key, value, table[index]);
            table[index] = new Entry(key, key, value, table[index]);
            return null;
        }

        //act like a linked list but with hashing
        /// <summary>
        ///  Removes the key (and its corresponding value) from this hashtable. This
        ///  method does nothing if the key is not in the hashtable.
        ///  
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Object Remove(char key)
        {
            int index = (key & 0x7FFFFFFF) % table.Length;

            Entry prev = null;
            int x = 0;

            for (Entry e = table[index]; e != null; x++, prev = e, e = e.next)
            {
                if (e.hash == key)
                {
                    if (prev != null)
                        prev.next = e.next;
                    else
                        table[index] = e.next;

                    int delPosition = keys.ToString().IndexOf("" + key);
                    keys.Remove(delPosition, 1);
                    Object oldValue = e.value;
                    e.value = null;

                    return (oldValue);
                }
            }

            return (null);
        }

        /// <summary>
        /// Clears this hashtable so that it contains no keys.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < table.Length; i++)
                table[i] = null;

            keys.Remove(0, keys.Length);
        }

        /// <summary>
        /// Move the enumerator 1 step ahead. index + 1
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            index++;
            return (index < Size);
        }

        /// <summary>
        /// Supporting the foreach loop 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {

            List<Symbol> collection = new List<Symbol>();

            for (int i = 0; i < Size; i++)
                collection.Add((Symbol)Get(i));

            //IEnumerable<object> myEnumerable = collection;


            yield return collection;
        }

        /// <summary>
        /// Reset the current iterating index to -1
        /// </summary>
        public void Reset() { index = -1; }

        #endregion

        #region -- Private inner class --

        //Act like the standard linkedlist data structure
        private class Entry
        {
            public int hash; //hash value
            public char key;
            public object value;
            public Entry next;

            public Entry(int hash, char key, Object value, Entry next)
            {
                this.hash = hash;
                this.key = key;
                this.value = value;
                this.next = next;
            }
        }

        #endregion    
    }
}
