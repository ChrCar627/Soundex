using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soundex
{
	class NYSoundex
	{
		//-----------------------------------------------------------------
		// Name: NYSoundex.GetSoundex()
		// Author: Chris Carucci
		// Date: 9-29-16
		// Description: Uses the NYSIIS Soundex algorithm to encode a string. 
		// Return: The Soundex of the string that was passed
		//-----------------------------------------------------------------
		public static string GetSoundex(string s) {
			StringBuilder newstring = new StringBuilder(s.ToUpper());
			ReplaceStartCharacters(newstring);
			ReplaceEndCharacters(newstring);
			char firstchar = newstring[0]; // Save first character
			char lastchar = ' ';
			newstring.Remove(0, 1); // Remove first character
			for ( int count = 0; count < newstring.Length; count++ ) {
				char c = newstring[count];
				if ( char.IsWhiteSpace(c) ) {
					newstring.Remove(count, 1);
					count--;
					continue;
				}
				//	Make sure that the string has at least 2 characters left. Otherwise, just set those characters to ' '
				char c2 = count + 1 < newstring.Length?newstring[count + 1]:' ';
				char c3 = count + 2 < newstring.Length ? newstring[count + 2]:' ';
				//	EV -> AF
				if ( c.Equals('E') && c2.Equals('V') ) {
					newstring[count] = 'A';
					newstring[count + 1] = 'F';
				}
				//	A, E, I, O, U -> A
				else if ( IsVowel(c) )
					newstring[count] = 'A';
				//	Q -> G
				else if ( c.Equals('Q') )
					newstring[count] = 'G';
				//	Z -> M
				else if ( c.Equals('Z') )
					newstring[count] = 'S';
				//	M -> N
				else if ( c.Equals('M') )
					newstring[count] = 'N';
				else if ( c.Equals('K') ) {   
					//	KN -> N
					if ( c2.Equals('N') ) {
						newstring.Remove(count, 1);
						count--;
					}
					//	K -> C
					else
						newstring[count] = 'C';
				}
				//	SCH -> SSS
				else if ( c.Equals('S') && c2.Equals('C') && c3.Equals('H') ) {
					newstring[count + 1] = 'S';
					newstring[count + 2] = 'S';
				}
				//	PH -> FF
				else if ( c.Equals('P') && c2.Equals('H') ) {
					newstring[count] = 'F';
					newstring[count + 1] = 'F';
				}
				//	If non-vowel before or after H -> Lastchar
				else if ( c.Equals('H') ) {
					if ( !IsVowel(lastchar) || !IsVowelOrWhiteSpace(c2) )
						if ( lastchar.Equals(' ') ) {
							newstring.Remove(count, 1);
							count--;
							continue;
						}
						else
							newstring[count] = lastchar;
				}
				//	If vowel before W -> A
				else if ( c.Equals('W') && IsVowel(lastchar) )
					newstring[count] = 'A';
				//	Don't repeat characters
				else if ( c.Equals(lastchar) ) {
					newstring.Remove(count, 1);
					count--;
				}
				lastchar = newstring[count];
			}
			//	Remove S, AY, and A from end of string
			if ( newstring.Length > 1 ) {
				if ( newstring[newstring.Length - 1].Equals('S') )
					newstring.Remove(newstring.Length - 1, 1);
				if ( newstring[newstring.Length - 1].Equals('Y') && (newstring.Length >= 2 ? newstring[newstring.Length - 2] == 'A' : false) )
					newstring.Remove(newstring.Length - 2, 1);
				if ( newstring[newstring.Length - 1].Equals('A') )
					newstring.Remove(newstring.Length - 1, 1);
			}
			//	Remove any adjacent duplicate letters
			for ( int count = 0; count < newstring.Length; count++ ) {
				char c = newstring[count];
				char nextchar = count + 1 < newstring.Length ? newstring[count + 1] : ' ';
				if ( c.Equals(nextchar) ) {
					newstring.Remove(count, 1);
					count--;
				}
			}
			//	Return first letter to the key
			newstring.Insert(0, firstchar);

			//	Trim string to 6 characters
			char[] chararray = new char[6];
			if ( newstring.Length >= 6 )
				newstring.CopyTo(0, chararray, 0, 6);
			else
				newstring.CopyTo(0, chararray, 0, newstring.Length);
			return new string(chararray);
		}

		//-----------------------------------------------------------------
		// Name: NYSoundex.ReplaceEndCharacters()
		// Author: Chris Carucci
		// Date: 9-29-16
		// Description: Used by the Soundex algorithm to replace the letters at the start of a string.
		//-----------------------------------------------------------------
		public static void ReplaceEndCharacters(StringBuilder newstring) {
			if ( newstring.Length < 2 ) return;
			char char1 = newstring[newstring.Length - 2];
			char char2 = newstring[newstring.Length - 1];
			//	Check from last character back. So in "Marie" this checks the last 'e' first.
			switch ( char2 ) {     
				case 'E':   // IE/EE -> Y
					if ( char1.Equals('I') || char1.Equals('E')) { 
						newstring[newstring.Length - 2] = 'Y';
						newstring.Remove(newstring.Length - 1, 1);
					}
					break;
				case 'T':   // DT/RT/NT -> D
					if (char1.Equals('D') || char1.Equals('R') || char1.Equals('N') ) { 
						newstring[newstring.Length - 2] = 'D';
						newstring.Remove(newstring.Length - 1, 1);
					}
					break;
				case 'D':   // RD/ND -> D
					if ( char1.Equals('R') || char1.Equals('N') ) { 
						newstring[newstring.Length - 2] = 'D';
						newstring.Remove(newstring.Length - 1, 1);
					}
					break;
			}
		}

		//-----------------------------------------------------------------
		// Name: NYSoundex.ReplaceStartCharacters()
		// Author: Chris Carucci
		// Date: 9-29-16
		// Description: Used by the Soundex algorithm to replace the letters at the end of a string.
		//-----------------------------------------------------------------
		public static void ReplaceStartCharacters(StringBuilder newstring) {
			char char1 = newstring[0];
			char char2 = newstring.Length >= 2 ? newstring[1] : ' ';
			char char3 = newstring.Length >= 3 ? newstring[2]: ' ';
			//	Checks the first character of the string
			switch ( char1 ) { 
				case 'M':   // MAC -> MCC
					if ( char2.Equals('A') && char3.Equals('C') ) 
						newstring[1] = 'C';
					break;
				case 'K':   
					if ( char2.Equals('N') )	// KN -> N
						newstring.Remove(0, 1);
					else
						newstring[0] = 'C';	// K -> C
					break;
				case 'P':
					switch ( char2 ) {
						case 'H': // PH -> FF
							newstring[0] = 'F';
							newstring[1] = 'F';
							break;
						case 'F': // PF -> FF
							newstring[0] = 'F';
							break;
					}
					break;
				case 'S':	//SCH -> SSS
					if ( char2.Equals('C') && char3.Equals('H') ) {	
						newstring[1] = 'S';
						newstring[2] = 'S';
					}
					break;
			}
		}

		//-----------------------------------------------------------------
		// Name: NYSoundex.IsVowel()
		// Author: Chris Carucci
		// Date: 9-29-16
		// Description: Checks whether a character is a vowel
		// Return: True or false
		//-----------------------------------------------------------------
		public static bool IsVowel(char c) {
			if ( c.Equals('A') || c.Equals('E') || c.Equals('I') || c.Equals('O') || c.Equals('U') )
				return true;
			else
				return false;
		}


		//-----------------------------------------------------------------
		// Name: NYSoundex.IsVowelOrWhiteSpace()
		// Author: Chris Carucci
		// Date: 9-29-16
		// Description: Checks whether a character is a vowel or whitespace
		// Return: True or false
		//-----------------------------------------------------------------
		public static bool IsVowelOrWhiteSpace(char c) {
			if ( IsVowel(c) || char.IsWhiteSpace(c) )
				return true;
			else
				return false;
		}
	}
}
