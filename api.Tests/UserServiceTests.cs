using api.Controllers;
using api.Dtos.AuthControllerDtos;
using api.Dtos.UserControllerDtos;
using api.Helpers;
using api.Models;
using api.Services;
using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace api.Tests
{
    public class UserServiceTests
    {
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserServiceTests()
        {
            _imageService = A.Fake<IImageService>();
            _mapper = A.Fake<IMapper>();
            _contextAccessor = A.Fake<HttpContextAccessor>();
            _config = A.Fake<IConfiguration>();
            _userManager = A.Fake<UserManager<ApplicationUser>>();
            _signInManager = A.Fake<SignInManager<ApplicationUser>>();
            _roleManager = A.Fake<RoleManager<ApplicationRole>>();
        }

        private UserService GetService()
        {
            return new UserService(_imageService, _mapper, _contextAccessor, 
                _config, _userManager, _signInManager, _roleManager);
        }

        [Fact]
        public async Task GetByIdAsync()
        {
            // Arrange
            var id = 1;
            var appUser = A.Dummy<ApplicationUser>();
            A.CallTo(() => _userManager.FindByIdAsync(id.ToString()))
                .Returns(Task.FromResult(appUser));
            A.CallTo(() => _mapper.Map<GetUserByIdResponse>(appUser))
                .Returns(A.Dummy<GetUserByIdResponse>());

            var service = GetService();

            // Act
            var foundUser = await service.GetByIdAsync(id);

            // Assert
            Assert.NotNull(foundUser);
        }
    }
}
