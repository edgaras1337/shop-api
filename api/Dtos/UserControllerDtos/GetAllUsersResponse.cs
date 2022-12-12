using api.Models;
using AutoMapper;

namespace api.Dtos.UserControllerDtos
{
    public class GetAllUsersResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ImageSource { get; set; } = string.Empty;
        public List<GetAllUsersResponse_UserRoleDto> UserRoles { get; set; } = new();
    }

    public class GetAllUsersResponse_UserRoleDto
    {
        public GetAllUsersResponse_RoleDto? Role { get; set; }
    }

    public class GetAllUsersResponse_RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class GetAllUsersResponseProfiles : Profile
    {
        public GetAllUsersResponseProfiles()
        {
            CreateMap<ApplicationUser, GetAllUsersResponse>();
            CreateMap<ApplicationUserRole, GetAllUsersResponse_UserRoleDto>();
            CreateMap<ApplicationRole, GetAllUsersResponse_RoleDto>();
        }
    }
}
