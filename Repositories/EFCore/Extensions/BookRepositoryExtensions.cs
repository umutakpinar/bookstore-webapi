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
        // books=orderBy=title asc,price, id mantik bu sekilde bir sey yazmas ise asc kabul ettim
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return books.OrderBy(b => b.Id);
        var orderParams = orderByQueryString.Trim().Split(",").ToList();
        
        // Book classinin propertylerini aldik reflection ile sanirim bu yapi dynamic programming
        var propertyInfos = typeof(Book).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var orderQueryBuilder = new StringBuilder();
        // burada id title price sirali gelmez veya hic gelmeyebilir. Bu nedenle bunlari dynamic olarak eklemeli
        foreach (var orderParam in orderParams)
        {
            //gelen bos ise yani order yok ise
            if (string.IsNullOrWhiteSpace(orderParam))
                continue;
            // ama var ise asc mi desc mi jonu bulacagiz or price asc dedi bosluga gore ikiye boluyoruz ilk parca price mi title mi id mi onu verecek
            var propertyFromQueryName = orderParam.Split(' ')[0];
            
            //burada
            var objectProperty = propertyInfos.FirstOrDefault(propertyInfo =>
                propertyInfo.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

            //demek ki boyle bir property yok kafasina gore bir deger girmis
            if(objectProperty is null)
                continue;

            var direction = orderParam.EndsWith(" desc") ? "descending" : "ascending";
            orderQueryBuilder.Append($"{objectProperty.Name} {direction}"); // or: title descending, price ascending,

            
        }
        
        var orderQuery = orderQueryBuilder.ToString()?.TrimEnd(',', ' ');
        
        if (orderQuery is null)
            return books.OrderBy(b => b.Id);

        return books.OrderBy(orderQuery);
    }
}