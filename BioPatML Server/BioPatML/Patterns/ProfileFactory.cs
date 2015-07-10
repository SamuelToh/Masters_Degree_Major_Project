using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    /// This factory creates profiles of different types.
    /// </summary>
    public static class ProfileFactory
    {
        #region -- Factory Instance Implementations --

        /// <summary>
        /// Creates a profile.
        /// </summary>
        /// <param name="type">Type of the profile. Must be "ALL" or "BEST".</param>
        /// <returns>Returns a profile of the specified type.</returns>
        static public Profile Create(String type)
        {
            if (type.Equals("ALL"))
                return (new ProfileAll());

            else 
                if (type.Equals("BEST"))
                    return (new ProfileBest());

            else
                throw new ArgumentException
                    ("Unknown type: " + type);
        }

        #endregion
    }
}
