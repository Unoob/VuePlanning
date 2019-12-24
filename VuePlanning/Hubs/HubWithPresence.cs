using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VuePlanning.Hubs
{
    public class HubWithPresence<TInterface> : Hub<TInterface> where TInterface : class
    {
        protected IRoomTracker _rooms;

        public HubWithPresence(IRoomTracker rooms)
        {
            _rooms = rooms;
        }
    }
}
