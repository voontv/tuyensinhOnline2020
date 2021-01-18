using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Student2020.Configs;
using Student2020.Handler;
using Student2020.Models;
using Student2020.Utils;
using IOFile = System.IO.File;

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
        public async Task<ActionResult<ThiSinh>> GetThiSinh(string cmnd)
        {
            var thiSinh = await FindThiSinh(cmnd);

            if (!string.IsNullOrEmpty(thiSinh.FileGcn))
            {
                thiSinh.FileGcn = "api/thisinhs/download/" + CreateFileToken(Path.GetFileName(thiSinh.FileGcn));
            }

            thiSinh.CmndImg = "api/thisinhs/download-pdf/" + CreateFileToken(cmnd + ".pdf");

            return thiSinh;
        }


        [HttpGet("enc/{enc}")]
        public string Enc(string enc)
        {
            return CreateFileToken(enc);
        }


        private string CreateFileToken(string fileName)
        {
            var fileDownload = new FileDownload()
            {
                FileName = fileName,
                ValidFrom = DateTime.Now
            };

            var token = JsonConvert.SerializeObject(fileDownload);

            return HexaEncode.Encode(RC4Encrypt.Encrypt(token));
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

        private string DecryptToken(string token)
        {
            var decrypted = RC4Encrypt.Decrypt(HexaEncode.Decode(token));

            var file = JsonConvert.DeserializeObject<FileDownload>(decrypted);

            if(DateTime.Now - file.ValidFrom > TimeSpan.FromHours(1))
            {
                throw new BadRequestException("Invalid token");
            }

            return file.FileName;
        }

        [HttpGet]
        [Route("download/{fileName}")]
        public IActionResult DownloadImage(string fileName)
        {
            return GetFile(appConfig.ImagePath, DecryptToken(fileName));
        }

        [HttpGet]
        [Route("download-pdf/{fileName}")]
        public IActionResult DownloadPdf(string fileName)
        {
            return GetFile(appConfig.DocumentPath, DecryptToken(fileName));
        }

        [HttpPut]
        public async Task UpdateThutuc([FromBody] InforNewSinhVien inforNewSinhVien)
        {
            var existing = await FindThiSinh(inforNewSinhVien.CMND);

            if (existing.XacNhanGcn is {})
            {
                throw new BadRequestException("Hệ thống đã xác nhận thông tin của bạn trước đó. Bạn không được phép nhập lại thông tin.");
            }

            existing.DiaChi = inforNewSinhVien.DiaChi;

            if(!string.IsNullOrEmpty(inforNewSinhVien.NganhChon))
            {
                existing.MaChon = inforNewSinhVien.NganhChon;
            }
            

            if (inforNewSinhVien.ImageData != null)
            {
                var base64Mark = "base64,";
                var fileContent = Convert.FromBase64String(inforNewSinhVien.ImageData.Substring(inforNewSinhVien.ImageData.IndexOf(base64Mark) + base64Mark.Length));

                foreach (var existingFile in Directory.EnumerateFiles(appConfig.ImagePath, inforNewSinhVien.CMND + ".*"))
                {
                    IOFile.Delete(existingFile);
                }

                var fileName = Path.Combine(appConfig.ImagePath, inforNewSinhVien.CMND + Path.GetExtension(inforNewSinhVien.ImageFileName));
                await IOFile.WriteAllBytesAsync(fileName, fileContent);
                existing.FileGcn = fileName;
                existing.NgayNopGcn = DateTime.Now;
            }

            await context.SaveChangesAsync();
        }
    }
}
