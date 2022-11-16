namespace MinimalApi.Api.Beers.Upsert;

public class BeerRequest
{
    [Required, StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }

    [Required]
    public HashedId BreweryId { get; set; }

    [Required]
    public HashedId StyleId { get; set; }
}
