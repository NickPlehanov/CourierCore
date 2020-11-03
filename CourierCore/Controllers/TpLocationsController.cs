using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourierCore.Data;
using CourierCore.Models;
using Microsoft.Data.SqlClient;

namespace CourierCore.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TpLocationsController : ControllerBase {
        private readonly TpdoriosContext _context;

        public TpLocationsController(TpdoriosContext context) {
            _context = context;
        }

        // GET: api/TpLocations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TpLocations>>> GetTpLocations() {
            return await _context.TpLocations.ToListAsync();
        }

        // GET: api/TpLocations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TpLocations>> GetTpLocations(Guid id) {
            var tpLocations = await _context.TpLocations.FindAsync(id);

            if(tpLocations == null) {
                return NotFound();
            }

            return tpLocations;
        }

        // PUT: api/TpLocations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTpLocations(Guid id,TpLocations tpLocations) {
            if(id != tpLocations.LocId) {
                return BadRequest();
            }

            _context.Entry(tpLocations).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) {
                if(!TpLocationsExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TpLocations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TpLocations>> PostTpLocations(TpLocations tpLocations) {
            _context.TpLocations.Add(tpLocations);
            try {
                await _context.Database.ExecuteSqlCommandAsync("tpsrv_logon",new SqlParameter("@Login","sa"),new SqlParameter("@Password","tillypad"));
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException) {
                if(TpLocationsExists(tpLocations.LocId)) {
                    return Conflict();
                }
                else {
                    throw;
                }
            }

            return CreatedAtAction("GetTpLocations",new { id = tpLocations.LocId },tpLocations);
        }

        // DELETE: api/TpLocations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TpLocations>> DeleteTpLocations(Guid id) {
            var tpLocations = await _context.TpLocations.FindAsync(id);
            if(tpLocations == null) {
                return NotFound();
            }

            _context.TpLocations.Remove(tpLocations);
            await _context.SaveChangesAsync();

            return tpLocations;
        }

        private bool TpLocationsExists(Guid id) {
            return _context.TpLocations.Any(e => e.LocId == id);
        }
    }
}
