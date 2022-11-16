namespace Example.Api.Features.GetBreweryDescription;

public record Request(
    [FromServices] BeerDbContext Database,
    [FromRoute] HashedId Id
);
