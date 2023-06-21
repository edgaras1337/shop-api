using api.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Helpers
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageService(IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string?> SaveImageAsync(IFormFile? imageFile)
        {
            if (imageFile == null)
            {
                return null;
            }

            string imageName = new(Path
                .GetFileNameWithoutExtension(imageFile.FileName)
                .Take(10)
                .ToArray());
            imageName += DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);

            //var imageName = Path.GetTempFileName();
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images", imageName);
            

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

        public async Task<string> GetImageSourceAsync(string imageName)
        {
            var context = _httpContextAccessor.HttpContext;

            if (context == null)
            {
                return string.Empty;
            }

            var source = string.Format("{0}://{1}{2}/Images/{3}",
                    context.Request.Scheme, context.Request.Host, context.Request.PathBase, imageName);

            return await Task.FromResult(source);
        }

        public async Task DeleteImageFileAsync(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images", imageName);

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }

            await Task.CompletedTask;
        }

        public async Task LoadImagesAsync<TEntity> (TEntity entity) where TEntity : class, new()
        {
            var task = entity switch
            {
                ApplicationUser user => LoadUserImageAsync(user),
                IEnumerable<ApplicationUser> users => LoadUserImageAsync(users.ToArray()),
                Category category => LoadCategoryImagesAsync(category),
                IEnumerable<Category> categories => LoadCategoryImagesAsync(categories.ToArray()),
                Item item => LoadItemImagesAsync(item),
                IEnumerable<Item> items => LoadItemImagesAsync(items.ToArray()),
                Cart cart => LoadCartItemImagesAsync(cart),
                IEnumerable<Cart> carts => LoadCartItemImagesAsync(carts.ToArray()),
                WishlistItem wishlistItem => LoadWishlistItemImagesAsync(wishlistItem),
                IEnumerable<WishlistItem> wishlistItems => LoadWishlistItemImagesAsync(wishlistItems.ToArray()),
                _ => throw new ArgumentException(null, nameof(entity))
            };

            await task;
        }

        //private async Task LoadUserImageAsync(ApplicationUser user) => await LoadUserImageAsync(new ApplicationUser[] { user });
        private async Task LoadUserImageAsync(params ApplicationUser[] users)
        {
            foreach (var user in users)
            {
                user.ImageSource = await GetImageSourceAsync(user.ImageName);

                foreach (var image in user.WishlistItems.SelectMany(wishlistItem => wishlistItem.Item!.Images))
                {
                    image.ImageSource = await GetImageSourceAsync(image.ImageName);
                };

                if (user.Cart == null)
                {
                    continue;
                }
                
                {
                    foreach (var image in user.Cart!.CartItems.SelectMany(cartItem => cartItem.Item!.Images))
                    {
                        image.ImageSource = await GetImageSourceAsync(image.ImageName);
                    }
                }
            }
        }

        private async Task LoadCategoryImagesAsync(params Category[] categories)
        {
            foreach (var category in categories)
            {
                // add image urls to parent categories
                var currentCategory = category;
                do
                {
                    currentCategory.ImageSource = await GetImageSourceAsync(category.ImageName);

                    currentCategory = currentCategory.ParentCategory;
                }
                while (currentCategory != null);

                // add image urls to children categories
                await WalkCategoryTree(category);
            }
        }

        private async Task LoadItemImagesAsync(params Item[] items)
        {
            foreach (var item in items)
            {
                foreach (var image in item!.Images)
                {
                    image.ImageSource = await GetImageSourceAsync(image.ImageName);

                    foreach (var review in item.Reviews)
                    {
                        review.User!.ImageSource = await GetImageSourceAsync(review.User.ImageName);
                    }
                }
            }
        }

        private async Task LoadCartItemImagesAsync(params Cart[] carts)
        {
            foreach (var cart in carts)
            {
                foreach (var image in cart.CartItems.SelectMany(cartItem => cartItem.Item!.Images))
                {
                    image.ImageSource = await GetImageSourceAsync(image.ImageName);
                }
            }
        }

        private async Task LoadWishlistItemImagesAsync(params WishlistItem[] wishlist)
        {
            foreach (var wishlistItem in wishlist)
            {
                foreach (var image in wishlistItem.Item!.Images)
                {
                    image.ImageSource = await GetImageSourceAsync(image.ImageName);
                }
            }
        }


        // recursive function to iterate through children categories and add image sources
        private async Task WalkCategoryTree(Category category)
        {
            if (category.ChildCategories is null)
            {
                return;
            }

            foreach (var child in category.ChildCategories)
            {
                child.ImageSource = await GetImageSourceAsync(child.ImageName);

                foreach (var image in child.Items.SelectMany(item => item.Images))
                {
                    image.ImageSource = await GetImageSourceAsync(image.ImageName);
                }

                await WalkCategoryTree(child);
            }
        }
    }
}
