using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    /// This class provides methods to work with arrays. Actually templates would
    /// be very nice here. 
    /// </summary>
    public class SArray : IDisposable
    {
        #region -- Default Constructor --
        //No unique constructor implemented
        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Initializes the given vector with an increasing counter
        /// starting by "start" and stepping with step size "stepSize".
        /// </summary>
        /// <param name="v"> Initialized vector. </param>
        /// <param name="start"> Start value for sequence. </param>
        /// <param name="stepSize"> Step size for sequence. </param>
        public static void InitLinear
            (double[] v, double start, double stepSize)
        {
            for (int i = 0; i < v.Length; i++)
                v[i] = start + i * stepSize;
        }

        /// <summary>
        /// Smoothes the contents of a double array by using a mean filter.
        /// </summary>
        /// <param name="v">Array</param>
        /// <param name="windowLen"> Length of the smoothing window. </param>
        /// <returns> Returns a new, smooth array. </returns>
        public static double[] smooth
            (double[] v, int windowLen)
        {
            double[] ret = new double[v.Length];
            int twenth = windowLen / 2;

            for(int i = 0; i < v.Length; i ++)
            {
                double sum = 0;

                for(int j = 0; j < windowLen; j++)
                {
                    int index = i - twenth + j;

                    if (index < 0)
                        sum += v[0];
                    else
                        if (index >= v.Length)
                            sum += v[v.Length - 1];

                    else
                         sum += v[index];

                }

                ret[i] = sum / windowLen;
            }

            return (ret);
        } //END Smooth method

        /// <summary>
        /// Returns the index of the biggest element within the array.
        /// </summary>
        /// <param name="v"> Array </param>
        /// <returns>
        /// Returns index of the biggest element or -1 one if the array
        /// contains only -Double.MAX_VALUE values.
        /// </returns>
        public static int MaxIndex(double[] v)
        {
            double max = -Double.MaxValue;
            int    max_i = -1;

            for (int i = 0; i < v.Length; i++)
            {
                if (v[i] > max)
                {
                    max = v[i];
                    max_i = i;
                }
            }

            return (max_i);
        }

        /// <summary>
        /// Returns the index of the smallest element within the array.
        /// </summary>
        /// <param name="v"> Array </param>
        /// <returns>
        /// Returns index of the smallest element or -1 one if the array 
        /// contains only Double.MAX_VALUE values.
        /// </returns>
        public static int MinIndex(double[] v)
        {
            double min = Double.MaxValue;
            int min_i = -1;

            for (int i = 0; i < v.Length; i++)
            {
                if (v[i] < min)
                {
                    min = v[i];
                    min_i = i;
                }
            }

            return (min_i);
        }

        #endregion

        #region -- IDisposable Member --

        /// <summary>
        /// Method to dispose on all objects implementing IDisposable interface
        /// </summary>
        public void Dispose() { }

        #endregion
    }
}
