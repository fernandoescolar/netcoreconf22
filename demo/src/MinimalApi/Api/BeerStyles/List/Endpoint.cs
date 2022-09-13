namespace MinimalApi.Api.BeerStyles.List;

public static class Endpoint
{
    public static RouteHandlerBuilder MapBeerStylesList(this RouteGroupBuilder group)
        => group.MapGet("/styles/", Delegate)
                .Produces<IEnumerable<BeerStyleDetail>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent)
                .WithName("GetBeerStyles");

    private static async Task<IResult> Delegate(BeerDbContext db, CancellationToken cancellationToken)
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
