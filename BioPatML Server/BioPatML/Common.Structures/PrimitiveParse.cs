using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
    ///  This class provides several conversion routines. 
    ///  This is a static class calculation method can be called directly without instantialing the class
    /// </summary>
    public class PrimitiveParse
    {
        #region -- Default Constructor --

        //No unique constructor is implemented

        #endregion

        #region -- Public Methods --

        /// <summary>
        ///  Converts a string with numbers in a array with doubles.
        /// </summary>
        /// <param name="str"> String with numbers. Valid delimiters are ";,: ". </param>
        /// <returns> Returns a double array with the string values. </returns>
        static public double[] StringToDoubleArray(String str)
        {
            str = str.Trim();
            String[] elements = Regex.Split(str, "[;,:\\s]+");
            double[] array = new double[elements.Length];


            for (int index = 0; index < elements.Length; index++)
            {
                try
                {
                    array[index] = double.Parse(elements[index]);
                }
                catch (Exception)
                {
                    array[index] = 0;
                }
            }

            return (array);
        }

        /// <summary>
        ///    arses the content of a string to produce a double value. If the string
        ///    is not parsable (empty, contains no number) zero is returned.
        /// </summary>
        /// <param name="str"> String to parse. </param>
        /// <returns> Double value of the string, or zero. </returns>
        static public double Atod(String str)
        {
            try
            {
                return (str == null ? 0 : Double.Parse(str));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Parses the content of a string to produce an integer value. If the string
        /// is not parsable (empty, contains no number) zero is returned.
        /// </summary>
        /// <param name="str"> String to parse. </param>
        /// <returns> Integer value of the string, or zero. </returns>
        static public int Atoi(String str)
        {
            return ((int)Atod(str));
        }

        /// <summary>
        ///  Converts a string with numbers in a array with integers.
        /// </summary>
        /// <param name="str"> String with numbers. Valid delimiters are ";,: ". </param>
        /// <returns> Returns a int array with the string values. </returns>
        static public int[] StringToIntArray(String str)
        {
            str = str.Trim();
            String[] elements = Regex.Split(str, "[;,:\\s]+");
            int[] array = new int[elements.Length];


            for (int index = 0; index < elements.Length; index++)
            {
                try
                {
                    array[index] = int.Parse(elements[index]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    array[index] = 0;
                }
            }

            return (array);
        }

        /// <summary>
        ///  Converts a double array into string. The values are separated by spaces.
        /// </summary>
        /// <param name="array"> Array. </param>
        /// <returns> String with double values. </returns>
        static public String ArrayToString(double[] array)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
                sb.Append(array[i]).Append(" ");

            return (sb.ToString());
        }

        #endregion
    }
}
