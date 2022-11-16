namespace MvcApi.Controllers;

[ApiController]
public class BeersDetailsController : Controller
{
    private readonly BeerDbContext _db;

    public BeersDetailsController(BeerDbContext db)
    {
        _db = db;
    }

    [HttpGet("beers/{id}", Name = "GetBeer")]
    [ProducesResponseType(typeof(BeerDetail), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(HypermediaObject<BeerDetail>), StatusCodes.Status200OK, HypermediaConstants.ContentType)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Tags("Beers")]
    public async Task<IActionResult> GetAsync(HashedId id)
    {
        var beer = await _db.Beers
                            .Include(nameof(Beer.Brewery))
                            .Include(nameof(Beer.Style))
                            .FirstOrDefaultAsync(b => b.Id == (int)id, HttpContext.RequestAborted);
        if (beer is null)
        {
            return NotFound();
        }

        var resource = (BeerDetail)beer;

        return Ok(resource);
    }
}
