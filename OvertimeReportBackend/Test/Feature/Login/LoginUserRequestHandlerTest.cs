using System.ComponentModel.DataAnnotations;
using Core.Feature.Login.LoginUser;
using Core.Models.Response;
using Test.Mock;
using Shouldly;
using Core.Models.Identity;
using Microsoft.Extensions.Options;
using System.Net;
using Core.Contracts.Overtime;
using Core.Models.Overtime;

namespace Test.Feature.Login
{
    public class LoginUserRequestHandlerTest
    {
        private readonly IOptions<LeaderSetting> _leaderSetting;
        private readonly IUserRepository _userRepository;
        public LoginUserRequestHandlerTest()
        {
            LeaderSetting setting = new()
            {
                Email = "test@test.mail",
                SolId = "SOLF11"
            };
            _leaderSetting = Options.Create(setting);
           

        }

        [Fact]
        public async Task LoginUserHandler_LoginUser_Success()
        {
            var loginRequestData = new LoginRequestDto()
            {
                Email = "test@hotmail.com",
                Password = "test123"
            };
            var data = new List<UserT>(){
                new(){
                    PositionTittle="test",
                    Email="test@hotmail.com",
                    Id=1,
                    Office="1234",
                    Name="test name",
                    SolId="SOLA123"
                }
            };
            var mockAuth = MockAuthService.MockAutService(false);
            var mockUserRep = MockUserRepository.UserRepositoryMock(data);
            var handler = new LoginUserRequestHandler(mockAuth.Object,mockUserRep.Object, _leaderSetting);
            var result = await handler.Handle(new LoginUserRequest(loginRequestData), CancellationToken.None);
            result.ShouldBeOfType<Response<LoginResponseDto>>();
            result.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
            result.Data.Id.ShouldBe("f4f9cb2a-a10b-4e57-ab21-a6d658af4458");
        }
        [Fact]
        public async Task LoginUserHandler_RegisterUser_Success()
        {
            var loginRequestData = new LoginRequestDto()
            {
                Email = "test@hotmail.com",
                Password = "test123"
            };
            var data = new List<UserT>(){
                new(){
                    PositionTittle="test",
                    Email="test@hotmail.com",
                    Id=1,
                    Office="1234",
                    Name="test name",
                    SolId="SOLA123"
                }
            };
            var mockRepo = MockAuthService.MockAutService(true);
            var mockUserRep = MockUserRepository.UserRepositoryMock(data);
            var handler = new LoginUserRequestHandler(mockRepo.Object, mockUserRep.Object, _leaderSetting);
            var result = await handler.Handle(new LoginUserRequest(loginRequestData), CancellationToken.None);
            result.ShouldBeOfType<Response<LoginResponseDto>>();
            result.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);
            result.Data.Id.ShouldBe("f4f9cb2a-a10b-4e57-ab21-a6d658af4459");
        }

        [Fact]
        public void LoginUserHandler_LoginRequestDTo_Email_Field_Required()
        {
            var loginRequestData = new LoginRequestDto()
            {
                Email = "t",
                Password = "test123"
            };
            var validationContext = new ValidationContext(loginRequestData);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(loginRequestData, validationContext, validationResults, true);
            isValid.ShouldBe(false);
            validationResults[0].ErrorMessage.ShouldBe("Email format is not the correct");
            validationResults.Count.ShouldBe(1);
        }
        [Fact]
        public void LoginUserHandler_LoginRequestDTo_SolId_Field_Required()
        {
            var loginRequestData = new LoginRequestDto()
            {
                Email = "test@hotmail.com",
                Password = "test123"
            };
            var validationContext = new ValidationContext(loginRequestData);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(loginRequestData, validationContext, validationResults, true);
            isValid.ShouldBe(false);
            validationResults[0].ErrorMessage.ShouldBe("The SolId field is required.");
            validationResults.Count.ShouldBe(1);
        }
        [Fact]
        public void LoginUserHandler_LoginRequestDTo_Password_Field_Required()
        {
            var loginRequestData = new LoginRequestDto()
            {
                Email = "test@hotmail.com",
                Password = ""
            };
            var validationContext = new ValidationContext(loginRequestData);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(loginRequestData, validationContext, validationResults, true);
            isValid.ShouldBe(false);
            validationResults[0].ErrorMessage.ShouldBe("The Password field is required.");
            validationResults.Count.ShouldBe(1);
        }
    }
}