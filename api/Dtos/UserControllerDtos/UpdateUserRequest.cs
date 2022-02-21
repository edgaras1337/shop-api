using api.Models;
using AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api.UserControllerDtos
{
    public class UpdateUserRequest
    {
        [MinLength(3)]
        public string? Name { get; set; }
        [MinLength(3)]
        public string? Surname { get; set; }
        [MinLength(5)]
        public string? Email { get; set; }
        public string? OldPassword { get; set; }
        [MinLength(3)]
        public string? NewPassword { get; set; }
        public string? RepeatNewPassword { get; set; }
        public IFormFile? ImageFile { get; set; }
        public bool DeleteImage { get; set; }
    }

    public class UpdateUserRequestProfiles : Profile
    {
        public UpdateUserRequestProfiles()
        {
            CreateMap<UpdateUserRequest, User>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
