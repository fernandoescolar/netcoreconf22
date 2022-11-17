namespace Example.Api.Features.GetBeers;

public record Handler() : GetEndpoint<Request>("/beers")
{
    protected override void OnConfigure(RouteHandlerBuilder builder)
        => builder
                .ProducesHypermedia<Response>(StatusCodes.Status200OK)
                .Produces<Response>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent)
                .WithName("GetBeers")
                .WithTags("Beers");

    protected override async Task<IResult> OnHandleAsync(Request req, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var total = await req.Database.Beers.CountAsync(cancellationToken);
        if (total == 0)
        {
            return Results.NoContent();
        }

        var beers = await req.Database
                                .Beers
                                .Select(b => new ResponseItem
                                {
                                    Id = b.Id,
                                    Name = b.Name,
                                    BreweryId = b.Brewery.Id,
                                    BreweryName = b.Brewery.Name,
                                    StyleId = b.StyleId
                                    //StyleId = b.Style.Id,
                                    //StyleName = b.Style.Name
                                })
                                .OrderBy(b => b.Name)
                                .Skip((req.Page - 1) * req.PageSize)
                                .Take(req.PageSize)
                                .ToListAsync(cancellationToken);

        var resourceList = new Response(beers, total, req.Page, req.PageSize);
        FillBeerStyles(req.Database, resourceList);

        return Results.Ok(resourceList);
    }

    // This is a demo method.
    // In a real world application, you would use the projection to get the beer styles with the beers on only one query.
    // This is just to show how a cache can be used to improve performance.
    // ISSUE: use caches
    private void FillBeerStyles(BeerDbContext database, Response resourceList)
    {
        foreach (var item in resourceList.Items)
        {
            var style = database.Styles.FirstOrDefault(s => s.Id == item.StyleId);
            if (style != null)
            {
                item.StyleId = style.Id;
                item.StyleName = style.Name;
            }
        }
    }

    // private void FillBeerStyles(BeerDbContext database, Response resourceList, IMemoryCache cache)
    // {
    //     var styles = cache.GetOrSetAsync("styles", () => database.Styles.AsNoTracking().ToListAsync())
    //     foreach (var item in resourceList.Items)
    //     {
    //         var style = styles.FirstOrDefault(s => s.Id == item.StyleId);
    //         if (style != null)
    //         {
    //             item.StyleId = style.Id;
    //             item.StyleName = style.Name;
    //         }
    //     }
    // }
}
