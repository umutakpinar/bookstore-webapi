using System.Collections;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;

namespace Services.Contracts;

public interface IBookService
{
    Task<(IEnumerable<BookDto> books,MetaData metaData)> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges);
    Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges);
    Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDtoForInsertion);
    Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDtoForUpdate, bool trackChanges);
    Task DeleteOneBookAsync(int id, bool trackChanges);
    Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges);
    Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book);
}