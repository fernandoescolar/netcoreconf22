namespace SimpleMinimalApi.Beers;

public record Put() : AbstractUpsert(EndpointVerb.Put, "/beers/{id}")
{
    public override void Configure(RouteHandlerBuilder builder)
        => builder
                .Produces<BeerDetail>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("UpsertBeer")
                .WithTags("Beers")
                .WithValidation();

    public Task<IResult> HandleAsync(HashedId id, BeerRequest input, BeerDbContext db, CancellationToken cancellationToken)
        => UpsertAsync(db, input, id, cancellationToken: cancellationToken);
}
