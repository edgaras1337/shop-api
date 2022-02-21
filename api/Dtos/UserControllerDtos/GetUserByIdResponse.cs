using api.Models;
using AutoMapper;

namespace api.Dtos.UserControllerDtos
{
    public class GetUserByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ImageSrc { get; set; } = string.Empty;
        public List<GUBIR_UserRoleDto> UserRoles { get; set; } = new();
    }

    public class GUBIR_UserRoleDto
    {
        public int Id { get; set; }
        public GUBIR_RoleDto? Role { get; set; }
    }

    public class GUBIR_RoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }

    public class GetUserByIdResponseProfiles : Profile
    {
        public GetUserByIdResponseProfiles()
        {
            CreateMap<User, GetUserByIdResponse>();
            CreateMap<UserRole, GUBIR_UserRoleDto>();
            CreateMap<Role, GUBIR_RoleDto>();
        }
    }
}
