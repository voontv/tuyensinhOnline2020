using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student2020.Models;

namespace Student2020.Controllers
{

    [Route("api/ThiSinhs")]
    [ApiController]
    public class ThiSinhsController : ControllerBase
    {
        private readonly NhapHoc2020Context _context;

        public ThiSinhsController(NhapHoc2020Context context)
        {
            _context = context;
        }

        // GET: api/ThiSinhs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThiSinh>>> GetThiSinh()
        {
            return await _context.ThiSinh.ToListAsync();
        }

        // GET: api/ThiSinhs/5
        [HttpGet("{cmnd}")]
        public async Task<ActionResult<ThiSinh>> GetThiSinh(string cmnd)
        {
            var thiSinh = await _context.ThiSinh.FirstOrDefaultAsync(x => x.Cmnd == cmnd);

            if (thiSinh == null)
            {
                return NotFound();
            }
            var test = thiSinh.Cmnd;
            return thiSinh;
        }

        // PUT: api/ThiSinhs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutThiSinh(string id, ThiSinh thiSinh)
        {
            if (id != thiSinh.Cmnd)
            {
                return BadRequest();
            }

            _context.Entry(thiSinh).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThiSinhExists(id))
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

        [HttpPut]
        [Route("sendInformation")]
        public async Task<IActionResult> UpdateThutuc([FromBody] InforNewSinhVien inforNewSinhVien)
        {

            if (await _context.ThiSinh.FirstOrDefaultAsync(x => x.Cmnd == inforNewSinhVien.CMND) is { } existing)
            {
                existing.DiaChi = inforNewSinhVien.DiaChi;
                existing.PathImage = inforNewSinhVien.Image;
                existing.NgayNopGcn = DateTime.UtcNow;               
                await _context.SaveChangesAsync();
            }        

            return NoContent();
        }

        // POST: api/ThiSinhs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ThiSinh>> PostThiSinh(ThiSinh thiSinh)
        {
            _context.ThiSinh.Add(thiSinh);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ThiSinhExists(thiSinh.Cmnd))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetThiSinh", new { id = thiSinh.Cmnd }, thiSinh);
        }

        // DELETE: api/ThiSinhs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ThiSinh>> DeleteThiSinh(string id)
        {
            var thiSinh = await _context.ThiSinh.FindAsync(id);
            if (thiSinh == null)
            {
                return NotFound();
            }

            _context.ThiSinh.Remove(thiSinh);
            await _context.SaveChangesAsync();

            return thiSinh;
        }
      

        private bool ThiSinhExists(string id)
        {
            return _context.ThiSinh.Any(e => e.Cmnd == id);
        }
    }
}
