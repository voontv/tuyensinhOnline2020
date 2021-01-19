using Microsoft.AspNetCore.Mvc;
using Student2020.Configs;
using Student2020.ForumDb;
using Student2020.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Student2020.Controllers
{
    [Route("api/documents")]
    public class DocumentController: ControllerBase
    {
        private readonly ForumDbContext dbContext;
        private readonly AppConfig appConfig;

        public DocumentController(ForumDbContext dbContext, AppConfig appConfig)
        {
            this.dbContext = dbContext;
            this.appConfig = appConfig;
        }

        [HttpGet("enc")]
        public string Enc(string enc)
        {
            return HexaEncode.Encode(RC4Encrypt.Encrypt(enc));
        }

        [HttpGet("{mssv}/{path}")]
        public IActionResult GetUsers(string mssv, string path)
        {
            if(path.Length == 0 )
            {
                return BadRequest();
            }
            var invalidTokens = new[] { ":", ".." };
            if (invalidTokens.Any(path.Contains))
            {
                return BadRequest();
            }

            var decrypted = RC4Encrypt.Decrypt(HexaEncode.Decode(path));

            if (dbContext.Users.Any(x => x.Name == mssv))
            {
                var file = Path.Combine(appConfig.SharedDocumentPath, decrypted);
                if (System.IO.File.Exists(file))
                {
                    var stream = System.IO.File.OpenRead(file);
                    return File(stream, "application/octet-stream", Path.GetFileName(file));
                }

                return NotFound();
            }

            return Forbid();
        }
    }
}
