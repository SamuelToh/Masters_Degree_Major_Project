using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Common.Structures;


/***************************************************************************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Alphabets
{
    /// <summary>
    /// This class creates the molecule alphabet with the given name.
    /// <para></para>
    /// Current supported type are : DNA / RNA / AA / Unknowns
    /// </summary>
    public static class AlphabetFactory
    {
        #region -- Static readonly alphabet codes -- 

        /// <summary>
        /// The DNA alphabet
        /// </summary>
        public static readonly string CODE_DNA = "DNA";

        /// <summary>
        /// The RNA alphabet
        /// </summary>
        public static readonly string CODE_RNA = "RNA";

        /// <summary>
        /// The protein alphabet consisting of amino acids
        /// </summary>
        public static readonly string CODE_PROTEIN = "AA";

        /// <summary>
        /// Other unknown latin 
        /// </summary>
        public static readonly string CODE_UNKNOWN = "UNKNOWN";

        #endregion -- Static readonly alphabet codes --

        #region -- Static Method --
        /// <summary>
        /// List of all supported Alphabets.
        /// </summary>
        private static List<string> supportedTypes = new List<string>(){
            CODE_DNA,
            CODE_RNA,
            CODE_PROTEIN,
            CODE_UNKNOWN
        };

        /// <summary>
        /// Retrieves a list of supported Alphabet in this version of BioPatML
        /// </summary>
        public static IList<string> ListTypes
        {
            get
            {
                return supportedTypes.AsReadOnly();
            }
        }

        #endregion -- Static Method --

        #region -- Factory Instance Implementation --
        /// <summary>
        ///  Creates an instance of the alphabet with the given name. This is a flyweight 
        ///  implementation. A specific alphabet will be created only once and then just 
        ///  the reference to the existing one will be returned.
        ///  The following alphabets are known:
        ///    
        /// "DNA"
        /// "RNA"
        /// "AA" or "PROTEIN"
        /// "UNKNOWN" 
        /// 
        /// "UNKNOWN" is a valid alphabet name and returns null.
        /// </summary>
        /// <param name="name">Name of the alphabet.</param>
        /// <returns>
        /// Returns the specified alphabet. (null for "UNKNOWN").
        /// </returns>
        static public Alphabet Instance(String name)
        {
            if (name.Equals("DNA"))
                   return (AlphabetDNA.Instance());

            else
                if (name.Equals("RNA"))
                    return (AlphabetRNA.Instance());

            else
                if (name.Equals("AA") || name.Equals("PROTEIN")) 
                    return (AlphabetAA.Instance());

           else
                if (name.Equals("UNKNOWN"))
                            return null;

            throw new ArgumentException
                ("Invalid alphabet name '" + name + "'!");

        }

        #endregion -- Factory Instance Implementation --

        #region -- Method - Methodology for recognising the alphabet type of a given character seq --
        /// <summary>
        /// Recognizes the alphabet of the given character sequence. The method uses
        /// a simple heuristics which might fail especially when the sequence is
        /// short. Please note, that a DNA or RNA sequence can contain more than
        /// the symbols of the four bases.
        /// </summary>
        /// <param name="characters"> Character sequence. </param>
        /// <returns> Returns an alphabet. </returns>
       static public Alphabet Recognize(IEnumerable<char> characters)
        {
            HistogramChar histo = new HistogramChar();

            histo.Inc(characters);

            if ((
                histo.Get('F') + histo.Get('E') + histo.Get('P') + histo.Get('Q') +
                histo.Get('I') + histo.Get('L') + histo.Get('F') + histo.Get('E') +
                histo.Get('p') + histo.Get('q') + histo.Get('i') + histo.Get('l')) > 0)

                return (AlphabetAA.Instance());

            if (histo.GetRelative('U') > 0.1 || histo.GetRelative('u') > 0.1)
                return (AlphabetRNA.Instance());

            if (histo.GetRelative('a') > 0.1 || histo.GetRelative('A') > 0.1)
                return (AlphabetDNA.Instance());

            return (AlphabetAA.Instance());
        }

        #endregion
    }
   
}
