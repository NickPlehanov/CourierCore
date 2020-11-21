using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourierCore.Data;
using CourierCore.Models;

namespace CourierCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TpChecksController : ControllerBase
    {
        private readonly TpdoriosContext _context;

        public TpChecksController(TpdoriosContext context)
        {
            _context = context;
        }

        // GET: api/TpChecks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TpChecks>>> GetTpChecks()
        {
            return await _context.TpChecks.ToListAsync();
        }

        // GET: api/TpChecks/chkInfo?gestID=5
        [HttpGet("chkInfo")]
        public async Task<ActionResult<IEnumerable<Checks_Payments>>> GetTpChecks([FromQuery] Guid gestID)
        {
            var payInfo = from o in _context.TpOrders
                          join oi in _context.TpOrderItems on o.OrdrId equals oi.OritOrdrId
                          join pi in _context.TpPreCheckItems on oi.OritPcitId equals pi.PcitId
                          join p in _context.TpPreChecks on pi.PcitPrchId equals p.PrchId
                          join c in _context.TpChecks on p.PrchId equals c.ChckPrchId
                          join cp in _context.TpCheckPayments on c.ChckId equals cp.ChpyChckId
                          join pt in _context.TpPayTypes on cp.ChpyPytpId equals pt.PytpId
                          where o.OrdrGestId == gestID
                          select new {
                              chckID = c.ChckId,
                              pytpName = pt.PytpName,
                              chpySum = cp.ChpySum
                          };
            List<Checks_Payments> c_p = new List<Checks_Payments>();
            foreach(var item in payInfo.Distinct())
                c_p.Add(new Checks_Payments() { 
                    ChckID=item.chckID,
                    PytpName=item.pytpName,
                    ChpySum=item.chpySum
                });
            return c_p;
        }

        // PUT: api/TpChecks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTpChecks(Guid id, TpChecks tpChecks)
        {
            if (id != tpChecks.ChckId)
            {
                return BadRequest();
            }

            _context.Entry(tpChecks).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TpChecksExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TpChecks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TpChecks>> PostTpChecks(TpChecks tpChecks)
        {
            _context.TpChecks.Add(tpChecks);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTpChecks", new { id = tpChecks.ChckId }, tpChecks);
        }

        // DELETE: api/TpChecks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TpChecks>> DeleteTpChecks(Guid id)
        {
            var tpChecks = await _context.TpChecks.FindAsync(id);
            if (tpChecks == null)
            {
                return NotFound();
            }

            _context.TpChecks.Remove(tpChecks);
            await _context.SaveChangesAsync();

            return tpChecks;
        }

        private bool TpChecksExists(Guid id)
        {
            return _context.TpChecks.Any(e => e.ChckId == id);
        }
    }
}
