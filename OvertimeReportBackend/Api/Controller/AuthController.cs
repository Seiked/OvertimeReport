using MediatR;
using Microsoft.AspNetCore.Mvc;
using Core.Feature.Login.LoginUser;
using Core.Feature.Login.ValidateToken;
using Core.Models.Response;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [Produces<Response<LoginResponseDto>>]
        public async Task<ActionResult<Response<LoginResponseDto>>> Login([FromBody] LoginRequestDto request)
        {
            return await _mediator.Send(new LoginUserRequest(request));
        }
        [HttpPost("validatetoken/{token}")]
        [Produces<Response<bool>>]
        public async Task<ActionResult<Response<bool>>> ValidateToken([FromRoute] string token)
        {
            return await _mediator.Send(new ValidateTokenRequest(token));
        }
    }
}