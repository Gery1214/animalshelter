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

        [HttpGet("allAnimalsWithShelter")]
        public async Task<IActionResult> AllAnimalsWithShelter()
        {
            try
            {
                var animalsWithShelter = await _context.Animals
                    .Join(
                        _context.Shelters,
                        animal => animal.ShelterId,
                        shelter => shelter.Id,
                        (animal, shelter) => new
                        {
                            animal.Id,
                            animal.Name,
                            animal.Species,
                            animal.Age,
                            animal.Weight,
                            animal.RegTime,
                            ShelterName = shelter.Name
                        })
                    .ToListAsync();

                if (animalsWithShelter.Count == 0)
                {
                    return StatusCode(404, new { message = "Nincs megjeleníthető adat!" });
                }

                return StatusCode(200, animalsWithShelter);
            }
            catch (Exception e)
            {
                return StatusCode(400, new { message = e.Message });
            }
        }

        [HttpGet("animalsOfShelter")]
        public async Task<IActionResult> AnimalsOfShelter([FromQuery] Guid shelterId)
        {
            try
            {
                var shelterWithAnimals = await _context.Shelters
                    .Include(s => s.Animals)
                    .Where(s => s.Id == shelterId)
                    .FirstOrDefaultAsync();

                if (shelterWithAnimals == null)
                {
                    return StatusCode(404, new { message = "Nincs ilyen azonosítójú menhely!" });
                }

                return StatusCode(200, shelterWithAnimals);
            }
            catch (Exception e)
            {
                return StatusCode(400, new { message = e.Message });
            }
        }
    }
}
