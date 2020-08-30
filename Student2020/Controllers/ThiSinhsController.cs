using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student2020.Configs;
using Student2020.Handler;
using Student2020.Models;

namespace Student2020.Controllers
{

    [Route("api/ThiSinhs")]
    [ApiController]
    public class ThiSinhsController : ControllerBase
    {
        private readonly NhapHoc2020Context context;
        private readonly AppConfig appConfig;

        public ThiSinhsController(NhapHoc2020Context context, AppConfig appConfig)
        {
            this.context = context;
            this.appConfig = appConfig;
        }

        private async Task<ThiSinh> FindThiSinh(string cmnd)
        {
            return await context.ThiSinh.FirstOrDefaultAsync(x => x.Cmnd == cmnd)
                ?? throw new BadRequestException("Không tìm thấy thí sinh có cmnd " + cmnd);
        }

        [HttpGet("{cmnd}")]
        //[EnableCors("AllowOrigin")]
        public async Task<ActionResult<ThiSinh>> GetThiSinh(string cmnd)
        {
            return await FindThiSinh(cmnd);
        }

        [HttpPut]
        //[EnableCors("AllowOrigin")]
        public async Task UpdateThutuc([FromBody] InforNewSinhVien inforNewSinhVien)
        {
            var existing = await FindThiSinh(inforNewSinhVien.CMND);

            if (existing.XacNhanGcn is {})
            {
                throw new BadRequestException("Hệ thống đã xác nhận thông tin của bạn trước đó. Bạn không được phép nhập lại thông tin.");
            }

            existing.DiaChi = inforNewSinhVien.DiaChi;
            existing.NgayNopGcn = DateTime.Now;

            if (inforNewSinhVien.ImageData != null)
            {
                var fileName = inforNewSinhVien.CMND + "." + Path.GetExtension(inforNewSinhVien.ImageFileName);
                var fileContent = Convert.FromBase64String(inforNewSinhVien.ImageData);

                foreach (var existingFile in Directory.EnumerateFiles(appConfig.DataPath, inforNewSinhVien.CMND + ".*"))
                {
                    Directory.Delete(existingFile);
                }

                await System.IO.File.WriteAllBytesAsync(fileName, fileContent);
                existing.FileGcn = fileName;
            }

            await context.SaveChangesAsync();
        }
    }
}
