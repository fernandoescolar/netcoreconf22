namespace MinimalApi.Api.Breweries.List;

public class BreweryList : PagedList<BreweryListItem>
{
    public BreweryList(): base()
    {
    }

    public BreweryList(IEnumerable<BreweryListItem> items, int total, int page, int pageSize) : base(items, total, page, pageSize)
    {
    }
}
