using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBattles.Models.Dto
{
    public class BattleHistoryDTO
    {
        public int BattleId { get; set; }
        public DateTime BattleDate { get; set; }
        public string AttackerId { get; set; }
        public string OpponentId { get; set; }
        public bool YouWon { get; set; }
        public string AttackerName { get; set; }
        public string OpponentName { get; set; }
        public int RoundsFought { get; set; }
        public int WinnerDamageDealt { get; set; }
    }
}
