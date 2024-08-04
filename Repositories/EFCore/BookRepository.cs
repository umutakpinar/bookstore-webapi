using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories.EFCore;

public class BookRepository : RepositoryBase<Book>, IBookRepository
{
    public BookRepository(RepositoryContext context) : base(context)
    {
    }
    
    // bunlar IBookRepository interfacesinden gelen metotlar
    public async Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges) 
    {
        return await FindAll(trackChanges).OrderBy(b => b.Id).ToListAsync();
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