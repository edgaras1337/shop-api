using api.Dtos.AuthControllerDtos;
using api.Dtos.UserControllerDtos;
using api.UserControllerDtos;

namespace api.Services
{
    public interface IUserService
    {
        Task<GetUserByIdResponse?> GetByIdAsync(string id);
        //Task<User?> GetByEmailAsync(string email);
        Task<GetCurrentUserResponse?> GetCurrentUserAsync();
        Task<List<GetAllUsersResponse>> GetAllUsersAsync();
        Task<List<FindUserResponse>> FindUserAsync(string searchKey);
        Task<LoginResponse?> AuthenticateAsync(LoginRequest dto);
        Task LogoutAsync();
        Task<RegisterResponse> CreateUserAsync(RegisterRequest dto);
        Task<UpdateUserResponse> UpdateCurrentUserAsync(UpdateUserRequest request);
        Task<bool> DeleteUserByIdAsync(string id);
        Task<bool> DeactivateAccountAsync();
    }
}
