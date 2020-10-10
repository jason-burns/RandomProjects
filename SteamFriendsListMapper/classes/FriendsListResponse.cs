
namespace SteamFriendsListMapper.classes {
    class FriendsListResponse {
        public FriendsList friendslist;
    }

    class FriendsList {
        public Friend[] friends;
    }

    class Friend {
        public string steamid;
        public string relationship;
        public int friend_since;
    }
}
