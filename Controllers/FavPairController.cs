using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using two_realities.Data;
using two_realities.Models;


namespace two_realities.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavPairController : ControllerBase
    {

        public readonly DataContext _context;

        public FavPairController(DataContext dataContext)
        {
            _context = dataContext;
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult<FavoritePair>> getAll()
        {
            try
            {
                var pairs = await _context.FavoritePairs.ToListAsync();
                return Ok(pairs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Failed to Fetch Fav Games");
            }
        }

        [HttpPost]
        public async Task<ActionResult<FavoritePair>> newP(FavoritePair newPair)
        {
            try
            {
                _context.FavoritePairs.Add(newPair);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Failed to add Favorites");
            }
        }

        [HttpPatch]
        public async Task<ActionResult<FavoritePair>> update(FavoritePair request)
        {
            try
            {
                var currPair = await _context.FavoritePairs.FindAsync(request.UserId);
                if (request.TitleOne != null) currPair.TitleOne = request.TitleOne;
                if (request.YearOne != 0) currPair.YearOne = request.YearOne;
                if (request.TitleTwo != null) currPair.TitleTwo = request.TitleTwo;
                if (request.YearTwo != 0) currPair.YearTwo = request.YearTwo;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Failed to update favorites");
            }
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> delete(string userId)
        {
            try
            {
                var pair = await _context.FavoritePairs.FindAsync(userId);
                _context.FavoritePairs.Remove(pair);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Failed to Delete Favorites");
            }
        }
    }
}

