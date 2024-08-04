using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using NLog;
using Repositories.Contracts;
using Services.Contracts;
using Exception = System.Exception;

namespace Services;

public class BookManager : IBookService
{
    private readonly IRepositoryManager _manager;
    private readonly ILoggerService _logger;
    private readonly IMapper _mapper;

    public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
    {
        _manager = manager;
        _logger = logger;
        _mapper = mapper;
    }

    public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
    {
        var books = _manager.BookRepo.GetAllBooks(trackChanges);
        var x = _mapper.Map<IEnumerable<BookDto>>(books);
        return x;
    }

    public Book GetOneBookById(int id, bool trackChanges)
    {
        var book = _manager.BookRepo.GetBookById(id, trackChanges);

        if (book is null)
            throw new BookNotFoundException(id);
        
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

    public void UpdateOneBook(int id, BookDtoForUpdate bookDto, bool trackChanges)
    {
        if (id != bookDto.Id)
            throw new Exception($"BookId and id is not same.");
        
        var entity = _manager.BookRepo.GetBookById(id, false);

        if (entity is null)
            throw new BookNotFoundException(id);

        //Mapping
        // entity.Title = book.Title;
        // entity.Price = book.Price;
        entity = _mapper.Map<Book>(bookDto);
        
        _manager.BookRepo.UpdateBook(entity); //aslinda izlenen nesne direkt save de edileiblir burasi gereksiz gibi
        _manager.Save();
    }

    public void DeleteOneBook(int id, bool trackChanges)
    {
        var entity = _manager.BookRepo.GetBookById(id, trackChanges);

        if (entity is null)
            throw new BookNotFoundException(id);
        
        _manager.BookRepo.DeleteBook(entity);
        _manager.Save();
    }
}