using api.Models;
using AutoMapper;

namespace api.Dtos.AuthControllerDtos
{
    public class RegisterResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
        public List<RR_UserRoleDto> UserRoles { get; set; } = new();
    }

    public class RR_UserRoleDto
    {
        public int Id { get; set; }
        public RR_RoleDto? Role { get; set; }
    }

    public class RR_RoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }

    public class RegisterResponseProfiles : Profile
    {
        public RegisterResponseProfiles()
        {
            CreateMap<User, RegisterResponse>();
            CreateMap<UserRole, RR_UserRoleDto>();
            CreateMap<Role, RR_RoleDto>();
        }
    }
}
