﻿using api.Models;
using AutoMapper;

namespace api.Dtos.CommentControllerDtos
{
    public class RemoveCommentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public RCR_CategoryDto? Category { get; set; }
        public List<RCR_ItemImagesDto>? Images { get; set; }
        public List<RCR_CommentDto> Comments { get; set; } = new List<RCR_CommentDto>();
    }


    // navigation dtos
    public class RCR_CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class RCR_ItemImagesDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
    }

    public class RCR_CommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public RCR_UserDto? User { get; set; }
    }

    public class RCR_UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public List<RCR_UserRoleDto> UserRoles { get; set; } = new List<RCR_UserRoleDto>();
    }

    public class RCR_UserRoleDto
    {
        public int Id { get; set; }
        public RCR_RoleDto? Role { get; set; }
    }

    public class RCR_RoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }


    // profiles for automapper
    public class RemoveCommentResponseProfiles : Profile
    {
        public RemoveCommentResponseProfiles()
        {
            CreateMap<Item, RemoveCommentResponse>();
            CreateMap<Category, RCR_CategoryDto>();
            CreateMap<ItemImage, RCR_ItemImagesDto>();
            CreateMap<Comment, RCR_CommentDto>();
            CreateMap<User, RCR_UserDto>();
            CreateMap<UserRole, RCR_UserRoleDto>();
            CreateMap<Role, RCR_RoleDto>();
        }
    }
}