namespace Example.Api.Features.GetBeers;

public record Request(
    [FromServices] BeerDbContext Database,
    [FromQuery] int Page = 1,
    [FromQuery] int PageSize = 10
);
