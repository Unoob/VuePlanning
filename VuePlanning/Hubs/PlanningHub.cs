using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VuePlanning.Models;

namespace VuePlanning.Hubs
{
    public interface IPlanningHub
    {
        Task RoomCreated(Room room);
        Task UserJoin(User user);
        Task UserVote(User user);
        Task SetStatus(bool status);
        Task UserUpdate(User user);
        Task SendMessage(string msg);
        Task UserLeaves(User user);
    }
    public class PlanningHub : HubWithPresence<IPlanningHub>
    {
        public PlanningHub(IRoomTracker rooms) : base(rooms)
        {
        }

        public async Task JoinRoom(User user)
        {
            if (user == null) return;

            user.ConnectionId = Context.ConnectionId;
            var room = _rooms.GetRoom(user.RoomId);
            
            await Clients.Caller.UserUpdate(user);
            await Groups.AddToGroupAsync(user.ConnectionId, room.Id);
            room.UserJoin(user);
            if (room.HasHost)
            {
                await Clients.Client(room.Host.ConnectionId).UserJoin(user);
            }
            await Clients.Caller.SendMessage(room.Message);
        }
        public async Task CreateRoom(User host)
        {
            if (host == null) return;
            host.ConnectionId = Context.ConnectionId;
            var room = _rooms.GetRoom(host.RoomId);
            room.Host = host;

            await Groups.AddToGroupAsync(host.ConnectionId, room.Id);
            await Clients.Caller.UserUpdate(host);
            await Clients.Caller.RoomCreated(room);
        }

        public async Task CardSelect(User user)
        {
            if (user == null) return;
            var room = _rooms.GetRoom(user.RoomId);
            room.UpdateUser(user);
            if (room.HasHost)
            {
                await Clients.Client(room.Host.ConnectionId).UserVote(user);
            }
        }

        public async Task SendMessage(string roomId, string msg)
        {
            var room = _rooms.GetRoom(roomId);
            room.Message = msg;
            await Clients.OthersInGroup(roomId).SendMessage(msg);
        }

        public async Task Disconnect(User user)
        {
            if (user == null) return;
            var room = _rooms.GetRoom(user.RoomId);
            room.UserLeaves(user);
            await Groups.RemoveFromGroupAsync(user.ConnectionId, user.RoomId);
            if (room.HasHost)
            {
                await Clients.Client(room.Host.ConnectionId).UserLeaves(user);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
    }
}
