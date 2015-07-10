using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bio;
using Bio.Util;
using Bio.IO.GenBank;
using QUT.Bio.BioPatML.Sequences.Annotations;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Sequences.List;

/*****************| Queensland University Of Technology |********************
 *  Original Author          : Samuel Toh (Email: yu.toh@connect.qut.edu.au)
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Parsers
{
    /// <summary>
    /// A parser that converts MBF sequences to BioPatML sequences.
    /// Only certain locus information are being transfer over to the
    /// BioPatML sequence. These information includes FeatureItems of MBF sequence
    /// and the sequence name. 
    /// 
    /// Should there be any other interesting information that one needs,
    /// one can manually extract the information and top it up using the AnnotationList.
    /// </summary>
    public class MBFParser : IDisposable
    {
        /// <summary>
        /// Default parser constructor
        /// </summary>
        public MBFParser() { }

        /// <summary>
        /// This method takes in a MBF sequence and converts it to BioPatML sequence.
        /// </summary>
        /// <param name="mbfseq">The MBF sequence one is interested in.</param>
        /// <returns>Returns the converted BioPatML sequence.</returns>
        public QUT.Bio.BioPatML.Sequences.Sequence ParseMBFSequence
                                                            (Sequence mbfseq)
        {
            GenBankMetadata metaData;

            #region If sequence is complex type e.g. Genbank file with features 
            if (mbfseq.Metadata.ContainsKey(Helper.GenBankMetadataKey))
            {
                //Retrieves our meta data out
                metaData = (GenBankMetadata)mbfseq.Metadata[Helper.GenBankMetadataKey];

                //Create our BioPatML sequence
                QUT.Bio.BioPatML.Sequences.Sequence bioSequence = new QUT.Bio.BioPatML.Sequences.Sequence
                                            (ParseAlphabet(mbfseq), mbfseq.ToString(),
                                                        IsCircularStrand(metaData.Locus));

                #region Top the BioPatML Sequence with Feature data and sequence name

                //Add features to sequence
                bioSequence.AddFeatures(ExtractFeatures(metaData));

                //Add mbf sequence name to BioPatML sequence
                //Create annotation to put in our sequence data like name
                AnnotationList jacobiMeta = new AnnotationList();
                jacobiMeta.Add("SequenceName", metaData.Locus.Name);
                bioSequence.AddAnnotations(jacobiMeta);

                #endregion

                return bioSequence;

            }
            #endregion
            #region Else we assume it is a simple type e.g. Fasta files
            else
            {
                QUT.Bio.BioPatML.Sequences.Sequence bioSequence = new QUT.Bio.BioPatML.Sequences.Sequence
                                            (ParseAlphabet(mbfseq), mbfseq.ToString(),
                                                        false);

                AnnotationList jacobiMeta = new AnnotationList();
                jacobiMeta.Add("SequenceName", mbfseq.DisplayID);
                bioSequence.AddAnnotations(jacobiMeta);
                return bioSequence;
            }
            #endregion
        }

        #region -- Identify Alphabet type --

        /// <summary>
        /// Converts the MBF Alphabet data structure to BioPatML's
        /// </summary>
        /// <exception cref="System.ArgumentException">
        /// Thrown when encounter invalid Alphabets
        /// </exception>
        /// <param name="sequence">Parsing MBF sequence</param>
        /// <returns>An unit of BioPatML Alphabet structure.</returns>
        private Alphabet ParseAlphabet(Sequence sequence)
        {
            //The code should fall under one of these cases otherwise throw exception.
            switch (sequence.Alphabet.Name)
            {
                // Values for each case is extracted from MBF library.
                // According to MBF library it is safe to use the following names "DNA RNA and PROTENTIN:
                // because they are fairly constant and they are not expected to have a different name in any
                // time soon
                case ("DNA"):
                    return AlphabetFactory.Instance
                                    (AlphabetFactory.CODE_DNA);

                case ("RNA"):
                    return AlphabetFactory.Instance
                                    (AlphabetFactory.CODE_RNA);

                case ("Protein"):
                    return AlphabetFactory.Instance
                                    (AlphabetFactory.CODE_PROTEIN);
            }

            // In this case we will hit argument exception.
            return AlphabetFactory.Instance
                                    (AlphabetFactory.CODE_UNKNOWN + "Alphabet");
        }

        #endregion Identify Molecule type

        #region -- Identify Strand information --

        /// <summary>
        /// There can only exist two type of strands namely the Circular and the Linear form.
        /// This method extracts the kind of strand from the Genbankmeta of a particular mbf sequence.
        /// </summary>
        /// <param name="locus">The locus information belonging to the MBF sequence</param>
        /// <returns>True: is circular ; else false.</returns>
        private bool IsCircularStrand(GenBankLocusInfo locus)
        {
            return locus.StrandTopology == SequenceStrandTopology.Circular ? true : false;
        }

        #endregion

        #region -- Extracts all Features from MBF sequence --

        /// <summary>
        /// This method transfers all available features from the MBF sequence and 
        /// populate them into biopatml features data type.
        /// In this version only its name, start and end location is populated.
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        private FeatureList ExtractFeatures(GenBankMetadata metadata)
        {
            List<FeatureItem> mbfFeatures = metadata.Features.All;
            FeatureList bioFeatureList = new FeatureList();

            foreach (FeatureItem item in mbfFeatures)
            {
                #region Constructs the feature outline first

                //Strand is always assumed to be forward +1
                QUT.Bio.BioPatML.Sequences.Feature bioFeature = new QUT.Bio.BioPatML.Sequences.Feature
                                        (item.Key, item.Location.Start, item.Location.End, 1);

                bioFeatureList.Add(bioFeature);

                #endregion

                #region Adds the qualifier key and values to Feature using AnnotationList

                AnnotationList annList = new AnnotationList();

                foreach (KeyValuePair<string, List<string>> qualitfier in item.Qualifiers)
                    annList.Add(qualitfier.Key, qualitfier.Value[0]);

                bioFeature.AddAnnotations(annList);

                #endregion
            }

            return bioFeatureList;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Implements the IDispose interface
        /// </summary>
        public void Dispose()
        { /* no implementation */}

        #endregion
    }
}
