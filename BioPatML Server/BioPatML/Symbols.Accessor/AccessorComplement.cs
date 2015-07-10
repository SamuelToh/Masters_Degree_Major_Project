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
namespace QUT.Bio.BioPatML.Symbols.Accessor
{
    /// <summary>
    ///  This class implements a complement symbol array accessor. 
    /// </summary>
    internal sealed class AccessorComplement : AccessorBase
    {
        #region -- Public Constructor --
        /// <summary>
        ///  Creates an accessor which returns complements of symbols.
        /// </summary>
        /// <param name="symbols">An array of symbols that implements ISymbolArray</param>
        public AccessorComplement(ISymbolArray symbols)
            : base(null, 0, symbols)
        { /* No implementation required */ }

        #endregion

        #region -- Properties --

        /// <summary>
        ///  Property for the symbol array, which our accessor is working on.
        /// </summary>
        public override ISymbolArray Symbols
        {
            get
            {
                return
                    ((IAccessor)symbols).Symbols;
            }
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        ///  Transforms the given index to an index within the symbol array.
        ///  For the complement accessor both indices are the same.
        /// </summary>
        /// <param name="index">Index to be transformed</param>
        /// <returns>The transformed value. Used for extracting our desired symbol
        /// in our symbolArray.</returns>
        public override int Transform(int index)
        {
            return base.Transform(index);
        }

        /// <summary>
        ///  Property for the symbol at a specified index. 
        /// </summary>
        /// <param name="index"> Basically the position of a symbol within the symbol array.</param>
        /// <returns> Returns desired symbol.</returns>
        public override Symbol SymbolAt(int index)
        {
            return symbols.SymbolAt(index).Complement;
        }

        #endregion
    }
}
