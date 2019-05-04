using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordEmojiCounter
{
    class DiscordEmojiCounter
    {
        static void Main(string[] args)
        {
            
            var fileLines = System.IO.File.ReadLines(args[0]).ToList();
            Dictionary<string, int> emojiCounter = new Dictionary<string, int>();

            Regex r = new Regex("^\\:[a-z]+\\:$");

            foreach ( string line in fileLines )
            {
                string str = line;
                while ( r.IsMatch( str ) ) {
                    Match m = r.Match( str );
                    if ( !emojiCounter.Keys.Contains( m.Value ) ) {
                        emojiCounter.Add( m.Value, 1 );
                    }
                    emojiCounter[m.Value]++;

                    str = str.Remove( 0, m.Index + m.Length );
                }
                
            }

            foreach( KeyValuePair<string,int> pair in emojiCounter.OrderBy( x => x.Value ))
            {
                Console.WriteLine(pair.Key + "  " + pair.Value);
            }

            Console.Read();
        }
    }
}
