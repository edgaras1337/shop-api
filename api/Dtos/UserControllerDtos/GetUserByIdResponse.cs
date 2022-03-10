using api.Models;
using AutoMapper;

namespace api.Dtos.UserControllerDtos
{
    public class GetUserByIdResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ImageSource { get; set; } = string.Empty;
        public List<GetUserByIdResponse_UserRoleDto> UserRoles { get; set; } = new();
    }

    public class GetUserByIdResponse_UserRoleDto
    {
        public GetUserByIdResponse_RoleDto? Role { get; set; }
    }

    public class GetUserByIdResponse_RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class GetUserByIdResponseProfiles : Profile
    {
        public GetUserByIdResponseProfiles()
        {
            CreateMap<ApplicationUser, GetUserByIdResponse>();
            CreateMap<ApplicationUserRole, GetUserByIdResponse_UserRoleDto>();
            CreateMap<ApplicationRole, GetUserByIdResponse_RoleDto>();
        }
    }
}
