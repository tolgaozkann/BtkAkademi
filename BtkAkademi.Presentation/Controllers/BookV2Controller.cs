using BtkAkademi.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BtkAkademi.Presentation.Controllers;

[ApiVersion("2.0",Deprecated = true)]
[ApiController]
[Route("api/Book")]
public class BookV2Controller : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public BookV2Controller(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet(Name = "GetAllBooks")]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _serviceManager.BookService.GetAllBooksAsync(false);

        return Ok(books);
    }
}