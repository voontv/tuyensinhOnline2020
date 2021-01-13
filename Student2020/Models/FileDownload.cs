using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student2020.Models
{
    public class FileDownload
    {
        [JsonProperty("f")]
        public string FileName { get; set; }

        [JsonProperty("v")]
        public DateTime ValidFrom { get; set; }
    }
}
