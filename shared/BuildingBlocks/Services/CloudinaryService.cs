using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Services;

public class CloudinaryService
{
    private readonly Cloudinary _cloudinary;
    
    public CloudinaryService(IConfiguration configuration)
    {
        var acc = new Account
        {
            Cloud = configuration["Cloudinary:CloudName"],
            ApiKey = configuration["Cloudinary:ApiKey"],
            ApiSecret = configuration["Cloudinary:ApiSecret"]
        };
        
        _cloudinary = new Cloudinary(acc);
    }

    public async Task<string> UploadImage(string base64Image)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription("Base64Image",base64Image),
            Folder = "Booking-images",
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        
        if (uploadResult.Error != null)
        {
            throw new Exception($"Error uploading image: {uploadResult.Error.Message}");
        }

        // Return the URL of the uploaded image
        return uploadResult.SecureUrl.ToString();
    }
}