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
            if(imageFile == null)
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
    }
}
