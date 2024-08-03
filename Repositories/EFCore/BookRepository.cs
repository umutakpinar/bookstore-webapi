using Entities.Models;
using Repositories.Contracts;

namespace Repositories.EFCore;

public class BookRepository : RepositoryBase<Book>, IBookRepository
{
    public BookRepository(RepositoryContext context) : base(context)
    {
    }
    
    // bunlar IBookRepository interfacesinden gelen metotlar
    public IQueryable<Book> GetAllBooks(bool trackChanges) => 
        FindAll(trackChanges).OrderBy(b => b.Id);
    
    // Ornegin ana RepositoryBase yapisisindan burada kopuyor o nedenle IBookRepository icinde de eklemeli bu sekilde
    public Book? GetBookById(int id, bool trackChanges) =>
        FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefault();

    // ornegin createbook islemi temel CRUD ile repositoryBase ile aynı oradaki Create'e de erisimim var zaten 
    // RepositoryBase abstract classını implement ettigim icin oradaki create'i aldim.
    public Book CreateBook(Book book) => Create(book);

    public void UpdateBook(Book book) => Update(book);

    public void DeleteBook(Book book) => Delete(book);
}