using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Core.Contracts.Identity;
using Core.Feature.Login.LoginUser;
using Core.Models.Identity;
using Core.Models.Response;

namespace Test.Mock
{
    public class MockAuthService
    {
        public static Mock<IAuthService> MockAutService(bool isRegister)
        {
            var mockRepo = new Mock<IAuthService>();
            if (isRegister)
            {
                mockRepo.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync((string email) =>
                {
                    return null;
                });
            }
            else
            {
                mockRepo.Setup(r => r.GetUserByEmail(It.IsAny<string>())).ReturnsAsync((string email) =>
                 {
                     return new ApplicationUser()
                     {
                         Email = "test"
                     };
                 });
            }

            mockRepo.Setup(r => r.Login(It.IsAny<LoginRequestDto>(), It.IsAny<ApplicationUser>())).Returns(() =>
            {
                var responseLogin = new Response<LoginResponseDto>()
                {
                    Success = true,
                    Data = new()
                    {
                        Email = "test@email.com",
                        Id = "f4f9cb2a-a10b-4e57-ab21-a6d658af4458",
                        SolId = "SOlT123",
                        Token = "FURACMlvJibBybkamOIeWorrJgp64WmOAB007kZh9vI8gXZ1BUDRj3BdSse9vAJb"
                    },
                    Errors = [],
                    StatusCode = System.Net.HttpStatusCode.OK
                };
                return Task.FromResult(responseLogin);
            });

            mockRepo.Setup(r => r.Register(It.IsAny<RegisterDtLoginRequest>())).Returns(() =>
            {
                var responseRegister = new Response<LoginResponseDto>()
                {
                    Success = true,
                    Data = new()
                    {
                        Email = "test@email.com",
                        Id = "f4f9cb2a-a10b-4e57-ab21-a6d658af4459",
                        SolId = "SOlT123",
                        Token = "FURACMlvJibBybkamOIeWorrJgp64WmOAB007kZh9vI8gXZ1BUDRj3BdSse9vAJC"
                    },
                    Errors = [],
                    StatusCode = System.Net.HttpStatusCode.Created
                };
                return Task.FromResult(responseRegister);
            });
            mockRepo.Setup(r => r.ValidateToken(It.IsAny<string>())).Returns(() =>
           {
               var responseValidation = new Response<bool>()
               {
                   Success = true,
                   Data = true,
                   Errors = [],
                   StatusCode = System.Net.HttpStatusCode.OK
               };
               return Task.FromResult(responseValidation);
           });
            return mockRepo;
        }
    }
}