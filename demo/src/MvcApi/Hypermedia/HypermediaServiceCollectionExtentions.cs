namespace MvcApi.Hypermedia;

public static class HypermediaServiceCollectionExtentions
{
    public static IServiceCollection AddHypermedia(this IServiceCollection services)
    {
        services.AddScoped<HypermediaConverter>();
        services.AddScoped<IHypermediaProvider, BeerDetailHypermediaProvider>();
        services.AddScoped<IHypermediaProvider, BeerListHypermediaProvider>();
        services.AddScoped<IHypermediaProvider, BreweryDetailHypermediaProvider>();
        services.AddScoped<IHypermediaProvider, BreweryListHypermediaProvider>();
        services.AddScoped<IHypermediaProvider, BeerStyleDetailsHypermediaProvider>();
        services.AddMvc(options =>
        {
            options.OutputFormatters.Add(new JsonHypermediaOutputFormatter(new JsonSerializerOptions()));
        });
        return services;
    }
}
