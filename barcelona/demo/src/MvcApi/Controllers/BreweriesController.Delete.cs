namespace MvcApi.Controllers;

[ApiController]
public class BreweriesDeleteController : Controller
{
    private readonly BeerDbContext _db;

    public BreweriesDeleteController(BeerDbContext db)
    {
        _db = db;
    }

    [HttpDelete("breweries/{id}", Name = "DeleteBrewery")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Tags("Breweries")]
    public async Task<IActionResult> DeleteAsync(HashedId id)
    {
        var brewery = await _db.Breweries.FindAsync(new object[] { (int)id }, HttpContext.RequestAborted);
        if (brewery is null)
        {
            return NotFound();
        }

        _db.Breweries.Remove(brewery);
        await _db.SaveChangesAsync(HttpContext.RequestAborted);

        return NoContent();
    }
}
