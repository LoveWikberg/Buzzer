using Buzzer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buzzer.Controllers
{
    public class BuzzerController : Controller
    {
        private IMemoryCache _cache;

        public BuzzerController(IMemoryCache cache)
        {
            _cache = cache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult JoinRoom(string playerName, string roomCode)
        {
            var roomCache = _cache.Get(roomCode);
            if (roomCache == null)
            {
                ModelState.AddModelError(nameof(roomCode), "Det finns inget rum med den koden");
                return View(nameof(Index));
            }
            else
            {
                var room = (RoomModel)roomCache;
                room.Players.Add(new PlayerModel { IsGameMaster = false, Name = playerName });
                _cache.Set(roomCode, room);
                return RedirectToAction(nameof(Room), new { roomCode = roomCode, playerName = playerName });
            }
        }

        [HttpPost]
        public IActionResult CreateRoom()
        {
            var roomCode = GenerateRoomCode(4);

            RoomModel room = new RoomModel
            {
                Players = new List<PlayerModel>
            {
                new PlayerModel {IsGameMaster = true, Name = ""}
            }
            };

            _cache.Set(roomCode, room, new TimeSpan(2, 0, 0));
            return RedirectToAction(nameof(Room), new { roomCode = roomCode, playerName = "" });
        }

        public IActionResult Room(string roomCode)
        {
            return View();
        }

        private string GenerateRoomCode(int length)
        {
            string characters = "ABCDEFGHIJKLMONPQRSTUVXYZ123456789";
            StringBuilder password = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                int characterIndex = random.Next(0, characters.Length - 1);
                password.Append(characters[characterIndex]);
            }
            return password.ToString();
        }

    }
}
