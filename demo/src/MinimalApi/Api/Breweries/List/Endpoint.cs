namespace MinimalApi.Api.Breweries.List;

public static class Endpoint
{
    public static RouteHandlerBuilder MapBreweriesList(this RouteGroupBuilder group)
        => group.MapGet("/breweries/", Delegate)
                .Produces<BreweryList>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent)
                .WithName("GetBreweries");

    private static async Task<IResult> Delegate(BeerDbContext _db, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var total = await _db.Breweries.CountAsync(cancellationToken);
        if (total == 0)
        {
            return Results.NoContent();
        }

        var breweries = await _db.Breweries
                             .Select(b => new BreweryListItem
                                 {
                                     Id = b.Id,
                                     Name = b.Name,
                                     City = b.City,
                                     Country = b.Country,
                                 })
                             .OrderBy(b => b.Name)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .ToListAsync(cancellationToken);

        var resourceList = new BreweryList(breweries, total, page, pageSize);

        return Results.Ok(resourceList);
    }
}
