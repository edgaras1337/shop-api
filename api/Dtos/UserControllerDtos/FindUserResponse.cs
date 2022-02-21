using api.Models;
using AutoMapper;

namespace api.Dtos.UserControllerDtos
{
    public class FindUserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ImageSrc { get; set; } = string.Empty;
        public List<FUR_UserRoleDto> UserRoles { get; set; } = new List<FUR_UserRoleDto>();

        public class FUR_UserRoleDto
        {
            public int Id { get; set; }
            public FUR_RoleDto? Role { get; set; }
        }

        public class FUR_RoleDto
        {
            public int Id { get; set; }
            public string RoleName { get; set; } = string.Empty;
        }

        public class FindUserResponseProfiles : Profile
        {
            public FindUserResponseProfiles()
            {
                CreateMap<User, FindUserResponse>();
                CreateMap<UserRole, FUR_UserRoleDto>();
                CreateMap<Role, FUR_RoleDto>();
            }
        }
    }
}
