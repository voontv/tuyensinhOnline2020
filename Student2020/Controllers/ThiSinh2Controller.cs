using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student2020.Configs;
using Student2020.Handler;
using Student2020.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IOFile = System.IO.File;

namespace Student2020.Controllers
{
    [Route("api/ThiSinh2")]
    [TypeFilter(typeof(IPFilterAttribute))]
    public class ThiSinh2Controller: ControllerBase
    {

        private readonly NhapHoc2020Context context;
        private readonly AppConfig appConfig;

        public ThiSinh2Controller(NhapHoc2020Context context, AppConfig appConfig)
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
        public async Task<ActionResult<ThiSinh>> GetThiSinh(string cmnd)
        {
            var thiSinh = await FindThiSinh(cmnd);

            if (!string.IsNullOrEmpty(thiSinh.FileGcn))
            {
                thiSinh.FileGcn = "api/thisinhs/download/" + Path.GetFileName(thiSinh.FileGcn);
            }

            thiSinh.CmndImg = "api/thisinhs/download-pdf/" + cmnd + ".pdf";

            return thiSinh;
        }

        private IActionResult GetFile(string folder, string fileName)
        {
            var path = Path.Combine(folder, fileName);
            if (IOFile.Exists(path))
            {
                var stream = IOFile.OpenRead(path);
                return File(stream, "application/octet-stream", fileName);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("download/{fileName}")]
        public IActionResult DownloadImage(string fileName)
        {
            return GetFile(appConfig.ImagePath, fileName);
        }

        [HttpGet]
        [Route("download-pdf/{fileName}")]
        public IActionResult DownloadPdf(string fileName)
        {
            return GetFile(appConfig.DocumentPath, fileName);
        }

    }
}
