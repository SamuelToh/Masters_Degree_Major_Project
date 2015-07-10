using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Symbols;

/***************************************************************************
 *  Original Author          : Dr Stefan Maetschke 
 *  Translated By            : Samuel Toh (Email: yu.toh@connect.qut.edu.au) 
 *  Project supervisors      : Dr James Hogan
 *                             Mr Lawrance BuckingHam
 * 
 ***************************************************************************/
namespace QUT.Bio.BioPatML.Symbols.Accessor
{
    /// <summary>
    /// This interface defines a symbol array accessor. An 
    /// <see cref="QUT.Bio.BioPatML.Symbols.Accessor.IAccessor"> IAccessor </see>
    /// describes how symbols in a {@link ISymbolArray} are accessed, e.g. linear, circular...
    /// <para></para>
    /// Accessor are mainly implemented to solve the index out of bounds problem.
    /// Invalid indices are mapped to vaild ones (e.g. circular) or instead of
    /// null replacement symbols (e.g. gap symbol) are returned.
    /// <para></para>
    /// Note, that the <see cref="QUT.Bio.BioPatML.Symbols.Accessor.IAccessor"> IAccessor </see>
    /// interface is an extension of the
    /// {ISymbolArray} interface and an accessor can therefore serve as a
    /// symbol array which is heavily used to implement subsequences of sequences. 
    /// </summary>
    public interface IAccessor : ISymbolArray
    {
        /// <summary>
        /// Used for retrieving a desired symbol through our symbol Array by 
        /// giving the method a specified index. 
        /// </summary>
        /// <param name="index"> A zero based index for a symbol of the array. </param>
        /// <returns> the symbol at the given index. </returns>
        new Symbol SymbolAt(int index);

        /// <summary>
        ///  This method transforms the given index to an index for the 
        ///  symbol array which is accessed by the accessor. This method is
        ///  useful to calculate the position in a super sequence based on the
        ///  given position in a subsequence.  
        /// </summary>
        /// <param name="index"> Index to transform. </param>
        /// <returns> Returns the transformed index (this is an index within the symbol array). </returns>
        int Transform(int index);

        /// <summary>
        /// Property our symbol array. The accessor provides access to by
        /// implementing the symbolAt(int) method.
        /// <para></para>
        /// Note that the symbolAt(int) method of the accessor and
        /// the symbol array are usually different.
        /// </summary>
        ISymbolArray Symbols { get; }
    }
}
