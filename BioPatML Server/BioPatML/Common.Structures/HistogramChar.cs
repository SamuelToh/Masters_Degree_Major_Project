using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/*****************| Queensland  University Of Technology |*******************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Common.Structures
{
    /// <summary>
    /// This class implements a histogram for chars.
    /// </summary>
    public class HistogramChar
    {
        #region -- Private Fields --

        /** Array with characters frequencies */
        private int[] frequencies = new int[128];

        /** Sum over all histogram values */
        private int sum = 0;

        /** Counter of different histogram values */
        private int count = 0;

        #endregion

        #region -- Properties --

        /// <summary>
        /// Getter of the counter of different histogram values. This is really just a
        /// getter for a stored sum and therefore efficient.
        /// 
        /// Return Counter of different histogram values.
        /// </summary>
        public int Count
        {
            get
            {
                return (count);
            }
        }


        /// <summary>
        /// Getter for the sum over all histogram values. This is really just a
        /// getter and therefore very efficient.
        /// 
        /// Return Sum over all histogram values.
        /// </summary>
        public int Sum
        {
            get
            {
                return sum;
            }
        }

        #endregion

        #region -- Default Constructor --

        //No special constructor is implemented.

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Increments the frequency of the given character by the given amount.
        /// </summary>
        /// <param name="ch">A char (ASCII code must be in [0,127]).</param>
        /// <param name="frequency">Frequency</param>
        public void Inc(char ch, int frequency)
        {
            if (frequencies[ch] == 0)
                count++;

            frequencies[ch] += frequency;
            sum += frequency;
        }

        /// <summary>
        /// Increments the frequency of the given character by one.
        /// </summary>
        /// <param name="ch">A char (ASCII code must be in [0,127]).</param>
        public void Inc(char ch)
        {
            Inc(ch, 1);
        }

        /// <summary>
        /// Increments the frequencies of the histogram symbols by scanning the given
        /// char sequence. Subsequently the histogram contains the frequncies 
        /// of all chars in the sequence (plus previous frequencies, if there are any).
        /// </summary>
        /// <param name="characters"> String of character sequence </param>
        public void Inc(IEnumerable<char> characters)
        {
            foreach (char ch in characters)
                Inc(ch);
        }

        /// <summary>
        /// Getter for the frequency of the given char.
        /// </summary>
        /// <param name="ch"> A char (ASCII code must be in [0,127]). </param>
        /// <returns> Returns the frequency of the given symbol. </returns>
        public int Get(char ch)
        {
            return (frequencies[ch]);
        }

        /// <summary>
        /// Getter for the relative frequency of the given char.
        /// </summary>
        /// <param name="ch"> A char (ASCII code must be in [0,127]). </param>
        /// <returns>
        /// Returns the relative frequency of the given symbol.
        /// </returns>
        public double GetRelative(char ch)
        {
            return 
                (Sum > 0 ? (double)frequencies[ch] / sum : 0);
        }

        #endregion
    }
}
