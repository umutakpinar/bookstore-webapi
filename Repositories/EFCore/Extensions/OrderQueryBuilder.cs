using System.Reflection;
using System.Text;

namespace Repositories.EFCore.Extensions;

public static class OrderQueryBuilder
{
    public static String? CreateOrderQuery<T>(String orderByQueryString)
    {
        var orderParams = orderByQueryString.Trim().Split(",").ToList();
        var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance); 
        
        var orderQueryBuilder = new StringBuilder();
        
        foreach (var orderParam in orderParams)
        {
            
            if (string.IsNullOrWhiteSpace(orderParam))
                continue;
            
            var propertyFromQueryName = orderParam.Split(' ')[0];
            
            var objectProperty = propertyInfos.FirstOrDefault(propertyInfo =>
                propertyInfo.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

            if(objectProperty is null)
                continue;

            var direction = orderParam.EndsWith(" desc") ? "descending" : "ascending";
            orderQueryBuilder.Append($"{objectProperty.Name} {direction}");

            
        }
        
        var orderQuery = orderQueryBuilder.ToString()?.TrimEnd(',', ' ');

        return orderQuery;
       
        
    }
}