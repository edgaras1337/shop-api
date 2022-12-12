using api.Controllers;
using api.Dtos.AuthControllerDtos;
using api.Services;
using Azure;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace api.Tests
{
    public class AuthControllerTests
    {
        private readonly IUserService _userService;
        public AuthControllerTests()
        {
            _userService = A.Fake<IUserService>();
        }

        [Fact]
        public async Task Register()
        {
            // Arrange
            var registerDto = A.Dummy<RegisterRequest>();
            A.CallTo(() => _userService.CreateUserAsync(registerDto))
                .Returns(Task.FromResult(A.Dummy<RegisterResponse>()));
            var controller = new AuthController(_userService);

            // Act
            var actionResult = await controller.RegisterAsync(registerDto);

            // Assert
            Assert.True(actionResult.Result is CreatedAtActionResult);
        }

        [Fact]
        public async Task Login()
        {
            // Arrange
            var loginDto = A.Fake<LoginRequest>();
            A.CallTo(() => _userService.AuthenticateAsync(loginDto))
                .Returns(Task.FromResult(A.Dummy<LoginResponse?>()));
            var controller = new AuthController(_userService);

            // Act
            var actionResult = await controller.LoginAsync(loginDto);
            
            // Assert
            Assert.True(actionResult.Result is OkObjectResult);
        }

        [Fact]
        public async Task Logout()
        {
            // Arrange
            var controller = new AuthController(_userService);
            A.CallTo(() => _userService.LogoutAsync());

            // Act
            var actionResult = await controller.LogoutAsync();

            // Assert
            Assert.True(actionResult is NoContentResult);
        }

        [Fact]
        public async Task GetCurrentUser()
        {
            // Arrange
            var controller = new AuthController(_userService);
            A.CallTo(() => _userService.GetCurrentUserAsync())
                .Returns(Task.FromResult(A.Dummy<GetCurrentUserResponse?>()));

            // Act
            var actionResult = await controller.GetCurrentUserAsync();

            // Assert
            Assert.True(actionResult.Result is OkObjectResult);
        }
    }
}