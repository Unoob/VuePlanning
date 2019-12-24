using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VuePlanning.Models;

namespace VuePlanning.Hubs
{
    public interface IPlanningHub
    {
        Task SendMessage(string message);
        Task UserJoin(User user);
        Task SetStatus(bool status);
    }
    public class PlanningHub : HubWithPresence<IPlanningHub>
    {
        public PlanningHub(IRoomTracker rooms) : base(rooms)
        {
        }

        public async Task CreateRoom(LoginModel model) {
            var roomNoExist = _rooms.GetRoom(model.Room) == null;
            if (roomNoExist) { 
                _rooms.CreateRoom(model.Room, new User { ConnectionId = Context.ConnectionId, Name = model.Name });
            }
            await Clients.Caller.SetStatus(roomNoExist);
        }
        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    await base.OnDisconnectedAsync(exception);
        //}

        //public override async Task OnConnectedAsync()
        //{
        //    await base.OnConnectedAsync();
        //}
    }
}
