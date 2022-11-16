namespace Example.Api.Features.GetBrewery;

public record Request(
    [FromServices] BeerDbContext Database,
    [FromServices] ILogger<Handler> Logger,
    [FromRoute] HashedId Id
);
