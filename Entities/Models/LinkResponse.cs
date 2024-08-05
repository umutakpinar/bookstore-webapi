using Entities.LinkModels;

namespace Entities.Models;

public class LinkResponse
{
    public bool HasLinks { get; set; }
    
    public List<Entity> ShapedEntities { get; set; }
    public LinkCollectionWrapper<Entity>  LinkCollectionWrapper { get; set; }

    public LinkResponse()
    {
        LinkCollectionWrapper = new LinkCollectionWrapper<Entity>();
        ShapedEntities = new List<Entity>();
    }
}