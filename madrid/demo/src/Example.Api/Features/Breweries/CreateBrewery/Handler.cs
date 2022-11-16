namespace Example.Api.Features.CreateBrewery;

public record Handler() : PostEndpoint<Request>("/breweries")
{
    protected override void OnConfigure(RouteHandlerBuilder builder)
        => builder
                .ProducesHypermedia<Response>(StatusCodes.Status201Created)
                .Produces<Response>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("CreateBrewery")
                .WithTags("Breweries")
                .WithValidation();

    // ISSUE: do not use HttpClient directly
    protected override async Task<IResult> OnHandleAsync(Request req, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        //var client = req.HttpClientFactory.CreateClient();
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://mybeers.azurewebsites.net");

        var description = await GetDesciptionFromBeersApiAsync(client, req.Body.Name, cancellationToken);

        var brewery = new Brewery {
            Name = req.Body.Name,
            City = req.Body.City,
            Country = req.Body.Country,
        };

        req.Database.Breweries.Add(brewery);
        await req.Database.SaveChangesAsync(cancellationToken);

        var resource = (Response)brewery;
        return Results.CreatedAtRoute("GetBrewery", new { id = resource.Id.ToString() }, resource);
    }

    public static async Task<string> GetDesciptionFromBeersApiAsync(HttpClient client, string query, CancellationToken cancellationToken)
    {
        var response = await client.GetAsync($"api/beers?q={query}", cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var json = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
            return json;
        }

        return default;
    }
}
