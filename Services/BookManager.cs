using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using Exception = System.Exception;

namespace Services;

public class BookManager : IBookService
{
    private IRepositoryManager _manager;

    public BookManager(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public IEnumerable<Book> GetAllBooks(bool trackChanges)
    {
        var books = _manager.BookRepo.GetAllBooks(trackChanges);
        return books;
    }

    public Book GetOneBookById(int id, bool trackChanges)
    {
        var book = _manager.BookRepo.GetBookById(id, trackChanges);
        
        if (book is null)
            throw new Exception($"There is no book with this id {id}");

        return book;
    }

    public Book CreateOneBook(Book? book)
    {
        if (book is null)
            throw new ArgumentNullException();
        
        var entity = _manager.BookRepo.CreateBook(book);
        _manager.Save();
        return entity;
    }

    public void UpdateOneBook(int id, Book book, bool trackChanges)
    {
        if (id != book.Id)
            throw new Exception($"BookId and id is not same.");
        
        var entity = _manager.BookRepo.GetBookById(id, trackChanges);

        if (entity is null)
            throw new Exception($"There is no book with this id {id}");

        entity.Title = book.Title;
        entity.Price = book.Price;
        
        _manager.BookRepo.UpdateBook(entity); //aslinda izlenen nesne direkt save de edileiblir burasi gereksiz gibi
        _manager.Save();
    }

    public void DeleteOneBook(int id, bool trackChanges)
    {
        var entity = _manager.BookRepo.GetBookById(id, trackChanges);

        if (entity is null)
            throw new Exception($"There is no book with this id {id}");
        
        _manager.BookRepo.DeleteBook(entity);
        _manager.Save();
    }
}