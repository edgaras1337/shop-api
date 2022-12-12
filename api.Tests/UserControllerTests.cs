using api.Controllers;
using api.Dtos.AuthControllerDtos;
using api.Dtos.UserControllerDtos;
using api.Services;
using api.UserControllerDtos;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Tests
{
    public class UserControllerTests
    {
        private readonly IUserService _userService;

        public UserControllerTests()
        {
            _userService = A.Fake<IUserService>();
        }


        //[Fact]
        //public async Task UpdateUser()
        //{
        //    // Arrange
        //    // Act
        //    // Assert
        //}

        [Fact]
        public async Task UpdateUser()
        {
            // Arrange
            var updateUserDto = A.Dummy<UpdateUserRequest>();
            A.CallTo(() => _userService.UpdateCurrentUserAsync(updateUserDto))
                .Returns(Task.FromResult(A.Dummy<UpdateUserResponse>()));
            var controller = new UserController(_userService);

            // Act
            var actionResult = await controller.UpdateUser(updateUserDto);

            // Assert
            Assert.True(actionResult.Result is OkObjectResult);
        }

        [Fact]
        public async Task GetAllUsers()
        {
            // Arrange
            A.CallTo(() => _userService.GetAllUsersAsync())
                .Returns(Task.FromResult(A.Fake<List<GetAllUsersResponse>>()));
            var controller = new UserController(_userService);

            // Act
            var actionResult = await controller.GetAllUsers();

            // Assert
            Assert.True(actionResult.Result is OkObjectResult);
        }

        [Fact]
        public async Task GetUserById()
        {
            // Arrange
            int id = 1;
            A.CallTo(() => _userService.GetByIdAsync(id))
                .Returns(Task.FromResult(A.Dummy<GetUserByIdResponse?>()));
            var controller = new UserController(_userService);

            // Act
            var actionResult = await controller.GetUserById(id);

            // Assert
            Assert.True(actionResult.Result is OkObjectResult);
        }

        [Fact]
        public async Task FindUser()
        {
            // Arrange
            var searchKey = "test";
            A.CallTo(() => _userService.FindUserAsync(searchKey))
                .Returns(Task.FromResult(A.CollectionOfFake<FindUserResponse>(5).ToList()));
            var controller = new UserController(_userService);

            // Act
            var actionResult = await controller.FindUser(searchKey);

            // Assert
            Assert.True(actionResult.Result is OkObjectResult);
        }


        [Fact]
        public async Task Deactivate()
        {
            // Arrange
            A.CallTo(() => _userService.DeactivateAccountAsync())
                .Returns(Task.FromResult(true));
            var controller = new UserController(_userService);

            // Act
            var actionResult = await controller.Delete();

            // Assert
            Assert.True(actionResult is NoContentResult);
        }


        [Fact]
        public async Task Delete()
        {
            // Arrange
            var id = "1";
            A.CallTo(() => _userService.DeleteUserByIdAsync(id))
                .Returns(Task.FromResult(true));
            var controller = new UserController(_userService);

            // Act
            var actionResult = await controller.DeleteUser(id);

            // Assert
            Assert.True(actionResult is NoContentResult);
        }
    }
}
