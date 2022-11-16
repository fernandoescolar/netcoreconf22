namespace MinimalApi.Api.Breweries.Delete;

public static class Endpoint
{
    public static RouteHandlerBuilder MapBreweriesDelete(this RouteGroupBuilder group)
        => group.MapDelete("/breweries/{id}", Delegate)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("DeleteBrewery");

    private static async Task<IResult> Delegate(BeerDbContext db, HashedId id, CancellationToken cancellationToken)
    {
        var brewery = await db.Breweries.FindAsync(new object[] { (int)id }, cancellationToken);
        if (brewery is null)
        {
            return Results.NotFound();
        }

        db.Breweries.Remove(brewery);
        await db.SaveChangesAsync(cancellationToken);

        return Results.NoContent();
    }
}
