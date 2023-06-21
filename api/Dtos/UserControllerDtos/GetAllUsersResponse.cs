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
        public List<UserRoleDto> UserRoles { get; set; } = new();


        public class UserRoleDto
        {
            public RoleDto? Role { get; set; }

            public class RoleDto
            {
                public int Id { get; set; }
                public string Name { get; set; } = string.Empty;
            }
        }
    }


    public class GetAllUsersResponseProfiles : Profile
    {
        public GetAllUsersResponseProfiles()
        {
            CreateMap<ApplicationUser, GetAllUsersResponse>();
            CreateMap<ApplicationUserRole, GetAllUsersResponse.UserRoleDto>();
            CreateMap<ApplicationRole, GetAllUsersResponse.UserRoleDto.RoleDto>();
        }
    }
}
