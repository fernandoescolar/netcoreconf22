namespace MvcApi.Model;

public class BeerDetail
{
    public HashedId Id { get; set; }
    public string Name { get; set; }

    public BeerStyleDetail Style { get; set; }
    public BreweryDetail Brewery { get; set; }

    public static implicit operator BeerDetail(Beer beer)
        => new BeerDetail
        {
            Id = beer.Id,
            Name = beer.Name,
            Style = beer.Style,
            Brewery = beer.Brewery
        };
}
