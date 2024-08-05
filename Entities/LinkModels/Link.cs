namespace Entities.LinkModels;

public class Link
{
    //nereye referans verecek - hypereference
    public String? Href { get; set; }
    // hangi islemle baglantili - relate
    public String? Rel { get; set; }
    public String? Method { get; set; }

    public Link()
    {
        
    }
    public Link(string? href, string? rel, string? method)
    {
        Href = href;
        Rel = rel;
        Method = method;
    }
}