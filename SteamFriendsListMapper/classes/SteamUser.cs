using System;
using System.Collections.Generic;

namespace SteamFriendsListMapper {
    internal class SteamUser {
        public string steamid;
        public int communityvisibilitystate;
        public int profilestate;
        public string personaname;
        
        public bool commentpermission;
        public Uri avatar;
        public Uri avatarmedium;
        public Uri avatarfull;
        
        public double lastlogoff;
        public int personastate;
        public string primaryclanid;
        public double timecreated;
        public int personastateflags;
        public string loccountrycode;

        public List<SteamUser> friends;

        public SteamUser() {
            friends = new List<SteamUser>();
        }

        public override string ToString() {
            return $"Account name: {personaname}";
        }
    }
}
