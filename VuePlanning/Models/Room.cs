using System;
using System.Collections.Generic;

namespace VuePlanning.Models
{
    public class Room
    {
        public Room()
        {
            Users = new List<User>();
            Host = new User();
        }

        public string Id { get; set; }
        public string Message { get; set; }
        public User Host { get; set; }
        public List<User> Users { get; }

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
