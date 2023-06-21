using api.Dtos.AuthControllerDtos;
using api.Dtos.UserControllerDtos;
using api.UserControllerDtos;

namespace api.Services
{
    public interface IUserService
    {
        Task<GetCurrentUserResponse?> GetCurrentUserAsync();
        Task<GetUserByIdResponse?> GetByIdAsync(int id);
        Task<List<GetAllUsersResponse>> GetUsersAsync();
        Task<LoginResponse?> AuthenticateAsync(LoginRequest dto);
        Task LogoutAsync();
        Task<RegisterResponse> CreateUserAsync(RegisterRequest dto);
        Task<UpdateUserResponse> UpdateCurrentUserAsync(UpdateUserRequest request);
        Task<bool> DeleteUserByIdAsync(string id);
        Task<bool> DeactivateAccountAsync();
    }
}
