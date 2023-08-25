using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBattles.Models.Dto.Auth
{
    public class RegistrationResponseDTO
    {
        public string? Data { get; set; }
        public bool IsRegisterationSuccessful { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
