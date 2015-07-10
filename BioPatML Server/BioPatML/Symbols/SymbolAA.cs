using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/***************************************************************************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Symbols
{
    /// <summary>
    ///  This class implements amino acid symbols as an extension of the {@link Symbol}
    ///  class. The difference between a {@link Symbol} and
    ///  an amino acid symbol is that the later has a vector of chemical properties.
    ///  <para></para>
    ///  A property vector contains 'T' and 'F' for the following chemical properties, 
    ///  taken from  http://en.wikipedia.org/wiki/Amino_acid:
    ///  [Hydrophobic, Polar, Positive, Negative, Small, Tiny, Aromatic, Aliphatic]
    /// </summary>
    public class SymbolAA : Symbol
    {
        #region -- Private Fields --

        /// <summary>
        /// A 8 character string representing properties of our amino acid.
        /// <para></para>
        /// Each character represents a property of an amino acid.
        /// </summary>
        public String Properties { get; internal set; }

        #endregion

        #region -- Public Constructors --

        /// <summary>
        ///  Creates an amino acid symbol. 
        /// </summary>
        /// <param name="letter"> The one character letter of a symbol. </param>
        /// <param name="code">   The three letter code of the symbol. </param>
        /// <param name="name">   The full name of the symbol. </param>
        /// <param name="properties"> properties String with property flags. </param>
        public SymbolAA (char letter, String code,
                            String name, String properties)
            : base (letter, code, name)
        {

            if (properties.Length != 8)
                throw new ArgumentOutOfRangeException
                    ("Invalid number of properties " + properties); 

            Properties = properties;

        }

        #endregion

        #region < Getters for the amino acid's properties >

        /// <summary>
        ///  Getter for hydrophobicity flag the amino acid. See class description for
        ///  details. Classification according to http://en.wikipedia.org/wiki/Amino_acid .
        ///  Returns true: amino acid is hydrophobic, false: otherwise.
        /// </summary>
        public bool IsHydrophobic
        {
            get
            {
                return Properties[0] == 'T';
            }
        }

        /// <summary>
        ///  Getter for the polar flag the amino acid. See class description for
        ///  details. Classification according to http://en.wikipedia.org/wiki/Amino_acid .
        ///  Returns true: amino acid is polar, false: otherwise.
        /// </summary>
        public bool IsPolar
        {
            get
            {
                return Properties[1] == 'T';
            }
        }

        /// <summary>
        ///  Getter for the positive flag the amino acid. See class description for
        ///  details. Classification according to http://en.wikipedia.org/wiki/Amino_acid .
        ///  Returns true: amino acid is positively charged, false: otherwise.
        /// </summary>
        public bool IsPositive
        {
            get
            {
                return Properties[2] == 'T';
            }
        }

        /// <summary>
        ///  Getter for the negative flag the amino acid. See class description for
        ///  details. Classification according to http://en.wikipedia.org/wiki/Amino_acid .
        ///  Returns true: amino acid is negatively charged, false: otherwise.
        /// </summary>
        public bool IsNegative
        {
            get
            {
                return Properties[3] == 'T';
            }
        }

        /// <summary>
        ///  Getter for the charged flag the amino acid. See class description for
        ///  details. Classification according to http://en.wikipedia.org/wiki/Amino_acid .
        ///  Returns true: amino acid is charged, false: otherwise.
        /// </summary>
        public bool IsCharged
        {
            get
            {
                return (Properties[2] == 'T' || Properties[3] == 'T');
            }
        }

        /// <summary>
        ///  Getter for the small flag the amino acid. See class description for
        ///  details. Classification according to http://en.wikipedia.org/wiki/Amino_acid .
        ///  Returns true: amino acid is small, false: otherwise.
        /// </summary>
        public bool IsSmall
        {
            get
            {
                return Properties[4] == 'T';
            }
        }

        /// <summary>
        ///  Getter for the tiny flag the amino acid. See class description for
        ///  details. Classification according to http://en.wikipedia.org/wiki/Amino_acid .
        ///  Returns true: amino acid is tiny, false: otherwise.
        /// </summary>
        public bool IsTiny
        {
            get
            {
                return Properties[5] == 'T';
            }
        }

        /// <summary>
        ///  Getter for the aromatic flag the amino acid. See class description for
        ///  details. Classification according to http://en.wikipedia.org/wiki/Amino_acid .
        ///  Returns true: amino acid is aromatic, false: otherwise.
        /// </summary>
        public bool IsAromatic
        {
            get
            {
                return Properties[6] == 'T';
            }
        }

        /// <summary>
        ///  Getter for the aliphatic flag the amino acid. See class description for
        ///  details. Classification according to http://en.wikipedia.org/wiki/Amino_acid .
        ///  Returns true: amino acid is aliphatic, false: otherwise.
        /// </summary>
        public bool IsAliphatic
        {
            get
            {
                return Properties[7] == 'T';
            }
        }

        #endregion
    }
}
