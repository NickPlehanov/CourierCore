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

        // GET: api/TpGuests/id?id=5
        [HttpGet("id")]
        public async Task<ActionResult<TpGuests>> GetTpGuests([FromQuery]Guid id) {
            var tpGuests = await _context.TpGuests.FindAsync(id);

            if(tpGuests == null) {
                return NotFound();
            }

            return tpGuests;
        }
        // GET: api/TpGuests/gestByUsrID?courierID=5
        [HttpGet("gestByUsrID")]
        public IEnumerable<Guests_GuestDeliveries_Clients> GetGuestsByCourier([FromQuery]Guid courierID/*, [FromQuery] DateTime date*/) {
            //DateTime dt = DateTime.Parse(date.ToShortDateString());
            var gest = from gd in _context.TpGuestDeliveries
                       join g in _context.TpGuests on gd.GsdlvGestId equals g.GestId
                       join c in _context.TpClients on g.GestClntId equals c.ClntId
                       where gd.GsdlvUsrIdCourier == courierID 
                       && gd.GsdlvDlvrmtId==1
                       //&& g.GestDateOpen.Date==dt
                       //&& gd.GsdlvDlvrstId != 0
                       select new {
                           gest_ID = g.GestId,
                           gestDateOpen = g.GestDateOpen,
                           gestDateClose = g.GestDateClose,
                           gestName = g.GestName,
                           gestComment = g.GestComment,
                           courierID = gd.GsdlvUsrIdCourier,
                           clntID = c.ClntId,
                           clntName=c.ClntName,
                           clntPhones = c.ClntPhones
                       };
            List<Guests_GuestDeliveries_Clients> g_gd_c = new List<Guests_GuestDeliveries_Clients>(); 
            foreach(var item in gest) {
                g_gd_c.Add(new Guests_GuestDeliveries_Clients() {
                    GestID=item.gest_ID,
                    GestDateClose=item.gestDateClose,
                    GestDateOpen=item.gestDateOpen,
                    GestName=item.gestName,
                    GestComment=item.gestComment,
                    ClntID=item.clntID,
                    ClntName=item.clntName,
                    ClntPhones=item.clntPhones,
                    CourierID=item.courierID
                });
                break;
            }
            return g_gd_c;
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
            await _context.Database.ExecuteSqlRawAsync("tpsrv_logon",new SqlParameter("@Login","sa"),new SqlParameter("@Password","tillypad"));
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
