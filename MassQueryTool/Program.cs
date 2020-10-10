using System.Net;

namespace MassQueryTool {
    class Program {
        private static WebClient client;

        // create a program that takes a query string, and sends a configurable amount of requests to .  then, write all the responses back to a file.
        static void Main( string[] args ) {
            client = new WebClient();

            int numberQueries = 10;


        }
    }
}
