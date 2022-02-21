using api.Dtos.AuthControllerDtos;
using api.Dtos.UserControllerDtos;
using api.UserControllerDtos;

namespace api.Services
{
    public interface IUserService
    {
        Task<GetUserByIdResponse?> GetByIdAsync(int id);
        //Task<User?> GetByEmailAsync(string email);
        Task<GetCurrentUserResponse?> GetCurrentUserAsync();
        Task<List<GetAllUsersResponse>> GetAllUsersAsync();
        Task<List<FindUserResponse>> FindUserAsync(FindUserRequest request);
        Task<LoginResponse?> AuthenticateAsync(LoginRequest dto);
        Task LogoutAsync();
        Task<RegisterResponse> CreateUserAsync(RegisterRequest dto);
        Task<UpdateUserResponse> UpdateCurrentUserAsync(UpdateUserRequest request);
        Task<bool> DeleteUserByIdAsync(int id);
        Task DeactivateAccountAsync();
    }
}
