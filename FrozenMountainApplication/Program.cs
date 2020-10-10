using System;
using System.Collections.Generic;
using System.Linq;

namespace FrozenMountainApplication
{
    class Program {
        static void Main( string[] args ) {
            string providedString = "zf3kabxcde224lkzf3mabxc51+crsdtzf3nab=";

            Dictionary<string,int> patterns = findUniquePatterns( providedString, 3 );
            PrintPatternDictionary( patterns );

            providedString = "abcabcabcabccabcabcabcabcabcabLLLLLLLLLLLLLLLLLL";
            patterns = findUniquePatterns( providedString, 4 );
            PrintPatternDictionary( patterns );

            patterns = findUniquePatterns( providedString, 5 );
            PrintPatternDictionary( patterns );

            patterns = findUniquePatterns( providedString, 2 );
            PrintPatternDictionary( patterns );

            // running this last one told me I had 17 'L' characters in my second provided string,
            // and after counting them I had 18.  Off by one errors will nail you every time.
            // adjusted my for loop where clause and added the + 1.
            patterns = findUniquePatterns( providedString, 1 );
            PrintPatternDictionary( patterns );

            providedString = "abcdefghijklmnopqrstuvwxyz1234567890";
            patterns = findUniquePatterns( providedString, 1 );
            PrintPatternDictionary( patterns );

            patterns = findUniquePatterns( providedString, 2 );
            PrintPatternDictionary( patterns );
        }

        private static Dictionary<string, int> findUniquePatterns( string input, int patternLength ) {
            var foundPatterns = new Dictionary<string, int>();

            if ( string.IsNullOrEmpty( input ) || input.Length < patternLength || patternLength <= 0 ) {
                // realized I should return an empty dict
                return foundPatterns;
            }

            // O(n) solution
            // loop through the string once and dump all the substrings into a map
            // filter out the map where count is not greater than 1
            // return dictionary result

            string pattern;
            for ( int i = 0; i < input.Length - patternLength + 1; i++ ) {
                pattern = input.Substring( i, patternLength );

                if ( !foundPatterns.ContainsKey( pattern ) ) {
                    foundPatterns.Add( pattern, 1 );
                } 
                else {
                    foundPatterns[input.Substring( i, patternLength )]++;
                }
            }

            // google searched "c# get keys from dictionary where condition"
            // handily enough, the second answer here was good
            // https://stackoverflow.com/questions/44920947/search-dictionary-values-and-return-list-of-keys-meeting-conditions

            // then I changed all of this to return a dictionary, instead of a string array after reading the email I was given more closely.  
            return foundPatterns.Where( x => x.Value > 1 ).ToDictionary( x => x.Key, x => x.Value );
        }

        private static void PrintPatternDictionary( Dictionary<string, int> patterns ) {
            if ( patterns.Count == 0 ) {
                Console.WriteLine( "Empty dictionary provided." );
                return;
            }

            foreach( var pattern in patterns ) {
                Console.WriteLine( $"Pattern: {pattern.Key}, Times found: {pattern.Value}" );
            }
        }
    }
}
