using System.Dynamic;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
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
    private readonly IBookLinks _bookLinks;


    public BookManager(
        IRepositoryManager manager,
        ILoggerService logger,
        IMapper mapper, IBookLinks bookLinks)
    {
        _manager = manager;
        _logger = logger;
        _mapper = mapper;
        _bookLinks = bookLinks;
    }

    public async Task<(LinkResponse linkResponse, MetaData metaData)> GetAllBooksAsync(
        LinkParameters linkParameters, bool trackChanges)
    {
        if (!linkParameters.BookParameters.ValidPriceRange)
            throw new PriceOutOfRangeBadRequestException();
            
        var booksWithMetaData = await _manager.BookRepo.GetAllBooksAsync(linkParameters.BookParameters, trackChanges);
        var booksDto = _mapper.Map<IEnumerable<BookDto>>(booksWithMetaData);
        // var shapedData = _shaper.ShapeData(booksDto, linkParameters.BookParameters.Fields);
        var links = _bookLinks.TryGenerateLinks(booksDto, linkParameters.BookParameters.Fields,linkParameters.HttpContext);
        return (LinkResponse: links, metaData: booksWithMetaData.MetaData);
    }

    public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
    {
        var book = await GetBookByIdAndCheckExist(id,trackChanges);

        return _mapper.Map<BookDto>(book);
    }

    public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDtoForInsertion)
    {
        if (bookDtoForInsertion is null)
            throw new ArgumentNullException();

        var book = _mapper.Map<Book>(bookDtoForInsertion);
        
        var entity = _manager.BookRepo.CreateBook(book);
        await _manager.SaveAsync();
        return _mapper.Map<BookDto>(entity);
    }

    public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges)
    {
        if (id != bookDto.Id)
            throw new Exception($"BookId and id is not same.");

        Book entity;
        await GetBookByIdAndCheckExist(id,trackChanges);

        //Mapping
        // entity.Title = book.Title;
        // entity.Price = book.Price;
        entity = _mapper.Map<Book>(bookDto);
        
        _manager.BookRepo.UpdateBook(entity); //aslinda izlenen nesne direkt save de edileiblir burasi gereksiz gibi
        await _manager.SaveAsync();
    }

    public async Task DeleteOneBookAsync(int id, bool trackChanges)
    {
        var entity = await GetBookByIdAndCheckExist(id,trackChanges);
        
        _manager.BookRepo.DeleteBook(entity);
        await _manager.SaveAsync();
    }

    public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
    {
        var book = await GetBookByIdAndCheckExist(id,trackChanges);

        var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);

        return (bookDtoForUpdate, book);
    }

    public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
    {
        _mapper.Map(bookDtoForUpdate, book);
        await _manager.SaveAsync();
    }

    private async Task<Book> GetBookByIdAndCheckExist(int id, bool trackChanges)
    {
        var book = await _manager.BookRepo.GetBookByIdAsync(id, trackChanges);
        
        if (book is null)
            throw new BookNotFoundException(id);

        return book;
    }
}

