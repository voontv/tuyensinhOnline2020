using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
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
        public async Task<string> UpdateThutuc([FromBody] InforNewSinhVien inforNewSinhVien)
        {

            if (await _context.ThiSinh.FirstOrDefaultAsync(x => x.Cmnd == inforNewSinhVien.CMND) is { } existing)
            {
                var stringImage = inforNewSinhVien.Image;
                existing.DiaChi = inforNewSinhVien.DiaChi;
                existing.PathImage = inforNewSinhVien.Image;
                existing.NgayNopGcn = DateTime.UtcNow;               
                await _context.SaveChangesAsync();
                return "Upload thanh cong";
            }
            else
            {
                return "That bai vui long thu lai";
            }
            
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

       /* public bool SaveImage(string ImgStr, string ImgName)
        {
            String path = System.Web.HttpContext.Current.Server.MapPath("~/ImageStorage"); //Path

            //Check if directory exist
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            string imageName = ImgName + ".jpg";

            //set the image path
            string imgPath = Path.Combine(path, imageName);

            byte[] imageBytes = Convert.FromBase64String(ImgStr);

            File.WriteAllBytes(imgPath, imageBytes);

            return true;
        }*/
    }
}
