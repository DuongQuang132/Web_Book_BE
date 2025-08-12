using System.Net.Http;
using System.IO;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Web_Book_BE.Services
{
    public interface IImageService
    {
        Task<string> UploadImageFromUrlAsync(string imageUrl);
        Task<string> UploadImageFromFileAsync(IFormFile imageFile);
        bool IsValidImageUrl(string url);
        bool IsValidImageFile(IFormFile file);
    }

    public class ImageService : IImageService
    {
        private readonly HttpClient _httpClient;
        private readonly Cloudinary _cloudinary;

        public ImageService(HttpClient httpClient, Cloudinary cloudinary)
        {
            _httpClient = httpClient;
            _cloudinary = cloudinary;
        }

        public async Task<string> UploadImageFromUrlAsync(string imageUrl)
        {
            try
            {
                // Tải ảnh từ URL
                var imageBytes = await _httpClient.GetByteArrayAsync(imageUrl);
                
                // Tạo stream từ bytes
                using var stream = new MemoryStream(imageBytes);
                
                // Upload lên Cloudinary
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription("image", stream),
                    PublicId = $"products/{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}",
                    Folder = "web-book"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi upload ảnh từ URL lên Cloudinary: {ex.Message}");
            }
        }

        public async Task<string> UploadImageFromFileAsync(IFormFile imageFile)
        {
            try
            {
                // Upload file lên Cloudinary
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageFile.FileName, imageFile.OpenReadStream()),
                    PublicId = $"products/{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}",
                    Folder = "web-book"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi upload file ảnh lên Cloudinary: {ex.Message}");
            }
        }

        public bool IsValidImageUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return Uri.TryCreate(url, UriKind.Absolute, out var uri) &&
                   (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps) &&
                   IsImageExtension(Path.GetExtension(uri.LocalPath));
        }

        public bool IsValidImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            return allowedExtensions.Contains(fileExtension);
        }

        private bool IsImageExtension(string extension)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            return allowedExtensions.Contains(extension.ToLowerInvariant());
        }
    }
}
