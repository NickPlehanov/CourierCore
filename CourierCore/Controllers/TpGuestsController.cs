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
    public class TpGuestsController : ControllerBase {
        private readonly TpdoriosContext _context;

        public TpGuestsController(TpdoriosContext context) {
            _context = context;
        }

        // GET: api/TpGuests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TpGuests>>> GetTpGuests() {
            return await _context.TpGuests.ToListAsync();
        }

        // GET: api/TpGuests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TpGuests>> GetTpGuests(Guid id) {
            var tpGuests = await _context.TpGuests.FindAsync(id);

            if(tpGuests == null) {
                return NotFound();
            }

            return tpGuests;
        }

        // PUT: api/TpGuests/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTpGuests(Guid id,TpGuests tpGuests) {
            if(id != tpGuests.GestId) {
                return BadRequest();
            }

            _context.Entry(tpGuests).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) {
                if(!TpGuestsExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TpGuests
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TpGuests>> PostTpGuests(TpGuests tpGuests) {
            _context.TpGuests.Add(tpGuests);
            await _context.Database.ExecuteSqlCommandAsync("tpsrv_logon",new SqlParameter("@Login","sa"),new SqlParameter("@Password","tillypad"));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTpGuests",new { id = tpGuests.GestId },tpGuests);
        }

        // DELETE: api/TpGuests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TpGuests>> DeleteTpGuests(Guid id) {
            var tpGuests = await _context.TpGuests.FindAsync(id);
            if(tpGuests == null) {
                return NotFound();
            }

            _context.TpGuests.Remove(tpGuests);
            await _context.SaveChangesAsync();

            return tpGuests;
        }

        private bool TpGuestsExists(Guid id) {
            return _context.TpGuests.Any(e => e.GestId == id);
        }
    }
}
