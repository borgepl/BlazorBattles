using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBattles.Models.Dto
{
    public class UserStatisticDTO
    {
        public int Rank { get; set; }
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public int Battles { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}
