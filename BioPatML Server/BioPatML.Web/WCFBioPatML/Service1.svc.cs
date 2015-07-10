using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.IO;
using QUT.Bio.BioPatML.Patterns;
using QUT.Bio.BioPatML.Patterns.Reader;
using QUT.Bio.BioPatML.Readers;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;


namespace BioPatMLEditor.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service1
    {


        [OperationContract]
        public SequenceContract ParseGenbankFile(string gbContent)
        {
            StringReader sr = new StringReader(gbContent);
            SequenceContract sequence = new SequenceContract();
            SequenceList bioSeqList = null; //BioPatML's data structure

            using (ReaderBase reader = new BioPatMBF_Reader())
            {
                bioSeqList = reader.Read(sr);
            }

            //for now we only always use the first sequence SequenceName
            sequence.Name = bioSeqList[0].Annotations("SequenceName").AnnotationValue as string;
            //sequence.Name = bioSeqList[0].Name;
            sequence.AlphabetName = bioSeqList[0].Alphabet.Name;
            sequence.Characters = bioSeqList[0].Letters();

            return sequence;
        }

        [OperationContract]
        public List<SequenceContract> ParseFastaFile(string fastaContent)
        {
            StringReader sr = new StringReader(fastaContent);
            List<SequenceContract> sequences = new List<SequenceContract>();
            SequenceList bioSeqList = null;

            using (ReaderBase reader = new BioFastaReader())
            {
                bioSeqList = reader.Read(sr);
            }

            foreach (Sequence seq in bioSeqList)

                sequences.Add
                    (new SequenceContract()
                            {
                                Name = seq.Annotations("SequenceName").AnnotationValue as string,
                                AlphabetName = seq.Alphabet.Name,
                                Characters = seq.Letters()
                            }
                    );


            return sequences;
        }

        [OperationContract]
        public List<Matched> PerformSearch(string BioPatMLContent, SequenceContract sequence)
        {
            List<Matched> matchedList = new List<Matched>();
            StringReader sr = new StringReader(BioPatMLContent);
            Definition MyPatterns = null;

            using (BioPatMLPatternReader reader = new BioPatMLPatternReader())
            {
                MyPatterns = reader.ReadBioPatML(sr);
            }

            Sequence targetSequence = new Sequence(sequence.AlphabetName, sequence.Characters);
            SequenceList matches = new FeatureList();

            IPattern MatchingPattern = MyPatterns.MainPattern;

            #region If bioPatML pattern is SeriesBest OR SetBest : Search by best

            if (MatchingPattern is SeriesBest
                    || MatchingPattern is SetBest)
            {
                Match match = targetSequence.SearchBest(0, 0, MatchingPattern);
                matches.Add(match);
            }

            #endregion

            #region Else, pattern is Motif, Any, Anchor, Prosite, RegularEx : Search by normal

            else
            //The rest
            {
                matches = targetSequence.Search(1, targetSequence.Length, MatchingPattern);
            }

            #endregion

            for (int i = 0; i < matches.Count; i++)
            {
                Match matched = matches[i] as Match;

                matchedList.Add(new Matched
                                         (matched.Similarity,
                                         matched.Start, matched.End,
                                         matched.Length, matched.Letters(), sequence.Name));
            }

            return matchedList;
        }

    }

    [DataContract]
    public class MatchResults
    {
        public List<Matched> Matches { get; set; }

        public MatchResults()
        {
            Matches = new List<Matched>();
        }
    }

    [DataContract]
    public class Matched
    {
        [DataMember]
        public double Similarity { get; set; }
        [DataMember]
        public int MatchAt_StartPosition { get; set; }
        [DataMember]
        public int Match_EndPosition { get; set; }
        [DataMember]
        public int Match_TotalLength { get; set; }
        [DataMember]
        public string MatchedRegion { get; set; }
        [DataMember]
        public string SequenceName { get; set; }

        public Matched(double similarity, int start, int end, 
                        int matchLength, string region, string refseqName)
        {
            Similarity = similarity;
            MatchAt_StartPosition = start;
            Match_EndPosition = end;
            Match_TotalLength = matchLength;
            MatchedRegion = region;
            SequenceName = refseqName;
        }
    }

    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }


    [DataContract]
    public class SequenceContract
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Characters { get; set; }

        [DataMember]
        public string AlphabetName { get; set; }

        [DataMember]
        public List<Feature> Features { get; set; }

       
    }

    [DataContract]
    public class Feature
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Start { get; set; }

        [DataMember]
        public int End { get; set; }

        public Feature(string name, int start, int end)
        {
            Name = name;
            Start = start;
            End = end;
        }

    }
}
