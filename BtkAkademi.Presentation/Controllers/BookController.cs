using BtkAkademi.Entities.Exceptions;
using BtkAkademi.Entities.Models;
using BtkAkademi.Services.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            if (book is null)
                return BadRequest("Request body is null");

            _manager.BookService.CreateOneBook(book);
            return StatusCode(201, book);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            if (book is null)
                return BadRequest("Request body is null");

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
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            var entity = _manager
                .BookService
                .GetOneBookById(id, false);

            bookPatch.ApplyTo(entity);
            _manager.BookService.UpdateOneBook(id, entity, true);

            return NoContent();//204
        }
    }
}
