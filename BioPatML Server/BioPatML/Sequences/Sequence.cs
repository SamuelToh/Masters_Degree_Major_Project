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
    /// A class ressembling a sequence. Basically a long array of
    /// Symbols of a certain <see cref="QUT.Bio.BioPatML.Alphabets.Alphabet">Alphabet</see>.
    /// <remarks>
    /// The reality however, is a bit more complex. 
    /// A sequence uses an AccessorBase and an Indexer to read Symbols off
    /// a SymbolArray. 
    /// <para></para>
    /// This approach has several advantages:
    /// <para></para>
    /// <list type="bullet">
    /// There is only one sequence (the main sequence) which contains the actual 
    /// sequence data. All other sequence are just references to this sequence.
    /// <para></para>
    /// <item><description>The <see cref="QUT.Bio.BioPatML.Symbols.Accessor">AccessorBase</see>
    /// can read data in a linear, circular, transparent or
    /// other way. This eliminates the problem "index out of bounds" for instance.</description></item>
    /// <para></para>
    /// <item><description> The <see cref="QUT.Bio.BioPatML.Symbols.Indexer.Indexer">Indexer</see>
    /// determines the direction of the reading process, e.g.
    /// direct or reverse. </description></item>
    /// <para></para>
    /// <item><description>Encapsulating the sequence data in a
    /// <see cref="QUT.Bio.BioPatML.Symbols.SymbolArray">SymbolArray</see> allows it
    /// to store the symbols of the sequence in an arbitrary from (e.g. packed,
    /// compressed, as a file, dynamically generated, ...). Every class which
    /// implements the <see cref="QUT.Bio.BioPatML.Symbols.SymbolArray">SymbolArray</see>
    /// interface can serve as a container
    /// for symbol data and can be used as sequence.</description></item>
    ///</list>
    /// <para></para>
    ///All <see cref="QUT.Bio.BioPatML.Symbols.Symbol">Symbols</see> in a sequence
    ///must belong to the same <see cref="QUT.Bio.BioPatML.Alphabets.Alphabet">Alphabet</see>.
    ///The standard alphabets as "DNA", "RNA" and "AA" are predefined. Unknwon
    ///symbols will be mapped to the default symbol of the alphabet, e.g. 'X'
    ///(if there is a default symbol defined). 
    ///<para></para>
    ///The case of the symbols will automatically converted to the standard case of 
    ///the alphabet, e.g. Upper case for amino acids and lower case for DNA/RNA sequences
    ///<para></para>
    /// Sequences are immutable like Strings. The content of a sequence cannot be changed.
    /// The only way to create a different sequence is to create a new one. To construct 
    /// a sequence a <see cref="System.Collections.IEnumerable">Enumerator char sequence</see>, e.g. a String, a 
    /// StringBuilder and an alphabet are required. A sequence constructed in this way 
    /// is called the main sequence.
    /// <para></para>
    /// Sequence constructed on base of other <see cref="QUT.Bio.BioPatML.Sequences.Sequence">Sequences</see> 
    /// are called sub subsequence. 
    /// They are just references to the main sequence and do not contain any symbol
    /// data. However they can be used as the main sequence itself but they usually
    /// behave differently if the index gets out of bounds.
    /// <para></para>
    /// A main feature of a subsequence is that indices out of bounds of the
    /// subsequence are mapped to vaild indices of the main sequence. This gives
    /// easy access to let say the -10 position of a gene since the gene is represented
    /// as a subsequence of a main sequence (the genome).
    /// <para></para>
    /// The first index position in a sequence is one and the last one is equal to
    /// the length of the sequence. However not all methods of this library use
    /// positions. To reduce confusion, zero based indicies are usually named "index"
    /// and sequence positions starting at one are named "position".
    /// <para></para>
    /// The sequence class implements the {@link CharSequence} interface and in
    /// many cases sequences can be handled as Strings, e.g. searching for 
    /// regular expressions using the <see cref="System.Text.RegularExpressions">RegularExpressions</see>
    /// method.
    /// <para></para>
    /// 
    /// Here are a few examples how to create a sequence:
    /// <para></para>
    /// <example>
    /// <code>
    /// Sequence    seq1 = new Sequence("DNA", "actg");
    /// Sequence    seq2 = new Sequence("DNA", new StringBuilder("actg"));
    /// Sequence    sub  = new Sequence(seq1, 1,3);
    /// </code>
    /// </example>
    /// </remarks>
    /// </summary>
    public partial class Sequence 
        : Region, ISequence, IAnnotated, IEnumerable<char>
    {
        #region -- Private Fields --

        /// <summary>
        /// Accessor type of the sequence
        /// <para></para>
        /// By default it is set to Linear Direct
        /// </summary>
        private int AccessorType { get; set; } 

        /// <summary>
        /// The accessor for the symbol array wich contains the actual data
        /// </summary>
        private IAccessor Accessor { get; set; }

        /// <summary>
        /// List of feature lists for this sequence
        /// </summary>
        private AnnotatedList ListFeatures { get; set; }

        /// <summary>
        /// List of annotations 
        /// </summary>
        private AnnotationList ListAnnotations { get; set; }
        
        /// <summary>
        /// Offset of the subsequence within the main sequence 
        /// </summary>
        private int Offset { get; set; }

        #endregion

        #region -- Public Fields -- 

        /// <summary>
        /// The main sequence which contains the actual sequence data. 
        /// </summary>
        public Sequence MainSequence { get; internal set; }

        /// <summary>
        /// Reference of the sequence the subsequence is based on
        /// </summary>
        public Sequence BaseSequence { get; internal set; }


        #endregion -- Public Fields --

        #region -- Public Constructors --

        /// <summary>
        /// Creates an empty sequence. The InitSubsequence(int, Sequence, int, int)
        /// or the InitSequence(Alphabet, CharSequence, boolean)
        /// method must be used to fill the sequence. This is an internal method
        /// and should be used only for extensions of Jacobi.
        /// </summary>
        internal Sequence()
        {
            //Setup an empty sequence container, incase there is a need for this. However in most cases we 
            //should avoid creating one as there is no need to own a container.
            MainSequence = this;
            this.InitSequence(AlphabetFactory.Instance("DNA"), "", false);
        }

        /// <summary>
        /// Constructor for a main sequence.
        /// Creates a sequence on base of the given character sequence. This is 
        /// always a sequence which describes the direct strand. 
        /// Casting a <see cref="QUT.Bio.BioPatML.Sequences.Sequence">sequence</see> to a 
        /// <see cref="System.Collections.IEnumerable"> Enumerable char </see> is possible but causes to create a copy of the
        /// sequence. Usage of Sequence(Sequence, int, int, int) constructor is
        /// recommended instead.
        /// <para></para>
        /// Unknown symbols within the character sequence (symbols which are not part 
        /// of the <see cref="QUT.Bio.BioPatML.Alphabets.Alphabet">Alphabet</see> are converted to the default symbol of the given 
        /// alphabet.
        /// </summary>
        /// <param name="alphabetName">Name of the alphabet which is used for this sequence,
        /// e.g. "DNA", "RNA", "AA". See <see cref="QUT.Bio.BioPatML.Alphabets.AlphabetFactory">AlphabetFactory</see>.
        /// For the alphabet
        /// name "UNKNOWN" the constructor trys to recognize the alphabet of the sequence
        /// using a simple heuristics. For short character sequence this heuristics
        /// may failed and the explicit specification of the alphabet is recommended.</param>
        /// <param name="characters">A character sequence of string
        /// which symbols describe the sequence on the direct strand.</param>
        public Sequence(String alphabetName, IEnumerable<char> characters)
        {
            MainSequence = this;
            this.InitSequence(AlphabetFactory.Instance(alphabetName), characters, false);
        }

        /// <summary>
        ///  Constructor for a main sequence.
        ///  Same as Sequence(String, CharSequence) constructor but the topology of the
        ///  sequence can be set.
        ///  <para></para>
        ///  Sequence positions smaller than one or bigger than the sequence length
        ///  will be mapped to valid positions in a circular way if the sequence is
        ///  circular. Otherwise invalid indices will typically return the gap symbol.
        /// </summary>
        /// <param name="alphabetName"> Name of the alphabet the charSequence is composed of. </param>
        /// <param name="characters"> A character sequence. </param>
        /// <param name="isCircular"> True: sequence is circular, false: sequence is linear. </param>
        public Sequence(String alphabetName, IEnumerable<char> characters, bool isCircular)
        {
            MainSequence = this;
            this.InitSequence(AlphabetFactory.Instance(alphabetName), characters, isCircular);
        }

        /// <summary>
        /// Constructor for a main sequence.
        /// Same as Sequence(String, CharSequence, boolean) constructor but takes
        /// an <see cref="QUT.Bio.BioPatML.Alphabets.Alphabet">Alphabet</see>
        /// reference instead of the alphabet name to create a
        /// sequence. 
        /// </summary>
        /// <param name="alphabet">Alphabet. Null is valid. In this case the constructor
        /// trys to recognize the alphabet of the charSequence automatically.</param>
        /// <param name="characters"> Character sequence. </param>
        /// <param name="isCircular"> true: sequence is circular, false: otherwise. </param>
        public Sequence
            (Alphabet alphabet, IEnumerable<char> characters, bool isCircular)
        {
            MainSequence = this;
            this.InitSequence(alphabet, characters, isCircular);
        }

        /// <summary>
        /// Constructor for a main sequence.
        /// Same as Sequence(String, CharSequence, boolean) constructor but takes
        /// an <see cref="QUT.Bio.BioPatML.Alphabets.Alphabet">Alphabet</see> reference and an array of symbols instead.
        /// </summary>
        /// <param name="alphabet"> Alphabet. Must not be null. </param>
        /// <param name="symbols"> Symbol array. There is no check if the symbols of
        /// the array belong to the specified alphabet! The symbol array will be
        /// cloned so that changes in the orignal array do not influence the 
        /// content of the constructed sequence.</param>
        /// <param name="isCircular"> true: sequence is circular, false: otherwise. </param>
        public Sequence
            (Alphabet alphabet, Symbol[] symbols, bool isCircular)
        {
            MainSequence = this;
            this.Set(1, symbols.Length, +1);

            Symbol[] clonedSymb = (Symbol[]) symbols.Clone();

            this.Accessor = AccessorFactory.Instance(AccessorType, 0,0, Length,
                                                        new SymbolArray(alphabet, clonedSymb));
        }

        /// <summary>
        ///  Constructor for a subsequence.<para></para>
        ///  Creates a subsequence of a sequence. The sequence the subsequence is 
        ///  based on (the base sequence) can be another subsequence or a main sequence, 
        ///  another words there can be many levels of hierarchy sequences where each subsequence could 
        ///  be another subsequence of another mainsequence.
        ///  <para></para>
        ///  The subsequence is transparent which means indices smaller than one or
        ///  bigger than the length of the subsequence will be mapped to valid symbols
        ///  on the main sequence.
        /// </summary>
        /// <param name="sequence"> The base sequence. </param>
        /// <param name="start"> Start position of the subsequence relativ to the base
        /// sequence. First position is one.  </param>
        /// <param name="end"> End position of the subsequence (inclusive). start = end means 
        /// a length of one of the subsequence. </param>
        /// <param name="strand"> Strand the sequence belongs to. +1 = forward strand, 
        /// -1 = backward strand, 0 will be converted to +1 
        /// The strand is always related to the strand of the main sequence.
        /// </param>
        public Sequence
            (Sequence sequence, int start, int end, int strand)
        {
            MainSequence = this;
            int type = strand < 0 ? AccessorFactory.TRANS_REV_COMP
                                        : AccessorFactory.TRANS_DIR;

            this.InitSubsequence(type, sequence, start, end);
        }

        /// <summary>
        ///  Creates a subsequence of a sequence which comprises the given region.
        /// </summary>
        /// <param name="sequence"> The base sequence. </param>
        /// <param name="region"> Region which describes the location, length and strand
        /// of the subsequence to create.
        /// </param>
        public Sequence(Sequence sequence, Region region)
        {
            MainSequence = this;
            int type = region.Strand < 0 ? AccessorFactory.TRANS_REV_COMP : AccessorFactory.TRANS_DIR;

            this.InitSubsequence(type, sequence, region.Start, region.End);
        }

        /// <summary>
        ///  Creates an empty subsequence but sets start, end and strand of the sequence.
        /// </summary>
        /// <param name="start"> 
        /// Start position of the subsequence relativ to the base sequence. 
        /// First position is one.
        /// </param>
        /// <param name="end">
        /// End position of the subsequence relativ to the base sequence.
        /// First position is one.
        /// </param>
        /// <param name="strand">
        /// Strand of the subsequence. This describes always the strand
        /// of the main sequence.
        /// </param>
        protected Sequence(int start, int end, int strand)
            : base(start, end, strand)
        { /* No implementation */ }


        #endregion

        #region -- Sequence Properties --

        /// <summary>
        ///  Gets the name of a sequence. This is just a conveniency method. 
        ///  Calling AnnotationValue("Name"); produces the same result.
        /// </summary>
        public String Name
        {
            get
            {
                if (ListAnnotations == null)
                    return null;

                return this.AnnotationValue("Name");
            }
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        ///  Initializes an empty sequence as a main sequence.  This is an internal method 
        ///  and should only be used within extensions of Jacobi.
        /// </summary>
        /// <param name="alphabet"> Alphabet. Null is valid. In this case the constructor
        /// trys to recognize the alphabet of the charSequence automatically.</param>
        /// <param name="characters"> Character sequence. </param>
        /// <param name="isCircular"> True: sequence is circular, false: otherwise. </param>
        internal void InitSequence
            (Alphabet alphabet, IEnumerable<char> characters, bool isCircular)
        {
            if (alphabet == null)
                alphabet = AlphabetFactory.Recognize(characters);

            this.Set(1, characters.Count(), +1);
            this.AccessorType = isCircular ? AccessorFactory.CIRC_DIR : AccessorFactory.LIN_DIR;
            this.Accessor = AccessorFactory.Instance(AccessorType, 0, 0, Length,
                                                        new SymbolArray(alphabet, characters));
        }

        /// <summary>
        /// Initializes an empty sequence as a subsequence. This is an internal method
        /// and should only be used within extensions of Jacobi.
        /// <see cref="QUT.Bio.BioPatML.Sequences.Sequence">Sequences</see> 
        /// constructor Sequence(Sequence, int, int, int)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sequence"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        internal void InitSubsequence(int type, Sequence sequence, int start, int end)
        {
            this.AccessorType = type;
            this.Set(start, end, (this.AccessorType & AccessorFactory.REVERSE) > 0 ? -1 : +1);
            BaseSequence = sequence;

            if (Length < 0)
                throw new ArgumentOutOfRangeException
                    ("End position smaller than " + "start position! Start = " + start +
                     " end = " + end + " : " + sequence.Name);


            ISymbolArray symbols = null;

            if (sequence.MainSequence != sequence)
            {
                MainSequence = sequence;
                symbols = sequence.Accessor.Symbols;
                Offset = sequence.Offset - (sequence.Strand == this.Strand ?
                                                this.Strand * (1 - Start) : this.Strand * (Start + Length));

            }
            else
            {
                MainSequence = sequence;
                symbols           = MainSequence.Accessor;
                Offset = Strand < 0 ? Start + Length : Start - 1;
            }

            if ((this.AccessorType & AccessorFactory.REVERSE) > 0)
                Accessor = AccessorFactory.Instance
                                            (this.AccessorType, 1, Offset, Length, symbols);

            else
                Accessor = AccessorFactory.Instance
                                            (this.AccessorType, Offset, 0, Length, symbols);
        }

       

        /// <summary>
        /// Compares the sequence at the given position with another sequence and
        /// returns the number of mismatches.
        /// </summary>
        /// <param name="position"> Start position for comparison. First position is one. </param>
        /// <param name="sequence"> The sequence to compare with.</param>
        /// <returns> Returns the number of mismatches. </returns>
        public virtual int Mismatches(int position, Sequence sequence)
        {
            int mismatches = 0;

            for (int i = 0; i < sequence.Length; i++)

                if (!sequence.SymbolAt(i).Equals(GetSymbol(position + i)))
                    mismatches++;

            return mismatches;
            
        }


        /// <summary>
        /// Searches for the pattern within the given sequence and creates a 
        /// <see cref="QUT.Bio.BioPatML.Sequences.List.FeatureList"> FeatureList </see> containning all matches.
        /// <para></para>
        /// Search finds only matches of the pattern which are within the specified range. 
        /// To find all matches in a sequence just call Search(0,0, pattern) or
        /// Search(1,1, pattern) for example.
        /// <para></para>
        /// To find all matching patterns of a circular genome the pattern length has 
        /// to be added to the sequence length:
        /// 
        /// search(1, seq.length()+pattern.length()-1, pattern);
        /// 
        /// </summary>
        /// <param name="startPos"> Start position for search (first position is one).
        /// The start position can be negative. </param>
        /// <param name="endPos"> End position for search. If endPos less than or equals startPos the
        /// start position will be set to one and the end position will be set to the 
        /// length of the sequence. The end position can be greater than the length of 
        /// the sequence (to search across the genome boundary in circular genomes
        /// for instance).</param>
        /// <param name="pattern"> An object which implements the 
        /// <see cref="QUT.Bio.BioPatML.Patterns.IPattern">IPattern</see>
        /// interface. </param>
        /// <returns> Returns a <see cref="QUT.Bio.BioPatML.Sequences.List.FeatureList"> FeatureList </see>
        /// with all matches. 
        /// </returns>
        public FeatureList Search(int startPos, int endPos, IPattern pattern)
        {
            FeatureList featureList = new FeatureList(pattern.PatternName);
            Match match;
            featureList.AttachSequence(this);

            if (endPos <= startPos)
            {
                startPos = 1;
                endPos = startPos + Length - 1;
            }

            while (startPos <= endPos)
            {
                match = pattern.Match(this, startPos);

                if (match != null && match.End <= endPos)
                {
                    match = match.Clone();

                    if (featureList.Name != null) { }
                       // match.Name = featureList.Name;
                    featureList.Add(match);
                }

                startPos += pattern.Increment;
            }

            return (featureList);
        }

        /// <summary>
        ///  Searches for the pattern with the highest similarity within the
        ///  given sequence. Search finds only matches of the pattern which are
        ///  within the specified range. 
        ///  <para></para>
        ///  To find all matches in a sequence just call SearchBest(0,0, pattern) or
        ///  SearchBest(1,1, pattern) for example.
        ///  <para></para>
        ///  To find all matching patterns of a circular genome the pattern length has 
        ///  to be added to the sequence length:
        ///  <example>
        /// <code>
        ///  SearchBest(1, seq.length()+pattern.length()-1, pattern);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="startPos"> Start position for search (first position is one).
        /// The start position can be negative.</param>
        /// <param name="endPos">End position for search. If endPos less than or equals startPos the
        /// start position will be set to one and the end position will be set to the 
        /// length of the sequence. The end position can be greater than the length of 
        /// the sequence (to search across the genome boundary in circular genomes
        /// for instance).
        /// </param>
        /// <param name="pattern"> 
        /// An object which implements the <see cref="QUT.Bio.BioPatML.Patterns.IPattern">IPattern</see> interface,
        /// e.g. a sequence or a start weight matrix. 
        /// </param>
        /// <returns> 
        /// Returns a match object which describes the best match of
        /// the pattern.
        /// </returns>
        public Match SearchBest(int startPos, int endPos, IPattern pattern)
        {
            Match maxMatch = new Match(null, null, 0, 0, +1, -1.0); //fx on 14 march 2010

            if (endPos <= startPos)
            {
                startPos = 1;
                endPos = startPos + Length - 1;
            }

            while (startPos <= endPos)
            {
                Match match = pattern.Match(this, startPos);
                if (match != null
                    && match.Similarity > maxMatch.Similarity
                    && match.End <= endPos)
                    maxMatch.Set(match);

                startPos += pattern.Increment;
            }

            if (maxMatch.Similarity < 0) //nothing found
                return (null);

            maxMatch.SetSequence(this);
            return (maxMatch);
        }


        /// <summary>
        /// Gets the topology of the sequence.
        /// </summary>
        /// <returns>
        /// Returns true: when the sequence is circular, false: otherwise.
        /// </returns>
        public bool IsCircular()
        {
            return (AccessorType & AccessorFactory.CIRCULAR) > 0;
        }


        /// <summary>
        /// The alphabet of this sequence.
        /// <para></para>
        /// It can be "DNA", "RNA" or "AminoAcid"
        /// </summary>
        public Alphabet Alphabet
        {
            get
            {
                return Accessor.Alphabet;
            }
        }

        /// <summary>
        ///  Gets the position of a subsequence relative to the start of the
        ///  main sequence the subsequence refers to. The first position is one.
        /// </summary>
        /// <returns> Returns the position of the subsequence within the main  
        /// sequence or one if the sequence is the main sequence.</returns>
        public int Position()
        {
            return Strand > 0 ? Offset + 1 : Offset - Length;
        }

        /// <summary>
        ///  Converts the position for the given sequence to an equal position for this
        ///  sequence. The method is useful to convert position within subsequences
        ///  (typically features) to positions in the referred sequence (typically a
        ///  main sequence) and vice versa.
        /// </summary>
        /// <param name="sequence">  A sequence. </param>
        /// <param name="position">  A position (starts with one) within the given sequence.</param>
        /// <returns> Returns the converted position.  </returns>
        public int Position(Sequence sequence, int position)
        {
            position = sequence.Offset + sequence.Strand * position;
            return Strand > 0 ? position - Offset : Offset - position;
        }


        /// <summary>
        /// Gets a symbol of the sequence by position.
        /// <para></para>
        /// This method can be confusing because its functionality is similiar to SymbolAt
        /// . In symbolAt the starting index is 0 however here we uses position where the first
        /// element index is 1.
        /// </summary>
        /// <param name="position">position Position of the sequence element. The first position in a
        /// sequence is one, however the position is not bound and "invalid" positions
        /// will be mapped to valid symbols according to the policy of the sequence
        /// accessor.
        /// As a consequence the GetSymbol method usually behaves different for 
        /// the main sequence and a subsequence. </param>
        /// <returns> Symbol of the sequence at the specified position. </returns>
        public Symbol GetSymbol(int position)
        {
            return Accessor.SymbolAt(position - 1);
        }

        /// <summary>
        /// Gets a symbol off the sequence through supplying an index. 
        /// </summary>
        /// <param name="index"> Zero based index of the sequence element. Invalid indices will
        /// be mapped to valid symbols according to the policy of the sequence
        /// accessor.</param>
        /// <returns> Symbol of the sequence for the given index. </returns>
        public Symbol SymbolAt(int index)
        {
            return Accessor.SymbolAt(index);
        }

        /// <summary>
        /// Returns a string representation of the sequence.
        /// </summary>
        /// <returns></returns>
        public String Letters()
        {
            return (Letters(1, Length));
        }

        /// <summary>
        ///  Creates a string representation of the sequence for the given
        ///  section.
        /// </summary>
        /// <param name="start"> Start position for the string representation. First
        /// position is one. </param>
        /// <param name="end">  End position for the string representation. start = end
        /// creates a string with one character.</param>
        /// <returns> Returns a string representation of the sequence section. </returns>
        public String Letters(int start, int end)
        {
            StringBuilder sb = new StringBuilder(end - start + 1);

            for (int i = start; i <= end; i++)
                sb.Append(GetSymbol(i).Letter);

            return (sb.ToString());
        }


        #endregion
 
    }
}
