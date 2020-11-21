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
    public class TpClientsController : ControllerBase {
        private readonly TpdoriosContext _context;

        public TpClientsController(TpdoriosContext context) {
            _context = context;
        }

        // GET: api/TpClients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TpClients>>> GetTpClients() {
            return await _context.TpClients.ToListAsync();
        }

        // GET: api/TpClients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TpClients>> GetTpClients(Guid id) {
            var tpClients = await _context.TpClients.FindAsync(id);

            if(tpClients == null) {
                return NotFound();
            }

            return tpClients;
        }

        // PUT: api/TpClients/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTpClients(Guid id,TpClients tpClients) {
            if(id != tpClients.ClntId) {
                return BadRequest();
            }

            _context.Entry(tpClients).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) {
                if(!TpClientsExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TpClients
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TpClients>> PostTpClients(TpClients tpClients) {
            _context.TpClients.Add(tpClients);
            await _context.Database.ExecuteSqlRawAsync("tpsrv_logon",new SqlParameter("@Login","sa"),new SqlParameter("@Password","tillypad"));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTpClients",new { id = tpClients.ClntId },tpClients);
        }

        // DELETE: api/TpClients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TpClients>> DeleteTpClients(Guid id) {
            var tpClients = await _context.TpClients.FindAsync(id);
            if(tpClients == null) {
                return NotFound();
            }

            _context.TpClients.Remove(tpClients);
            await _context.SaveChangesAsync();

            return tpClients;
        }

        private bool TpClientsExists(Guid id) {
            return _context.TpClients.Any(e => e.ClntId == id);
        }
    }
}
