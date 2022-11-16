namespace Example.Api.Features.GetBrewery;

// ISSUE: too many logs
public record Handler() : GetEndpoint<Request>("/breweries/{id}")
{
    protected override void OnConfigure(RouteHandlerBuilder builder)
        => builder
                .ProducesHypermedia<Response>(StatusCodes.Status200OK)
                .Produces<Response>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetBrewery")
                .WithTags("Breweries")
                .AddEndpointFilter(async (context, next) => {
                    var logger = context.HttpContext.RequestServices.GetService<ILogger<Handler>>();
                    logger.LogInformation("Method Start: GetBrewery");
                    var result = await next(context);
                    logger.LogInformation("Method End: GetBrewery");
                    return result;
                });

    protected override async Task<IResult> OnHandleAsync(Request req, CancellationToken cancellationToken)
    {
        req.Logger.LogInformation("Getting brewery {Id}", req.Id);
        req.Logger.LogInformation("Call database");
        var brewery = await req.Database.Breweries.FindAsync(new object[] { (int)req.Id }, cancellationToken);
        if (brewery is null)
        {
            req.Logger.LogInformation("Brewery {Id} not found", req.Id);
            req.Logger.LogWarning("Brewery {Id} not found", req.Id);
            return Results.NotFound();
        }

        req.Logger.LogInformation("Brewery {Id} found", req.Id);
        req.Logger.LogInformation("Converting to Response resource");
        var resource = (Response)brewery;

        req.Logger.LogInformation("Converted to Response resource");
        req.Logger.LogInformation("Return Ok");
        return Results.Ok(resource);
    }
}
