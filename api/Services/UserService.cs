using api.Controllers;
using api.CustomExceptions;
using api.Data;
using api.Dtos;
using api.Dtos.AuthControllerDtos;
using api.Dtos.UserControllerDtos;
using api.Helpers;
using api.Models;
using api.UserControllerDtos;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;

namespace api.Services
{
    public class UserService : IUserService
    {
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _config;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserService(
            IImageService imageService,
            IMapper mapper,
            IHttpContextAccessor contextAccessor,
            IConfiguration config,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _imageService = imageService;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _config = config;

            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<GetUserByIdResponse?> GetByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
            {
                return null;
            }

            user = await user.WithImagesAsync(_imageService);

            var dto = _mapper.Map<GetUserByIdResponse>(user);

            return dto;
        }

        public async Task<GetCurrentUserResponse?> GetCurrentUserAsync()
        {
            // get current user
            var user = await GetCurrentUserModelAsync();

            if (user is null)
            {
                return null;
            }

            user = await user.WithImagesAsync(_imageService);

            var dto = _mapper.Map<GetCurrentUserResponse>(user);

            return dto;
        }

        public async Task<List<GetAllUsersResponse>> GetAllUsersAsync()
        {
            var dtoList = new List<GetAllUsersResponse>();

            var users = await _userManager.Users
                .ToListAsync();

            users.ForEach(async user =>
            {
                user = await user.WithImagesAsync(_imageService);

                var dto = _mapper.Map<GetAllUsersResponse>(user);

                dtoList.Add(dto);
            });

            return dtoList;
        }

        public async Task<List<FindUserResponse>> FindUserAsync(string searchKey)
        {
            searchKey = searchKey.Trim();

            var users = await _userManager.Users
                .Where(e => e.Name.Contains(searchKey) ||
                    e.Surname.Contains(searchKey) ||
                    e.Email.Contains(searchKey))
                .ToListAsync();

            var resDto = new List<FindUserResponse>();

            users.ForEach(async user =>
            {
                user = await user.WithImagesAsync(_imageService);

                var resDtoItem = _mapper.Map<FindUserResponse>(user);

                resDto.Add(resDtoItem);
            });

            return resDto;
        }

        public async Task<LoginResponse?> AuthenticateAsync(LoginRequest dto)
        {
            var result = await _signInManager
                .PasswordSignInAsync(dto.Email, dto.Password, true, false);

            if (!result.Succeeded)
            {
                return null;
            }

            var user = await _userManager.Users
                .Include(e => e.WishlistItems)
                .Include(e => e.Cart)
                .SingleOrDefaultAsync(e => e.Email == dto.Email);

            if (user is null)
            {
                return null;
            }

            user = await user.WithImagesAsync(_imageService);

            var resDto = _mapper.Map<LoginResponse>(user);
            
            return resDto;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterResponse> CreateUserAsync(RegisterRequest dto)
        {
            if (await _userManager.Users.AnyAsync(e => e.Email == dto.Email))
            {
                throw new DuplicateDataException();
            }

            var user = _mapper.Map<ApplicationUser>(dto);

            if (dto.Password != dto.PasswordRepeat)
            {
                throw new InvalidPasswordException();
            }

            var password = new PasswordHasher<ApplicationUser>();
            var hashed = password.HashPassword(user, dto.Password);
            user.PasswordHash = hashed;

            user.ImageName = _config["ImagesConfiguration:DefaultUserImageName"];

            user.UserName = user.Email;

            var createResult = await _userManager.CreateAsync(user);

            if (!createResult.Succeeded)
            {
                throw new UserRegistrationException();
            }

            user.Cart = new Cart(user.Id);

            await _userManager.AddToRoleAsync(user, "Customer");

            user = await user.WithImagesAsync(_imageService);
            
            if (dto.RoleId != null)
            {
                var role = await _roleManager.FindByIdAsync(dto.RoleId.ToString());
                await _userManager.AddToRoleAsync(user, role.Name);
            }

            var resDto = _mapper.Map<RegisterResponse>(user);

            return resDto;
        }

        public async Task<UpdateUserResponse> UpdateCurrentUserAsync(UpdateUserRequest dto)
        {
            var user = await GetCurrentUserModelAsync();

            if (user == null)// || user.Id != dto.Id)
            {
                throw new UnauthorizedException();
            }

            if (dto.Email != null)
            {
                var tmp = await _userManager.FindByEmailAsync(dto.Email);
                if (await _userManager.Users.AnyAsync(e => e.Email == dto.Email))
                {
                    throw new DuplicateNameException();
                }
            }

            _mapper.Map(dto, user);
            user.UserName = user.Email;

            var defaultImageName = _config["ImagesConfiguration:DefaultUserImageName"];

            if (dto.DeleteImage)
            {
                if (user.ImageName != defaultImageName)
                {
                    await _imageService.DeleteImageFileAsync(user.ImageName);

                    user.ImageName = defaultImageName;
                }
            }

            if (dto.ImageFile != null)
            {
                if (user.ImageName != defaultImageName)
                {
                    await _imageService.DeleteImageFileAsync(user.ImageName);
                }

                user.ImageName = await _imageService.SaveImageAsync(dto.ImageFile) ??
                    defaultImageName;
            }

            var oldPw = dto.OldPassword?.Trim();
            var newPw = dto.NewPassword?.Trim();
            var repeatNewPw = dto.RepeatNewPassword?.Trim();

            if (oldPw != null || newPw != null || repeatNewPw != null)
            {
                if (newPw != repeatNewPw)
                {
                    throw new InvalidPasswordException();
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPw, newPw);

                if (!changePasswordResult.Succeeded)
                {
                    throw new InvalidPasswordException();
                }
            }

            await _userManager.UpdateAsync(user);

            user = await user.WithImagesAsync(_imageService);

            var resDto = _mapper.Map<UpdateUserResponse>(user);

            return resDto;
        }

        public async Task<bool> DeleteUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            await _userManager.DeleteAsync(user);

            return true;
        }
        public async Task<bool> DeactivateAccountAsync()
        {
            var user = await GetCurrentUserModelAsync();

            if (user == null)
            {
                return false;
            }

            await _signInManager.SignOutAsync();
            await _userManager.DeleteAsync(user);

            return true;
        }

        private async Task<ApplicationUser?> GetCurrentUserModelAsync()
        {
            var name = _contextAccessor?.HttpContext?.User?.Identity?.Name;

            if (name is null)
            {
                return null;
            }

            var user = await _userManager.Users
                .Include(e => e.Cart)
                .Include(e => e.WishlistItems)
                .Include(e => e.Purchases)
                .SingleOrDefaultAsync(e => e.UserName == name);

            return user;
        }
    }
}

