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
    public class TpUserLocationPresencesController : ControllerBase {
        private readonly TpdoriosContext _context;

        public TpUserLocationPresencesController(TpdoriosContext context) {
            _context = context;
        }

        // GET: api/TpUserLocationPresences
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<TpUserLocationPresence>>> GetTpUserLocationPresence() {
        //    return await _context.TpUserLocationPresence.ToListAsync();
        //}

        // GET: api/TpUserLocationPresences/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<TpUserLocationPresence>> GetTpUserLocationPresence(Guid id) {
        //    var tpUserLocationPresence = await _context.TpUserLocationPresence.FindAsync(id);

        //    if(tpUserLocationPresence == null) {
        //        return NotFound();
        //    }

        //    return tpUserLocationPresence;
        //}
        // GET: api/TpUserLocationPresences/5
        [HttpGet("{usr_id}")]
        public IEnumerable<TpUserLocationPresence> GetTpUserLocationPresence([FromQuery]string usr_id) {
            Guid _usrID = Guid.TryParse(usr_id,out _) ? Guid.Parse(usr_id) : Guid.Empty;
            if(_usrID == Guid.Empty)
                return (IEnumerable<TpUserLocationPresence>)NotFound();
            else {
                var tpUserLocationPresence = _context.TpUserLocationPresence.Where(x => x.UslpUsrId == _usrID && x.UslpDateEnd==null);

                if(tpUserLocationPresence == null) {
                    return (IEnumerable<TpUserLocationPresence>)NotFound();
                }

                return (IEnumerable<TpUserLocationPresence>)tpUserLocationPresence;
            }
        }

        // PUT: api/TpUserLocationPresences/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTpUserLocationPresence(Guid id,TpUserLocationPresence tpUserLocationPresence) {
        //    if(id != tpUserLocationPresence.UslpId) {
        //        return BadRequest();
        //    }

        //    _context.Entry(tpUserLocationPresence).State = EntityState.Modified;

        //    try {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch(DbUpdateConcurrencyException) {
        //        if(!TpUserLocationPresenceExists(id)) {
        //            return NotFound();
        //        }
        //        else {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}
        [HttpPut]
        public async Task<IActionResult> PutTpUserLocationPresence([FromBody] TpUserLocationPresence tpUserLocationPresence) {
            //if(id != tpUserLocationPresence.UslpId) {
            //    return BadRequest();
            //}

            _context.Entry(tpUserLocationPresence).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) {
                if(!TpUserLocationPresenceExists(tpUserLocationPresence.UslpId)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return Accepted();
        }


        // POST: api/TpUserLocationPresences
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TpUserLocationPresence>> PostTpUserLocationPresence([FromBody] TpUserLocationPresence tpUserLocationPresence) {
            _context.TpUserLocationPresence.Add(tpUserLocationPresence);
            try {
                await _context.Database.ExecuteSqlRawAsync("tpsrv_logon",new SqlParameter("@Login","sa"),new SqlParameter("@Password","tillypad"));
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException) {
                if(TpUserLocationPresenceExists(tpUserLocationPresence.UslpId)) {
                    return Conflict();
                }
                else {
                    throw;
                }
            }

            return CreatedAtAction("GetTpUserLocationPresence",new { id = tpUserLocationPresence.UslpId },tpUserLocationPresence);
        }

        // DELETE: api/TpUserLocationPresences/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TpUserLocationPresence>> DeleteTpUserLocationPresence(Guid id) {
            var tpUserLocationPresence = await _context.TpUserLocationPresence.FindAsync(id);
            if(tpUserLocationPresence == null) {
                return NotFound();
            }

            _context.TpUserLocationPresence.Remove(tpUserLocationPresence);
            await _context.SaveChangesAsync();

            return tpUserLocationPresence;
        }

        private bool TpUserLocationPresenceExists(Guid id) {
            return _context.TpUserLocationPresence.Any(e => e.UslpId == id);
        }
    }
}
