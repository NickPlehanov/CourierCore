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
    public class TpGuestDeliveriesController : ControllerBase {
        private readonly TpdoriosContext _context;

        public TpGuestDeliveriesController(TpdoriosContext context) {
            _context = context;
        }

        // GET: api/TpGuestDeliveries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TpGuestDeliveries>>> GetTpGuestDeliveries() {
            return await _context.TpGuestDeliveries.ToListAsync();
        }

        // GET: api/TpGuestDeliveries/5
        [HttpGet("id")]
        public async Task<ActionResult<TpGuestDeliveries>> GetTpGuestDeliveries([FromQuery] Guid id) {
            var tpGuestDeliveries = await _context.TpGuestDeliveries.FindAsync(id);

            if(tpGuestDeliveries == null) {
                return NotFound();
            }

            return tpGuestDeliveries;
        }

        // PUT: api/TpGuestDeliveries/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut()]
        public async Task<IActionResult> PutTpGuestDeliveries([FromBody]TpGuestDeliveries tpGuestDeliveries) {
            //if(id != tpGuestDeliveries.GsdlvGestId) {
            //    return BadRequest();
            //}

            _context.Entry(tpGuestDeliveries).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) {
                //if(!TpGuestDeliveriesExists(id)) {
                    return BadRequest();
                //}
                //else {
                //    throw;
                //}
            }

            return Accepted();
        }


        // POST: api/TpGuestDeliveries
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TpGuestDeliveries>> PostTpGuestDeliveries(TpGuestDeliveries tpGuestDeliveries) {
            _context.TpGuestDeliveries.Add(tpGuestDeliveries);
            try {
                await _context.Database.ExecuteSqlRawAsync("tpsrv_logon",new SqlParameter("@Login","sa"),new SqlParameter("@Password","tillypad"));
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException) {
                if(TpGuestDeliveriesExists(tpGuestDeliveries.GsdlvGestId)) {
                    return Conflict();
                }
                else {
                    throw;
                }
            }

            return CreatedAtAction("GetTpGuestDeliveries",new { id = tpGuestDeliveries.GsdlvGestId },tpGuestDeliveries);
        }

        // DELETE: api/TpGuestDeliveries/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TpGuestDeliveries>> DeleteTpGuestDeliveries(Guid id) {
            var tpGuestDeliveries = await _context.TpGuestDeliveries.FindAsync(id);
            if(tpGuestDeliveries == null) {
                return NotFound();
            }

            _context.TpGuestDeliveries.Remove(tpGuestDeliveries);
            await _context.SaveChangesAsync();

            return tpGuestDeliveries;
        }

        private bool TpGuestDeliveriesExists(Guid id) {
            return _context.TpGuestDeliveries.Any(e => e.GsdlvGestId == id);
        }
    }
}
