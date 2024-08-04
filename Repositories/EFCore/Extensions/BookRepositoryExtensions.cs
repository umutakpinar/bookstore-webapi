using System.Reflection;
using System.Text;
using Entities.Models;
using System.Linq.Dynamic.Core;

namespace Repositories.EFCore.Extensions;

public static class BookRepositoryExtensions
{
    public static IQueryable<Book> FilterBookPrice(this IQueryable<Book> books, uint minPrice, uint maxPrice)
    {
        return books.Where(b => b.Price >= minPrice && b.Price <= maxPrice);
    }

    public static IQueryable<Book> SearchBookWithTitle(this IQueryable<Book> books, string? searchTerm)
    {
        return searchTerm is not null ? books.Where(b => b.Title.ToLower().Contains(searchTerm.ToLower())) : books;
    }

    public static IQueryable<Book> OrderByQueryString(this IQueryable<Book> books, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return books.OrderBy(b => b.Id);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Book>(orderByQueryString);
        
        if (orderQuery is null)
            return books.OrderBy(b => b.Id);

        return books.OrderBy(orderQuery);
    }
}