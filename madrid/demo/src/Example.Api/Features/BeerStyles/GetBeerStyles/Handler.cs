namespace Example.Api.Features.GetBeerStyles;

public record Handler() : GetEndpoint<Request>("/styles")
{
    protected override void OnConfigure(RouteHandlerBuilder builder)
        => builder
                .Produces<IEnumerable<Response>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent)
                .WithName("GetBeerStyles")
                .WithTags("Styles");

    // ISSUE: do not use CancellationToken or HttpContext.RequestAborted
    protected override async Task<IResult> OnHandleAsync(Request req, CancellationToken cancellationToken)
    {
        // cancellationToken.ThrowIfCancellationRequested();
        await LongCallAsync();

        // var total = await req.Database.Styles.CountAsync(cancellationToken);
        var total = await req.Database.Styles.CountAsync();
        if (total == 0)
        {
            return Results.NoContent();
        }

        var styles = await req.Database
                                .Styles
                                .Select(s => new Response
                                {
                                    Id = s.Id,
                                    Name = s.Name,
                                })
                                .OrderBy(s => s.Name)
                                //.ToListAsync(cancellationToken);
                                .ToListAsync();

        return Results.Ok(styles);
    }

    private static readonly Random _random = new Random();
    private Task LongCallAsync(CancellationToken cancellationToken = default)
    {
        var ms = _random.Next(0, 100) > 10 ?_random.Next(100, 400) : _random.Next(1000, 50000);
        return Task.Delay(ms, cancellationToken);
    }
}
