using System.ComponentModel.DataAnnotations;

namespace Core.Feature.Login.LoginUser
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email format is not the correct")]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class RegisterDtLoginRequest : LoginRequestDto
    {
        public string SolId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool TeamMemberRole { get; set; } = false;
        public bool LeaderRole { get; set; } = false;
        public string PositionTittle { get; set; } = string.Empty;
        public string Office { get; set; } = string.Empty;
    }
}