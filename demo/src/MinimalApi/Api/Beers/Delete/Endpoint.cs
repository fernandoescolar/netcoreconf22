using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.Api.Beers.Delete;

public static class Endpoint
{
    public static RouteHandlerBuilder MapBeersDelete(this RouteGroupBuilder group)
        => group.MapDelete("/beers/{id}", Delegate)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("DeleteBeer");

    private static async Task<IResult> Delegate(BeerDbContext db, HashedId id, CancellationToken cancellationToken)
    {
        var beer = await db.Beers.FindAsync(new object[] { (int)id }, cancellationToken);
        if (beer is null)
        {
            return Results.NotFound();
        }

        db.Beers.Remove(beer);
        await db.SaveChangesAsync(cancellationToken);

        return Results.NoContent();
    }
}
