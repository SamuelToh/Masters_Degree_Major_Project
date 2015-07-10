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
	/// This alphabet describes the alphabet of amino acid symbols. All symbols are 
	/// in upper case. Lower case symbols are converted to upper case if necessary.
	/// The alphabet contains the following symbols:
	/// <para></para>
	/// ---------------------------- TABLE OF ALPHABETS --------------------<para></para>
	/// Symbol    Complement    Code      Name<para></para>
	/// X          X         XXX       Any amino acid  <para></para>
	/// A          A         Ala       Alanine<para></para>
	/// R          R         Arg       Arginine<para></para>
	/// N          N         Asn       Asparagine<para></para>
	/// D          D         Asp       Aspartic acid<para></para>
	/// C          C         Cys       Cysteine<para></para>
	/// Q          Q         Gln       Glutamine<para></para>
	/// E          E         Glu       Glutamic acid<para></para>
	/// G          G         Gly       Glycine<para></para>
	/// H          H         His       Histidine<para></para>
	/// I          I         Ile       Isoleucine<para></para>
	/// L          L         Leu       Leucine<para></para>
	/// K          K         Lys       Lysine<para></para>
	/// M          M         Met       Methionine<para></para>
	/// F          F         Phe       Phenylalanine<para></para>
	/// P          P         Pro       Proline<para></para>
	/// S          S         Ser       Serine<para></para>
	/// T          T         Thr       Threonine<para></para>
	/// W          W         Trp       Tryptophan<para></para>
	/// Y          Y         Tyr       Tyrosine<para></para>
	/// V          V         Val       Valine<para></para>
	/// *          *         STP       Stop<para></para>
	/// -          -         GAP       Gap<para></para>
	/// U          U         Sel       Selenocysteine <para></para>
	/// B          B         ASX       Asparagine or asparatic acid<para></para>
	/// Z          Z         GLX       Glutamine or glutamic acid    <para></para>
	/// ==================== END OF TABLE =======================<para></para>
	/// <para></para>
	/// 'X' is the default symbol of the alphabet and all unknown letters in a
	/// sequence will be mapped to the default symbol.
	/// 
	/// </summary>
	public sealed class ProteinAlphabet : Alphabet {
		/// <summary> The amino acid alphabet as a singelton 
		/// </summary>

		private static ProteinAlphabet singleton = new ProteinAlphabet();

		private static Symbol defaultSymbol = new Symbols.SymbolMeta( 'X', "XXX", "Any amino acid" );

		/// <summary>
		/// Creates an instance (which is a singleton) of the alpahbet.
		/// </summary>
		/// <returns> Returns an instance of the alphabet. </returns>

		public static ProteinAlphabet Instance () {
			return ( singleton );
		}

		/// <summary>
		///  Creates a map with all amino acid symbols. This constructor is private
		///  to ensure that there will be always only one amino acid alphabet. Use
		///  the instance() or the 
		///  <see cref="QUT.Bio.BioPatML.Alphabets.AlphabetFactory"> AlphabetFactory </see> to create 
		///  the alphabet.
		///  Unknown symbols will be mapped to 'X'.
		/// </summary>

		private ProteinAlphabet ()
			: base( "AA" ) {
			Add( new SymbolAA( 'A', "Ala", "Alanine(A)", "TFFFTTFF" ) );
			Add( new SymbolAA( 'R', "Arg", "Arginine(R)", "FTTFFFFF" ) );
			Add( new SymbolAA( 'N', "Asn", "Asparagine(N)", "FTFFTFFF" ) );
			Add( new SymbolAA( 'D', "Asp", "Aspartic acid(D)", "FTFTTFFF" ) );
			Add( new SymbolAA( 'C', "Cys", "Cysteine(C)", "TFFFTFFF" ) );
			Add( new SymbolAA( 'Q', "Gln", "Glutamine(Q)", "FTFFFFFF" ) );
			Add( new SymbolAA( 'E', "Glu", "Glutamic acid(E)", "FTFTFFFF" ) );
			Add( new SymbolAA( 'G', "Gly", "Glycine(G)", "TFFFTTFF" ) );
			Add( new SymbolAA( 'H', "His", "Histidine(H)", "FTTFFFTF" ) );
			Add( new SymbolAA( 'I', "Ile", "Isoleucine(I)", "TFFFFFFT" ) );
			Add( new SymbolAA( 'L', "Leu", "Leucine(L)", "TFFFFFFT" ) );
			Add( new SymbolAA( 'K', "Lys", "Lysine(K)", "FTTFFFFF" ) );
			Add( new SymbolAA( 'M', "Met", "Methionine(M)", "TFFFFFFF" ) );
			Add( new SymbolAA( 'F', "Phe", "Phenylalanine(F)", "TFFFFFTF" ) );
			Add( new SymbolAA( 'P', "Pro", "Proline(P)", "TFFFTFFF" ) );
			Add( new SymbolAA( 'S', "Ser", "Serine(S)", "FTFFTTFF" ) );
			Add( new SymbolAA( 'T', "Thr", "Threonine(T)", "TTFFTFFF" ) );
			Add( new SymbolAA( 'W', "Trp", "Tryptophan(W)", "TFFFFFTF" ) );
			Add( new SymbolAA( 'Y', "Tyr", "Tyrosine(Y)", "TTFFFFTF" ) );
			Add( new SymbolAA( 'V', "Val", "Valine(V)", "TFFFTFFT" ) );

			Add( new SymbolMeta( '*', "STP", "Stop" ) );
			( (SymbolMeta) this['*'] ).Add( this['*'] );

			Add( new SymbolMeta( '-', "GAP", "Gap" ) );

			Add( new SymbolMeta( 'B', "ASX", "Asparagine or asparatic acid" ) );
			( (SymbolMeta) this[( 'B' )] ).Add( this[( 'N' )] );
			( (SymbolMeta) this[( 'B' )] ).Add( this[( 'D' )] );

			Add( new SymbolMeta( 'Z', "GLX", "Glutamine or glutamic acid" ) );
			( (SymbolMeta) this[( 'Z' )] ).Add( this[( 'Q' )] );
			( (SymbolMeta) this[( 'Z' )] ).Add( this[( 'E' )] );

		}

		/// <summary> Determines if a the given amino acid is hydrophobic or not. 
		/// 
		/// Classification according to http://en.wikipedia.org/wiki/Amino_acid"> wiki amino acid
		/// 
		/// </summary>
		/// <param name="symbol"> Symbol to classify. </param>
		/// <returns> Returns true: if the amino acid is hydrophobic, false: otherwise. </returns>

		public bool IsHydrophobic ( Symbol symbol ) {
			if ( symbol is SymbolAA )
				return ( (SymbolAA) symbol ).IsHydrophobic;

			return false;
		}

		/// <summary>
		/// Determines if a the given amino acid is polar or not. 
		/// 
		/// Classification according to http://en.wikipedia.org/wiki/Amino_acid wiki amino acid 
		/// </summary>
		/// <param name="symbol"> Symbol to classify. </param>
		/// <returns> Returns true: if the amino acid is polar, false: otherwise. </returns>
		public bool IsPolar ( Symbol symbol ) {
			if ( symbol is SymbolAA )
				return ( (SymbolAA) symbol ).IsPolar;

			return false;
		}

		/// <summary>
		///  Determines if a the given amino acid is negatively charged or not. 
		///  
		///  Classification according to http://en.wikipedia.org/wiki/Amino_acid"> wiki amino acid 
		/// </summary>
		/// <param name="symbol">Symbol to classify.</param>
		/// <returns>
		/// Returns true: if the amino acid is negatively charged, false: otherwise.
		/// </returns>
		public bool IsNegative ( Symbol symbol ) {
			if ( symbol is SymbolAA )
				return ( (SymbolAA) symbol ).IsNegative;

			return false;
		}

		/// <summary>
		/// Determines if a the given amino acid is charged or not. 
		/// 
		/// Classification according to http://en.wikipedia.org/wiki/Amino_acid"> wiki amino acid
		/// </summary>
		/// <param name="symbol">Symbol to classify.</param>
		/// <returns>
		/// Returns true: if the amino acid is charged, false: otherwise.
		/// </returns>
		public bool IsCharged ( Symbol symbol ) {
			if ( symbol is SymbolAA )
				return ( (SymbolAA) symbol ).IsCharged;

			return false;
		}

		/// <summary>
		/// Determines if a the given amino acid is small or not. 
		///                             
		/// Classification according to  http://en.wikipedia.org/wiki/Amino_acid wiki amino acid
		/// </summary>
		/// <param name="symbol">Symbol to classify.</param>
		/// <returns>
		/// Returns true: if the amino acid is small, false: otherwise.
		/// </returns>
		public bool IsSmall ( Symbol symbol ) {
			if ( symbol is SymbolAA )
				return ( (SymbolAA) symbol ).IsSmall;

			return false;
		}

		/// <summary>
		/// Determines if a the given amino acid is tiny or not. 
		/// Classification according to http://en.wikipedia.org/wiki/Amino_acid wiki Amino acid
		/// </summary>
		/// <param name="symbol">Symbol to classify.</param>
		/// <returns>Returns true: if the amino acid is tiny, false: otherwise.</returns>
		public bool IsTiny ( Symbol symbol ) {
			if ( symbol is SymbolAA )
				return ( (SymbolAA) symbol ).IsTiny;

			return false;
		}

		/// <summary>
		/// Determines if a the given amino acid is aromatic or not.
		/// 
		/// Classification according to http://en.wikipedia.org/wiki/Amino_acid wiki amino acid
		/// </summary>
		/// <param name="symbol">Symbol to classify.</param>
		/// <returns>Returns true: if the amino acid is aromatic, false: otherwise.</returns>
		public bool IsAromatic ( Symbol symbol ) {
			if ( symbol is SymbolAA )
				return ( (SymbolAA) symbol ).IsAromatic;

			return false;
		}

		/// <summary>
		/// Determines if a the given amino acid is aliphatic or not. 
		/// 
		/// Classification according to http://en.wikipedia.org/wiki/Amino_acid wiki amino acid 
		/// </summary>
		/// <param name="symbol"> Symbol to classify. </param>
		/// <returns>
		/// Returns true: if the amino acid is aliphatic, false: otherwise.
		/// </returns>
		public bool IsAliphatic ( Symbol symbol ) {
			if ( symbol is SymbolAA )
				return ( (SymbolAA) symbol ).IsAliphatic;

			return false;
		}

		/// <summary>
		/// Tests if a letter is a valid letter of the alphabet. Uppercase and 
		/// lowercase letters are accepted.
		/// </summary>
		/// <param name="letter"> Letter of a symbol. </param>
		/// <returns> true: the letter is valid, false: otherwise. </returns>
		public override bool IsValid ( char letter ) {
			return ( base.IsValid( char.ToUpper( letter ) ) );
		}

		/// <summary> Converts the given letter to uppercase and uses the get method of
		/// the super class. This ensures that regardless the letter case the
		/// assigned amino acid symbol will be returned.
		/// </summary>
		/// <param name="letter">character</param>
		/// <returns>returns the symbol based on the given letter</returns>
		
		public override Symbol this[char letter] {
			get { return base[char.ToUpper( letter )]; }
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
