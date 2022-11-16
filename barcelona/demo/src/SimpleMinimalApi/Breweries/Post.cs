namespace SimpleMinimalApi.Breweries;

public record Post() : AbstractUpsert(EndpointVerb.Post, "/breweries")
{
    public override void Configure(RouteHandlerBuilder builder)
        => builder
                .Produces<BreweryDetail>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("CreateBrewery")
                .WithTags("Breweries")
                .WithValidation();

    public Task<IResult> HandleAsync(BreweryRequest input, BeerDbContext db, CancellationToken cancellationToken)
        => UpsertAsync(db, input, forceCreation: true, cancellationToken: cancellationToken);
}
