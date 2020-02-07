using MessagePack;
using System;
using System.Collections.Generic;

namespace VuePlanning.Models
{
    [MessagePackObject]
    public class Room
    {
        public Room()
        {
            Users = new List<User>();
            Host = new User();
        }
        [Key("Id")]
        public string Id { get; set; }
        [Key("message")]
        public string Message { get; set; }
        [Key("host")]
        public User Host { get; set; }
        [Key("users")]
        public List<User> Users { get; }
        [IgnoreMember]
        public bool HasHost => Host.ConnectionId != null;
        internal void UpdateUser(User user)
        {
            var result = Users.Find(w => w.ConnectionId == user.ConnectionId);
            result.Vote = user.Vote;
        }

        internal void UserJoin(User user)
        {
            var userExist = Users.Find(w => w.Name == user.Name);
            if (userExist == null)
            {
                Users.Add(user);
            }
            else
            {
                userExist.ConnectionId = user.ConnectionId;
            }
        }

        internal void UserLeaves(User user)
        {
            if (user.IsHost)
            {
                Host = new User();
            }
            else
            {
                Users.Remove(user);
            }
        }
    }
}
