using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Symbols;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Sequences;

/***************************************************************************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Statistic
{
    /// <summary>
    ///  This class implements a histogram over symbols.
    ///  <see> Symbol </see>
    /// </summary>
    public class HistogramSymbol
    {
        #region -- Private Fields --

        /// <summary>
        /// Sum over all bins 
        /// </summary>
        protected int sum = 0;

        /// <summary>
        /// Holds the counters for all symbol names */
        /// </summary>
        private Dictionary<String, Counter> DictionaryCounter = new Dictionary<string,Counter>();

        /** this list gives linear access to the histogram elements */
        private List<Counter> list = new List<Counter>();

        #endregion

        #region -- Public Constructors --
        /// <summary>
        ///  Constructs an empty histogram
        /// </summary>
        public HistogramSymbol() { }

        /// <summary>
        ///  Constructs a histogram based on the provided list of sequencs.
        /// </summary>
        /// <param name="sequenceList"> List of sequences. </param>
        public HistogramSymbol(SequenceList sequenceList)
        {
            Add(sequenceList);
        }

        /// <summary>
        ///  Constructs a histogram based on the provided sequence.
        /// </summary>
        /// <param name="sequence"></param>
        public HistogramSymbol(Sequence sequence)
        {
            Add(sequence);
        }
#endregion

        #region -- Method : Add Component of our Histogram --

        /// <summary>
        ///  Adds all symbols contained in the given sequence to the histogram.
        /// </summary>
        /// <param name="sequence"> The sequence with sysmbols to add. </param>
        public void Add(Sequence sequence)
        {
            for (int pos = 1; pos <= sequence.Length; pos++)
                Add(sequence.GetSymbol(pos));
        }

        /// <summary>
        ///  Constructs a histogram based on the provided list of sequencs.
        /// </summary>
        /// <param name="sequenceList"> List of sequences. </param>
        public void Add(SequenceList sequenceList)
        {
            for (int i = 0; i < sequenceList.Count; i++)
                Add(sequenceList[i]);
        }

        /// <summary>
        ///  Adds all symbols and their frequencies of the given histogram to this histogram.
        /// </summary>
        /// <param name="histogram"> A symbol histogram. </param>
        public void Add(HistogramSymbol histogram)
        {
            for (int i = 0; i < histogram.Count; i++)
            {
                Symbol symbol = histogram[i];
                Add(symbol, histogram.HistoValue(i));
            }
        }

        /// <summary>
        ///  Adds a symbol to the histogram.
        /// </summary>
        /// <param name="symbol"> The symbol to add. </param>
        public void Add(Symbol symbol)
        {
            Add(symbol, 1);
        }

        /// <summary>
        ///  Adds an symbol to the histogram with a given number of counts.
        /// </summary>
        /// <param name="symbol"> A symbol. </param>
        /// <param name="counts"> Counts for the given symbol. </param>
        public void Add(Symbol symbol, int counts)
        {
            Counter counter = null; //did the C# style

            if (DictionaryCounter.ContainsKey(symbol.Name))
                counter = DictionaryCounter[symbol.Name];

            if (counter == null)
            {
                counter = new Counter(counts, symbol, list.Count);
                DictionaryCounter.Add(symbol.Name, counter);
                list.Add(counter);
                sum++;
            }
            else
            {
                counter.counts += counts;
                sum += counts;
            }
        }

        #endregion

        #region -- Method: Substract Component --

        /// <summary>
        ///  Subtract all symbols and their frequencies of the given histogram from this
        ///  histogram.
        /// </summary>
        /// <param name="histogram"></param>
        public void Substract(HistogramSymbol histogram)
        {
            for (int i = 0; i < histogram.Count; i++)
            {
                Symbol symbol = histogram[i];
                Add(symbol, -histogram.HistoValue(i));
            }
        }

        #endregion

        #region -- Methods --

        /// <summary>
        ///  Getter for the histogram bin (= the symbol itself, not its counter).
        /// </summary>
        /// <param name="binIndex"> Index of a bin. Value must be in range 0..size()-1. </param>
        /// <returns> Returns the symbol for the given bin index. </returns>
        public Symbol this[int binIndex]
        {
            get
            {
                return (list[binIndex].symbol);
            }
        }

        /// <summary>
        ///  Getter for the histogram value (=counter) for the specified bin.
        /// </summary>
        /// <param name="binIndex"> Index of a bin. Value must be in range 0..size()-1. </param>
        /// <returns> Histogram value (=counter) for the specified bin. </returns>
        public int HistoValue(int binIndex)
        {
            return (list[binIndex].counts);
        }

        /// <summary>
        ///  Getter for the histogram value (=counts) for the specified object.
        /// </summary>
        /// <param name="symbol"> Symbol </param>
        /// <returns> Histogram value (=counts) for the given object. Zero if the 
        ///           symbol is not in the histogram. </returns>
        public int HistoValue(Symbol symbol)
        {
            Counter counter = null;
            
            if(DictionaryCounter.ContainsKey(symbol.Name))
                counter = DictionaryCounter[symbol.Name];

            if (counter == null) //unknown symbol
                return (0);

            return (counter.counts);
        }

        /// <summary>
        ///  Returns the relative frequency of the given symbol within the histogram.
        /// </summary>
        /// <param name="symbol"> A symbol. </param>
        /// <returns> Returns the relative frequency or zero if the symbol is unknown. </returns>
        public double Frequency(Symbol symbol)
        {
            return (sum > 0 ? (double)HistoValue(symbol) / sum : 0);
        }

        /// <summary>
        /// Gets for the bin index the given symbol belongs to.
        /// </summary>
        /// <param name="symbol"> The symbol. </param>
        /// <returns> Bin index, or -1 if the symbol isn't contained </returns>
        public int CalSymIndex(Symbol symbol)
        {
            Counter counter = null;
            
            if(DictionaryCounter.ContainsKey(symbol.Name))
                counter =  DictionaryCounter[symbol.Name];

            if (counter == null)
                return (-1);
            return (counter.binIndex);
        }

        /// <summary>
        ///  Removes all entries from the histogram.
        /// </summary>
        public void Clear()
        {
            sum = 0;
            DictionaryCounter.Clear();
            list.Clear();
        }

        /// <summary>
        ///  Returns a string representation of the histogram.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Count; i++)
                sb.Append(this[i].Letter).Append(":").Append(HistoValue(i)).Append(" ");
            return sb.ToString();
        }


        #endregion

        #region -- Properties --
        /// <summary>
        /// Counts the total number of histogram entries or bins.
        /// </summary>
        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        /// <summary>
        ///  Returns the sum over all counts within the histogram. 
        /// </summary>
        public int Sum
        {
            get
            {
                return (sum);
            }
        }

        /// <summary>
        ///  Finds the maximum value within the histogram.
        /// </summary>
        public int Max
        {
            get
            {
                int max = -int.MaxValue;
                for (int i = 0; i < Count; i++)
                {
                    int counts = HistoValue(i);
                    if (counts > max)
                        max = counts;
                }
                return max;
            }
        }

        #endregion

        #region Private Class

        /// <summary>
        ///  This class is just a counter for symbols.
        /// </summary>
        private class Counter
        {
            public int counts = 0;

            public int binIndex;
             
            public Symbol symbol;

            /// <summary>
            ///   Creates a counter instance.
            /// </summary>
            /// <param name="counts"> Counter for the symbol. </param>
            /// <param name="symbol"> The symbol itself. </param>
            /// <param name="binIndex"> Index of the histogram where the symbol belongs to. </param>
            public Counter
                (int counts, Symbol symbol, int binIndex)
            {
                this.counts = counts;
                this.symbol = symbol;
                this.binIndex = binIndex;
            }

        }

        #endregion
    }
}
