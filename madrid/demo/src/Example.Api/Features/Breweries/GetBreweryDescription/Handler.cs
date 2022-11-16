namespace Example.Api.Features.GetBreweryDescription;

public record Handler() : GetEndpoint<Request>("/breweries/{id}/description")
{
    protected override void OnConfigure(RouteHandlerBuilder builder)
        => builder
                .ProducesHypermedia<Response>(StatusCodes.Status200OK)
                .Produces<Response>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetBreweryDescription")
                .WithTags("Breweries");

    // ISSUE: use StringBuilder
    protected override async Task<IResult> OnHandleAsync(Request req, CancellationToken cancellationToken)
    {
        var brewery = await req.Database.Breweries.FindAsync(new object[] { (int)req.Id }, cancellationToken);
        if (brewery is null)
        {
            return Results.NotFound();
        }

        var beers = await req.Database.Beers.Where(b => b.Brewery.Id == brewery.Id).ToListAsync(cancellationToken);
        var description = string.Empty;
        description += "Brewery: " + brewery.Name + Environment.NewLine;
        description += "City: " + brewery.City + Environment.NewLine;
        description += "Country: " + brewery.Country + Environment.NewLine;
        description += "Beers: " + Environment.NewLine;
        foreach(var beer in beers)
        {
            description += "\t" + beer.Name + Environment.NewLine;
        }

        // var sb = new System.Text.StringBuilder();
        // sb.Append("Brewery: ");
        // sb.AppendLine(brewery.Name);
        // sb.Append("City: ");
        // sb.AppendLine(brewery.City);
        // sb.Append("Country: ");
        // sb.AppendLine(brewery.Country);
        // sb.AppendLine("Beers:");
        // foreach(var beer in beers)
        // {
        //     sb.Append("\t");
        //     sb.AppendLine(beer.Name);
        // }

        // var description = sb.ToString();

        return Results.Ok(new { description });
    }
}
