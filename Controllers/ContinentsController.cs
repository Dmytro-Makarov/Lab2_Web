using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab2_Web.Models;
using Lab2_Web.Models.DTO;

namespace Lab2_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContinentsController : ControllerBase
    {
        private readonly MapAPIContext _context;

        public ContinentsController(MapAPIContext context)
        {
            _context = context;
        }

        // GET: api/Continents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContinentDtoRead>>> GetContinents()
        {
            var continentsDtoRead = new List<ContinentDtoRead>();
            var continents = await _context.Continents.ToListAsync();
            foreach (var continent in continents)
            {
                var continentsCountries = _context.ContinentCountries.Where(cc => cc.ContinentId == continent.Id).ToList();
                var countries = "";
                foreach (var country in continentsCountries)
                {
                    var foundCountry = _context.Countries.First(c => c.Id == country.CountryId); 
                    countries += foundCountry.Name + ", ";
                }

                continentsDtoRead.Add(new ContinentDtoRead()
                {
                    Id = continent.Id,
                    Name = continent.Name,
                    Countries = countries
                });

            }

            return continentsDtoRead;
        }

        // GET: api/Continents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContinentDtoRead>> GetContinent(int id)
        {
            var continent = await _context.Continents.FindAsync(id);
            if (continent == null) return NotFound();
            
            var continentsCountries = _context.ContinentCountries.Where(cc => cc.ContinentId == continent.Id).ToList();
            var countries = "";
            foreach (var country in continentsCountries)
            {
                var foundCountry = _context.Countries.First(c => c.Id == country.CountryId); 
                countries += foundCountry.Name + ", ";
            }

            var continentDtoRead = new ContinentDtoRead()
            {
                Id = continent.Id,
                Name = continent.Name,
                Countries = countries
            };

            return continentDtoRead;
        }

        // PUT: api/Continents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContinent(int id, Continent continent)
        {
            if (id != continent.Id)
            {
                return BadRequest();
            }
            if (_context.Blocks.Any(b => b.Name.ToLower().Equals(continent.Name.ToLower())))
            {
                return ValidationProblem("Континент/регіон з таким ім'ям вже існує");
            }
            _context.Entry(continent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContinentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Continents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Continent>> PostContinent(Continent continent)
        {
          if (_context.Continents == null)
          {
              return Problem("Entity set 'MapAPIContext.Continents'  is null.");
          }
          if (_context.Blocks.Any(b => b.Name.ToLower().Equals(continent.Name.ToLower())))
          {
              return ValidationProblem("Континент/регіон з таким ім'ям вже існує");
          }
            _context.Continents.Add(continent);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetContinent", new { id = continent.Id });
        }

        // DELETE: api/Continents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContinent(int id)
        {
            if (_context.Continents == null)
            {
                return NotFound();
            }
            var continent = await _context.Continents.FindAsync(id);
            if (continent == null)
            {
                return NotFound();
            }

            
            _context.ContinentCountries.RemoveRange(_context.ContinentCountries.Where(cc => cc.ContinentId == continent.Id));
            await _context.SaveChangesAsync();
            _context.Countries.RemoveRange(_context.Countries.Where(c => c.ContinentsCountries.Count == 0));
            _context.Continents.Remove(continent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContinentExists(int id)
        {
            return (_context.Continents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
