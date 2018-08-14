using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IdleRPGRandomnessFinder {

    public class IdleRPGRandomnessFinder {

        class Flip {
            public string userRawTimeStamp;
            public string botRawTimeStamp;

            public string user;
            public int amount;

            public bool flipRequestIsHeads;
            public bool flipResponseIsHeads;

            public bool isWin {
                get {
                    return flipRequestIsHeads == flipResponseIsHeads;
                }
            }

            public override string ToString() {
                StringBuilder s = new StringBuilder();
                //s.Append( base.ToString() );

                s.AppendLine( userRawTimeStamp + " " + user + " " + ( flipRequestIsHeads ? "heads" : "tails" ) );
                s.AppendLine( botRawTimeStamp + " " + ( flipResponseIsHeads ? "heads" : "tails" ) );
                s.AppendLine( amount.ToString() + " " + ( isWin ? "won" : "lost" ) );

                return s.ToString();
            }
        }

        private bool isFlipLine( string line ) {

            return false;
        }

        private static bool isUserMessageHeader( string line ) {
            return new Regex( "^\\[[0-9]{2}-[A-Za-z]{3}-[0-9]{2} [0-9]{2}:[0-9]{2} (A|P)M\\] [^#]+#[0-9]{4}" ).Match( line ).Success;
        }

        private static bool isRPGBotMessageHeader( string line ) {
            return new Regex( "^\\[[0-9]{2}-[A-Za-z]{3}-[0-9]{2} [0-9]{2}:[0-9]{2} (A|P)M\\] IdleRPG#8939" ).Match( line ).Success;
        }

        private static bool isFlipRequestMessage( string line ) {
            return new Regex( "((\\$flip) (heads|tails) \\d+)" ).Match( line ).Success;
        }

        private static bool isFlipResultMessage( string line ) {
            return new Regex( "^(\\:(heads|tails)\\: It's \\*\\*(heads|tails)\\*\\*! You (won|lost) \\*\\*\\$\\d+\\*\\*!)" ).Match( line ).Success;
        }

        private static bool isWin( string line ) {
            return line.Contains( "won" );
        }

        private static Flip createFlipFromLines( Queue<string> lines ) {
            Flip f = new Flip();

            string tmp = lines.Dequeue();
            
            // parse the first element, the username and timestamp
            f.userRawTimeStamp = tmp.Substring( 0, 20 );
            f.user = tmp.Substring( 21 );

            // parse the flip request
            tmp = lines.Dequeue();
            f.flipRequestIsHeads = tmp.Contains( "heads" );
            f.amount = int.Parse( tmp.Substring( 12 ) );

            // parse the bot answer timestamp
            tmp = lines.Dequeue();
            f.botRawTimeStamp = tmp.Substring( 0, 20 );

            // parse the flip response
            tmp = lines.Dequeue();
            f.flipResponseIsHeads = tmp.Contains( "heads" );

            return f;
        }


        // sample flip message
        //[13-Aug-18 02:57 PM] maofx#7392
        //$flip tails 200

        // [13-Aug-18 03:01 PM] IdleRPG#8939
        // :tails: It's **tails**! You lost **$2000**!
        static void Main( string[] args ) {

            // C:/Users/Jason/Desktop/RD2L - idlerpg.txt
            var fileLines = System.IO.File.ReadLines( args[0] ).ToList();
            var lineQueue = new Queue<string>();
            var flipResults = new List<Flip>();


            for ( int i = 0; i < fileLines.Count; i++ ) {

                // find a user message
                if ( isUserMessageHeader( fileLines[i] ) ) {

                    // make sure that message only contains one line, and it's a flip request
                    if ( isFlipRequestMessage( fileLines[i + 1] ) ) {

                        // ensure the user didn't put multiple lines
                        if ( fileLines[i + 2] == string.Empty ) {


                            for ( int j = i; j < Math.Min( i + 5, fileLines.Count ); j++ ) {

                                // look ahead for the idlebot header and flip response
                                // we can possibly get a flip response or a cooldown message.  maybe an error message or blank too
                                if ( isRPGBotMessageHeader( fileLines[j] ) ) {

                                    if ( isFlipResultMessage( fileLines[j + 1] ) ) {

                                        // i-1 = user/timestamp
                                        lineQueue.Enqueue( fileLines[i] );
                                        // i   = flip request
                                        lineQueue.Enqueue( fileLines[i + 1] );
                                        // j   = rpgbot response
                                        lineQueue.Enqueue( fileLines[j] );
                                        // j+1 = flip response
                                        lineQueue.Enqueue( fileLines[j + 1] );


                                        flipResults.Add( createFlipFromLines( lineQueue ) );

                                        lineQueue.Clear();
                                    }
                                }
                            }
                        }
                    }
                }
            }


            foreach( var x in flipResults ) {

                Console.WriteLine( x.ToString() );
            }

            Console.Read();
        }
    }
}
