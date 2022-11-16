namespace Data;

public class Beer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int StyleId { get; set; }
    public BeerStyle Style { get; set; }
    public Brewery Brewery { get; set; }
}
