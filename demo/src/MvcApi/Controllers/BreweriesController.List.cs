namespace MvcApi.Controllers;

[ApiController]
public class BreweriesListController : Controller
{
    private readonly BeerDbContext _db;

    public BreweriesListController(BeerDbContext db)
    {
        _db = db;
    }

    [HttpGet("breweries", Name = "GetBreweries")]
    [ProducesResponseType(typeof(BreweryListItem), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(HypermediaObject<BreweryListItem>), StatusCodes.Status200OK, HypermediaConstants.ContentType)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Tags("Breweries")]
    public async Task<IActionResult> GetAsync(int page = 1, int pageSize = 10)
    {
        var total = await _db.Breweries.CountAsync(HttpContext.RequestAborted);
        if (total == 0)
        {
            return NoContent();
        }

        var breweries = await _db.Breweries
                            .Select(b => new BreweryListItem
                            {
                                Id = b.Id,
                                Name = b.Name,
                                City = b.City,
                                Country = b.Country,
                            })
                            .OrderBy(b => b.Name)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync(HttpContext.RequestAborted);

        var resourceList = new BreweryList(breweries, total, page, pageSize);

        return Ok(resourceList);
    }
}
