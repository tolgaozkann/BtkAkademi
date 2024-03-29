﻿using BtkAkademi.Entities.Dtos;
using BtkAkademi.Entities.RequestFeatures;
using BtkAkademi.Presentation.ActionFilters;
using BtkAkademi.Services.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace BtkAkademi.Presentation.Controllers
{
    [ApiVersion("1.0")]
    [ServiceFilter(typeof(LogFilterAttribute))]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    //[ResponseCache(CacheProfileName = "5mins")]
    public class BookController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BookController(IServiceManager manager)
        {
            _manager = manager;
        }

        [Authorize]
        [HttpHead]
        [ServiceFilter(typeof(ValidateMediaTypesAttribute))]
        [HttpGet(Name ="GetAllBooks")]
        //[ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetAllBooks([FromQuery]BookParameters bookParameters)
        {

            var linkParameters = new LinkParameters
            {
                BookParameters = bookParameters,
                HttpContext = HttpContext
            };

            var result = await _manager.BookService.GetAllBooksAsync(linkParameters, false);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(result.metaData));

            return result.linkResponse.HasLinks ? 
                Ok(result.linkResponse.LinkedEntities) : 
                Ok(result.linkResponse.ShapedEntities);

        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBook([FromRoute(Name = "id")] int id)
        {
            return Ok(await _manager.BookService.GetOneBookByIdAsync(id, false));
        }

        [Authorize]
        [HttpGet("details")]
        public async Task<IActionResult> GetAllBooksWithDetailsAsync()
        {
            return Ok(await _manager
                .BookService
                .GetAllBooksWithDetailsAsync(false));
        }

        [Authorize(Roles = "Admin, Editor")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost(Name = "CreateOneBook")]
        public async Task<IActionResult> CreateOneBook([FromBody] InsertBookDto bookDto)
        {
            var book = await _manager.BookService.CreateOneBookAsync(bookDto);
            return StatusCode(201, book);
        }

        [Authorize(Roles = "Admin, Editor")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] UpdateBookDto book)
        {
            if (book is null)
                return BadRequest("Request body is null");

            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _manager.BookService.UpdateOneBookAsync(id, book, true);

            return NoContent();//204
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            await _manager.BookService.DeleteOneBookAsync(id, false);
            return NoContent();
        }

        [Authorize(Roles = "Admin, Editor")]
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<UpdateBookDto> bookPatch)
        {
            if (bookPatch is null)
                return BadRequest();

            var result = await _manager.BookService.GetOneBookForPatchAsync(id, false);

            bookPatch.ApplyTo(result.updateBookDto);

            TryValidateModel(result.updateBookDto);

            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);


            await _manager.BookService.SaveChangesForPatchAsync(result.updateBookDto, result.book);

            return NoContent();//204
        }

        [Authorize]
        [HttpOptions]
        public IActionResult GetBooksOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, PATCH, DELETE, HEAD, OPTIONS");
            return Ok();
        }
    }
}
