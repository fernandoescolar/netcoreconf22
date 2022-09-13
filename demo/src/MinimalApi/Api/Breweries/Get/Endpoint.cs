namespace MinimalApi.Api.Breweries.Get;

public static class Endpoint
{
    public static RouteHandlerBuilder MapBreweriesGet(this RouteGroupBuilder group)
        => group.MapGet("/breweries/{id}", Delegate)
                .Produces<BeerDetail>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetBrewery");

    private static async Task<IResult> Delegate(BeerDbContext db, HashedId id, CancellationToken cancellationToken)
    {
        var brewery = await db.Breweries.FindAsync(new object[] { (int)id }, cancellationToken);
        if (brewery is null)
        {
            return Results.NotFound();
        }

        var resource = (BreweryDetail)brewery;

        return Results.Ok(resource);
    }
}
