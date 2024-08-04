using Entities.Models;
using Entities.RequestFeatures;

namespace Repositories.Contracts;

public interface IBookRepository : IRepositoryBase<Book>
{
    // IRepositoryBase'de asagidaki ifadeler zaten vardi ancak olur da CRUD islemleri farkli olursa diye ekledim yine de
    Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters,bool trackChanges);
    Task<Book> GetBookByIdAsync(int id, bool trackChanges);
    Book CreateBook(Book book);
    void UpdateBook(Book book);
    void DeleteBook(Book book);
}