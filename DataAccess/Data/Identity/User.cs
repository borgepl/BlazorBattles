using DataAccess.Data.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data.Identity
{
    public class User : IdentityUser
    {
        // public string Username { get; set; } = string.Empty; // Already in IdentityUser as UserName
        public int Bananas { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public List<UserUnit> Units { get; set; } = new List<UserUnit>();
    //    public int Battles { get; set; }
    //    public int Victories { get; set; }
    //    public int Defeats { get; set; }
    }
}
