using System.Dynamic;
using System.Reflection;
using Services.Contracts;

namespace Services;

public class DataShaper<T> : IDataShaper<T> 
    where T : class
{
    public PropertyInfo[] Properties { get; set; }
    
    public DataShaper()
    {
        Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldString)
    {
        var requiredProperties = GetRequiredProperties(fieldString);
        return FetchData(entities, requiredProperties);
    }

    public ExpandoObject ShapeData(T entity, string fieldString)
    {
        var requiredProperties = GetRequiredProperties(fieldString);
        return FetchDataForEntity(entity, requiredProperties);
    }
    
    // books?fields=id, title istenmis olabilir
    private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldString)
    {
        var requiredFields = new List<PropertyInfo>();
        if (!string.IsNullOrWhiteSpace(fieldString))
        {
            var fields = fieldString.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries); // ["iD","tItLe"]

            foreach (var field in fields)
            {
                var property = Properties.FirstOrDefault(pi =>
                    pi.Name.Equals(field.Trim(),StringComparison.InvariantCultureIgnoreCase));

                if (property is null)
                    continue;
                
                requiredFields.Add(property);
            }
        }
        else
        {
            requiredFields = Properties.ToList();
        }

        return requiredFields;
    }

    private ExpandoObject FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedObject = new ExpandoObject(); // runtime'da yeni bir obje olusturduk

        foreach (var property in requiredProperties)
        {
            var objectPropertyValue = property.GetValue(entity);
            shapedObject.TryAdd(property.Name, objectPropertyValue); // ilgili property'nin degerini atadik
        }

        return shapedObject;
    }

    private IEnumerable<ExpandoObject> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedData = new List<ExpandoObject>();
        foreach (var entity in entities)
        {
            ExpandoObject shapedObject = FetchDataForEntity(entity, requiredProperties);
            shapedData.Add(shapedObject);
        }

        return shapedData;
    }
}