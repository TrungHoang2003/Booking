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
        // Nếu base64 có dạng: "data:image/png;base64,iVBORw0KGgoAAAANS..." thì tách phần dữ liệu ra
        var base64Data = base64Image.Contains(',') ? base64Image.Split(',')[1] : base64Image;

        // Convert base64 string to byte[]
        var imageBytes = Convert.FromBase64String(base64Data);

        await using var stream = new MemoryStream(imageBytes);

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription("Base64Image.png", stream),
            Folder = "Booking-images"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
            throw new Exception($"Error uploading image: {uploadResult.Error.Message}");

        return uploadResult.SecureUrl.ToString();
    }
}