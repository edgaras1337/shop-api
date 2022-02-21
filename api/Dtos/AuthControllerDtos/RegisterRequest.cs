using api.Models;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.AuthControllerDtos
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MinLength(3)]
        public string Surname { get; set; } = string.Empty;
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [MinLength(5)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(3)]
        public string Password { get; set; } = string.Empty;
        public int? RoleId { get; set; }
    }

    public class RegisterRequestProfiles : Profile
    {
        public RegisterRequestProfiles()
        {
            CreateMap<RegisterRequest, User>();
        }
    }
}
