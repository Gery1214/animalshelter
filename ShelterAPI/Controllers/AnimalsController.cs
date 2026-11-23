using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterAPI.Models;
using ShelterAPI.Models.DTOs;

namespace ShelterAPI.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly AnimalshelterContext _context;

        public AnimalsController(AnimalshelterContext context)
        {
            _context = context;
        }
        [HttpPost("newAnimal")]
        public async Task<IActionResult> NewAnimal(AddNewAnimal newAnimalDto)
        {
            try
            {
                var animal = new Animal
                {
                    Id = Guid.NewGuid(),
                    Name = newAnimalDto.Name,
                    Species = newAnimalDto.Species,
                    Age = newAnimalDto.Age,
                    Weight = newAnimalDto.Weight,
                    RegTime = DateTime.Now,
                    ShelterId = newAnimalDto.ShelterId
                };

                if (animal != null)
                {
                    await _context.Animals.AddAsync(animal);
                    await _context.SaveChangesAsync();

                    return StatusCode(201, animal);
                }

                return StatusCode(404, animal);
            }
            catch (Exception e)
            {
                return StatusCode(400, new { message = e.Message });
            }
        }

        [HttpGet("allAnimal")]
        public async Task<IActionResult> AllAnimal()
        {
            try
            {
                var animals = await _context.Animals.ToListAsync();

                if (animals.Count == 0)
                {
                    return StatusCode(404, new { message = "Nincs megjeleníthető állat az adatbázisban!" });
                }

                return StatusCode(200, animals);
            }
            catch (Exception e)
            {
                return StatusCode(400, new { message = e.Message });
            }
        }
    }
}

