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
    public async Task<IActionResult> GetImage(string filePath)
    {
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("File Not Found");
        }
        var imageBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        if (imageBytes.Length == 0)
        {
            return NotFound("Invalid File");
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