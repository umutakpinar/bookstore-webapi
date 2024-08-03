using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IServiceManager _manager;
    
    public BooksController(IServiceManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public IActionResult GetAllBooks()
    {
        try
        {
            var books = _manager.BookService.GetAllBooks(false);
            return Ok(books);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
    {
        try
        {
            var book = _manager.BookService.GetOneBookById(id,false);
            
            return Ok(book);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public IActionResult CreateOneBook([FromBody] Book? book)
    {
        try
        {
            var addedEntity = _manager.BookService.CreateOneBook(book);
            
            return StatusCode(201,addedEntity);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
    {
        try
        {
            if (book.Id != id)
                return BadRequest("Route id ve gönderilen kitap id eşleşmiyor.");

            book.Id = id; // ihtiyac yok gibi ama yine de :/

            _manager.BookService.UpdateOneBook(id, book, true);
            
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteBook([FromRoute(Name="id")] int id)
    {
        try
        {
            _manager.BookService.DeleteOneBook(id,true);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("{id:int}")]
    public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")]int id, [FromBody]JsonPatchDocument<Book> bookPatch)
    {
        try
        {
            var entity = _manager.BookService.GetOneBookById(id, true);

            bookPatch.ApplyTo(entity);
            _manager.BookService.UpdateOneBook(id,entity,true);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}