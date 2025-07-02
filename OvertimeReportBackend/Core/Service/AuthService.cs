using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Core.Contracts.Identity;
using Core.Feature.Login.LoginUser;
using Core.Models.Identity;
using Core.Models.Response;

namespace Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSetting _jwtSetting;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JwtSetting> jwtSetting)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSetting = jwtSetting.Value;
        }

        public async Task<Response<LoginResponseDto>> Login(LoginRequestDto request, ApplicationUser user)
        {
            var response = new Response<LoginResponseDto>();
            var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!checkPassword.Succeeded)
            {
                response.Success = false;
                response.Errors.Add($"Credentials for {request.Email.ToLower()} are not valid.");
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            var authResponse = new LoginResponseDto
            {
                Id = user.Id,
                SolId = user.SolId,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                Name = user.Name,
                PositionTittle = user.PositionTittle,
                Office = user.Office,
                Roles = [.. roles],
            };
            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = authResponse;

            return response;
        }

        public async Task<Response<LoginResponseDto>> Register(RegisterDtLoginRequest loginData)
        {
            var response = new Response<LoginResponseDto>();
            var user = new ApplicationUser
            {
                Email = loginData.Email.ToLower(),
                SolId = loginData.SolId.ToUpper(),
                UserName = loginData.Email.ToLower().Split('@')[0],
                Name = loginData.Name,
                PositionTittle = loginData.PositionTittle,
                EmailConfirmed = true,
                Office = loginData.Office
            };

            var result = await _userManager.CreateAsync(user, loginData.Password);

            if (result.Succeeded)
            {
                var rolesList = new List<string>();
                if (loginData.LeaderRole)
                {
                    await _userManager.AddToRoleAsync(user, "Leader");
                    rolesList.Add("Leader");
                }
                if (loginData.TeamMemberRole)
                {
                    await _userManager.AddToRoleAsync(user, "TeamMember");
                    rolesList.Add("TeamMember");
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.Created;
                JwtSecurityToken jwtSecurityToken = await GenerateToken(user);
                response.Data = new()
                {
                    Id = user.Id,
                    Email = user.Email,
                    SolId = user.SolId,
                    Name = user.Name,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Roles = rolesList,
                    PositionTittle = user.PositionTittle,
                    Office = user.Office
                };
                return response;
            }
            else
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                foreach (var err in result.Errors)
                {
                    response.Errors.Add(err.Description);
                }
                return response;
            }
        }

        public async Task<ApplicationUser?> GetUserByEmail(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email.ToLower());
            return user;
        }

        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var rolesClaims = roles.Select(x => new Claim("roles", x)).ToList();

            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Name,user.Name),
                new Claim("uid",user.Id),
                new Claim("solId",user.SolId)
            }
            .Union(userClaims)
            .Union(rolesClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSetting.Issuer,
                audience: _jwtSetting.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_jwtSetting.DurationInMinutes)),
                signingCredentials: signingCredentials
            );
            return jwtSecurityToken;
        }

        public async Task DeleteUser(ApplicationUser user)
        {
            await _userManager.DeleteAsync(user);
        }

        public async Task<Response<bool>> ValidateToken(string token)
        {
            var response = new Response<bool>();
            if (string.IsNullOrWhiteSpace(token))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Success = false;
                response.Errors.Add("Token No puede estar vacio");
                return response;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtSetting.Issuer,
                    ValidAudience = _jwtSetting.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key)),
                    ClockSkew = TimeSpan.Zero
                };
                var tokenValidatedResult = await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);
                response.StatusCode = HttpStatusCode.OK;
                response.Success = true;
                response.Data = tokenValidatedResult.IsValid;
                return response;
            }
            catch (System.Exception e)
            {
                response.StatusCode = HttpStatusCode.BadGateway;
                response.Success = false;
                response.Errors.Add(e.Message);
                return response;
            }
        }
    }
}