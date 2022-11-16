namespace SimpleMinimalApi.Beers;

public record Get() : GetEndpoint("/beers/{id}")
{
    public override void Configure(RouteHandlerBuilder builder)
        => builder
                .Produces<BeerDetail>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetBeer")
                .WithTags("Beers");

    public async Task<IResult> HandleAsync(BeerDbContext db, HashedId id, CancellationToken cancellationToken)
    {
        var beer = await db.Beers
                            .Include(nameof(Beer.Brewery))
                            .Include(nameof(Beer.Style))
                            .FirstOrDefaultAsync(b => b.Id == (int)id, cancellationToken);

        if (beer is null)
        {
            return Results.NotFound();
        }

        var resource = (BeerDetail)beer;

        return Results.Ok(resource);
    }
}
