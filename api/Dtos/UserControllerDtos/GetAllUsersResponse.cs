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
        public string ImageSrc { get; set; } = string.Empty;
        public List<GAUR_UserRoleDto> UserRoles { get; set; } = new();
    }

    public class GAUR_UserRoleDto
    {
        public int Id { get; set; }
        public GAUR_RoleDto? Role { get; set; }
    }

    public class GAUR_RoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }

    public class GetAllUsersResponseProfiles : Profile
    {
        public GetAllUsersResponseProfiles()
        {
            CreateMap<User, GetAllUsersResponse>();
            CreateMap<UserRole, GAUR_UserRoleDto>();
            CreateMap<Role, GAUR_RoleDto>();
        }
    }
}
