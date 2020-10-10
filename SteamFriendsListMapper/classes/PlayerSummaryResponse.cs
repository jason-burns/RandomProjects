using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamFriendsListMapper.classes {
    class PlayerSummaryResponse {
        public Players response;
    }

    class Players {
        public SteamUser[] players;
    }
}
