using System.Collections.Generic;

namespace VuePlanning.Models
{
    public class Room
    {
        public Room()
        {
            Users = new List<User>();
        }

        public string Id { get; set; }
        public string Message { get; set; }
        public User Host { get; set; }
        public List<User> Users { get; }
    }
}
