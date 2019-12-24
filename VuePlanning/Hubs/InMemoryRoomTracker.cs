using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VuePlanning.Models;

namespace VuePlanning.Hubs
{
    public interface IRoomTracker
    {
        Room GetRoom(string id);
        string GetUserConnectionId(string name);
        void CreateRoom(string room, User user);
        void OnConnect(string connectionId, string name);
    }

    public class InMemoryRoomTracker : IRoomTracker
    {
        private readonly ConcurrentDictionary<string, Room> _rooms = new ConcurrentDictionary<string, Room>();
        private readonly ConcurrentDictionary<string, string> _users = new ConcurrentDictionary<string, string>();

        public void CreateRoom(string room, User user)
        {
            _rooms.TryAdd(room, new Room
            {
                Id = room,
                Host = user
            });
        }

        public Room GetRoom(string id)
        {
            if (_rooms.TryGetValue(id, out Room room))
            {
                return room;
            }
            return null;
        }

        public string GetUserConnectionId(string name)
        {
            if (_users.TryGetValue(name, out string connectionId))
            {
                return connectionId;
            }
            return null;
        }

        public void OnConnect(string connectionId, string name)
        {
            _users.TryAdd(name, connectionId);
        }
    }
}
