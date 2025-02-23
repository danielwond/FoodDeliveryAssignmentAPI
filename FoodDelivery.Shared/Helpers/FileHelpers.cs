using FoodDelivery.Shared.Enums;
using Microsoft.AspNetCore.Http;

namespace FoodDelivery.Shared.Helpers;

public static class FileHelpers
{
    public static async Task<string> UploadImage(IFormFile file, FileTypeEnum fileType )
    {
        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = Guid.NewGuid() + $".{fileExtension}";
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", fileType.ToString());
        var filePath = Path.Combine(folderPath, fileName);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return filePath;
    }
}