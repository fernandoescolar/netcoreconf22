namespace SimpleMinimalApi.Beers;

public record ListByBrewery() : GetEndpoint("/breweries/{id}/beers")
{
    public override void Configure(RouteHandlerBuilder builder)
        => builder
                .Produces<BeerList>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetBeersByBrewery")
                .WithTags("Beers");

    public async Task<IResult> HandleAsync(BeerDbContext db, HashedId id, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var brewery = await db.Breweries.FindAsync(new object[] { (int)id }, cancellationToken);
        if (brewery is null)
        {
            return Results.NotFound();
        }

        var total = await db.Beers.CountAsync(b => b.Brewery.Id == (int)id, cancellationToken);
        if (total == 0)
        {
            return Results.NoContent();
        }

        var beers = await db.Beers
                        .Where(b => b.Brewery.Id == (int)id)
                        .Select(b => new BeerListItem
                        {
                            Id = b.Id,
                            Name = b.Name,
                            BreweryId = b.Brewery.Id,
                            BreweryName = b.Brewery.Name,
                            StyleId = b.Style.Id,
                            StyleName = b.Style.Name,
                        })
                        .OrderBy(b => b.Name)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync(cancellationToken);

        var resourceList = new BeerList(beers, total, page, pageSize);

        return Results.Ok(resourceList);
    }
}
