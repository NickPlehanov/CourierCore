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
    public class TpOrdersController : ControllerBase {
        private readonly TpdoriosContext _context;

        public TpOrdersController(TpdoriosContext context) {
            _context = context;
        }

        // GET: api/TpOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TpOrders>>> GetTpOrders() {
            return await _context.TpOrders.ToListAsync();
        }

        // GET: api/TpOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TpOrders>> GetTpOrders(Guid id) {
            var tpOrders = await _context.TpOrders.FindAsync(id);

            if(tpOrders == null) {
                return NotFound();
            }

            return tpOrders;
        }

        // PUT: api/TpOrders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTpOrders(Guid id,TpOrders tpOrders) {
            if(id != tpOrders.OrdrId) {
                return BadRequest();
            }

            _context.Entry(tpOrders).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) {
                if(!TpOrdersExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TpOrders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TpOrders>> PostTpOrders(TpOrders tpOrders) {
            _context.TpOrders.Add(tpOrders);
            await _context.Database.ExecuteSqlCommandAsync("tpsrv_logon",new SqlParameter("@Login","sa"),new SqlParameter("@Password","tillypad"));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTpOrders",new { id = tpOrders.OrdrId },tpOrders);
        }

        // DELETE: api/TpOrders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TpOrders>> DeleteTpOrders(Guid id) {
            var tpOrders = await _context.TpOrders.FindAsync(id);
            if(tpOrders == null) {
                return NotFound();
            }

            _context.TpOrders.Remove(tpOrders);
            await _context.SaveChangesAsync();

            return tpOrders;
        }

        private bool TpOrdersExists(Guid id) {
            return _context.TpOrders.Any(e => e.OrdrId == id);
        }
    }
}
