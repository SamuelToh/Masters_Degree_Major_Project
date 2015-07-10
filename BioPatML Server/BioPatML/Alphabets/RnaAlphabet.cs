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
	/// This alphabet describes the RNA alphabet. All symbols are in lower case.
	/// Upper case symbols are converted to lower case if necessary. The alphabet
	/// contains the following symbols:
	/// <para></para>
	/// Symbol   Complement    Code     Name <para></para>
	/// n         n         Any      Any Nucleotide <para></para>
	/// x         x         Any      Any Nucleotide <para></para>
	/// g         c         Gua      Guanine <para></para>
	/// a         u         Ade      Adenine <para></para>
	/// u         a         Ura      Uracil <para></para>
	/// c         g         Cyt      Cytosine <para></para>
	/// -         -         Gap      Gap <para></para>
	/// m         k         A/C      Ade or Cyt <para></para>
	/// r         y         A/G      Ade or Gua <para></para>
	/// w         w         A/U      Ade or Ura<para></para>
	/// s         s         C/G      Cyt or Gua <para></para>
	/// y         r         C/U      Cyt or Ura <para></para>
	/// k         m         G/U      Gua or Ura <para></para>
	/// v         b         ACG      Ade or Cyt or Gua<para></para>
	/// h         d         ACU      Ade or Cyt or Ura<para></para>
	/// d         h         AGU      Ade or Gua or Ura<para></para>
	/// b         v         CGU      Cyt or Gua or Ura<para></para>
	/// .         .         NON      No Nucleotide <para></para>
	/// <para></para>
	/// 'n' is the default symbol. Any unknown letter will be mapped to this symbol
	/// when the this[) method is used.
	/// </summary>
	public sealed class RnaAlphabet : Alphabet {
		//The one and only instance of this class
		private static RnaAlphabet singleton = new RnaAlphabet();

		private static Symbol defaultSymbol = new SymbolMeta( 'n', "Any", "Any Nucleotide" );

		/// <summary>
		/// Creates an instance (which is a singleton) of the alpahbet.
		/// </summary>
		/// <returns> Returns an instance of the alphabet. </returns>

		public static RnaAlphabet Instance () {
			return ( singleton );
		}

		/// <summary>
		///  Creates a map with all RNA symbols. This constructor is private
		///  to ensure that there will be always only one RNA alphabet. Use
		///  the Instance() method or the <see cref="QUT.Bio.BioPatML.Alphabets.AlphabetFactory"> AlphabetFactory </see> to create
		///  the alphabet.
		///  Unknown symbols will be mapped to 'n'.
		/// </summary>
		private RnaAlphabet ()
			: base( "RNA" ) {

			Add( new Symbol( 'g', "Gua", "Guanine" ) );
			Add( new Symbol( 'a', "Ade", "Adenine" ) );
			Add( new Symbol( 'u', "Ura", "Uracil" ) );
			Add( new Symbol( 'c', "Cyt", "Cytosine" ) );

			Add( new SymbolMeta( 'x', "Any", "Any Nucleotide" ) );
			Add( new SymbolMeta( '.', "NON", "No Nucleotide" ) );
			Add( new SymbolMeta( '-', "Gap", "Gap" ) );

			Add( new SymbolMeta( 'm', "A/C", "Ade or Cyt" ) );
			Add( new SymbolMeta( 'r', "A/G", "Ade or Gua" ) );
			Add( new SymbolMeta( 'w', "A/U", "Ade or Ura" ) );
			Add( new SymbolMeta( 's', "C/G", "Cyt or Gua" ) );
			Add( new SymbolMeta( 'y', "C/U", "Cyt or Ura" ) );
			Add( new SymbolMeta( 'k', "G/U", "Gua or Ura" ) );
			Add( new SymbolMeta( 'v', "ACG", "Ade or Cyt or Gua" ) );
			Add( new SymbolMeta( 'h', "ACU", "Ade or Cyt or Ura" ) );
			Add( new SymbolMeta( 'd', "AGU", "Ade or Gua or Ura" ) );
			Add( new SymbolMeta( 'b', "CGU", "Cyt or Gua or Ura" ) );

			// complements
			this['g'].Complement = ( this['c'] );
			this['a'].Complement = ( this['u'] );
			this['u'].Complement = ( this['a'] );
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

			// equals symbols
			( (SymbolMeta) this['.'] ).Add( this['.'] );
			( (SymbolMeta) this['-'] ).Add( this['-'] );

			( (SymbolMeta) this['m'] ).Add( this['a'] );
			( (SymbolMeta) this['m'] ).Add( this['c'] );
			( (SymbolMeta) this['r'] ).Add( this['a'] );
			( (SymbolMeta) this['r'] ).Add( this['g'] );
			( (SymbolMeta) this['w'] ).Add( this['a'] );
			( (SymbolMeta) this['w'] ).Add( this['u'] );
			( (SymbolMeta) this['s'] ).Add( this['c'] );
			( (SymbolMeta) this['s'] ).Add( this['g'] );
			( (SymbolMeta) this['y'] ).Add( this['c'] );
			( (SymbolMeta) this['y'] ).Add( this['u'] );
			( (SymbolMeta) this['k'] ).Add( this['g'] );
			( (SymbolMeta) this['k'] ).Add( this['u'] );
			( (SymbolMeta) this['v'] ).Add( this['a'] );
			( (SymbolMeta) this['v'] ).Add( this['c'] );
			( (SymbolMeta) this['v'] ).Add( this['g'] );
			( (SymbolMeta) this['h'] ).Add( this['a'] );
			( (SymbolMeta) this['h'] ).Add( this['c'] );
			( (SymbolMeta) this['h'] ).Add( this['u'] );
			( (SymbolMeta) this['d'] ).Add( this['a'] );
			( (SymbolMeta) this['d'] ).Add( this['g'] );
			( (SymbolMeta) this['d'] ).Add( this['u'] );
			( (SymbolMeta) this['b'] ).Add( this['c'] );
			( (SymbolMeta) this['b'] ).Add( this['g'] );
			( (SymbolMeta) this['b'] ).Add( this['u'] );
		}

		/// <summary> Tests if a letter is a valid letter of the alphabet. Uppercase and 
		/// lowercase letters are accepted.
		/// </summary>
		/// <param name="letter"> Letter of a symbol. </param>
		/// <returns>
		/// True: the letter is valid, false: otherwise.
		/// </returns>

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

		/// <summary> Gets the default symbol of this alphabet.
		/// </summary>

		public override Symbol DefaultSymbol {
			get {
				return defaultSymbol;
			}
		}
	}
}
