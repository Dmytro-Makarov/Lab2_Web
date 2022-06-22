using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab2_Web.Models;
using Lab2_Web.Models.DTO;
using Microsoft.Build.Framework;

namespace Lab2_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly MapAPIContext _context;

        public CountriesController(MapAPIContext context)
        {
            _context = context;
        }
        
        //Get countries
        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDtoRead>>> GetCountries()
        {
            var countriesDtoRead = new List<CountryDtoRead>();
            var countries = await _context.Countries.ToListAsync();
            foreach (var country in countries)
            {
                var leader = _context.Leaders.First(l => l.Id == country.LeaderId);
                var block = _context.Blocks.First(b => b.Id == country.BlockId);
                var continentsCountries = _context.ContinentCountries.Where(cc => cc.CountryId == country.Id).ToList();
                var continents = "";
                foreach (var continent in continentsCountries)
                {
                    var foundContinent = _context.Continents.First(c => c.Id == continent.ContinentId); 
                    continents += foundContinent.Name + ", ";
                }
                var countryDtoRead = new CountryDtoRead()
                {
                    Id = country.Id,
                    Name = country.Name,
                    MilitaryStrength = country.MilitaryStrength,
                    Leader = leader!.Name,
                    Block = block!.Name,
                    Continents = continents
                };
                countriesDtoRead.Add(countryDtoRead);
            }

            return countriesDtoRead;
        }

        //Get country with that id
        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDtoRead>> GetCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null) return NotFound(); 
            
            var leader = _context.Leaders.First(l => l.Id == country.LeaderId);
            var block = _context.Blocks.First(b => b.Id == country.BlockId);
            var continentsCountries = _context.ContinentCountries.Where(cc => cc.CountryId == country.Id).ToList();
            var continents = ""; 
            foreach (var continent in continentsCountries) 
            { 
                var foundContinent = _context.Continents.First(c => c.Id == continent.ContinentId); 
                continents += foundContinent.Name + ", ";
            }
            var countryDtoRead = new CountryDtoRead()
            {
                Id = country.Id, 
                Name = country.Name,
                MilitaryStrength = country.MilitaryStrength,
                Leader = leader!.Name,
                Block = block!.Name,
                Continents = continents
            };

            return countryDtoRead;
        }

        //Put a new country into that id
        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry([FromRoute]int? id, CountryDtoWrite countryDtoWrite)
        {
            if (id is null) return BadRequest("No id");
            var country = await _context.Countries.FindAsync(id);
            if (country is null) return BadRequest("Bad id");
            
            Block block;
            if (_context.Blocks.Any(b => b.Name == countryDtoWrite.BlockName))
            {
                block = _context.Blocks.First(b => b.Name.ToLower() == countryDtoWrite.BlockName.ToLower());
            }
            else
            {
                block = new Block()
                {
                    Name = countryDtoWrite.BlockName
                };
                _context.Blocks.Add(block);
                await _context.SaveChangesAsync();
            }
            if (block is null) return BadRequest("Bad BlockId");

            Leader leader;
            leader = _context.Leaders.First(l => l.Id == country.LeaderId);
            if (leader is null)
            {
                leader = _context.Leaders.First(l => l.Name == countryDtoWrite.LeaderName);
            }

            if (leader is null)
            {
                leader = new Leader()
                {
                    Name = countryDtoWrite.LeaderName,
                    Country = country
                };
                _context.Leaders.Add(leader);
                await _context.SaveChangesAsync();
            }
            leader.Name = countryDtoWrite.LeaderName;
            
            country.Name = countryDtoWrite.Name;
            country.MilitaryStrength = countryDtoWrite.MilitaryStrength;
            country.Leader = leader;
            country.LeaderId = leader.Id;
            country.Block = block;
            country.BlockId = block.Id;
            leader.Country = country;

            _context.ContinentCountries.RemoveRange(_context.ContinentCountries.Where(cc => cc.CountryId == id));
            
            List<Continent> continents = new List<Continent>();
            foreach (var item in countryDtoWrite.ContinentNames)
            {
                Continent continent;
                if (_context.Continents.Any(c => c.Name.ToLower().Equals(item.ToLower())))
                {
                    continent = _context.Continents.First(c => c.Name.ToLower().Equals(item.ToLower()));
                }
                else
                {
                    continent = new Continent()
                    {
                        Name = item
                    };
                    _context.Continents.Add(continent);
                    await _context.SaveChangesAsync();
                }
                continents.Add(continent);
            }
            
            var continentCountries = new List<ContinentCountry>();
            
            foreach (var continent in continents)
            {
                continentCountries.Add(new ContinentCountry()
                {
                    Continent = continent,
                    ContinentId = continent.Id,
                    Country = country,
                    CountryId = country.Id
                });
            }
            _context.ContinentCountries.AddRange(continentCountries);

            _context.Entry(country).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
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

        //Add a new Country
        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CountryDtoWrite countryDtoWrite)
        {
            Block block;
            if (_context.Blocks.Any(b => b.Name == countryDtoWrite.BlockName))
            {
                block = _context.Blocks.First(b => b.Name == countryDtoWrite.BlockName);
            }
            else
            {
                block = new Block()
                {
                    Name = countryDtoWrite.BlockName
                };
                _context.Blocks.Add(block);
                await _context.SaveChangesAsync();
            }

            List<Continent> continents = new List<Continent>();
            foreach (var item in countryDtoWrite.ContinentNames)
            {
                Continent continent;
                if (_context.Continents.Any(c => c.Name.ToLower().Equals(item.ToLower())))
                {
                    continent = _context.Continents.First(c => c.Name.ToLower().Equals(item.ToLower()));
                }
                else
                {
                    continent = new Continent()
                    {
                        Name = item
                    };
                    _context.Continents.Add(continent);
                    await _context.SaveChangesAsync();
                }
                continents.Add(continent);
            }

            var leader = new Leader()
            {
                Name = countryDtoWrite.LeaderName
            };

            var country = new Country()
            {
                Name = countryDtoWrite.Name,
                LeaderId = leader.Id,
                Leader = leader,
                BlockId = block.Id,
                Block = block,
                MilitaryStrength = countryDtoWrite.MilitaryStrength
            };
            leader.Country = country;
            
            var continentCountries = new List<ContinentCountry>();
            
            foreach (var continent in continents)
            {
                continentCountries.Add(new ContinentCountry()
                {
                Continent = continent,
                ContinentId = continent.Id,
                Country = country,
                CountryId = country.Id
                });
            }
            _context.ContinentCountries.AddRange(continentCountries);
            
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetCountry", new { id = country.Id });
        }

        //Delete a country with that id
        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (_context.Countries == null)
            {
                return NotFound();
            }
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            _context.Leaders.Remove(_context.Leaders.First(l => l.Id == country.LeaderId));
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(int? id)
        {
            return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
