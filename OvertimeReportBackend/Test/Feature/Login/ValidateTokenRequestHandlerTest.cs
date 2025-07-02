using Core.Feature.Login.ValidateToken;
using Test.Mock;
using Shouldly;

namespace Test.Feature.Login
{
    public class ValidateTokenRequestHandlerTest
    {
        [Fact]
        public async Task ValidateToken_Success()
        {
            var mockAuthService = MockAuthService.MockAutService(false);
            var handler = new ValidateTokenRequestHandler(mockAuthService.Object);
            var result = await handler.Handle(new ValidateTokenRequest("test"), CancellationToken.None);
            result.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
            result.Data.ShouldBeTrue();
        }
    }
}