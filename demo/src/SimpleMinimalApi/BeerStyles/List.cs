namespace SimpleMinimalApi.BeerStyles;

public record List() : GetEndpoint("/styles")
{
    public override void Configure(RouteHandlerBuilder builder)
        => builder
                .Produces<IEnumerable<BeerStyleDetail>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent)
                .WithName("GetBeerStyles")
                .WithTags("Styles");

    public async Task<IResult> HandleAsync(BeerDbContext db, CancellationToken cancellationToken)
    {
        var total = await db.Styles.CountAsync(cancellationToken);
        if (total == 0)
        {
            return Results.NoContent();
        }

        var styles = await db.Styles
                        .Select(s => new BeerStyleDetail
                        {
                            Id = s.Id,
                            Name = s.Name,
                        })
                        .OrderBy(s => s.Name)
                        .ToListAsync(cancellationToken);

        return Results.Ok(styles);
    }
}
