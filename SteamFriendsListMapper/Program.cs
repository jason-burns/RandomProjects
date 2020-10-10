using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SteamFriendsListMapper.classes;

namespace SteamFriendsListMapper {

    class Program {

        private static WebClient client;
        private static string GetPlayerSummariesQueryBase = "http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=";
        private static string GetPlayerSummariesQueryExtension = "&steamids=";

        private static string GetPlayerFriendsQueryBase = "http://api.steampowered.com/ISteamUser/GetFriendList/v0001/?key=";
        private static string GetPlayerFriendsQueryExtension = "&steamid=";
        private static string GetPlayerFriendsQueryExtension2 = "&relationship=friend";

        static string GetPlayerSummariesRaw( IEnumerable<string> steamIds ) {
            return Encoding.UTF8.GetString( client.DownloadData( $"{GetPlayerSummariesQueryBase}{Keys.SteamAPIKey}{GetPlayerSummariesQueryExtension}{string.Join( ",", steamIds )}" ) );
        }

        static string GetPlayerSummaryRaw( string steamId ) {
            return Encoding.UTF8.GetString( client.DownloadData( $"{GetPlayerSummariesQueryBase}{Keys.SteamAPIKey}{GetPlayerSummariesQueryExtension}{ steamId }" ) );
        }

        static string GetPlayerFriendsRaw( string steamid ) {
            return Encoding.UTF8.GetString( client.DownloadData( $"{GetPlayerFriendsQueryBase}{Keys.SteamAPIKey}{GetPlayerFriendsQueryExtension}{steamid}{GetPlayerFriendsQueryExtension2}" ) );
        }

        static PlayerSummaryResponse GetPlayerSummary( string steamId ) {
            return JsonConvert.DeserializeObject<PlayerSummaryResponse>( GetPlayerSummaryRaw( steamId ) );
        }

        static PlayerSummaryResponse GetPlayerSummaries( IEnumerable<string> steamIds ) {
            string result = GetPlayerSummariesRaw( steamIds );

            return JsonConvert.DeserializeObject<PlayerSummaryResponse>( result );
        }

        static FriendsListResponse GetPlayerFriends( string steamId ) {
            string result = GetPlayerFriendsRaw( steamId );

            return JsonConvert.DeserializeObject<FriendsListResponse>( result );
        }

        static void Main( string[] args ) {
            client = new WebClient();

            int levelsDeep = 1;
            SteamUser rootUser = startFriendsTree( 1, Keys.SelfSteamID64 );



            Console.Read();
        }

        static SteamUser startFriendsTree( int maxLevels, string rootSteamKey ) {
            SteamUser rootUser = GetPlayerSummary( rootSteamKey ).response.players.First();

            FriendsListResponse rootFriends = GetPlayerFriends( rootSteamKey );

            List<PlayerSummaryResponse> responses = new List<PlayerSummaryResponse>();
            foreach ( List<string> steamIdChunk in rootFriends.friendslist.friends.Select( x => x.steamid ).ChunkBy( 100 ) ) {

                responses.Add( GetPlayerSummaries( steamIdChunk ) );

                Thread.Sleep( 3000 );
            }

            foreach ( PlayerSummaryResponse response in responses ) {
                foreach ( SteamUser foundUser in response.response.players ) {
                    rootUser.friends.Add( foundUser );

                    walkFriendsTree( 0, maxLevels, foundUser );
                }
            }

            return rootUser;
        }

        static void walkFriendsTree( int levelsDeep, int maxLevels, SteamUser user ) {
            if ( levelsDeep < maxLevels ) {
                FriendsListResponse rootFriends = GetPlayerFriends( user.steamid );

                // chunk the friends in groups of 100
                // put this call in a loop with a second delay between each
                // dump the data out to a format useable by whatever mapping utility
                List<PlayerSummaryResponse> responses = new List<PlayerSummaryResponse>();
                foreach ( List<string> steamIdChunk in rootFriends.friendslist.friends.Select( x => x.steamid ).ChunkBy( 100 ) ) {

                    responses.Add( GetPlayerSummaries( steamIdChunk ) );

                    Thread.Sleep( 5000 );
                }

                foreach ( PlayerSummaryResponse response in responses ) {
                    foreach ( SteamUser foundUser in response.response.players ) {
                        user.friends.Add( foundUser );

                        walkFriendsTree( levelsDeep++, maxLevels, foundUser );
                    }
                }
            }
        }
    }
}
