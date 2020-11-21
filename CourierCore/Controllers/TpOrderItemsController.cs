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
    public class TpOrderItemsController : ControllerBase {
        private readonly TpdoriosContext _context;

        public TpOrderItemsController(TpdoriosContext context) {
            _context = context;
        }

        // GET: api/TpOrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TpOrderItems>>> GetTpOrderItems() {
            return await _context.TpOrderItems.ToListAsync();
        }

        // GET: api/TpOrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TpOrderItems>> GetTpOrderItems(Guid id) {
            var tpOrderItems = await _context.TpOrderItems.FindAsync(id);

            if(tpOrderItems == null) {
                return NotFound();
            }

            return tpOrderItems;
        }

        // PUT: api/TpOrderItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTpOrderItems(Guid id,TpOrderItems tpOrderItems) {
            if(id != tpOrderItems.OritId) {
                return BadRequest();
            }

            _context.Entry(tpOrderItems).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) {
                if(!TpOrderItemsExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TpOrderItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TpOrderItems>> PostTpOrderItems(TpOrderItems tpOrderItems) {
            _context.TpOrderItems.Add(tpOrderItems);
            await _context.Database.ExecuteSqlRawAsync("tpsrv_logon",new SqlParameter("@Login","sa"),new SqlParameter("@Password","tillypad"));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTpOrderItems",new { id = tpOrderItems.OritId },tpOrderItems);
        }

        // DELETE: api/TpOrderItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TpOrderItems>> DeleteTpOrderItems(Guid id) {
            var tpOrderItems = await _context.TpOrderItems.FindAsync(id);
            if(tpOrderItems == null) {
                return NotFound();
            }

            _context.TpOrderItems.Remove(tpOrderItems);
            await _context.SaveChangesAsync();

            return tpOrderItems;
        }

        private bool TpOrderItemsExists(Guid id) {
            return _context.TpOrderItems.Any(e => e.OritId == id);
        }
    }
}
