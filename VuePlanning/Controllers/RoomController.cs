using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using VuePlanning.Hubs;
using VuePlanning.Models;

namespace VuePlanning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        IHubContext<PlanningHub, IPlanningHub> _context;
        IRoomTracker _roomTracer;

        public RoomController(IHubContext<PlanningHub, IPlanningHub> context, IRoomTracker roomTracer)
        {
            _context = context;
            _roomTracer = roomTracer;
        }

        public IActionResult CreateRoom(string name, string roomNo)
        {
            if (_roomTracer.GetRoom(roomNo) != null) return new JsonResult(false);

            _roomTracer.CreateRoom(roomNo, new User
            {
                ConnectionId = _roomTracer.GetUserConnectionId(name),
                Name = name
            });
            return Ok(true);
        }

        public IActionResult JoinRoom(string name, string roomNo)
        {
            var room = _roomTracer.GetRoom(roomNo);
            if (room == null) return new JsonResult(false);

            var connectionId = _roomTracer.GetUserConnectionId(name);
            var user = new User { ConnectionId = connectionId, Name = name };
            room.Users.Add(user);

            _context.Clients.Client(connectionId).SendMessage(room.Message);
            _context.Clients.Client(room.Host.ConnectionId).UserJoin(user);

            return Ok(true);
        }
    }
}