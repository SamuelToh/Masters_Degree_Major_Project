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
    /// Consists of enumerator methods.
    /// </summary>
    partial class Sequence
    {
        #region -- IEnumerable Members --

        /// <summary>
        /// Returns an enumerator for this sequence (list of browsable symbols)
        /// </summary>
        /// <returns></returns>
        //public IEnumerator<Symbol> GetEnumerator()
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new SequenceSymbolEnumerator(this);
        }

        /// <summary>
        /// Request for an enumerator of this sequence
        /// </summary>
        /// <returns>The common interface of enumerator</returns>
        public IEnumerator<char> GetEnumerator()
        {
            return new SequenceSymbolEnumerator(this);
        }

        #endregion

        /// <summary>
        /// The enumerator class makes the browsing of sequence characters possible.
        /// </summary>
        public class SequenceSymbolEnumerator : IEnumerator<char>
        {
            private ISequence seq;
            private int index = 0;

            #region -- Constructor --

            /// <summary>
            /// Building a enumerator base on the given sequence
            /// </summary>
            /// <param name="givenSeq">The sequence that this enumerator is built based on</param>
            public SequenceSymbolEnumerator(ISequence givenSeq)
            {
                seq = givenSeq;
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
            public char Current
            {
                get
                {
                    if (index < 0)
                        return ' ';

                    return seq.GetSymbol(index).Letter;
                }
            }

            #endregion

            #region -- IEnumerator Members --

            /// <summary>
            /// Get the current letter 
            /// </summary>
            object System.Collections.IEnumerator.Current
            {
                get { return seq.GetSymbol(index).Letter; }
            }

            /// <summary>
            /// Iterate to the next character of current letter.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                //if (index < (seq.Length - 1))
                if (index < (seq.Length))
                {
                    index++;
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Reset the iteration back to first character of this sequence
            /// </summary>
            public void Reset()
            {
                index = 0;
            }

            #endregion
        }

    }
}
