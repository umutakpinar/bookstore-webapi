using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories.EFCore;

public class BookRepository : RepositoryBase<Book>, IBookRepository
{
    public BookRepository(RepositoryContext context) : base(context)
    {
    }
    
    // bunlar IBookRepository interfacesinden gelen metotlar
    public async Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges) 
    {
        var books =  await FindAll(trackChanges)
            .OrderBy(b => b.Id)
            .ToListAsync();
        return PagedList<Book>.ToPagedList(
                source: books,
                pageNumber: bookParameters.PageNumber,
                pageSize: bookParameters.PageSize
            );
    }

    // Ornegin ana RepositoryBase yapisisindan burada kopuyor o nedenle IBookRepository icinde de eklemeli bu sekilde
    public async Task<Book> GetBookByIdAsync(int id, bool trackChanges) 
    {
        return await FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
    }

    // ornegin createbook islemi temel CRUD ile repositoryBase ile aynı oradaki Create'e de erisimim var zaten 
    // RepositoryBase abstract classını implement ettigim icin oradaki create'i aldim.
    public Book CreateBook(Book book) 
    {
        return Create(book);
    }

    public  void UpdateBook(Book book) 
    {
        Update(book);
    }

    public void DeleteBook(Book book)  
    {
        Delete(book);
    }
}