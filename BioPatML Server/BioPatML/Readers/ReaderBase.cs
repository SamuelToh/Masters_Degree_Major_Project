using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using QUT.Bio.BioPatML.Sequences.List;

/*****************| Queensland University Of Technology |********************
 *  Original Author          : Samuel Toh (Email: yu.toh@connect.qut.edu.au)
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Readers
{
    /// <summary>
    /// Base class for all readers.
    /// At the moment there is only 1 reader (MBF Reader) that extends this.
    /// </summary>
    public abstract class ReaderBase : IDisposable
    {
        #region -- C# Automatic Properties --
        /// <summary>
        /// Property that tells the read whether current content is online or offline.
        /// *might get it removed as it is bad design
        /// </summary>
        protected bool IsOnline { get; set; }

        #endregion

        #region -- Constructors --
        /// <summary>
        /// Default constructor
        /// </summary>
        public ReaderBase() { IsOnline = false; }

        #endregion

        #region -- Generic Read Method for ReadBase --
        /// <summary>
        /// Reads in a sequence file path and check whether its content is online or offline.
        /// </summary>
        /// <param name="sequenceFilePath">path of the sequence file</param>
        /// <returns></returns>
        public virtual SequenceList Read(string sequenceFilePath)
        {
            if (sequenceFilePath.StartsWith("http:"))
                IsOnline = true;

            else
                if (!File.Exists(sequenceFilePath))
                    throw new FileNotFoundException
                        ("The sequence file was not found!");

            if (IsOnline)
                throw new Exception("Online retrieving genbank data is not available on this version.");

            return new SequenceList();
        }

        /// <summary>
        /// Reads in a sequence by its already processed content
        /// </summary>
        /// <param name="sequenceReader">A textreader with content of the sequence</param>
        /// <returns></returns>
        public virtual SequenceList Read(TextReader sequenceReader)
        {
            return new SequenceList();
        }
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Implementing the IDisposable interface.
        /// </summary>
        public void Dispose() { /* No Implementation */ }

        #endregion
    }
}
