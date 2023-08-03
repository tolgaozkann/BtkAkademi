﻿using Azure;
using BtkAkademi.Entities.Models;
using BtkAkademi.Services;
using BtkAkademi.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BtkAkademi.WebAPI.Controllers
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
        public IActionResult GetAllBooks() {
            
            try
            {
                var books = _manager.BookService.GetAllBooks(false);

                return Ok(books);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name ="id")]int id) { 

            try
            {
                var book = _manager
                    .BookService
                    .GetOneBookById(id, false);

                if (book is null)
                    return NotFound();

                return Ok(book);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody]Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest("Request body is null");

                _manager.BookService.CreateOneBook(book);
                return StatusCode(201, book);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id" )]int id, [FromBody]Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest("Request body is null");

                _manager.BookService.UpdateOneBook(id, book, true);

                return NoContent();//204

            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id) 
        {
            try
            {
                
                _manager.BookService.DeleteOneBook(id, false);
                return NoContent();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody]JsonPatchDocument<Book> bookPatch)
        {

            try
            {
                var entity = _manager
                    .BookService
                    .GetOneBookById(id, false);

                if (entity is null)
                    return BadRequest("Request body is null");


                bookPatch.ApplyTo(entity);
                _manager.BookService.UpdateOneBook(id, entity, true);

                return NoContent();//204

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
