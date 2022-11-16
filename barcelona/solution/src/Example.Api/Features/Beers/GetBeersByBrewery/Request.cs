namespace Example.Api.Features.GetBeersByBrewery;

public record Request(
    [FromRoute(Name = "id")] HashedId BreweryId,
    [FromServices] BeerDbContext Database,
    [FromQuery] int Page = 1,
    [FromQuery] int PageSize = 10
);
