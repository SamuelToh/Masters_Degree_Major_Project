using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Common.Structures;
using QUT.Bio.BioPatML.Common.XML;
using QUT.Bio.BioPatML.Alphabets;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Symbols;
using QUT.Bio.BioPatML.Statistic;
using System.Globalization;

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
    /// This class describes a position weight matrix (PWM).
    /// </summary>
    public class PWM : Pattern
    {
        #region -- Automatic Properties --

        /// <summary>
        /// Alphabet the PWM is based on
        /// </summary>
        public Alphabet PWMalphabet { get; protected set; }

        /// <summary>
        ///  Maps a symbol to a weight vector
        /// </summary>
        protected Dictionary<Symbol, double[]> map
                     = new Dictionary<BioPatML.Symbols.Symbol,double[]>();

        /// <summary>
        /// length of the weight vectors
        /// </summary>
        public int WeightedVectorLength { get; protected set; }

        /// <summary>
        /// maximum score 
        /// </summary>
        public double MaxScore { get; protected set; }

        /// <summary>
        /// minimum score
        /// </summary>
        public double MinScore { get; protected set; }

        /// <summary>
        /// Range of the score (= maxScore-minScore) 
        /// </summary>
        public double RangeScore { get; protected set; }

        /// <summary>
        /// conensus as symbol array
        /// </summary>
        protected Symbol[] consensus;

        /// <summary>
        ///  anti-consensus
        /// </summary>
        protected Symbol[] antiConsensus;

        #endregion

        #region -- Constructors --

        /// <summary>
        /// Default Constructor - Creates an empty PWM.
        /// </summary>
        internal PWM()
            : base ("PWM" + Id.ToString())
        { RangeScore = 1; }

        /// <summary>
        /// Constructs an empty PWM. Use Add(Symbol, String) or 
        /// Add(char, String) to add weights.
        /// </summary>
        /// <param name="name">Name of this pattern</param>
        /// <param name="alphabet">Alphabet the PWM is based on. </param>
        /// <param name="threshold">Similarity threshold.</param>
        public PWM
            (string name, Alphabet alphabet, double threshold)
                : base (name)
        {
            RangeScore = 1;
            SetupPWM(alphabet, threshold);
        }

        /// <summary>
        /// Constructs a PWM from a motif. Each symbol within the motif is given a
        /// weight of 100 for the correlated position within the matrix. All other 
        /// symbols have a weight of 1. Note that a motif can also contain alternative
        /// symbols, e.g. "AT[TG]GC". However, alternatives in alternatives are not
        /// supported and will be ignored.
        /// </summary>
        /// <param name="name">Name of this element</param>
        /// <param name="alphabet">Alphabet the motif is based on. </param>
        /// <param name="motif">Motiv description. Can contain alternative symbols.</param>
        /// <param name="threshold">Similarity threshold. </param>
        public PWM(string name, Alphabet alphabet, string motif, double threshold)
            : base(name)
        {
            SetupPWM(alphabet, threshold);

            List<Symbol> symbols = Motif.Symbols(alphabet, motif);

            foreach (Symbol sym in alphabet)
            {
                Add(sym, new double[symbols.Count]);
            }

            for(int i = 0; i < symbols.Count; i ++)
            {
                for(int j = 0 ; j < alphabet.Count(false); j ++)
                {
                    Symbol symbol = alphabet[j, false];
                    Set(symbol, i , symbols[i].Equals(symbol) ? 100.0 : 1.0);
                }
            }
        }

        #endregion -- Public Constructors -- 

        #region -- Properties --

        /// <summary>
        /// Getter for the number of symbols and weight arrays contained in the 
        /// weight matrix.
        /// </summary>
        public int SymbolNumber
        {
            get
            {
                return (map.Count);
            }
        }

        /// <summary>
        /// Getter for the consensus of the PWM.
        /// </summary>
        public Sequence Consensus
        {
            get
            {
                return
                    (new Sequence(PWMalphabet, consensus, false));
            }
        }

        /// <summary>
        /// Getter for the anti-consensus of the PWM.
        /// </summary>
        public Sequence AntiConsensus
        {
            get
            {
                return
                    (new Sequence(PWMalphabet, antiConsensus, false));
            }
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Common method used by varies constructors to populate the essential values
        /// </summary>
        /// <param name="alphabet"></param>
        /// <param name="threshold"></param>
        private void SetupPWM(Alphabet alphabet, double threshold)
        {
            this.PWMalphabet = alphabet;
            this.Threshold = threshold;//Access level error
        }

        /// <summary>
        /// Initializes the length of the PWM.
        /// </summary>
        /// <param name="length">Length of the PWM.</param>
        protected void Init(int length)
        {
            this.WeightedVectorLength = length;
            this.consensus = new Symbol[length];
            this.antiConsensus = new Symbol[length];
        }


        /// <summary>
        /// Setter for the weight of the given symbol at the index position. 
        /// </summary>
        /// <param name="symbol">Symbol. Unknown symbols will be ignored.</param>
        /// <param name="index">A zero based index for the weight array.</param>
        /// <param name="weight">The weight value to set.</param>
        internal void Set(Symbol symbol, int index, double weight)
        {
            double[] weights = Get(symbol);
            if (weights != null)
                weights[index] = weight;
            UpdateConsensus(symbol, index);
            UpdateMinMaxScore();
        }

        /// <summary>
        /// Setter for the weight of the given letter at the index position. 
        /// </summary>
        /// <param name="letter"> Letter of the PWM alphabet.</param>
        /// <param name="index"> A zero based index for the weight array.</param>
        /// <param name="weight"> The weight value to set.  </param>
        public void Set(char letter, int index, double weight)
        {
            Set(PWMalphabet[letter], index, weight);
        }


        #region Add Methods

        /// <summary>
        /// Adds a weight vector for the given alphabet letter to the weight matrix. 
        /// If the letter is already contained the existing weight vector will be 
        /// replaced. If the letter is not part of the alphabet it will be replaced
        /// by the default letter of the alphabet (If no default letter is set an
        /// exception will be thrown).
        /// </summary>
        /// <param name="letter"> Letter, e.g. Nucleotide or amino acid letter.  </param>
        /// <param name="weights">
        /// Weight vector. All vectors added to the matrix must be
        /// of the same length otherwise an ArgumentOutOfRangeException will be thrown.
        /// </param>
        public void Add(char letter, double[] weights)
        {
            this.Add(PWMalphabet[letter], weights);
        }

        /// <summary>
        /// Adds a weight vector (described as string) for the given letter to the 
        /// weight matrix.
        /// </summary>
        /// <param name="letter">
        ///  Letter, e.g. Nucleotide or amino acid letter of the  alphabet the PWM is using. 
        /// </param>
        /// <param name="weights">
        /// Weight vector as string. Valid delimiters are ";,: ".
        /// All vectors added to the matrix must be of the same length otherwise an 
        /// ArgumentOutOfRangeException will be thrown.
        /// </param>
        public void Add(char letter, String weights)
        {
            Add(PWMalphabet[letter], weights);
        }

        /// <summary>
        /// Adds a weight vector for the given symbol to the weight matrix. If the
        /// symbol is already contained the existing weight vector will be replaced.
        /// </summary>
        /// <param name="symbol">
        /// Symbol, e.g. Nucleotide or amino acid symbol of the PWM alphabet. 
        /// </param>
        /// <param name="weights">
        /// Weight vector. All vectors added to the matrix must be
        /// of the same length. Otherwise an ArgumentOutOfRangeException will be thrown.
        /// </param>
        public void Add(Symbol symbol, double[] weights)
        {
            if (WeightedVectorLength == 0)
                Init(weights.Length);

            if (weights.Length != WeightedVectorLength)
                throw new ArgumentOutOfRangeException
                    ("Length of weight vector does not match size of PWM!");

            //Test if exist, if do. Remove it.
            if (map.ContainsKey(symbol)) { map.Remove(symbol); }

            map.Add(symbol, weights);

            for (int i = 0; i < WeightedVectorLength; i++)
            {
                UpdateConsensus(symbol, i);
            }

            UpdateMinMaxScore();
        }

        /// <summary>
        /// Adds a weight vector (described as string) for the given symbol to the 
        /// weight matrix.
        /// </summary>
        /// <param name="symbol">Symbol, e.g. Nucleotide or amino acid symbol. </param>
        /// <param name="weights">
        /// Weight vector as string. Valid delimiters are ";,: ".
        /// All vectors added to the matrix must be of the same length otherwise an 
        /// ArgumentOutOfRangeException will be thrown.
        /// </param>
        public void Add(Symbol symbol, String weights)
        {
            Add(symbol, PrimitiveParse.StringToDoubleArray(weights));
        }




#endregion

        #region Get Methods

        /// <summary>
        /// Getter for the weight of the given symbol at the index position. 
        /// </summary>
        /// <param name="symbol">Unknown symbols will return the minimum weight
        /// of the PWM in the index column.</param>
        /// <param name="index">A zero based index for the weight array.</param>
        /// <returns>Returns the weight for the symbol at the index position.  </returns>
        public double Get(Symbol symbol, int index)
        {
            double[] weights = Get(symbol);
            return (weights == null ? Get(antiConsensus[index], index) : weights[index]);
        }

        /// <summary>
        /// Getter for the weight of the given letter at the index position. 
        /// </summary>
        /// <param name="letter">Letter of the PWM alphabet. </param>
        /// <param name="index">A zero based index for the weight array.</param>
        /// <returns>Returns the weight for the symbol at the index position.  </returns>
        public double Get(char letter, int index)
        {
            return (Get(PWMalphabet[letter], index));  
        }

        /// <summary>
        /// Getter for the weight array assigned to the given symbol.
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <returns>
        /// Returns the weight array for this symbol or null if the symbol
        /// is not in the PWM.
        /// </returns>
        public double[] Get(Symbol symbol)
        {
            //Fixed to accomodate C#'s dictionary method   
            return map.ContainsKey(symbol) ? (map[symbol]) : null;
        }

        /// <summary>
        /// Getter for the weight array assigned to the given letter. 
        /// </summary>
        /// <param name="letter">Letter of the PWM alphabet. </param>
        /// <returns>
        /// Returns the weight array for this letter or null if the letter
        /// is not in the PWM.
        /// </returns>
        public double[] Get(char letter)
        {
            return (Get(PWMalphabet[letter]));
        }

        #endregion

        /// <summary>
        /// Implements the pattern interface.
        /// 
        /// The maximum similarity is reached when at each sequence position the
        /// symbol with the highest weight for this position appears (Consensus).
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Match Match(BioPatML.Sequences.Sequence sequence, int position)
        {
            double sim = 0;

            for (int i = 0; i < WeightedVectorLength; i++)
                sim += Get(sequence.GetSymbol(position + i), i);

            sim = (sim - MinScore) / RangeScore;

            if (sim < Threshold)
                return null;

            Matched.Set(sequence, position, WeightedVectorLength, sequence.Strand, sim);
            return (Matched);
        }

        /// <summary>
        /// Creates an sorting index (using index sort) according to the weights for the 
        /// specified column of the PWM
        /// </summary>
        /// <param name="col">Column the sorting index should be generated for.</param>
        /// <returns>Returns an array with symbols. First entry is the symbol with
        /// the lowest weight.</returns>
        public Symbol[] SortingIndex(int col)
        {
            int n = 1;
            Symbol[] symbols = new Symbol[SymbolNumber];

            foreach (Symbol sym in map.Keys)
            {
                for (int i = 0; i < n; i++)
                {
                    if (symbols[i] == null || Get(sym, col) < Get(symbols[i], col))
                    {
                        for (int j = n - 1; j > i; j--)
                            symbols[j] = symbols[j - 1];

                        symbols[i] = sym;
                        n ++;
                        break;
                    }
                }
           }

            return symbols;
        }

        /// <summary>
        /// Updates the consensus and the anticonsenus arrays. The consensus contains
        /// the symbols with the highest weights in the PWM columns. The anticonsensus
        /// contains the symbols with the lowests weights in the PWM columns.
        /// This methods updates {#consensus} and {#antiConsensus}
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="index"></param>
        private void UpdateConsensus(Symbol symbol, int index)
        {
            double weight = Get(symbol, index);

            if (consensus[index] == null || Get(consensus[index], index) < weight)
                consensus[index] = symbol;

            if (antiConsensus[index] == null || Get(antiConsensus[index], index) > weight)
                antiConsensus[index] = symbol;
       
        }

        /// <summary>
        /// Calculates the minimum and the maximum score achievable by this PWM. This is
        /// the sum of all minimum or maximum weights in each matrix column which
        /// is equivalent to the sum over all symbols weights of the consensus
        /// or the anticonsensus respectively.
        /// This methods sets the member variables {#minScore} and 
        /// {#maxScore}.
        /// </summary>
        private void UpdateMinMaxScore()
        {
            MaxScore = 0;
            MinScore = 0;

            for (int i = 0; i < WeightedVectorLength; i++)
            {
                MaxScore += Get(consensus[i], i);
                MinScore += Get(antiConsensus[i], i);
            }

            RangeScore = MaxScore - MinScore;

            if (RangeScore == 0)
                RangeScore = 1;
        }

        /// <summary>
        /// Creates a sub PWM with the same number of rows/symbols but a reduced
        /// number of colums/positions.
        /// </summary>
        /// <param name="name">PWM element name</param>
        /// <param name="start">Start index (zero based) for the sub PWM.</param>
        /// <param name="end">End index (zero based) for the sub PWM.</param>
        /// <returns>
        /// Returns a sub PWM of the given PWM containing the columns from
        /// start to end.
        /// </returns>
        public PWM SubPWM(string name, int start, int end)
        {
            int length = end - start + 1;
            PWM pwm = new PWM
                        (name, PWMalphabet, Threshold);

            if (length <= 0 || length > 
                        WeightedVectorLength || start < 0)

                throw new ArgumentException
                        ("Invalid start or end parameter!");


            foreach (Symbol sym in map.Keys)
            {
                double[] weights = new double[length];
                for (int i = 0; i < length; i++)
                    weights[i] = Get(sym, i + start);

                pwm.Add(sym, weights);
            }

            return (pwm);
        }

        /// <summary>
        /// Creates a string representation of the position weight matrix.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Symbol sym in map.Keys)
            {
                sb.Append(sym.Letter).Append(":\t");
                for (int col = 0; col < WeightedVectorLength; col++)
                    sb.Append(string.Format("% 3.2f ", Get(sym, col)));
                sb.Append("\n");
            }

            return (sb.ToString());
        }


        /// <summary>
        /// Estimates a position weight matrix for a section of the given list
        /// of sequences. The PWM will contain only weights for symbols exisiting
        /// in the background sequences/histogram. Symbols which are contained in
        /// the sequenceList but not in the background histogram will not appear
        /// in the PWM!
        /// </summary>
        /// <param name="sequenceList">
        ///  The sequence list. Must contain at least one sequence
        ///  and all sequences must use the same alphabet.
        /// </param>
        /// <param name="startPosition">
        /// Start position (first is one) of the section used
        /// to calculate the PWM.
        /// </param>
        /// <param name="background">
        /// Histogram with base counts of the background  sequences.
        /// </param>
        /// <returns></returns>
        public double Estimate
            (SequenceList sequenceList, int startPosition, HistogramSymbol background)
        {
            if (sequenceList.Count == 0)
                throw new ArgumentException
                    ("Sequence list is empty!");

            double log2 = Math.Log(2);
            int endPosition = startPosition + WeightedVectorLength - 1;
            int symNumber = sequenceList[0].Alphabet.Count(false);
            double ic = 0; //information content
            HistogramSymbol histo = new HistogramSymbol();

            //Create PWM rows for background symbols
            for (int bin = 0; bin < background.Count; bin++)
                Add(background[bin], new double[WeightedVectorLength]);

            //over all columns of alignmenr
            for (int pos = startPosition; pos <= endPosition; pos++)
            {
                histo.Clear(); //reset the histogram of symbols for 1 column
                for (int seqIndex = 0; seqIndex < sequenceList.Count; seqIndex++)
                {
                    Sequence seq = sequenceList[seqIndex];
                    if (seq.Alphabet != PWMalphabet)
                        throw new ArgumentException
                            ("Alphabet in sequence does not match PWM alphabet: " + PWMalphabet.Name);
                    histo.Add(seq.GetSymbol(pos));
                }

                int sum = histo.Sum + symNumber;
                for (int bin = 0; bin < background.Count; bin++)
                {
                    Symbol symbol = background[bin];
                    double bp = background.Frequency(symbol);
                    double p = (histo.HistoValue(symbol) + 1.0) / sum;
                    double w = Math.Log(p / bp) / log2;
                    Set(symbol, pos - startPosition, w);
                    ic += p * w;
                }
            }

            return ic;
        }

        #endregion

        #region -- BioPatML XML Reader Component --

        /// <summary>
        /// Implementation of the pattern interface.
        /// 
        /// <see cref="QUT.Bio.BioPatML.Patterns.IPattern">IPattern.ReadNode</see>
        /// </summary>
        /// <param name="node"></param>
        /// <param name="definition"></param>
        public override void ReadNode(System.Xml.XmlNode node, Definition definition)
        {
            PatternName = XMLHelper.GetAttrValueString(node, "name");
            Threshold = XMLHelper.GetAttrValDouble(node, "threshold");
            Impact = XMLHelper.GetAttrValDouble(node, "impact");
            PWMalphabet = AlphabetFactory.Instance(XMLHelper.GetAttrValueString(node, "alphabet"));

            node = node.FirstChild;
            while (node != null) // read all rows of the PWM
            {
                if (node.Name.Equals("Row"))
                {
                    char letter = XMLHelper.GetAttrValueString(node, "letter")[0];
                    String weights = node.InnerText;

                    if (weights == null)
                        throw new ArgumentException("Weights for PWM are missing!");

                    if (!PWMalphabet.IsValid(letter))
                        throw new ArgumentException("Invalid alphabet letter '" + letter + "'!");

                    Add(letter, weights);
                }

                node = node.NextSibling;
            }

        }

        #endregion
    }
}
