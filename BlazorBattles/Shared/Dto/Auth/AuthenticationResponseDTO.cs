namespace BlazorBattles.Models.Dto.Auth
{
    public class AuthenticationResponseDTO
    {
        public bool IsAuthSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }
        //public UserDTO? UserDTO { get; set; }
    }
}
