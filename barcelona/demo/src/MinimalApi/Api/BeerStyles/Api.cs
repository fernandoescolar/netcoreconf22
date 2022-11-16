using MinimalApi.Api.BeerStyles.List;

namespace MinimalApi.Api.BeerStyles;

public static class Api
{
    public static RouteGroupBuilder MapBeerStylesApi(this RouteGroupBuilder group)
    {
        group.MapBeerStylesList();

        return group;
    }
}
