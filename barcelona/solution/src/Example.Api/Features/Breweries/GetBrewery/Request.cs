namespace Example.Api.Features.GetBrewery;

public record Request(
    [FromServices] BeerDbContext Database,
    [FromRoute] HashedId Id
);
