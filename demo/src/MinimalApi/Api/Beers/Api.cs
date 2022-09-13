using MinimalApi.Api.Beers.Delete;
using MinimalApi.Api.Beers.Get;
using MinimalApi.Api.Beers.List;
using MinimalApi.Api.Beers.List.ByBrewery;
using MinimalApi.Api.Beers.Upsert;

namespace MinimalApi.Api.Beers;

public static class Api
{
    public static RouteGroupBuilder MapBeersApi(this RouteGroupBuilder group)
    {
        group.MapBeersGet();
        group.MapBeersList();
        group.MapBeersListByBrewery();
        group.MapBeersUpsert().ToList(); // materialize
        group.MapBeersDelete();

        return group;
    }
}
