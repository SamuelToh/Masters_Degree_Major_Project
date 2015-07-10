using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using QUT.Bio.BioPatML.Sequences;

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
    /// This interface defines a pattern. Every object which implements the 
    /// pattern interface can be searched within a {Sequence}.
    /// </summary>
    public interface IPattern : IMatcher
    {
        /// <summary>
        /// Gets/Sets the pattern name.
        /// </summary>
        string PatternName { get; set;  }

        /// <summary>
        /// Sets/Gets the similarity threshold. Only matches will be returned with a 
        /// similarity equal or higher than the given similarity threshold.
        /// It is recommended that the constructor of a pattern requires the 
        /// threshold parameter as well to make sure that a proper threshold is
        /// set for new pattern.
        /// 
        /// </summary>
        Double Threshold { get; set; }

        /// <summary>
        /// Sets/ Gets the impact of a pattern. This a weight is taken into account
        /// when the overall similarity of a structured pattern, consisting of
        /// other patterns, is calculated.
        /// *Note given param value Impact weight. Must be between zero and one.
        /// </summary>
        Double Impact { get; set; }

        /// <summary>
        /// Reads the parameters for a pattern at the given node.
        /// </summary>
        /// <param name="node"> A pattern node found in XML. </param>
        /// <param name="definition">The definition container wrapping the node param</param>
        void ReadNode(XmlNode node, Definition definition);
    }
}
