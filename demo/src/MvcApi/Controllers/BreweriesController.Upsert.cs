namespace MvcApi.Controllers;

[ApiController]
public class BreweriesUpsertController : Controller
{
    private readonly BeerDbContext _db;

    public BreweriesUpsertController(BeerDbContext db)
    {
        _db = db;
    }

    [HttpPost("breweries", Name = "CreateBrewery")]
    [ProducesResponseType(typeof(BreweryDetail), StatusCodes.Status201Created)]
    //[ProducesResponseType(typeof(HypermediaObject<BreweryDetail>), StatusCodes.Status201Created, HypermediaConstants.ContentType)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Tags("Breweries")]
    public Task<IActionResult> PostAsync([FromBody] BreweryRequest value)
        => UpsertAsync(value, forceCreation: true, cancellationToken: HttpContext.RequestAborted);

    [HttpPut("breweries/{id}", Name = "UpdateBrewery")]
    [ProducesResponseType(typeof(BreweryDetail), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(HypermediaObject<BreweryDetail>), StatusCodes.Status200OK, HypermediaConstants.ContentType)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Tags("Breweries")]
    public Task<IActionResult> PutAsync(HashedId id, [FromBody] BreweryRequest value)
        => UpsertAsync(value, id, cancellationToken: HttpContext.RequestAborted);

    private async Task<IActionResult> UpsertAsync(BreweryRequest input, int? id = null, bool forceCreation = false, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var brewery = (id.HasValue && !forceCreation) ? await _db.Breweries.FindAsync(new object[] { id }, cancellationToken) : null;
        if (brewery is null)
        {
            brewery = new Brewery();
            if (id.HasValue)
            {
                brewery.Id = id.Value;
            }

            _db.Breweries.Add(brewery);
        }

        brewery.Name = input.Name;
        brewery.City = input.City;
        brewery.Country = input.Country;

        await _db.SaveChangesAsync(cancellationToken);

        var resource = (BreweryDetail)brewery;
        if (id.HasValue)
        {
            return Ok(resource);
        }
        else
        {
            return CreatedAtRoute("GetBrewery", new { id = resource.Id.ToString() }, resource);
        }
    }
}
