namespace Example.Api.Features.CreateBeer;

public record Request(
    [FromServices] BeerDbContext Database,
    [FromBody] RequestBody Body
);

public class RequestBody
{
    [Required, StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }

    [Required]
    public HashedId BreweryId { get; set; }

    [Required]
    public HashedId StyleId { get; set; }
}