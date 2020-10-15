using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CustomerDawacoIT.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IOFile = System.IO.File;

namespace CustomerDawacoIT.Controllers
{
    [Route("api/FileUploads")]
    [ApiController]
    public class FileUploadsController : ControllerBase
    {
        public static IWebHostEnvironment _webHostEnvironment;

        public FileUploadsController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public string Post([FromForm] FileUpload objectFile)
        {
            try
            {
                if(objectFile.files.Length > 0)
                {
                    string path = _webHostEnvironment.WebRootPath + "\\uploads\\";

                    if(!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream fileStream = System.IO.File.Create(path + objectFile.files.FileName))
                    {
                        objectFile.files.CopyTo(fileStream);
                        fileStream.Flush();
                        return "Succeed";
                    }    
                }
                else
                {
                    return "Failed";
                }    

            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        [Route("download-pdf/{fileName}")]
        public IActionResult DownloadPdf(string fileName)
        {
            return GetFile(_webHostEnvironment.WebRootPath + "\\uploads\\", fileName);
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
    }
}
