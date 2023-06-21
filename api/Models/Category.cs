using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    //public class Category
    //{
    //    private readonly ILazyLoader? _lazyLoader;
    //    public Category()
    //    {
    //    }

    //    public Category(ILazyLoader lazyLoader)
    //    {
    //        _lazyLoader = lazyLoader;
    //    }

    //    private List<Category>? _children;
    //    private Category? _parent;

    //    [Key]
    //    public int Id { get; set; }
    //    public string Name { get; set; } = string.Empty;
    //    public DateTimeOffset CreatedDate { get; set; }
    //    public DateTimeOffset ModifiedDate { get; set; }
    //    public string ImageName { get; set; } = string.Empty;
    //    [NotMapped]
    //    public string ImageSource { get; set; } = string.Empty;
    //    [ForeignKey("ParentCategoryId")]
    //    public int? ParentCategoryId { get; set; }

    //    public virtual List<Item> Items { get; set; } = new();

    //    public virtual Category? Parent
    //    {
    //        get => _lazyLoader.Load(this, ref _parent);
    //        set => _parent = value;
    //    }
    //    public virtual List<Category> Children
    //    {
    //        get => _lazyLoader.Load(this, ref _children!)!;
    //        set => _children = value;
    //    }
    //}

    public class Category
    {
        public Category()
        {
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ImageName { get; set; } = string.Empty;
        [NotMapped]
        public string ImageSource { get; set; } = string.Empty;
        [ForeignKey("ParentCategoryId")]
        public int? ParentCategoryId { get; set; }

        public List<Item> Items { get; set; } = new();
        public Category? ParentCategory { get; set; }
        public List<Category>? ChildCategories { get; set; }
    }
}
