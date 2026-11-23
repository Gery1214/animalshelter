using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterAPI.Models;

namespace ShelterAPI.Controllers
{
    [Route("api/queries")]
    [ApiController]
    public class QueriesController : ControllerBase
    {
        private readonly AnimalshelterContext _context;

        public QueriesController(AnimalshelterContext context)
        {
            _context = context;
        }

        [HttpGet("allSheltersWithAnimals")]
        public async Task<IActionResult> AllSheltersWithAnimals()
        {
            try
            {
                var sheltersWithAnimals = await _context.Shelters
                    .Include(s => s.Animals)
                    .ToListAsync();

                if (sheltersWithAnimals.Count == 0)
                {
                    return StatusCode(404, new { message = "Nincs megjeleníthető menhely!" });
                }

                return StatusCode(200, sheltersWithAnimals);
            }
            catch (Exception e)
            {
                return StatusCode(400, new { message = e.Message });
            }
        }
    }
}
