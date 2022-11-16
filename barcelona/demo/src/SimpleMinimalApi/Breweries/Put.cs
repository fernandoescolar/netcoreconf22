namespace SimpleMinimalApi.Breweries;

public record Put() : AbstractUpsert(EndpointVerb.Put, "/breweries/{id}")
{
    public override void Configure(RouteHandlerBuilder builder)
        => builder
                .Produces<BreweryDetail>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("UpsertBrewery")
                .WithTags("Breweries")
                .WithValidation();

    public Task<IResult> HandleAsync(HashedId id, BreweryRequest input, BeerDbContext db, CancellationToken cancellationToken)
        => UpsertAsync(db, input, id, cancellationToken: cancellationToken);
}
