using MarkShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarkShop.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        QLBanQuanAoDataContext db = new QLBanQuanAoDataContext();
        public ActionResult LayoutAdmin()
        {
            return View();
        }

        public ActionResult TrangPhucNam()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            var listTrangPhucNam = db.SanPhams.OrderBy(sp => sp.MaSP).ToList();
            return View(listTrangPhucNam);
        }

        public ActionResult TrangPhucNu(int page = 1, int pageSize = 12)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            var trangPhucNu = new Product();
            var model = trangPhucNu.ListAll(page, pageSize);
            return View(model);
        }

        public ActionResult DanhMucCacSanPham(int page = 1, int pageSize = 12)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            var sanPham = new Product();
            var model = sanPham.ListAll(page, pageSize);
            return View(model);
        }

        public ActionResult DangXuat()
        {
            Session["Admin"] = null;
            return RedirectToAction("Home", "Home");
        }
    }
}