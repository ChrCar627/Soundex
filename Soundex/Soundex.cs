using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soundex
{
	class Soundex
	{

		//-----------------------------------------------------------------
		// Name: Soundex.GetSoundex()
		// Author: Chris Carucci
		// Date: 9-29-16
		// Description: Uses the Soundex algorithm to encode a string
		// Return: The Soundex of the string that was passed
		//-----------------------------------------------------------------
		public static string GetSoundex(string s) {
			const int MaxSoundexLength = 4;
			string upperstr = s.ToUpper();
			string newstring = "";
			string lastchar = "";
			char firstchar = upperstr[0];
			for ( int count = 0; count < upperstr.Length; count++ ) {
				char c = upperstr[count];
				string digit = GetSoundexDigit(c);
				//	No repeating digits
				if ( digit != lastchar ) {
					newstring += digit;
				}
				//	If digit was H or W, just remove it
				if ( !string.IsNullOrEmpty(digit) )
					lastchar = digit;
			}
			//	If first char is a digit, put it at the start of the new string
			if ( char.IsDigit(newstring[0]) )
				newstring = upperstr[0] + newstring.Substring(1);
			//	Replace all 0's (vowels)
			newstring = newstring.Replace("0", "");

			//	Trim down to 4 characters
			if ( newstring.Length > 4 )
				newstring = newstring.Substring(0, 4);
			newstring = newstring.PadRight(MaxSoundexLength, '0');
			return newstring;
		}



		//-----------------------------------------------------------------
		// Name: Soundex.GetSoundexDigit()
		// Author: Chris Carucci
		// Date: 9-29-16
		// Description: Used by the Soundex algorithm to get the numeric value of each letter
		// Return: A string representation of the numeric value of the letter
		//-----------------------------------------------------------------
		public static string GetSoundexDigit(char c) {
			if ( "HW".Contains(c) )
				return "";
			if ( "BFPV".Contains(c) )
				return "1";
			if ( "CGJKQSXZ".Contains(c) )
				return "2";
			if ( "DT".Contains(c) )
				return "3";
			if ( c == 'L' )
				return "4";
			if ( "MN".Contains(c) )
				return "5";
			if ( c == 'R' )
				return "6";
			else
				return "0";
		}
	}
}
