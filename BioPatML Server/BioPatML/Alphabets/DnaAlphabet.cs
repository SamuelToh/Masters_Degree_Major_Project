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
namespace QUT.Bio.BioPatML.Alphabets {
	/// <summary>
	/// 
	/// This alphabet describes the DNA alphabet. All symbols are in lower case.
	/// Upper case symbols are converted to lower case if necessary. The alphabet
	/// contains the following symbols:
	/// <para></para>
	/// <para></para>
	/// Symbol   Complement    Code     Name    <para></para>
	/// n         n         Any      Any Nucleotide <para></para>
	/// x         x         Any      Any Nucleotide <para></para>
	/// g         c         Gua      Guanine <para></para>
	/// a         t         Ade      Adenine <para></para>
	/// t         a         Thy      Thymine <para></para>
	/// c         g         Cyt      Cytosine <para></para>
	/// -         -         Gap      Gap <para></para>
	/// m         k         A/C      Ade or Cyt <para></para>
	/// r         y         A/G      Ade or Gua (Purine)<para></para>
	/// w         w         A/T      Ade or Thy <para></para>
	/// s         s         C/G      Cyt or Gua <para></para>
	/// y         r         C/T      Cyt or Thy (Pyrimidine)<para></para>
	/// k         m         G/T      Gua or Thy <para></para>
	/// v         b         ACG      Ade or Cyt or Gua<para></para>
	/// h         d         ACT      Ade or Cyt or Thy<para></para>
	/// d         h         AGT      Ade or Gua or Thy<para></para>
	/// b         v         CGT      Cyt or Gua or Thy<para></para>
	/// .         .         NON      No Nucleotide <para></para>
	/// <para></para>
	/// 'n' is the default symbol. Any unknown letter will be mapped to this symbol
	/// when the get() method is used.
	/// 
	/// </summary>
	
	public sealed class DnaAlphabet : Alphabet {
		//The one and only instance of this class
		private static DnaAlphabet singleton = new DnaAlphabet();

		private static Symbol defaultSymbol;

		/// <summary>
		/// Creates an instance (which is a singleton) of the alpahbet.
		/// </summary>
		/// <returns>
		/// Returns an instance of the alphabet.
		/// </returns>

		public static DnaAlphabet Instance () {
			return ( singleton );
		}

		/// <summary>
		/// Creates a map with all DNA symbols. This constructor is private
		/// to ensure that there will be always only one DNA alphabet. Use
		/// the Instance() method or the <see cref="QUT.Bio.BioPatML.Alphabets.AlphabetFactory"> AlphabetFactory </see> to create  to create 
		/// the alphabet.
		/// <para></para>
		/// Unknown symbols will be mapped to 'n'.
		/// 
		/// </summary>

