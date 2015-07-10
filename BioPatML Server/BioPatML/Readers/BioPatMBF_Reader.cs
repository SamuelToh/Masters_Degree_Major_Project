using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using QUT.Bio.BioPatML.Parsers;
using Bio;
using Bio.IO;
using Bio.IO.GenBank;
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
    /// This is a MBF --> BioPatML reader.
    /// It reads in a Genbank sequence file and have it parsed by
    /// the MBF library then the sequence is being parsed again to get 
    /// it converted to BioPatML sequence.
    /// </summary>
    public sealed class BioPatMBF_Reader : ReaderBase
    {
        #region -- Constructor --
        /// <summary>
        /// Default empty constructor
        /// </summary>
        public BioPatMBF_Reader() : base() { }

        #endregion

        #region -- Methods --
        /// <summary>
        /// Reads the Genbank file and have it parsed by MBF library.
        /// </summary>
        /// <param name="genbankFileURL">Your genbank file path</param>
        /// <returns></returns>
        private SequenceList ParseSequencePath
                                    (string genbankFileURL)
        {
            if (IsOnline)  
                throw new NotImplementedException
                    ("online genbank reading is not supported in this version!"); 
            
            //Download the file and parse it

            //Create the parser first
            ISequenceParser gbParser = new GenBankParser();

            //Always Try parsing multi sequence in a file
            List<ISequence> mbfSequences = gbParser.Parse(genbankFileURL);

            SequenceList bioSeqList = new SequenceList();

            foreach (Sequence mbfseq in mbfSequences)
            {
                ConvertToBioPatMLSeq(mbfseq);
                bioSeqList.Add(ConvertToBioPatMLSeq(mbfseq));
            }

            return bioSeqList;
        }

        /// <summary>
        /// The param could also be a stringreader.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private SequenceList ParseSequencePath
                                    (TextReader reader)
        {
            //Create the parser first
            ISequenceParser gbParser = new GenBankParser();

            //Always Try parsing multi sequence in a reader
            List<ISequence> mbfSequences = gbParser.Parse(reader);

            SequenceList bioSeqList = new SequenceList();

            foreach (Sequence mbfseq in mbfSequences)
            {
                ConvertToBioPatMLSeq(mbfseq);
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

        #region -- Read Method for Genbank file --
        /// <summary>
        /// Reads in the Genbank file.
        /// </summary>
        /// <param name="sequenceFilePath">your local filepath for genbank</param>
        /// <returns>a list of BioPatML Sequences</returns>
        public override SequenceList Read(string sequenceFilePath)
        {
            base.Read(sequenceFilePath);

            return ParseSequencePath(sequenceFilePath);
        }

        /// <summary>
        /// Reads in the Genbank file.
        /// </summary>
        /// <param name="reader">your local filepath for genbank</param>
        /// <returns>list of BioPatML Sequences</returns>
        public override SequenceList Read(TextReader reader)
        {
            //base.Read(sequenceFilePath);

            return ParseSequencePath(reader);
        }
      
        #endregion
    }
}
