﻿using api.Models;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.CommentControllerDtos
{
    public class AddCommentRequest
    {
        [Required]
        [MinLength(1)]
        public string CommentText { get; set; } = string.Empty;
        [Required]
        public int ItemId { get; set; }
    }

    public class AddCommentRequestProfiles : Profile
    {
        public AddCommentRequestProfiles()
        {
            CreateMap<AddCommentRequest, Comment>();
        }
    }
}
