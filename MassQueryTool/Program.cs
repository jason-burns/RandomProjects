using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MassQueryTool {
    class Program {
        private static HttpClient client;

        // create a program that takes a query string, and sends a configurable amount of requests to an undisclosed website.
        // then, write all the responses back to a file.
        static void Main( string[] args ) {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add( new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue( "application/json" ) );

            int numberQueries = 20;
            Task<HttpResponseMessage>[] responses = new Task<HttpResponseMessage>[numberQueries];

            string url = Keys.Keys.MassQueryUrl;
            string prompt = "";
            string content = "";

            Request r = new Request( prompt, content );

            string jsonBody = JsonConvert.SerializeObject(r);
            StringContent strContent = new StringContent( jsonBody, Encoding.UTF8, "application/json" );

            for ( int i = 0; i < numberQueries; i++ ){
                responses[i] = client.PostAsync( url, strContent );
            }

            Thread.Sleep( 6000 );

            string resultDirectory = Keys.Keys.MassQueryResultDirectory;

            StringBuilder builder = new StringBuilder();
            builder.Append( DateTime.Now.Year );
            builder.Append( "-" );
            builder.Append( DateTime.Now.Month );
            builder.Append( "-" );
            builder.Append( DateTime.Now.Day );
            builder.Append( "-" );
            builder.Append( DateTime.Now.Hour );
            builder.Append( "-" );
            builder.Append( DateTime.Now.Minute );
            builder.Append( "-" );
            builder.Append( DateTime.Now.Second );

            string currentTime = builder.ToString();

            using ( StreamWriter writer = new StreamWriter( resultDirectory + currentTime + ".txt" ) ) {
                writer.WriteLine( "Prompt: " + prompt );
                writer.WriteLine( "Content: " + content );

                Response response;
                for( int i = 0; i < numberQueries; i++ ) {
                    response = JsonConvert.DeserializeObject<Response>( responses[i].Result.Content.ReadAsStringAsync().Result );
                    writer.WriteLine( response.text );
                }
            }
        }

        private class Request {
            public string prompt;
            public string content;

            public Request ( string p, string c ) {
                prompt = p;
                content = c;
            }
        }

        private class Response {
            public string text;
        }

    }
}
