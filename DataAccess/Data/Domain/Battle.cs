using DataAccess.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data.Domain
{
    public class Battle
    {
        public int Id { get; set; }
        public User Attacker { get; set; }
        public string AttackerId { get; set; }
        public User Opponent { get; set; }
        public string OpponentId { get; set; }
        public User Winner { get; set; }
        public string WinnerId { get; set; }
        public int WinnerDamage { get; set; }
        public int RoundsFought { get; set; }
        public DateTime BattleDate { get; set; } = DateTime.Now;
    }
}
