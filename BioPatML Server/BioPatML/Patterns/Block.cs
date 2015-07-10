using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using QUT.Bio.BioPatML.Symbols;
using QUT.Bio.BioPatML.Sequences.List;
using QUT.Bio.BioPatML.Sequences;
using QUT.Bio.BioPatML.Statistic;
using QUT.Bio.BioPatML.Common.XML;
using QUT.Bio.BioPatML.Alphabets;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Patterns
{
    /// <summary>
    /// This pattern describes a block of aligned sequences. Simplification
    /// of a <see cref="QUT.Bio.BioPatML.Patterns.PWM"> PWM (position weight matrix) </see>
    /// and it is directly derived from the PWM class.
    /// </summary>
    public sealed class Block : PWM
    {
        #region -- Constructors --

        /// <summary>
        /// Default Constructor
        /// </summary>
        internal Block()
            : base()
        { /* No implementation */ }

        /// <summary>
        /// Constructs a Block of aligned sequences
        /// (<see cref="QUT.Bio.BioPatML.Patterns.PWM"> PWM </see>).
        /// </summary>
        /// <param name="name">Name for element block</param>
        /// <param name="sequenceList"> List of aligned sequences. </param>
        /// <param name="background"> Histogram with base counts of the background
        /// sequences.</param>
        /// <param name="threshold"> Similarity threshold. </param>
        public Block
            (String name, SequenceList sequenceList,
                HistogramSymbol background, double threshold)
            : base(name, sequenceList[0].Alphabet, threshold)
        {
            Estimate(sequenceList, background);
        }

        #endregion

        #region -- Private Methods --

        /// <summary>
        /// Estimates the weights of the PWM that's behind a Block pattern.
        /// </summary>
        /// <exception cref="System.ArgumentException">
        /// Thrown when sequences length are not equal</exception>
        /// <param name="sequenceList"> List of aligned sequences. </param>
        /// <param name="background"> Histogram with base counts of the background
        /// sequences. Can be null. In that case all frequencies are set equally.</param>
        private void Estimate
            (SequenceList sequenceList, HistogramSymbol background)
        {
            int length = sequenceList.MinLength();

            if (sequenceList.MaxLength() != length)
                throw new ArgumentException
                    ("Sequences must be of equal length!");

            if (background == null)
            {
                background = new HistogramSymbol();

                foreach (Symbol sym in PWMalphabet)
                    background.Add(sym);
            }

            base.Init(length);
            base.Estimate(sequenceList, 1, background);
        }
        #endregion

        #region -- BioPatML XML Read Component --
        /// <summary>
        /// Reads the parameters and populate the attributes for this pattern.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when sequences in blocks are missing.</exception>
        /// <param name="node"></param>
        /// <param name="definition">The Definition element where the node sits in</param>
        public override void ReadNode
            (XmlNode node, Definition definition)
        {
            PatternName = (XMLHelper.GetAttrValueString(node, "name"));
            Threshold = (XMLHelper.GetAttrValDouble(node, "threshold"));
            Impact = (XMLHelper.GetAttrValDouble(node, "impact"));

            PWMalphabet = AlphabetFactory.Instance
                        (XMLHelper.GetAttrValueString(node, "alphabet"));
            SequenceList seqList = new SequenceList();

            node = node.FirstChild;
            while (node != null)
            {
                if (node.Name.Equals("Sequence"))
                {
                    String letters = node.InnerText.Trim();

                    if (letters == null)
                        throw new ArgumentNullException
                            ("Sequences in Block are missing!");

                    seqList.Add(new Sequence(PWMalphabet, letters, false));
                }
                node = node.NextSibling;
            }

            Estimate(seqList, null);
        }
        #endregion
    }
}
