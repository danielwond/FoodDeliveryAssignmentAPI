using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace FoodDelivery.API.Controllers;

[Route("api/files")]
[ApiController]
//[Authorize("AdminPolicy")]
public class FilesController() : ControllerBase
{
    [HttpGet("get")]
    public IActionResult GetImage(string filePath)
    {
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("File Not Found");
        }
        var imageBytes = System.IO.File.ReadAllBytes(filePath);
        if (imageBytes == null)
        {
            return NotFound();
        }
        return File(imageBytes, GetMimeType(filePath));
    }
    private string GetMimeType(string fileName)
    {
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(fileName, out var contentType))
        {
            contentType = "application/octet-stream";
        }
        return contentType;
    }
    
    [HttpGet("get-multiple")]
    public IActionResult GetImages([FromQuery] List<string> filePaths)
    {
        var imageList = new List<object>();

        foreach (var filePath in filePaths)
        {
            if (!System.IO.File.Exists(filePath))
            {
                continue; // Skip missing files
            }

            var imageBytes = System.IO.File.ReadAllBytes(filePath);
            if (imageBytes != null)
            {
                var base64String = Convert.ToBase64String(imageBytes);
                imageList.Add(new
                {
                    FileName = Path.GetFileName(filePath),
                    MimeType = GetMimeType(filePath),
                    Base64 = $"data:{GetMimeType(filePath)};base64,{base64String}"
                });
            }
        }

        return Ok(imageList);
    }
    
    [HttpGet("get-urls")]
    public IActionResult GetImageUrls([FromQuery] List<string> filePaths)
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}/images/";
        var imageUrls = filePaths
            .Where(System.IO.File.Exists)
            .Select(file => new { FileName = Path.GetFileName(file), Url = baseUrl + Path.GetFileName(file) })
            .ToList();

        return Ok(imageUrls);
    }

}