using BtkAkademi.Entities.Dtos;
using BtkAkademi.Services.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BtkAkademi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BookController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _manager.BookService.GetAllBooks(false);

            return Ok(books);

        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            return Ok(_manager.BookService.GetOneBookById(id, false));
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] InsertBookDto bookDto)
        {
            if (bookDto is null)
                return BadRequest("Request body is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var book = _manager.BookService.CreateOneBook(bookDto);
            return StatusCode(201, book);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] UpdateBookDto book)
        {
            if (book is null)
                return BadRequest("Request body is null");

            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            _manager.BookService.UpdateOneBook(id, book, true);

            return NoContent();//204
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            _manager.BookService.DeleteOneBook(id, false);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<UpdateBookDto> bookPatch)
        {
            if (bookPatch is null)
                return BadRequest();

            var result = _manager.BookService.GetOneBookForPatch(id, false);

            bookPatch.ApplyTo(result.updateBookDto, ModelState);

            TryValidateModel(result.updateBookDto);

            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);


            _manager.BookService.SaveChangesForPatch(result.updateBookDto, result.book);

            return NoContent();//204
        }
    }
}
