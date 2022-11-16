namespace Example.Api.Features.GetBeerStyles;

public record Handler() : GetEndpoint<Request>("/styles")
{
    protected override void OnConfigure(RouteHandlerBuilder builder)
        => builder
                .Produces<IEnumerable<Response>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent)
                .WithName("GetBeerStyles")
                .WithTags("Styles");

    protected override async Task<IResult> OnHandleAsync(Request req, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var total = await req.Database.Styles.CountAsync(cancellationToken);
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
                                .ToListAsync(cancellationToken);

        return Results.Ok(styles);
    }
}
