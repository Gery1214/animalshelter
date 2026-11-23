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
        [HttpGet("byId")]
        public async Task<IActionResult> ById([FromQuery] Guid id)
        {
            try
            {
                var animal = await _context.Animals.FirstOrDefaultAsync(x => x.Id == id);

                if (animal == null)
                {
                    return StatusCode(404, new { message = "Nincs ilyen azonosítójú állat!" });
                }

                return StatusCode(200, animal);
            }
            catch (Exception e)
            {
                return StatusCode(400, new { message = e.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromBody] UpdateAnimalDTO updateAnimalDto)
        {
            try
            {
                var existingAnimal = await _context.Animals.FirstOrDefaultAsync(x => x.Id == id);

                if (existingAnimal == null)
                {
                    return StatusCode(404, new { message = "Nincs ilyen azonosítójú állat!" });
                }

                existingAnimal.Name = updateAnimalDto.Name;
                existingAnimal.Species = updateAnimalDto.Species;
                existingAnimal.Age = updateAnimalDto.Age;
                existingAnimal.Weight = updateAnimalDto.Weight;
                existingAnimal.ShelterId = updateAnimalDto.ShelterId;

                _context.Animals.Update(existingAnimal);
                await _context.SaveChangesAsync();
                return StatusCode(200, existingAnimal);
            }
            catch (Exception e)
            {
                return StatusCode(400, new { message = e.Message });
            }
        }

        [HttpDelete("deleteAnimal")]
        public async Task<IActionResult> DeleteAnimal([FromQuery] Guid id)
        {
            try
            {
                var animal = await _context.Animals.FirstOrDefaultAsync(x => x.Id == id);

                if (animal == null)
                {
                    return StatusCode(404, new { message = "Nincs ilyen azonosítójú állat!" });
                }

                _context.Animals.Remove(animal);
                await _context.SaveChangesAsync();

                return StatusCode(200, new { message = "Sikeres törlés!" });
            }
            catch (Exception e)
            {
                return StatusCode(400, new { message = e.Message });
            }
        }
    }
}

