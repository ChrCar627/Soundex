using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soundex
{
	class Program
	{
		static void Main(string[] args) {
			while ( true ) {
				Console.WriteLine("Enter a name: ");
				string entered = Console.ReadLine();
				if ( String.IsNullOrEmpty(entered) )
					break;
				else {
					Console.WriteLine("Soundex: "+Soundex.GetSoundex(entered));
					Console.WriteLine("NYSIIS: " + NYSoundex.GetSoundex(entered));
					Console.WriteLine("**********");
				}
			}
		}
	}
}
