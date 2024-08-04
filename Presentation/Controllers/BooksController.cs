using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
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
    public async Task<IActionResult> GetAllBooksAsync()
    {
        var books = await _manager.BookService.GetAllBooksAsync(false);
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")] int id)
    {
            var book = await _manager.BookService.GetOneBookByIdAsync(id,false);
            if (book is null)
                throw new BookNotFoundException(id);
            return Ok(book);
    }
    
    [HttpPost]
    [ServiceFilter(typeof(IsModelStateNotValid))]
    public async Task<IActionResult> CreateOneBookAsync([FromBody] BookDtoForInsertion bookDtoForInsertion)
    {
        // if (!ModelState.IsValid)
        //     return UnprocessableEntity(ModelState);
        
        var addedEntity = await _manager.BookService.CreateOneBookAsync(bookDtoForInsertion);
        return StatusCode(201,addedEntity);
    }

    [HttpPut("{id:int}")]
    [ServiceFilter(typeof(IsModelStateNotValid))]
    public async Task<IActionResult> UpdateBookAsync([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate bookDto)
    {
        await _manager.BookService.UpdateOneBookAsync(id, bookDto, true);
        
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBook([FromRoute(Name="id")] int id)
    {
        await _manager.BookService.DeleteOneBookAsync(id,true);
        return NoContent();
    }

    
    [HttpPatch("{id:int}")]
    [ServiceFilter(typeof(IsModelStateNotValid))]
    public async Task<IActionResult> PartiallyUpdateOneBookAsync([FromRoute(Name = "id")]int id, [FromBody]JsonPatchDocument<BookDtoForUpdate> bookDtoPatch)
    {
        var result = await _manager.BookService.GetOneBookForPatchAsync(id, true);
        bookDtoPatch.ApplyTo(result.bookDtoForUpdate, ModelState);
        
        TryValidateModel(result.bookDtoForUpdate);
        
        // if (!ModelState.IsValid)
        //     return UnprocessableEntity(ModelState); //422
        
        // _manager.BookService.UpdateOneBook(id,new BookDtoForUpdate()
        // {
        //     Id = result.bookDtoForUpdate.Id,
        //     Price = result.bookDtoForUpdate.Price,
        //     Title = result.bookDtoForUpdate.Title
        // },true);
        await _manager.BookService.SaveChangesForPatchAsync(bookDtoForUpdate: result.bookDtoForUpdate, result.book);
        return NoContent();
    }
}