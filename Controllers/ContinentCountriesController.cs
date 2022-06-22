using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab2_Web.Models;

namespace Lab2_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContinentCountriesController : ControllerBase
    {
        private readonly MapAPIContext _context;

        public ContinentCountriesController(MapAPIContext context)
        {
            _context = context;
        }

        // GET: api/ContinentCountries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContinentCountry>>> GetContinentCountries()
        {
          if (_context.ContinentCountries == null)
          {
              return NotFound();
          }
            return await _context.ContinentCountries.ToListAsync();
        }

        // GET: api/ContinentCountries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContinentCountry>> GetContinentCountry(int id)
        {
          if (_context.ContinentCountries == null)
          {
              return NotFound();
          }
            var continentCountry = await _context.ContinentCountries.FindAsync(id);

            if (continentCountry == null)
            {
                return NotFound();
            }

            return continentCountry;
        }

        // PUT: api/ContinentCountries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContinentCountry(int id, ContinentCountry continentCountry)
        {
            if (id != continentCountry.Id)
            {
                return BadRequest();
            }

            _context.Entry(continentCountry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContinentCountryExists(id))
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

        // POST: api/ContinentCountries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ContinentCountry>> PostContinentCountry(ContinentCountry continentCountry)
        {
          if (_context.ContinentCountries == null)
          {
              return Problem("Entity set 'MapAPIContext.ContinentCountries'  is null.");
          }
            _context.ContinentCountries.Add(continentCountry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContinentCountry", new { id = continentCountry.Id }, continentCountry);
        }

        // DELETE: api/ContinentCountries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContinentCountry(int id)
        {
            if (_context.ContinentCountries == null)
            {
                return NotFound();
            }
            var continentCountry = await _context.ContinentCountries.FindAsync(id);
            if (continentCountry == null)
            {
                return NotFound();
            }

            _context.ContinentCountries.Remove(continentCountry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContinentCountryExists(int id)
        {
            return (_context.ContinentCountries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
