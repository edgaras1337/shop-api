using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Helpers
{
    public static class Extensions
    { 
        public async static Task<ApplicationUser> WithImagesAsync(this ApplicationUser user, 
            IImageService imageService)
        {
            user.ImageSource = await imageService
                .GetImageSourceAsync(user.ImageName);

            foreach (var wishlistItem in user.WishlistItems)
            {
                foreach (var image in wishlistItem.Item!.Images)
                {
                    image.ImageSource = await imageService
                        .GetImageSourceAsync(image.ImageName);
                }
            };

            if (user.Cart != null)
            {
                foreach (var cartItem in user.Cart!.CartItems)
                {
                    foreach (var image in cartItem.Item!.Images)
                    {
                        image.ImageSource = await imageService
                            .GetImageSourceAsync(image.ImageName);
                    }
                };
            }

            return user;
        }

        // recursive function to iterate through children categories and add image sources
        private static async Task<Category> WalkCategoryTree(Category category, IImageService imageService)
        {
            if (category.Children is null)
            {
                return category;
            }

            foreach (var child in category.Children)
            {
                child.ImageSource = await imageService
                    .GetImageSourceAsync(child.ImageName);

                foreach (var item in child.Items)
                {
                    foreach (var image in item.Images)
                    {
                        image.ImageSource = await imageService
                            .GetImageSourceAsync(image.ImageName);
                    }
                }

                await WalkCategoryTree(child, imageService);
            }

            return category;
        }

        public async static Task<Category> WithImagesAsync(this Category category, IImageService imageService)
        {
            // add image urls to parent categories
            var currentCategory = category;
            do
            {
                currentCategory.ImageSource = await imageService
                    .GetImageSourceAsync(category.ImageName);

                currentCategory = currentCategory.Parent;
            } 
            while (currentCategory != null);

            // add image urls to children categories
            category = await WalkCategoryTree(category, imageService);

            return category;
        }

        public async static Task<Item> WithImagesAsync(this Item item, IImageService imageService)
        {
            foreach (var image in item!.Images)
            {
                image.ImageSource = await imageService.GetImageSourceAsync(image.ImageName);

                foreach (var comment in item.Comments)
                {
                    comment.User!.ImageSource = await imageService.GetImageSourceAsync(comment.User.ImageName);
                }
            }

            return item;
        }

        public async static Task<Cart> WithImages(this Cart cart, IImageService imageService)
        {
            foreach (var cartItem in cart.CartItems)
            {
                foreach (var image in cartItem.Item!.Images)
                {
                    image.ImageSource = await imageService.GetImageSourceAsync(image.ImageName);
                }
            }

            return cart;
        }

        public async static Task<List<WishlistItem>> WithImages(this List<WishlistItem> 
            wishlist, IImageService imageService)
        {
            foreach (var wishlistItem in wishlist)
            {
                foreach (var image in wishlistItem.Item!.Images)
                {
                    image.ImageSource = await imageService.GetImageSourceAsync(image.ImageName);
                }
            }

            return wishlist;
        }
    }
}
