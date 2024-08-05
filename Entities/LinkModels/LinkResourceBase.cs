namespace Entities.LinkModels;

public class LinkResourceBase
{
    // birden fazla linki buradan kontrol edecegiz
    public LinkResourceBase()
    {
        
    }

    public List<Link> Links { get; set; } = new List<Link>();
}

