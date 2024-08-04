using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
        var books = _manager.BookService.GetAllBooks(false);
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
    {
            var book = _manager.BookService.GetOneBookById(id,false);
            if (book is null)
                throw new BookNotFoundException(id);
            return Ok(book);
    }

    [HttpPost]
    public IActionResult CreateOneBook([FromBody] Book book)
    {
        var addedEntity = _manager.BookService.CreateOneBook(book);
        return StatusCode(201,addedEntity);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
    {
        book.Id = id; // ihtiyac yok gibi ama yine de :/
        _manager.BookService.UpdateOneBook(id, new BookDtoForUpdate()
        {
            Id = book.Id,
            Price = book.Price,
            Title = book.Title
        }, true);
        
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteBook([FromRoute(Name="id")] int id)
    {
        _manager.BookService.DeleteOneBook(id,true);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")]int id, [FromBody]JsonPatchDocument<Book> bookPatch)
    {
        var entity = _manager.BookService.GetOneBookById(id, true);
        bookPatch.ApplyTo(entity);
        _manager.BookService.UpdateOneBook(id,new BookDtoForUpdate()
        {
            Id = entity.Id,
            Price = entity.Price,
            Title = entity.Title
        },true);
        return NoContent();
    }
}