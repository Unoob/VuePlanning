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
        Room CreateRoom(string room);
        Room GetUserRoom(string connectionId);
    }

    public class InMemoryRoomTracker : IRoomTracker
    {
        private readonly ConcurrentDictionary<string, Room> _rooms = new ConcurrentDictionary<string, Room>();

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

        public Room GetUserRoom(string connectionId)
        {
            return _rooms.Where(w => w.Value.Users.Any(x => x.ConnectionId == connectionId))
                .Select(s => s.Value).FirstOrDefault() ?? new Room();
        }
    }
}
