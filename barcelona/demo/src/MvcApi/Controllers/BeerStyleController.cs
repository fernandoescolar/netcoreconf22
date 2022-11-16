using Microsoft.AspNetCore.OutputCaching;

namespace MvcApi.Controllers;

[ApiController]
public class BeerStyleController : Controller
{
    private readonly BeerDbContext _db;

    public BeerStyleController(BeerDbContext db)
    {
        _db = db;
    }

    [HttpGet("styles", Name = "GetStyles")]
    [OutputCache(Duration = 3600)]
    [ProducesResponseType(typeof(IEnumerable<BeerStyle>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HypermediaObject<BeerStyle>), StatusCodes.Status200OK, HypermediaConstants.ContentType)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Tags("Styles")]
    public async Task<IActionResult> GetAsync()
    {
        var total = await _db.Styles.CountAsync(HttpContext.RequestAborted);
        if (total == 0)
        {
            return NoContent();
        }

        var styles = await _db.Styles
                        .Select(s => new BeerStyleDetail
                        {
                            Id = s.Id,
                            Name = s.Name,
                        })
                        .OrderBy(s => s.Name)
                        .ToListAsync(HttpContext.RequestAborted);

        return Ok(styles);
    }
}
