namespace MinimalApi.Api.Beers.Get;

public static class Endpoint
{
    public static RouteHandlerBuilder MapBeersGet(this RouteGroupBuilder group)
        => group.MapGet("/beers/{id}", Delegate)
                .Produces<BeerDetail>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetBeer");

    private static async Task<IResult> Delegate(BeerDbContext db, HashedId id, CancellationToken cancellationToken)
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
