using MediatR;
using Core.Contracts.Identity;
using Core.Models.Response;

namespace Core.Feature.Login.ValidateToken
{
    public class ValidateTokenRequestHandler : IRequestHandler<ValidateTokenRequest, Response<bool>>
    {
        private readonly IAuthService _authService;

        public ValidateTokenRequestHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<Response<bool>> Handle(ValidateTokenRequest request, CancellationToken cancellationToken)
        {
            return _authService.ValidateToken(request.Token);
        }
    }
}