using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Bio.BioPatML.Symbols;

namespace QUT.Bio.BioPatML.Alphabets {
	/// <summary>
	/// Creates a map with all latin symbols. This constructor is private
	/// to ensure that there will be always only one latin alphabet. 
	/// <para></para>
	/// Unknown symbols will be mapped to '_'.
	/// </summary>
	public sealed class LatinAlphabet : Alphabet {
		private LatinAlphabet ()
			: base( "Latin" ) {

			Add( new Symbol( 'A', "-A-", "Letter A" ) );
			Add( new Symbol( 'B', "-B-", "Letter B" ) );
			Add( new Symbol( 'C', "-C-", "Letter C" ) );
			Add( new Symbol( 'D', "-D-", "Letter D" ) );
			Add( new Symbol( 'E', "-E-", "Letter E" ) );
			Add( new Symbol( 'F', "-F-", "Letter F" ) );
			Add( new Symbol( 'G', "-G-", "Letter G" ) );
			Add( new Symbol( 'H', "-H-", "Letter H" ) );
			Add( new Symbol( 'I', "-I-", "Letter I" ) );
			Add( new Symbol( 'J', "-J-", "Letter J" ) );
			Add( new Symbol( 'K', "-K-", "Letter K" ) );
			Add( new Symbol( 'L', "-L-", "Letter L" ) );
			Add( new Symbol( 'M', "-M-", "Letter M" ) );
			Add( new Symbol( 'N', "-N-", "Letter N" ) );
			Add( new Symbol( 'O', "-O-", "Letter O" ) );
			Add( new Symbol( 'P', "-P-", "Letter P" ) );
			Add( new Symbol( 'Q', "-Q-", "Letter Q" ) );
			Add( new Symbol( 'R', "-R-", "Letter R" ) );
			Add( new Symbol( 'S', "-S-", "Letter S" ) );
			Add( new Symbol( 'T', "-T-", "Letter T" ) );
			Add( new Symbol( 'U', "-U-", "Letter U" ) );
			Add( new Symbol( 'V', "-V-", "Letter V" ) );
			Add( new Symbol( 'W', "-W-", "Letter W" ) );
			Add( new Symbol( 'X', "-X-", "Letter X" ) );
			Add( new Symbol( 'Y', "-Y-", "Letter Y" ) );
			Add( new Symbol( 'Z', "-Z-", "Letter Z" ) );
			Add( new Symbol( '-', " - ", "Letter -" ) );
		}

		private static Symbol defaultSymbol = new SymbolMeta( '_', "UKN", "Unknown" );

		/// <summary> Gets the default symbol in the alphabet.
		/// </summary>

		public override Symbol DefaultSymbol {
			get {
				return defaultSymbol;
			}
		}

		private static Alphabet instance = new LatinAlphabet();

		/// <summary> Gets a reference to the singleton instance.
		/// </summary>
		/// <returns>A reference to the singleton instance</returns>

		public static Alphabet Instance () {
			return instance;
		}
	}
}
