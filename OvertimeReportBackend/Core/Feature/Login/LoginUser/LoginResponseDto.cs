namespace Core.Feature.Login.LoginUser
{
    public class LoginResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string SolId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = [];
        public string Token { get; set; } = string.Empty;
        public string PositionTittle { get; set; } = string.Empty;
        public string Office { get; set; } = string.Empty;
    }
}