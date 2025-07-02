using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Core.Models.Response;

namespace Core.Feature.Login.LoginUser
{
    public class LoginUserRequest : IRequest<Response<LoginResponseDto>>
    {
        public LoginRequestDto LoginData { get; set; }
        public LoginUserRequest(LoginRequestDto loginData)
        {
            LoginData = loginData;
        }
    }
}