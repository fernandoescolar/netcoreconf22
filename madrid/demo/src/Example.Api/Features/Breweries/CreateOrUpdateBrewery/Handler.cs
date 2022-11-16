namespace Example.Api.Features.CreateOrUpdateBrewery;

public record Handler() : PutEndpoint<Request>("/breweries/{id}")
{
    protected override void OnConfigure(RouteHandlerBuilder builder)
        => builder
                .ProducesHypermedia<Response>(StatusCodes.Status200OK)
                .Produces<Response>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("CreateOrUpdateBrewery")
                .WithTags("Beers")
                .WithValidation();

    protected override async Task<IResult> OnHandleAsync(Request req, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var brewery = await req.Database.Breweries.FindAsync(new object[] { (int)req.Id }, cancellationToken);
        if (brewery is null)
        {
            brewery = new Brewery {
                Id = (int)req.Id
            };

            req.Database.Breweries.Add(brewery);
        }

        brewery.Name = req.Body.Name;
        brewery.City = req.Body.City;
        brewery.Country = req.Body.Country;

        await req.Database.SaveChangesAsync(cancellationToken);

        var resource = (Response)brewery;
        return Results.Ok(resource);
    }
}
