namespace Example.Api.Features.DeleteBrewery;

public record Request(
    [FromServices] BeerDbContext Database,
    [FromRoute] HashedId Id
);
