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
    public class TpUsersController : ControllerBase {
        private readonly TpdoriosContext _context;

        public TpUsersController(TpdoriosContext context) {
            _context = context;
        }

        // GET: api/TpUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TpUsers>>> GetTpUsers() {
            return await _context.TpUsers.ToListAsync();
        }

        // GET: api/TpUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TpUsers>> GetTpUsers(Guid id) {
            var tpUsers = await _context.TpUsers.FindAsync(id);

            if(tpUsers == null) {
                return NotFound();
            }

            return tpUsers;
        }

        // PUT: api/TpUsers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTpUsers(Guid id,TpUsers tpUsers) {
            if(id != tpUsers.UsrId) {
                return BadRequest();
            }

            _context.Entry(tpUsers).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) {
                if(!TpUsersExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TpUsers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TpUsers>> PostTpUsers(TpUsers tpUsers) {
            _context.TpUsers.Add(tpUsers);
            await _context.Database.ExecuteSqlCommandAsync("tpsrv_logon",new SqlParameter("@Login","sa"),new SqlParameter("@Password","tillypad"));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTpUsers",new { id = tpUsers.UsrId },tpUsers);
        }

        // DELETE: api/TpUsers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TpUsers>> DeleteTpUsers(Guid id) {
            var tpUsers = await _context.TpUsers.FindAsync(id);
            if(tpUsers == null) {
                return NotFound();
            }

            _context.TpUsers.Remove(tpUsers);
            await _context.SaveChangesAsync();

            return tpUsers;
        }

        private bool TpUsersExists(Guid id) {
            return _context.TpUsers.Any(e => e.UsrId == id);
        }
    }
}
