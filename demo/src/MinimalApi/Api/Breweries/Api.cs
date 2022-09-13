using MinimalApi.Api.Breweries.Delete;
using MinimalApi.Api.Breweries.Get;
using MinimalApi.Api.Breweries.List;
using MinimalApi.Api.Breweries.Upsert;

namespace MinimalApi.Api.Breweries;

public static class Api
{
    public static RouteGroupBuilder MapBreweriesApi(this RouteGroupBuilder group)
    {
        group.MapBreweriesGet();
        group.MapBreweriesList();
        group.MapBreweriesUpsert().ToList(); // materialize
        group.MapBreweriesDelete();

        return group;
    }
}
