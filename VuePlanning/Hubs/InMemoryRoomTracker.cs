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
        Room CreateRoom(string room);
        void OnConnect(string connectionId, string name);
    }

    public class InMemoryRoomTracker : IRoomTracker
    {
        private readonly ConcurrentDictionary<string, Room> _rooms = new ConcurrentDictionary<string, Room>();
        private readonly ConcurrentDictionary<string, string> _users = new ConcurrentDictionary<string, string>();

        public Room CreateRoom(string roomId)
        {
            var room = new Room
            {
                Id = roomId
            };
            _rooms.TryAdd(roomId, room);
            return room;
        }

        public Room GetRoom(string id)
        {
            if (_rooms.TryGetValue(id, out Room room))
            {
                return room;
            }
            return CreateRoom(id);
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
