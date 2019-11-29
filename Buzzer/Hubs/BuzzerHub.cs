using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buzzer.Hubs
{
    public class BuzzerHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            //HttpRuntime.Cache.Insert(key, data, null, DateTime.Now.AddMinutes(180), System.Web.Caching.Cache.NoSlidingExpiration);



            return base.OnConnectedAsync();
        }
    }
}
