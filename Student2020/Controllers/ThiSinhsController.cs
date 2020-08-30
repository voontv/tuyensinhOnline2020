using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student2020.Handler;
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

        private async Task<ThiSinh> FindThiSinh(string cmnd)
        {
            return await _context.ThiSinh.FirstOrDefaultAsync(x => x.Cmnd == cmnd)
                ?? throw new NotFoundException("Không tìm thấy thí sinh có cmnd " + cmnd);
        }

        [HttpGet("{cmnd}")]
        public async Task<ActionResult<ThiSinh>> GetThiSinh(string cmnd)
        {
            return await FindThiSinh(cmnd);
        }

        [HttpPut]
        public async Task UpdateThutuc([FromBody] InforNewSinhVien inforNewSinhVien)
        {
            var existing = await FindThiSinh(inforNewSinhVien.CMND);

            existing.DiaChi = inforNewSinhVien.DiaChi;
            existing.PathImage = inforNewSinhVien.Image;
            existing.NgayNopGcn = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}
