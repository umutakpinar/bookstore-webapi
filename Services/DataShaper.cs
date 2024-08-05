using System.Dynamic;
using System.Reflection;
using Entities.Models;
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

    public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldString)
    {
        var requiredProperties = GetRequiredProperties(fieldString);
        return FetchData(entities, requiredProperties);
    }

    public ShapedEntity ShapeData(T entity, string fieldString)
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

    private ShapedEntity FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedObject = new ShapedEntity(); // runtime'da yeni bir obje olusturduk

        foreach (var property in requiredProperties)
        {
            var objectPropertyValue = property.GetValue(entity);
            shapedObject.Entity.TryAdd(property.Name, objectPropertyValue); // ilgili property'nin degerini atadik
        }

        var objectProperty = entity.GetType().GetProperty("Id");
        shapedObject.Id = (int)objectProperty.GetValue(entity);
        
        return shapedObject;
    }

    private IEnumerable<ShapedEntity> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedData = new List<ShapedEntity>();
        foreach (var entity in entities)
        {
            ShapedEntity shapedObject = FetchDataForEntity(entity, requiredProperties);
            shapedData.Add(shapedObject);
        }

        return shapedData;
    }
}