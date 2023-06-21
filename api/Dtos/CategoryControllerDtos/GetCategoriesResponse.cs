using api.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace api.Dtos.CategoryControllerDtos
{
    public class GetCategoriesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public List<ChildrenDto> ChildCategories { get; set; } = new();
        
        public class ChildrenDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string ImageName { get; set; } = string.Empty;
            public string ImageSource { get; set; } = string.Empty;
            public List<ChildrenDto> ChildCategories { get; set; } = new();
        }
    }

    public class GetCategoriesResponseProfiles : Profile
    {
        public GetCategoriesResponseProfiles()
        {
            CreateMap<Category, GetCategoriesResponse>();
            CreateMap<Category, GetCategoriesResponse.ChildrenDto>();
        }
    }
    
    #region obsolete
    // public class GetCategoriesResponse
    // {
    //     public int Id { get; set; }
    //     public string Name { get; set; } = string.Empty;
    //     public DateTimeOffset CreatedDate { get; set; }
    //     public DateTimeOffset ModifiedDate { get; set; }
    //     public string ImageSource { get; set; } = string.Empty;
    //
    //     [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    //     public ParentDto? Parent { get; set; }
    //     public List<ChildrenDto> ChildCategories { get; set; } = new();
    //     //private List<ItemDto>? _items;
    //     [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    //     public List<ItemDto>? Items { get; set; }
    //     //{
    //     //    get => NullableListGetter(_items);
    //     //    set => _items = value; 
    //     //}
    //
    //     private static List<T>? NullableListGetter<T>(List<T>? obj) where T : class
    //     {
    //         if (obj != null && !obj.Any())
    //         {
    //             obj = null;
    //         }
    //         return obj;
    //     }
    //
    //     //public bool ShouldSerializeChildren() =>
    //     //    Children.Count > 0;
    //
    //     //public bool ShouldSerializeItems() =>
    //     //    Children.Count == 0;
    //
    //
    //     public class ParentDto
    //     {
    //         public int Id { get; set; }
    //         public string Name { get; set; } = string.Empty;
    //         public string ImageName { get; set; } = string.Empty;
    //         public string ImageSource { get; set; } = string.Empty;
    //         public DateTimeOffset CreatedDate { get; set; }
    //
    //
    //         [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    //         public ParentDto? ParentCategory { get; set; }
    //     }
    //
    //
    //     public class ChildrenDto
    //     {
    //         public int Id { get; set; }
    //         public string Name { get; set; } = string.Empty;
    //         public string ImageName { get; set; } = string.Empty;
    //         public string ImageSource { get; set; } = string.Empty;
    //
    //         //private List<ItemDto>? _items;
    //         [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    //         public List<ItemDto>? Items { get; set; }
    //         //{
    //         //    get => NullableListGetter(_items);
    //         //    set => _items = value;
    //         //}
    //
    //         public List<ChildrenDto> ChildCategories { get; set; } = new();
    //
    //
    //
    //         //public bool ShouldSerializeItems() =>
    //         //    Children.Count == 0;
    //
    //         //public bool ShouldSerializeChildren() =>
    //         //    Children.Count > 0;
    //     }
    //
    //     public class ItemDto
    //     {
    //         public int Id { get; set; }
    //         public string Name { get; set; } = string.Empty;
    //     }
    // }
    //
    // public class GetCategoriesResponseProfiles : Profile
    // {
    //     public GetCategoriesResponseProfiles()
    //     {
    //         CreateMap<Category, GetCategoriesResponse>();
    //         CreateMap<Category, GetCategoriesResponse.ParentDto>();
    //         CreateMap<Category, GetCategoriesResponse.ChildrenDto>();
    //         CreateMap<Item, GetCategoriesResponse.ItemDto>();
    //     }
    // }
    #endregion
}
