using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirplaneApi.Data;
using AirplaneApi.Models;

namespace AirplaneApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AirplanesController : ControllerBase
    {
        private readonly AirplaneContext _context;

        public AirplanesController(AirplaneContext context)
        {
            _context = context;
        }

        // GET: api/Airplanes
        [HttpGet]
        public IEnumerable<Airplane> GetAirplanes()
        {
            Console.WriteLine("Hey");
            return _context.Airplanes;
        }

        // GET: api/Airplanes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAirplane([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var airplane = await _context.Airplanes.FindAsync(id);

            if (airplane == null)
            {
                return NotFound();
            }

            return Ok(airplane);
        }

        // PUT: api/Airplanes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAirplane([FromRoute] int id, [FromBody] Airplane airplane)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != airplane.Id)
            {
                return BadRequest();
            }

            _context.Entry(airplane).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AirplaneExists(id))
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

        // POST: api/Airplanes
        [HttpPost]
        public async Task<IActionResult> PostAirplane([FromBody] Airplane airplane)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            airplane.CreationDate = DateTime.UtcNow;
            _context.Airplanes.Add(airplane);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAirplane", new { id = airplane.Id }, airplane);
        }

        // DELETE: api/Airplanes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAirplane([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var airplane = await _context.Airplanes.FindAsync(id);
            if (airplane == null)
            {
                return NotFound();
            }

            _context.Airplanes.Remove(airplane);
            await _context.SaveChangesAsync();

            return Ok(airplane);
        }

        private bool AirplaneExists(int id)
        {
            return _context.Airplanes.Any(e => e.Id == id);
        }
    }
}