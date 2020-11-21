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

        //// GET: api/TpOrders/id?id=5
        //[HttpGet("id")]
        //public async Task<ActionResult<TpOrders>> GetTpOrders([FromQuery]Guid id) {
        //    var tpOrders = await _context.TpOrders.FindAsync(id);

        //    if(tpOrders == null) {
        //        return NotFound();
        //    }

        //    return tpOrders;
        //}
        // GET: api/TpOrders/OrdrInfo?id=5
        [HttpGet("OrdrInfo")]
        public async Task<ActionResult<IEnumerable<Orders_OrderItem_MenuItems>>> GetTpOrders([FromQuery] Guid gest_ID) {
            var ordr_info = from o in _context.TpOrders
                            join oi in _context.TpOrderItems on o.OrdrId equals oi.OritOrdrId
                            join mi in _context.TpMenuItems on oi.OritMitmId equals mi.MitmId
                            where o.OrdrGestId == gest_ID
                            select new {
                                ordrGestID = o.OrdrGestId,
                                ordrID = o.OrdrId,
                                oritMitmID = oi.OritMitmId,
                                oritVolume = oi.OritVolume,
                                oritCount = oi.OritCount,
                                mitmName = mi.MitmName,
                                oritPrice=oi.OritPrice
                            };
            List<Orders_OrderItem_MenuItems> o_oi_mi = new List<Orders_OrderItem_MenuItems>();
            foreach(var item in ordr_info) 
                o_oi_mi.Add(new Orders_OrderItem_MenuItems() {
                    OrdrGestID=item.ordrGestID,
                    OrdrID=item.ordrID,
                    OrdrMitmID=item.oritMitmID,
                    OritVolume=item.oritVolume,
                    OritCount=item.oritCount,
                    OritPrice=item.oritPrice,
                    MitmName=item.mitmName
                });
            return o_oi_mi;
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
            await _context.Database.ExecuteSqlRawAsync("tpsrv_logon",new SqlParameter("@Login","sa"),new SqlParameter("@Password","tillypad"));
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
