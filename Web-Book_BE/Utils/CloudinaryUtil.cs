using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

public class CloudinaryUtil
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryUtil(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<ImageUploadResult> UploadImageAsync(IFormFile file, string folder = "default")
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File không hợp lệ.");

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            Folder = folder,
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = true
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return uploadResult;
    }

    public async Task<DeletionResult> DeleteImageAsync(string publicId)
    {
        var deletionParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deletionParams);
        return result;
    }
}
