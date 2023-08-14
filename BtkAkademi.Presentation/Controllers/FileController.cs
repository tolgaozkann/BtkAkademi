using BtkAkademi.Presentation.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace BtkAkademi.Presentation.Controllers;


[ApiVersion("1.0")]
[ServiceFilter(typeof(LogFilterAttribute))]
[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
public class FileController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var folder = Path.Combine(Directory.GetCurrentDirectory(), "Media");

        if(!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
            
        var path = Path.Combine(folder,file.FileName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new
        {
            file = file.FileName,
            path = path,
            size = file.Length
        });
    }

    [HttpGet]
    public async Task<IActionResult> Download(string fileName)
    {
        //path
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Media", fileName);

        //content type : (MIME)
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(fileName, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        //read
        var bytes = await System.IO.File.ReadAllBytesAsync(path);

        return File(bytes, contentType, Path.GetFileName(path));
    }
}