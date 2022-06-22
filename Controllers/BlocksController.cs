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
    public class BlocksController : ControllerBase
    {
        private readonly MapAPIContext _context;

        public BlocksController(MapAPIContext context)
        {
            _context = context;
        }

        // Done
        // GET: api/Blocks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlockDtoRead>>> GetBlocks()
        {
            var blocksDtoRead = new List<BlockDtoRead>();
            var blocks = await _context.Blocks.ToListAsync();
            foreach (var block in blocks)
            {
                var blocCountries = _context.Countries.Where(c => c.BlockId == block.Id);
                var countries = "";
                foreach (var country in blocCountries)
                {
                    countries += country.Name + ", ";
                }
                blocksDtoRead.Add(new BlockDtoRead()
                {
                    Id = block.Id,
                    Name = block.Name,
                    Countries = countries 
                });
            }
            return blocksDtoRead;
        }

        // Done
        // GET: api/Blocks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlockDtoRead>> GetBlock(int id)
        {
            var block = await _context.Blocks.FindAsync(id);
            if (block == null) return NotFound();
            
            var blocCountries = _context.Countries.Where(c => c.BlockId == block.Id);
            var countries = "";
            foreach (var country in blocCountries)
            { 
                countries += country.Name + ", "; 
            } 
            var blockDtoRead = new BlockDtoRead() 
            {
                Id = block.Id, 
                Name = block.Name, 
                Countries = countries
            }; 
        
            return blockDtoRead;
        }

        // Done
        // PUT: api/Blocks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlock(int id, Block block)
        {
            if (id != block.Id)
            {
                return BadRequest();
            }
            if (_context.Blocks.Any(b => b.Name.ToLower().Equals(block.Name.ToLower())))
            {
                return ValidationProblem("Блок з таким ім'ям вже існує");
            }
            _context.Entry(block).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlockExists(id))
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

        // Done
        // POST: api/Blocks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Block>> PostBlock(Block block)
        {
          if (_context.Blocks == null)
          {
              return Problem("Entity set 'MapAPIContext.Blocks'  is null.");
          }

          if (_context.Blocks.Any(b => b.Name.ToLower().Equals(block.Name.ToLower())))
          {
              return ValidationProblem("Блок з таким ім'ям вже існує");
          }
            _context.Blocks.Add(block);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetBlock", new { id = block.Id });
        }

        // Done
        // DELETE: api/Blocks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlock(int id)
        {
            if (_context.Blocks == null)
            {
                return NotFound();
            }
            var block = await _context.Blocks.FindAsync(id);
            if (block == null)
            {
                return NotFound();
            }

            _context.Blocks.Remove(block);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlockExists(int id)
        {
            return (_context.Blocks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
