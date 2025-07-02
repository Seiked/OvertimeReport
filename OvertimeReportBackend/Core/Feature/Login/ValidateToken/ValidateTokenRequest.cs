using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Core.Models.Response;

namespace Core.Feature.Login.ValidateToken
{
    public class ValidateTokenRequest : IRequest<Response<bool>>
    {
        public string Token { get; set; }

        public ValidateTokenRequest(string token)
        {
            Token = token;
        }
    }
}