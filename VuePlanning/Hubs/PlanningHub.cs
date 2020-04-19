using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VuePlanning.Hubs;
using VuePlanning.Models;
using WebPush;

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
        Task UserDisconnected(string connectionId);
        Task ChangeRoomUserState(User user);
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
            if (room.HasHost && room.Host.Name != host.Name) // pokoj juz istnieje z hostem i nie zastępujemy tylko dołączamy użytkownika
            {
                host.IsHost = false;
                await JoinRoom(host);
            }
            else // nie istnieje pokój - można stworzyć
            {
                room.Host = host;

                await Groups.AddToGroupAsync(host.ConnectionId, room.Id);
                await Clients.Caller.UserUpdate(host);
                await Clients.Caller.RoomCreated(room);
            }
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

            room.Users.Where(w => w.Subscription != null).ToList().ForEach(user =>
              {
                  try
                  {
                      var device = user.Subscription;
                      var pushSubscription = new PushSubscription(device.Endpoint, device.Keys.P256dh, device.Keys.Auth);
                      var vapidDetails = new VapidDetails("http://fakvat.pl/", "BPbHlo7Z4-5Z_rr-ZKte830jOf6R4tnAS8J0IonjF269gRCdu8cdj9fsRKD9RkxprzgNO-UTFarud7QP94cHJEM", "GSMD4HwHOnybEmYeEQjylLPE3kpcA3WTKpJVd9bOOzA");

                      var webPushClient = new WebPushClient();
                      webPushClient.SendNotification(pushSubscription, msg, vapidDetails);
                  }
                  catch
                  {
                      user.Subscription = null;
                      room.UpdateUser(user);
                  }
              });
        }

        public async Task ChangeUserState(User user)
        {
            if (user == null) return;
            var room = _rooms.GetRoom(user.RoomId);
            room.UpdateUser(user);
            if (room.HasHost)
            {
                await Clients.Client(room.Host.ConnectionId).ChangeRoomUserState(user);
            }
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

        public async Task SaveUserSubscription(User user)
        {
            if (user == null) return;
            var room = _rooms.GetRoom(user.RoomId);
            room.UpdateUser(user);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var room = _rooms.GetUserRoom(Context.ConnectionId);
            if (room.HasHost)
            {
                await Clients.Client(room.Host.ConnectionId).UserDisconnected(Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
    }
}