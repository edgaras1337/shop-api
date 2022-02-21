using api.Models;
using AutoMapper;

namespace api.Dtos.UserControllerDtos
{
    public class UpdateUserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
        public List<UUR_UserRoleDto> UserRoles { get; set; } = new List<UUR_UserRoleDto>();
    }

    public class UUR_UserRoleDto
    {
        public int Id { get; set; }
        public UUR_RoleDto? Role { get; set; }
    }

    public class UUR_RoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }

    public class UpdateUserResponseProfiles : Profile
    {
        public UpdateUserResponseProfiles()
        {
            CreateMap<User, UpdateUserResponse>();
            CreateMap<UserRole, UUR_UserRoleDto>();
            CreateMap<Role, UUR_RoleDto>();
        }
    }
}
