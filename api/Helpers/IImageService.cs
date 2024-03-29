﻿namespace api.Helpers
{
    public interface IImageService
    {
        Task<string?> SaveImageAsync(IFormFile? imageFile);
        Task<string> GetImageSourceAsync(string imageName);
        Task DeleteImageFileAsync(string imageName);
        Task LoadImagesAsync<T>(T entity) where T : class, new();
    }
}
