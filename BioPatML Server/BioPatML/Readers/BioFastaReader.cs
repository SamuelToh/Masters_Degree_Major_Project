using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using QUT.Bio.BioPatML.Parsers;
using Bio;
using Bio.IO;
using Bio.IO.Fasta;
using Bio.Util;
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
    /// Class for reading fasta sub sequence file. The implementation is quite similiar to
    /// genbank file parser.
    /// </summary>
    public sealed class BioFastaReader : ReaderBase
    {
        #region -- Constructor -- 
        /// <summary>
        /// Default empty fasta parser constructor
        /// </summary>
        public BioFastaReader() : base() { }
        #endregion

        #region -- Method --

        #endregion -- Method --

        #region -- Read Method for Fasta file --

        /// <summary>
        /// Reads in the fasta file.
        /// </summary>
        /// <param name="sequenceFilePath">Your local filepath for fasta file</param>
        /// <returns>A list of BioPatML Sequences</returns>
        public override SequenceList Read(string sequenceFilePath)
        {
            //Create the parser first
            ISequenceParser fastaParser = new FastaParser();

            List<ISequence> mbfSequences = fastaParser.Parse(sequenceFilePath);

            SequenceList bioSeqList = new SequenceList();

            foreach (Sequence mbfseq in mbfSequences)
            {
                ConvertToBioPatMLSeq(mbfseq);
                bioSeqList.Add(ConvertToBioPatMLSeq(mbfseq));
            }

            return bioSeqList;
        }

        /// <summary>
        /// Reads in the fasta file.
        /// </summary>
        /// <param name="reader">your local filepath for genbank</param>
        /// <returns>list of BioPatML Sequences</returns>
        public override SequenceList Read(TextReader reader)
        {
            //Create the parser first
            ISequenceParser fastaParser = new FastaParser();

            List<ISequence> mbfSequences = fastaParser.Parse(reader);

            SequenceList bioSeqList = new SequenceList();

            foreach (Sequence mbfseq in mbfSequences)
            {
                bioSeqList.Add(ConvertToBioPatMLSeq(mbfseq));
            }

            return bioSeqList;
        }

        /// <summary>
        /// Converts the desired MBF sequence to a BioPatML sequence.
        /// </summary>
        /// <param name="mbfSequence">Your MBF sequence.</param>
        /// <returns>Returns a BioPatML compatible sequence.</returns>
        private QUT.Bio.BioPatML.Sequences.Sequence
                    ConvertToBioPatMLSeq(Sequence mbfSequence)
        {
            using (MBFParser parser = new MBFParser())
            {
                return parser.ParseMBFSequence(mbfSequence);
            }
        }
        
        #endregion
    }
}
