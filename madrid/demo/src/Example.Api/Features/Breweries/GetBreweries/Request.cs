namespace Example.Api.Features.GetBreweries;

public record Request(
    [FromServices] BeerDbContext Database,
    [FromQuery] int Page = 1,
    [FromQuery] int PageSize = 10
);
