namespace MvcApi.Model;

public class BeerStyleDetail
{
    public HashedId Id { get; set; }
    public string Name { get; set; }

    public static implicit operator BeerStyleDetail(BeerStyle style)
        => new BeerStyleDetail
        {
            Id = style.Id,
            Name = style.Name
        };
}
