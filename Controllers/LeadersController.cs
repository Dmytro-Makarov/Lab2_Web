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
    public class LeadersController : ControllerBase
    {
        private readonly MapAPIContext _context;

        public LeadersController(MapAPIContext context)
        {
            _context = context;
        }

        // GET: api/Leaders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaderDtoRead>>> GetLeaders()
        {
            var leadersDtoRead = new List<LeaderDtoRead>();
            var leaders = await _context.Leaders.ToListAsync();
            foreach (var leader in leaders)
            {
                var leaderCountry = _context.Countries.First(c => c.LeaderId == leader.Id);
                leadersDtoRead.Add(new LeaderDtoRead()
                {
                    Id = leader.Id,
                    Name = leader.Name,
                    Country = leaderCountry.Name 
                });
            }
            return leadersDtoRead;
        }

        // GET: api/Leaders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaderDtoRead>> GetLeader(int id)
        {
            var leader = await _context.Leaders.FindAsync(id);
            if (leader == null) return NotFound();

            var leaderCountry = _context.Countries.First(c => c.LeaderId == leader.Id);
            var leaderDtoRead = new LeaderDtoRead()
            {
                Id = leader.Id, 
                Name = leader.Name, 
                Country = leaderCountry.Name 
            };
            
            return leaderDtoRead;
        }

        // PUT: api/Leaders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeader(int id, Leader leader)
        {
            if (id != leader.Id)
            {
                return BadRequest();
            }

            _context.Entry(leader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaderExists(id))
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

        // POST: api/Leaders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Leader>> PostLeader(Leader leader)
        {
          if (_context.Leaders == null)
          {
              return Problem("Entity set 'MapAPIContext.Leaders'  is null.");
          }
          if (_context.Leaders.Any(l => l.Name.ToLower().Equals(leader.Name.ToLower())))
          {
              return ValidationProblem("Лідер з таким ім'ям вже існує");
          }
            _context.Leaders.Add(leader);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetLeader", new { id = leader.Id });
        }

        // DELETE: api/Leaders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeader(int id)
        {
            if (_context.Leaders == null)
            {
                return NotFound();
            }
            var leader = await _context.Leaders.FindAsync(id);
            if (leader == null)
            {
                return NotFound();
            }

            _context.Leaders.Remove(leader);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeaderExists(int id)
        {
            return (_context.Leaders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
