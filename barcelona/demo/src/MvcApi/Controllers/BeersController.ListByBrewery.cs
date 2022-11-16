namespace MvcApi.Controllers;

[ApiController]
public class BeersListByBreweryController : ControllerBase
{
    private readonly BeerDbContext _db;

    public BeersListByBreweryController(BeerDbContext db)
    {
        _db = db;
    }

    [HttpGet("breweries/{id}/beers", Name = "GetBeersInBrewery")]
    [ProducesResponseType(typeof(BeerList), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(HypermediaObject<BeerList>), StatusCodes.Status200OK, HypermediaConstants.ContentType)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Tags("Beers")]
    public async Task<IActionResult> GetAsync(HashedId id, int page = 1, int pageSize = 10)
    {
        var brewery = await _db.Breweries.FindAsync(new object[] { (int)id }, HttpContext.RequestAborted);
        if (brewery is null)
        {
            return NotFound();
        }

        var total = await _db.Beers.CountAsync(b => b.Brewery.Id == (int)id, HttpContext.RequestAborted);
        if (total == 0)
        {
            return NoContent();
        }

        var beers = await _db.Beers
                        .Where(b => b.Brewery.Id == (int)id)
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