namespace MvcApi.Controllers;

[ApiController]
public class BeersListController : ControllerBase
{
    private readonly BeerDbContext _db;

    public BeersListController(BeerDbContext db)
    {
        _db = db;
    }

    [HttpGet("beers", Name = "GetBeers")]
    [ProducesResponseType(typeof(BeerList), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(HypermediaObject<BeerList>), StatusCodes.Status200OK, HypermediaConstants.ContentType)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Tags("Beers")]
    public async Task<IActionResult> GetAsync(int page = 1, int pageSize = 10)
    {
        var total = await _db.Beers.CountAsync(HttpContext.RequestAborted);
        if (total == 0)
        {
            return NoContent();
        }

        var beers = await _db.Beers
                        .Select(b => new BeerListItem
                        {
                            Id = b.Id,
                            Name = b.Name,
                            BreweryId = b.Brewery.Id,
                            BreweryName = b.Brewery.Name,
                            StyleId = b.Style.Id,
                            StyleName = b.Style.Name,
                        })
                        .OrderBy(b => b.Name)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync(HttpContext.RequestAborted);

        var resourceList = new BeerList(beers, total, page, pageSize);

        return Ok(resourceList);
    }
}
