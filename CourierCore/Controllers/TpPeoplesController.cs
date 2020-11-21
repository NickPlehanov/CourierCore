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
    public class TpPeoplesController : ControllerBase {
        private readonly TpdoriosContext _context;

        public TpPeoplesController(TpdoriosContext context) {
            _context = context;
        }

        // GET: api/TpPeoples
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TpPeople>>> GetTpPeople() {
            return await _context.TpPeople.ToListAsync();
        }

        // GET: api/TpPeoples/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TpPeople>> GetTpPeople(Guid id) {
            var tpPeople = await _context.TpPeople.FindAsync(id);

            if(tpPeople == null) {
                return NotFound();
            }

            return tpPeople;
        }

        // PUT: api/TpPeoples/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTpPeople(Guid id,TpPeople tpPeople) {
            if(id != tpPeople.PeplId) {
                return BadRequest();
            }

            _context.Entry(tpPeople).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) {
                if(!TpPeopleExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TpPeoples
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TpPeople>> PostTpPeople(TpPeople tpPeople) {
            _context.TpPeople.Add(tpPeople);
            await _context.Database.ExecuteSqlRawAsync("tpsrv_logon",new SqlParameter("@Login","sa"),new SqlParameter("@Password","tillypad"));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTpPeople",new { id = tpPeople.PeplId },tpPeople);
        }

        // DELETE: api/TpPeoples/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TpPeople>> DeleteTpPeople(Guid id) {
            var tpPeople = await _context.TpPeople.FindAsync(id);
            if(tpPeople == null) {
                return NotFound();
            }

            _context.TpPeople.Remove(tpPeople);
            await _context.SaveChangesAsync();

            return tpPeople;
        }

        private bool TpPeopleExists(Guid id) {
            return _context.TpPeople.Any(e => e.PeplId == id);
        }
    }
}
