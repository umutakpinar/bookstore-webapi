using Entities.Models;

namespace Repositories.Contracts;

public interface IBookRepository : IRepositoryBase<Book>
{
    // IRepositoryBase'de asagidaki ifadeler zaten vardi ancak olur da CRUD islemleri farkli olursa diye ekledim yine de
    IQueryable<Book> GetAllBooks(bool trackChanges);
    Book? GetBookById(int id, bool trackChanges);
    Book CreateBook(Book book);
    void UpdateBook(Book book);
    void DeleteBook(Book book);
}