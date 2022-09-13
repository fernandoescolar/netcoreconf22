namespace SimpleMinimalApi.Beers;

public record Post() : AbstractUpsert(EndpointVerb.Post, "/beers")
{
    public override void Configure(RouteHandlerBuilder builder)
        => builder
                .Produces<BeerDetail>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("CreateBeer")
                .WithTags("Beers")
                .WithValidation();

    public Task<IResult> HandleAsync(BeerRequest input, BeerDbContext db, CancellationToken cancellationToken)
        => UpsertAsync(db, input, forceCreation: true, cancellationToken: cancellationToken);
}
