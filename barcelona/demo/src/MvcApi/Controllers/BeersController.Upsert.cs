namespace MvcApi.Controllers;

[ApiController]
public class BeerUpsertController : ControllerBase
{
    private readonly BeerDbContext _db;

    public BeerUpsertController(BeerDbContext db)
    {
        _db = db;
    }

    [HttpPut("beers/{id}", Name = "UpsertBeer")]
    [ProducesResponseType(typeof(BeerDetail), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(HypermediaObject<BeerDetail>), StatusCodes.Status200OK, HypermediaConstants.ContentType)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Tags("Beers")]
    public Task<IActionResult> PutAsync(HashedId id, [FromBody] BeerRequest value)
        => UpsertAsync(value, id, cancellationToken: HttpContext.RequestAborted);

    [HttpPost("beers", Name = "CreateBeer")]
    [ProducesResponseType(typeof(BeerDetail), StatusCodes.Status201Created)]
    //[ProducesResponseType(typeof(HypermediaObject<BeerDetail>), StatusCodes.Status201Created, HypermediaConstants.ContentType)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Tags("Beers")]
    public Task<IActionResult> PostAsync([FromBody] BeerRequest value)
        => UpsertAsync(value, forceCreation: true, cancellationToken: HttpContext.RequestAborted);

    private async Task<IActionResult> UpsertAsync(BeerRequest input, int? id = null, bool forceCreation = false, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var brewery = await _db.Breweries.FindAsync(new object[] { (int)input.BreweryId }, cancellationToken);
        if (brewery is null)
        {
            return BadRequest("Unkown Brewery");
        }

        var style = await _db.Styles.FindAsync(new object[] { (int)input.StyleId }, cancellationToken);
        if (style is null)
        {
            return BadRequest("Invalid beer style");
        }


        var beer = (id.HasValue && !forceCreation) ? await _db.Beers.FindAsync(new object[] { id }, cancellationToken) : null;
        if (beer is null)
        {
            beer = new Beer();
            if (id.HasValue)
            {
                beer.Id = id.Value;
            }

            _db.Beers.Add(beer);
        }

        beer.Name = input.Name;
        beer.Brewery = brewery;
        beer.Style = style;

        await _db.SaveChangesAsync(cancellationToken);

        var resource = (BeerDetail)beer;
        if (id.HasValue)
        {
            return Ok(resource);
        }
        else
        {
            return CreatedAtRoute("GetBeer", new { id = resource.Id.ToString() }, resource);
        }
    }
}
