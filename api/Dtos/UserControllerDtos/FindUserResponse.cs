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
        public string ImageSource { get; set; } = string.Empty;
        public List<FindUserResponse_UserRoleDto> UserRoles { get; set; } = new();

        public class FindUserResponse_UserRoleDto
        {
            public FindUserResponse_RoleDto? Role { get; set; }
        }

        public class FindUserResponse_RoleDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        public class FindUserResponseProfiles : Profile
        {
            public FindUserResponseProfiles()
            {
                CreateMap<ApplicationUser, FindUserResponse>();
                CreateMap<ApplicationUserRole, FindUserResponse_UserRoleDto>();
                CreateMap<ApplicationRole, FindUserResponse_RoleDto>();
            }
        }
    }
}