		private DnaAlphabet ()
			: base( "DNA" ) {
			#region Prefixed set of DNA symbols

			Add( new SymbolMeta( 'n', "Any", "Any Nucleotide" ) );
			Add( new SymbolMeta( 'x', "Any", "Any Nucleotide" ) );
			Add( new SymbolMeta( '.', "NON", "No Nucleotide" ) );
			Add( new SymbolMeta( '-', "Gap", "Gap" ) );

			Add( new Symbol( 'g', "Gua", "Guanine" ) );
			Add( new Symbol( 'a', "Ade", "Adenine" ) );
			Add( new Symbol( 't', "Thy", "Thymine" ) );
			Add( new Symbol( 'c', "Cyt", "Cytosine" ) );

			#endregion

			#region Meta symbols of DNA

			Add( new SymbolMeta( 'm', "A/C", "Ade or Cyt" ) );
			Add( new SymbolMeta( 'r', "A/G", "Ade or Gua" ) );
			Add( new SymbolMeta( 'w', "A/T", "Ade or Thy" ) );
			Add( new SymbolMeta( 's', "C/G", "Cyt or Gua" ) );
			Add( new SymbolMeta( 'y', "C/T", "Cyt or Thy" ) );
			Add( new SymbolMeta( 'k', "G/T", "Gua or Thy" ) );
			Add( new SymbolMeta( 'v', "ACG", "Ade or Cyt or Gua" ) );
			Add( new SymbolMeta( 'h', "ACT", "Ade or Cyt or Thy" ) );
			Add( new SymbolMeta( 'd', "AGT", "Ade or Gua or Thy" ) );
			Add( new SymbolMeta( 'b', "CGT", "Cyt or Gua or Thy" ) );

			#endregion

			#region Complements

			this['g'].Complement = ( this['c'] );
			this['a'].Complement = ( this['t'] );
			this['t'].Complement = ( this['a'] );
			this['c'].Complement = ( this['g'] );

			this['m'].Complement = ( this['k'] );
			this['r'].Complement = ( this['y'] );
			this['w'].Complement = ( this['w'] );
			this['s'].Complement = ( this['s'] );
			this['y'].Complement = ( this['r'] );
			this['k'].Complement = ( this['m'] );
			this['v'].Complement = ( this['b'] );
			this['h'].Complement = ( this['d'] );
			this['d'].Complement = ( this['h'] );
			this['b'].Complement = ( this['v'] );

			#endregion

			#region equivalents
			// equals symbols
			( (SymbolMeta) this['.'] ).Add( this['.'] );
			( (SymbolMeta) this['-'] ).Add( this['-'] );
			( (SymbolMeta) this['m'] ).Add( this['a'] );
			( (SymbolMeta) this['m'] ).Add( this['c'] );
			( (SymbolMeta) this['r'] ).Add( this['a'] );
			( (SymbolMeta) this['r'] ).Add( this['g'] );
			( (SymbolMeta) this['w'] ).Add( this['a'] );
			( (SymbolMeta) this['w'] ).Add( this['t'] );
			( (SymbolMeta) this['s'] ).Add( this['c'] );
			( (SymbolMeta) this['s'] ).Add( this['g'] );
			( (SymbolMeta) this['y'] ).Add( this['c'] );
			( (SymbolMeta) this['y'] ).Add( this['t'] );
			( (SymbolMeta) this['k'] ).Add( this['g'] );
			( (SymbolMeta) this['k'] ).Add( this['t'] );
			( (SymbolMeta) this['v'] ).Add( this['a'] );
			( (SymbolMeta) this['v'] ).Add( this['c'] );
			( (SymbolMeta) this['v'] ).Add( this['g'] );
			( (SymbolMeta) this['h'] ).Add( this['a'] );
			( (SymbolMeta) this['h'] ).Add( this['c'] );
			( (SymbolMeta) this['h'] ).Add( this['t'] );
			( (SymbolMeta) this['d'] ).Add( this['a'] );
			( (SymbolMeta) this['d'] ).Add( this['g'] );
			( (SymbolMeta) this['d'] ).Add( this['t'] );
			( (SymbolMeta) this['b'] ).Add( this['c'] );
			( (SymbolMeta) this['b'] ).Add( this['g'] );
			( (SymbolMeta) this['b'] ).Add( this['t'] );

			( (SymbolMeta) this['n'] ).Add( this['a'] );
			( (SymbolMeta) this['n'] ).Add( this['t'] );
			( (SymbolMeta) this['n'] ).Add( this['c'] );
			( (SymbolMeta) this['n'] ).Add( this['g'] );

			( (SymbolMeta) this['x'] ).Add( this['a'] );
			( (SymbolMeta) this['x'] ).Add( this['t'] );
			( (SymbolMeta) this['x'] ).Add( this['c'] );
			( (SymbolMeta) this['x'] ).Add( this['g'] );

			#endregion

			defaultSymbol = this['n'];
		}

		/// <summary> Tests if a letter is a valid letter of the alphabet. Uppercase and 
		/// lowercase letters are accepted.
		/// </summary>
		/// <param name="letter"> Letter of a symbol.</param>
		/// <returns> true: the letter is valid, false: otherwise.</returns>

		public override bool IsValid ( char letter ) {
			return base.IsValid( char.ToLower( letter ) );
		}

		/// <summary> Converts the given letter to lower case and uses the get method of
		/// the super class. This ensures that regardless the letter case the
		/// assigned nucleotide symbol will be returned.
		/// </summary>
		/// <param name="letter"></param>
		/// <returns></returns>

		public override Symbol this[char letter] {
			get { return base[char.ToLower( letter )]; }
		}

		/// <summary> Gets the default symbol for this alphabet.
		/// </summary>

		public override Symbol DefaultSymbol {
			get {
				return defaultSymbol;
			}
		}
	}
}
