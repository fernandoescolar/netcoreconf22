namespace MvcApi.Controllers;

[ApiController]
public partial class BreweriesDetailsController : Controller
{
    private readonly BeerDbContext _db;

    public BreweriesDetailsController(BeerDbContext db)
    {
        _db = db;
    }

    [HttpGet("breweries/{id}", Name = "GetBrewery")]
    [ProducesResponseType(typeof(BreweryDetail), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(HypermediaObject<BreweryDetail>), StatusCodes.Status200OK, HypermediaConstants.ContentType)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Tags("Breweries")]
    public async Task<IActionResult> GetAsync(HashedId id)
    {
        var brewery = await _db.Breweries.FindAsync(new object[] { (int)id }, HttpContext.RequestAborted);
        if (brewery is null)
        {
            return NotFound();
        }

        var resource = (BreweryDetail)brewery;

        return Ok(resource);
    }
}
