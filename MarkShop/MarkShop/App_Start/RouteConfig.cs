using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MarkShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Home",
                url: "trang-chu",
                defaults: new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "thongBaoRong",
                url: "khong-tim-thay-san-pham",
                defaults: new { controller = "SanPham", action = "thongBaoRong", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "LichSu",
                url: "lich-su-giao-dich",
                defaults: new { controller = "User", action = "LichSu", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "DatHang",
                url: "dat-hang",
                defaults: new { controller = "GioHang", action = "DatHang", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "XacNhanDatHang",
                url: "xac-nhan-dat-hang",
                defaults: new { controller = "GioHang", action = "XacNhanDatHang", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "TrangPhucNam",
                url: "trang-phuc-nam",
                defaults: new { controller = "Admin", action = "TrangPhucNam", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "TrangPhucNu",
                url: "trang-phuc-nu",
                defaults: new { controller = "Admin", action = "TrangPhucNu", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "DangNhap",
                url: "dang-nhap-admin",
                defaults: new { controller = "DangNhap", action = "DangNhap", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "GioiThieu",
                url: "gioi-thieu",
                defaults: new { controller = "Home", action = "GioiThieu", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "QuanLiDonHang",
                url: "quan-li-don-hang",
                defaults: new { controller = "ThemXoaSua", action = "QuanLiDonHang", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "QuanLiKhachHang",
                url: "quan-li-khach-hang",
                defaults: new { controller = "ThemXoaSua", action = "QuanLiKhachHang", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}
