namespace MinimalApi.Api.Beers.Upsert;

public static class Endpoint
{
    public static IEnumerable<RouteHandlerBuilder> MapBeersUpsert(this RouteGroupBuilder group)
    {
        yield return group.MapPost("/beers/", PostDelegate)
                          .Produces<BeerDetail>(StatusCodes.Status201Created)
                          .Produces(StatusCodes.Status204NoContent)
                          .Produces(StatusCodes.Status400BadRequest)
                          .WithName("CreateBeer")
                          .WithValidation();

        yield return group.MapPut("/beers/{id}", PutDelegate)
                          .Produces<BeerDetail>(StatusCodes.Status200OK)
                          .Produces(StatusCodes.Status204NoContent)
                          .Produces(StatusCodes.Status400BadRequest)
                          .WithName("UpsertBeer")
                          .WithValidation();
    }

    private static Task<IResult> PostDelegate(BeerRequest input, BeerDbContext db, CancellationToken cancellationToken)
        => UpsertAsync(db, input, forceCreation: true, cancellationToken: cancellationToken);

    private static Task<IResult> PutDelegate(HashedId id, BeerRequest input, BeerDbContext db, CancellationToken cancellationToken)
        => UpsertAsync(db, input, id, cancellationToken: cancellationToken);

    private static async Task<IResult> UpsertAsync(BeerDbContext db, BeerRequest input, int? id = null, bool forceCreation = false, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var brewery = await db.Breweries.FindAsync(new object[] { (int)input.BreweryId }, cancellationToken);
        if (brewery is null)
        {
            return Results.BadRequest("Unkown Brewery");
        }

        var style = await db.Styles.FindAsync(new object[] { (int)input.StyleId }, cancellationToken);
        if (style is null)
        {
            return Results.BadRequest("Invalid beer style");
        }


        var beer = (id.HasValue && !forceCreation) ? await db.Beers.FindAsync(new object[] { id }, cancellationToken) : null;
        if (beer is null)
        {
            beer = new Beer();
            if (id.HasValue)
            {
                beer.Id = id.Value;
            }

            db.Beers.Add(beer);
        }

        beer.Name = input.Name;
        beer.Brewery = brewery;
        beer.Style = style;

        await db.SaveChangesAsync(cancellationToken);

        var resource = (BeerDetail)beer;
        if (id.HasValue)
        {
            return Results.Ok(resource);
        }
        else
        {
            return Results.CreatedAtRoute("GetBeer", new { id = resource.Id.ToString() }, resource);
        }
    }
}
