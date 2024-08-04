using System.Collections;
using Entities.DataTransferObjects;
using Entities.Models;

namespace Services.Contracts;

public interface IBookService
{
    IEnumerable<BookDto> GetAllBooks(bool trackChanges);
    BookDto GetOneBookById(int id, bool trackChanges);
    BookDto CreateOneBook(BookDtoForInsertion bookDtoForInsertion);
    void UpdateOneBook(int id, BookDtoForUpdate bookDtoForUpdate, bool trackChanges);
    void DeleteOneBook(int id, bool trackChanges);
    (BookDtoForUpdate bookDtoForUpdate, Book book) GetOneBookForPatch(int id, bool trackChanges);
    void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book);
}