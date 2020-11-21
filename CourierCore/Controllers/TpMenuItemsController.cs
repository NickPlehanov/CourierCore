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
    public class TpMenuItemsController : ControllerBase {
        private readonly TpdoriosContext _context;

        public TpMenuItemsController(TpdoriosContext context) {
            _context = context;
        }

        // GET: api/TpMenuItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TpMenuItems>>> GetTpMenuItems() {
            return await _context.TpMenuItems.ToListAsync();
        }

        // GET: api/TpMenuItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TpMenuItems>> GetTpMenuItems(Guid id) {
            var tpMenuItems = await _context.TpMenuItems.FindAsync(id);

            if(tpMenuItems == null) {
                return NotFound();
            }

            return tpMenuItems;
        }

        // PUT: api/TpMenuItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTpMenuItems(Guid id,TpMenuItems tpMenuItems) {
            if(id != tpMenuItems.MitmId) {
                return BadRequest();
            }

            _context.Entry(tpMenuItems).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) {
                if(!TpMenuItemsExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TpMenuItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TpMenuItems>> PostTpMenuItems(TpMenuItems tpMenuItems) {
            _context.TpMenuItems.Add(tpMenuItems);
            await _context.Database.ExecuteSqlRawAsync("tpsrv_logon",new SqlParameter("@Login","sa"),new SqlParameter("@Password","tillypad"));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTpMenuItems",new { id = tpMenuItems.MitmId },tpMenuItems);
        }

        // DELETE: api/TpMenuItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TpMenuItems>> DeleteTpMenuItems(Guid id) {
            var tpMenuItems = await _context.TpMenuItems.FindAsync(id);
            if(tpMenuItems == null) {
                return NotFound();
            }

            _context.TpMenuItems.Remove(tpMenuItems);
            await _context.SaveChangesAsync();

            return tpMenuItems;
        }

        private bool TpMenuItemsExists(Guid id) {
            return _context.TpMenuItems.Any(e => e.MitmId == id);
        }
    }
}
