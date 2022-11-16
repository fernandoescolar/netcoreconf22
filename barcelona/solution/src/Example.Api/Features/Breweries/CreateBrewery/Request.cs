namespace Example.Api.Features.CreateBrewery;

public record Request(
    [FromServices] BeerDbContext Database,
    [FromBody] RequestBody Body
);

public class RequestBody
{
    [Required, StringLength(150, MinimumLength = 3)]
    public string Name { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string Country { get; set; }
}