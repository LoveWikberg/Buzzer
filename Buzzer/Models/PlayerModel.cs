using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buzzer.Models
{

    public class RoomModel
    {
        public int QuestionIndex { get; set; }
        public List<PlayerModel> Players { get; set; }
        public List<QuestionModel> Questions { get; set; }
    }
    public class PlayerModel
    {
        public bool IsGameMaster { get; set; }
        public string Name { get; set; }
        public string ConnectionId { get; set; }
        public bool HasBuzzed { get; set; }
        public DateTime BuzzedTime { get; set; }
    }

    public class QuestionModel
    {
        public string Text { get; set; }
        public string ImagePath { get; set; }
    }
}
