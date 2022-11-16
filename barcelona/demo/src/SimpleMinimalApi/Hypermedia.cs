namespace SimpleMinimalApi;

public static class HypermediaServiceCollectionExtentions
{
    public static IServiceCollection AddHypermedia(this IServiceCollection services)
    {
        services.AddScoped<HypermediaConverter>();
        services.AddScoped<IHypermediaProvider, BeerDetailHypermediaProvider>();
        services.AddScoped<IHypermediaProvider, BeerListHypermediaProvider>();
        services.AddScoped<IHypermediaProvider, BeerStyleDetailsHypermediaProvider>();
        services.AddScoped<IHypermediaProvider, BreweryDetailHypermediaProvider>();
        services.AddScoped<IHypermediaProvider, BreweryListHypermediaProvider>();

        return services;
    }
}
