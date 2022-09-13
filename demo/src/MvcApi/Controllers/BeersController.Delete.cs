namespace MvcApi.Controllers;

[ApiController]
public class BeersDeleteController : Controller
{
    private readonly BeerDbContext _db;

    public BeersDeleteController(BeerDbContext db)
    {
        _db = db;
    }

    [HttpDelete("beers/{id}", Name = "DeleteBeer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Tags("Beers")]
    public async Task<IActionResult> DeleteAsync(HashedId id)
    {
        var beer = await _db.Beers.FindAsync(new object[] { (int)id }, HttpContext.RequestAborted);
        if (beer is null)
        {
            return NotFound();
        }

        _db.Beers.Remove(beer);
        await _db.SaveChangesAsync(HttpContext.RequestAborted);

        return NoContent();
    }
}
