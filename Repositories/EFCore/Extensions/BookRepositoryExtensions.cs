using Entities.Models;
using Repositories.Contracts;

namespace Repositories.EFCore.Extensions;

public static class BookRepositoryExtensions
{
    public static IQueryable<Book> FilterBookPrice(this IQueryable<Book> books, uint minPrice, uint maxPrice)
    {
        return books.Where(b => b.Price >= minPrice && b.Price <= maxPrice);
    }
}