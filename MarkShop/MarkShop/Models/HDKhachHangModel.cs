using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarkShop.Models
{
    public class HDKhachHangModel
    {
        public int MaKH { get; set; }
        public string TenKH { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string SoDienThoai { get; set; }
        public int MaHD { get; set; }
        public bool TinhTrang { get; set; }
    }
}