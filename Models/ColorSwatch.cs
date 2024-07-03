namespace bakery.Models;
public class ColorSwatch
{
    public string PageKey { get; set; }
    public string ImageUrl { get; set; }
    public string ImageValue { get; set; }
}

public class CatalogKeybase
{
    public string Language { get; set; }
    public string Country { get; set; }
    public int CatalogId { get; set; }
    public override string ToString() => $"Language:{Language},Country:{Country},CatalogId:{CatalogId}";
}

