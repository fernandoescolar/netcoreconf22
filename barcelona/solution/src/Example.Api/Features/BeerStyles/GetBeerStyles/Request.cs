namespace Example.Api.Features.GetBeerStyles;

public record Request(
    [FromServices] BeerDbContext Database
);
