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
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;

namespace api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IJwtService _jwtService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _config;

        public UserService(
            IUserRepository userRepository,
            ICartRepository cartRepository,
            IRoleRepository roleRepository,
            IJwtService jwtService,
            IImageService imageService,
            IMapper mapper,
            IHttpContextAccessor contextAccessor,
            IConfiguration config)
        {
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _roleRepository = roleRepository;
            _jwtService = jwtService;
            _imageService = imageService;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _config = config;
        }

        public async Task<GetUserByIdResponse?> GetByIdAsync(int id)
        {
            // get user by id from repo
            var user = await _userRepository.GetByIdAsync(id);
            user = await AppendImgSourceAsync(user);

            // append image source and return user
            return _mapper.Map<GetUserByIdResponse>(user);
        }

        public async Task<GetCurrentUserResponse?> GetCurrentUserAsync()
        {
            var user = await GetCurrentUserModelAsync();
            user = await AppendImgSourceAsync(user);

            // add images
            user = await AppendImgSourceAsync(user);
            user!.Cart?.CartItems.ForEach(cartItem =>
            {
                cartItem.Item?.Images.ForEach(async image =>
                    image.ImageSrc = await _imageService.GetImageSourceAsync(image.ImageName));
            });

            user.WishlistItems.ForEach(wishlistItem =>
            {
                wishlistItem.Item?.Images.ForEach(async image =>
                    image.ImageSrc = await _imageService.GetImageSourceAsync(image.ImageName));
            });

            return _mapper.Map<GetCurrentUserResponse>(user);
        }

        public async Task<List<GetAllUsersResponse>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            // create dtos list and append all users converted to dtos
            var dtos = new List<GetAllUsersResponse>();
            foreach(var user in users)
            {
                var userWithImageSrc = await AppendImgSourceAsync(user);

                if(userWithImageSrc != null)
                {
                    dtos.Add(_mapper.Map<GetAllUsersResponse>(userWithImageSrc));
                }
            }

            return dtos;
        }

        public async Task<List<FindUserResponse>> FindUserAsync(FindUserRequest request)
        {
            var users = await _userRepository.FindUserAsync(request.SearchKey);

            var dtoList = new List<FindUserResponse>();
            foreach(var user in users)
            {
                var userWithImage = await AppendImgSourceAsync(user);
                dtoList.Add(_mapper.Map<FindUserResponse>(userWithImage));
            }

            return dtoList;
        }

        public async Task<LoginResponse?> AuthenticateAsync(LoginRequest dto)
        {
            // check for email
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user is null)
            {
                return null;
            }

            // authenticate password
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return null;
            }
            // generate jwt token
            var jwt = await _jwtService.Generate(user);

            // append token to an http cookie
            _contextAccessor.HttpContext?
                .Response.Cookies
                .Append(_config["JwtConfiguration:CookieName"], jwt, new CookieOptions
                {
                    HttpOnly = true,
                });

            // add images
            user = await AppendImgSourceAsync(user);
            user!.Cart?.CartItems.ForEach(cartItem =>
            {
                cartItem.Item?.Images.ForEach(async image =>
                    image.ImageSrc = await _imageService.GetImageSourceAsync(image.ImageName));
            });

            user.WishlistItems.ForEach(wishlistItem =>
            {
                wishlistItem.Item?.Images.ForEach(async image =>
                    image.ImageSrc = await _imageService.GetImageSourceAsync(image.ImageName));
            });

            var response = _mapper.Map<LoginResponse>(user);
            response.Token = jwt;

            return response;
        }

        public async Task LogoutAsync()
        {
            _contextAccessor.HttpContext?
                .Response.Cookies.Delete(_config["JwtConfiguration:CookieName"]);

            await Task.CompletedTask;
        }

        public async Task<RegisterResponse> CreateUserAsync(RegisterRequest dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser is not null)
            {
                throw new DuplicateNameException();
            }

            // create a new user from dto
            var user = _mapper.Map<User>(dto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.ImageName = _config["ImagesConfiguration:DefaultUserImageName"];

            user.Cart = new Cart()
            {
                UserId = user.Id,
                TotalPrice = 0,
                ModifiedDate = DateTimeOffset.Now
            };
            await Task.FromResult(user);

            // get user role id from db
            var defaultRole = await _roleRepository.GetByRoleNameAsync("Customer");
            if (defaultRole is null)
            {
                throw new ObjectNotFoundException();
            }

            // add the user role
            user.UserRoles.Add(new UserRole()
            {
                UserId = user.Id,
                RoleId = defaultRole.Id
            });
            await Task.FromResult(user);

            // add role from dto
            if (dto.RoleId != null && dto.RoleId != defaultRole.Id)
            {
                // get specified role by id from db
                var dtoRole = await _roleRepository.GetByIdAsync((int)dto.RoleId);
                if (dtoRole is null)
                {
                    throw new ObjectNotFoundException();
                }

                // add another role to userRoles list
                user.UserRoles.Add(new UserRole() 
                { 
                    UserId = user.Id, 
                    RoleId = (int)dto.RoleId 
                });
                await Task.FromResult(user);
            }
            await _userRepository.AddAsync(user);

            user = await AppendImgSourceAsync(user);

            return _mapper.Map<RegisterResponse>(user);
        }

        public async Task<UpdateUserResponse> UpdateCurrentUserAsync(UpdateUserRequest dto)
        {
            // get current user
            var id = await GetCurrentUserIdAsync();
            if(id is null)
            {
                throw new UnauthorizedAccessException();
            }
            var user = await _userRepository.GetByIdAsync((int)id);
            if(user is null)
            {
                throw new UnauthorizedAccessException();
            }

            var defaultImageName = _config["ImagesConfiguration:DefaultUserImageName"];

            // check dto email for existing email
            if (dto.Email != null)
            {
                var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
                if (existingUser != null && existingUser.Email != dto.Email)
                {
                    throw new DuplicateNameException();
                }
            }
            if (dto.ImageFile != null)
            {
                user.ImageName = await _imageService.SaveImageAsync(dto.ImageFile) ??
                    defaultImageName;
            }

            // map properties which are not null from dto to user
            _mapper.Map(dto, user);

            // update image
            if (dto.DeleteImage)
            {
                // delete image only when its not the default one
                // when deleted set the default image
                if (user.ImageName != defaultImageName)
                {
                    await _imageService.DeleteImageFileAsync(user.ImageName);
                    user.ImageName = defaultImageName;
                }
            }
            if (dto.ImageFile != null)
            {
                // save the selected image, if failed, set default image
                user.ImageName = await _imageService.SaveImageAsync(dto.ImageFile) ??
                    defaultImageName;
            }


            // check if any of three fields isnt null
            // all three null:
            //      - do nothing
            // at least one wasnt null:
            //      - check if all three are not null
            //      - update password

            var oldPw = dto.OldPassword?.Trim();
            var newPw = dto.NewPassword?.Trim();
            var repeatNewPw = dto.RepeatNewPassword?.Trim();

            if (oldPw != null || newPw != null || repeatNewPw != null)
            {
                if (newPw != repeatNewPw || newPw is null || repeatNewPw is null || oldPw is null ||
                   !BCrypt.Net.BCrypt.Verify(oldPw, user.Password))
                {
                    throw new InvalidPasswordException();
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(newPw);
            }

            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UpdateUserResponse>(await AppendImgSourceAsync(user));
        }

        public async Task<bool> DeleteUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if(user is null)
            {
                return false;
            }

            await _userRepository.DeleteAsync(user);

            return true;
        }
        public async Task DeactivateAccountAsync()
        {
            var user = await GetCurrentUserModelAsync();

            if(user == null)
            {
                throw new UnauthorizedAccessException();
            }

            await _userRepository.DeleteAsync(user);
            _contextAccessor.HttpContext?.Response.Cookies.Delete(_config["JwtConfiguration:CookieName"]);
        }


        // helpers
        private async Task<User?> AppendImgSourceAsync(User? user)
        {
            if(user != null && user.ImageName != null)
            {
                user.ImageSrc = await _imageService.GetImageSourceAsync(user.ImageName);
            }
            return user;
        }

        private async Task<int?> GetCurrentUserIdAsync()
        {
            // get user id from claims
            var id = await Task.FromResult(_contextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return id is null ? null : int.Parse(id);
        }

        private async Task<User?> GetCurrentUserModelAsync()
        {
            // get current user id
            var id = await GetCurrentUserIdAsync();
            if (id is null)
            {
                return null;
            }

            // get user by the id found in claims
            var user = await _userRepository.GetByIdAsync((int)id);

            return await AppendImgSourceAsync(user);
        }

        /*private async Task<User?> GetByEmailAsync(string email)
        {
            // get user by email from repo
            var user = await _userRepository.GetByEmailAsync(email);

            // append image source and return user
            return await AppendImgSourceAsync(user);
        }*/
    }
}

