namespace MinimalHypermedia;

public static class HypermediaEndpointFilterExtensions
{

    public static RouteHandlerBuilder WithHypermedia(this RouteHandlerBuilder builder)
    {
        builder.AddEndpointFilter<HypermediaEndpointFilter>();
        return builder;
    }

    public static RouteGroupBuilder WithHypermedia(this RouteGroupBuilder builder)
    {
        builder.AddEndpointFilter<HypermediaEndpointFilter>();
        return builder;
    }
}