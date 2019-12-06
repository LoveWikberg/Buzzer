using Buzzer.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;

namespace Buzzer.Hubs
{
    public class BuzzerHub : Hub
    {
        private IMemoryCache _cache;

        public BuzzerHub(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void OnConnected(string roomCode, string playerName)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, roomCode);

            var room = (RoomModel)_cache.Get(roomCode);
            if (room.Players.Count == 1)
            {
                room.Players.Single(p => p.IsGameMaster).ConnectionId = Context.ConnectionId;
                _cache.Set(roomCode, room);
            }
            else
            {
                room.Players.Single(p => p.Name == playerName).ConnectionId = Context.ConnectionId;
                _cache.Set(roomCode, room);
                string gameMasterConnId = room.Players.Single(p => p.IsGameMaster).ConnectionId;
                Clients.Client(gameMasterConnId).SendAsync("playerJoin", playerName);
            }
        }

        public void Buzz(string roomCode)
        {
            var room = (RoomModel)_cache.Get(roomCode);
            room.Players.First(p => p.ConnectionId == Context.ConnectionId).BuzzedTime = DateTime.Now;
            _cache.Set(roomCode, room);
            if (room.Players.Count(p => p.BuzzedTime != default) <= 1)
            {
                string gameMasterConnId = room.Players.Single(p => p.IsGameMaster).ConnectionId;
                Clients.Client(gameMasterConnId).SendAsync("playerBuzz");
            }
        }

        public void GetFirstBuzzer(string roomCode)
        {
            var room = (RoomModel)_cache.Get(roomCode);
            var firstBuzzer = room.Players.SkipWhile(p => p.IsGameMaster).OrderBy(p => p.BuzzedTime).First();
            Clients.Client(Context.ConnectionId).SendAsync("showFirstBuzzer", firstBuzzer.Name);
        }

        public void ResetBuzzer(string roomCode)
        {
            var room = (RoomModel)_cache.Get(roomCode);
            foreach (var player in room.Players)
            {
                player.BuzzedTime = default;
            }
            _cache.Set(roomCode, room);
            string gameMasterConnId = room.Players.Single(p => p.IsGameMaster).ConnectionId;
            Clients.Client(gameMasterConnId).SendAsync("resetBuzzers");
        }

    }
}
