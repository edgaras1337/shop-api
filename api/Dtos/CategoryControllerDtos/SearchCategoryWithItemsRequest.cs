using api.Models;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.CategoryControllerDtos
{
    public class SearchCategoryWithItemsRequest
    {
        [Required]
        [MinLength(1)]
        public string SearchKey { get; set; } = string.Empty;
    }
}
