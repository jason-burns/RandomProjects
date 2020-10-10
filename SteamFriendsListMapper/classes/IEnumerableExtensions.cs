using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamFriendsListMapper.classes {
    public static class IEnumerableExtensions {
        public static IEnumerable<IEnumerable<T>> ChunkBy<T>( this IEnumerable<T> source, int chunkSize ) {
            return source
                .Select( ( x, i ) => new { Index = i, Value = x } )
                .GroupBy( x => x.Index / chunkSize )
                .Select( x => x.Select( v => v.Value ).ToList() )
                .ToList();
        }
    }
}
