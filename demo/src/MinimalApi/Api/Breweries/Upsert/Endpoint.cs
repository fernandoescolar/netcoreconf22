namespace MinimalApi.Api.Breweries.Upsert;

public static class Endpoint
{
    public static IEnumerable<RouteHandlerBuilder> MapBreweriesUpsert(this RouteGroupBuilder group)
    {
        yield return group.MapPost("/breweries/", PostDelegate)
                          .Produces<BeerDetail>(StatusCodes.Status201Created)
                          .Produces(StatusCodes.Status204NoContent)
                          .Produces(StatusCodes.Status400BadRequest)
                          .WithName("CreateBrewery")
                          .WithValidation();

        yield return group.MapPut("/breweries/{id}", PutDelegate)
                          .Produces<BeerDetail>(StatusCodes.Status200OK)
                          .Produces(StatusCodes.Status204NoContent)
                          .Produces(StatusCodes.Status400BadRequest)
                          .WithName("UpsertBrewery")
                          .WithValidation();
    }

    private static Task<IResult> PostDelegate(BreweryRequest input, BeerDbContext db, CancellationToken cancellationToken)
        => UpsertAsync(db, input, forceCreation: true, cancellationToken: cancellationToken);

    private static Task<IResult> PutDelegate(HashedId id, BreweryRequest input, BeerDbContext db, CancellationToken cancellationToken)
        => UpsertAsync(db, input, id, cancellationToken: cancellationToken);

    private static async Task<IResult> UpsertAsync(BeerDbContext db, BreweryRequest input, int? id = null, bool forceCreation = false, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var brewery = (id.HasValue && !forceCreation) ? await db.Breweries.FindAsync(new object[] { id }, cancellationToken) : null;
        if (brewery is null)
        {
            brewery = new Brewery();
            if (id.HasValue)
            {
                brewery.Id = id.Value;
            }

            db.Breweries.Add(brewery);
        }

        brewery.Name = input.Name;
        brewery.City = input.City;
        brewery.Country = input.Country;

        await db.SaveChangesAsync(cancellationToken);

        var resource = (BreweryDetail)brewery;
        if (id.HasValue)
        {
            return Results.Ok(resource);
        }
        else
        {
            return Results.CreatedAtRoute("GetBrewery", new { id = resource.Id.ToString() }, resource);
        }
    }
}
