using System;
using System.Collections.Generic;

namespace Student2020.Models
{
    public partial class ThiSinh
    {
        public int Id { get; set; }
        public string Cmnd { get; set; }
        public string HotenTs { get; set; }
        public string DienThoai { get; set; }
        public string MaNganh { get; set; }
        public string TenNganh { get; set; }
        public string FileGcn { get; set; }
        public DateTime? NgayNopGcn { get; set; }
        public string DiaChi { get; set; }
        public DateTime? XacNhanGcn { get; set; }
        public DateTime? XacNhanBoHs { get; set; }
        public DateTime? XacNhanTien { get; set; }
        public string Note1 { get; set; }
        public string UsXacNhanGcn { get; set; }
        public string UsXacNhanBoHs { get; set; }
        public string UsXacNhanTien { get; set; }

        public int? SoTien { set; get; }
        public DateTime? NgaySinh { get; set; }
    }
}
