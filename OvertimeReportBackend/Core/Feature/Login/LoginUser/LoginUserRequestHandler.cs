using System.Net;
using MediatR;
using Core.Contracts.Identity;
using Core.Models.Response;
using Core.Models.Identity;
using Microsoft.Extensions.Options;
using Core.Contracts.Overtime;

namespace Core.Feature.Login.LoginUser
{
    public class LoginUserRequestHandler : IRequestHandler<LoginUserRequest, Response<LoginResponseDto>>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly LeaderSetting _leaderSetting;
        public LoginUserRequestHandler(IAuthService authService, IUserRepository userRepository, IOptions<LeaderSetting> leaderSetting)
        {
            _authService = authService;
            _userRepository = userRepository;
            _leaderSetting = leaderSetting.Value;
        }

        public async Task<Response<LoginResponseDto>> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            Response<LoginResponseDto> response = new();
            var usersOvertime = await _userRepository.GetAllUser();

            var userCheck = usersOvertime.FirstOrDefault(x => x.Email.ToLower() == request.LoginData.Email.ToLower());
            if (userCheck == null)
            {
                response.Success = false;
                response.Errors.Add($"Access denied.");
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
            if (userCheck.SolId != request.LoginData.Password || !userCheck.Active)
            {
                response.Success = false;
                response.Errors.Add($"Username and/or password incorrect.");
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            var user = await _authService.GetUserByEmail(request.LoginData.Email);

            if (user == null)
            {
                var registerData = new RegisterDtLoginRequest()
                {
                    Email = request.LoginData.Email,
                    Password = request.LoginData.Password,
                    SolId = userCheck.SolId,
                    Name = userCheck.Name,
                    PositionTittle = userCheck.PositionTittle,
                    Office = userCheck.Office,
                    TeamMemberRole = true
                };
                if (_leaderSetting.Email == request.LoginData.Email)
                {
                    registerData.LeaderRole = true;
                }
                response = await _authService.Register(registerData);

                return response;
            }
            else
            {
                response = await _authService.Login(request.LoginData, user);

                return response;
            }
        }
    }
}