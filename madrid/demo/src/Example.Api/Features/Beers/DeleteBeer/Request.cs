namespace Example.Api.Features.DeleteBeer;

public record Request(
    [FromServices] BeerDbContext Database,
    [FromRoute] HashedId Id
);
