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
    public class TpMenuVolumeTypesController : ControllerBase {
        private readonly TpdoriosContext _context;

        public TpMenuVolumeTypesController(TpdoriosContext context) {
            _context = context;
        }

        // GET: api/TpMenuVolumeTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TpMenuVolumeTypes>>> GetTpMenuVolumeTypes() {
            return await _context.TpMenuVolumeTypes.ToListAsync();
        }

        // GET: api/TpMenuVolumeTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TpMenuVolumeTypes>> GetTpMenuVolumeTypes(Guid id) {
            var tpMenuVolumeTypes = await _context.TpMenuVolumeTypes.FindAsync(id);

            if(tpMenuVolumeTypes == null) {
                return NotFound();
            }

            return tpMenuVolumeTypes;
        }

        // PUT: api/TpMenuVolumeTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTpMenuVolumeTypes(Guid id,TpMenuVolumeTypes tpMenuVolumeTypes) {
            if(id != tpMenuVolumeTypes.MvtpId) {
                return BadRequest();
            }

            _context.Entry(tpMenuVolumeTypes).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) {
                if(!TpMenuVolumeTypesExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TpMenuVolumeTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TpMenuVolumeTypes>> PostTpMenuVolumeTypes(TpMenuVolumeTypes tpMenuVolumeTypes) {
            _context.TpMenuVolumeTypes.Add(tpMenuVolumeTypes);
            await _context.Database.ExecuteSqlCommandAsync("tpsrv_logon",new SqlParameter("@Login","sa"),new SqlParameter("@Password","tillypad"));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTpMenuVolumeTypes",new { id = tpMenuVolumeTypes.MvtpId },tpMenuVolumeTypes);
        }

        // DELETE: api/TpMenuVolumeTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TpMenuVolumeTypes>> DeleteTpMenuVolumeTypes(Guid id) {
            var tpMenuVolumeTypes = await _context.TpMenuVolumeTypes.FindAsync(id);
            if(tpMenuVolumeTypes == null) {
                return NotFound();
            }

            _context.TpMenuVolumeTypes.Remove(tpMenuVolumeTypes);
            await _context.SaveChangesAsync();

            return tpMenuVolumeTypes;
        }

        private bool TpMenuVolumeTypesExists(Guid id) {
            return _context.TpMenuVolumeTypes.Any(e => e.MvtpId == id);
        }
    }
}
