using MarkShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarkShop.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        QLBanQuanAoDataContext db = new QLBanQuanAoDataContext();
        public ActionResult DangKy()
        {
            return View();
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(Information user, FormCollection f)
        {
            if (!String.IsNullOrEmpty(user.UserName) && !String.IsNullOrEmpty(user.Password))
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(khachHang => khachHang.TaiKhoan == user.UserName && khachHang.MatKhau == user.Password);
                if (kh != null)
                {
                    Session["taikhoan"] = kh;
                    Session.Add("User", kh.TaiKhoan);
                    return RedirectToAction("SanPhamPartial", "SanPham");
                }
                else
                {
                    ViewBag.ThongBao = "Sai tên đăng nhập hoặc mật khẩu";
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(Information user)
        {
            try
            {
                // Kiểm tra trùng tài khoản
                KhachHang checkKH = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan.Equals(user.tendangnhap));
                if (checkKH != null)
                {
                    TempData["ErrorUserName"] = "Tên tài khoản đã được sử dụng";
                }
                else
                {
                    KhachHang kh = new KhachHang();
                    kh.TenKH = user.tenKhachHang;
                    kh.NgaySinh = DateTime.Parse(user.ngaySinh);
                    kh.GioiTinh = user.gioitinh;
                    kh.SDT = user.dienthoai;
                    kh.TaiKhoan = user.tendangnhap;
                    kh.MatKhau = user.matkhau;
                    kh.Email = user.email;
                    kh.DiaChi = user.diachi;
                    db.KhachHangs.InsertOnSubmit(kh);
                    db.SubmitChanges();
                }
                return RedirectToAction("DangNhap", "User");
            }
            catch
            {

            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["taikhoan"] = null;
            Session["GioHang"] = null;
            return RedirectToAction("Home", "Home");
        }
        public ActionResult LichSu()
        {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSP);

            // Nếu như chưa đăng nhập thì quay về trang đăng nhập trong trường hợp user tự động get URL đến trang lịch sử giao dịch
            if (Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "User");
            }
            else
            {
                // Lấy thông tin khách hàng đã đăng nhập
                KhachHang kh = (KhachHang)Session["taikhoan"];

                // Kiểm tra xem trong hoá đơn khách hàng đó đã đặt hàng hay chưa, nếu chưa đặt hàng thì sao xem lịch sử dc đúng k
                var checkKH = db.HoaDons.Where(n => n.MaKH.Equals(kh.MaKH)).ToList();
                if (checkKH.Count == 0)
                {
                    TempData["Message"] = "Bạn chưa có giao dịch nào";
                    return RedirectToAction("SanPhamPartial", "SanPham");
                }
                else
                { // Đã có trong hoá đơn => tức là đặt hàng rồi

                    // Kiểm tra và lấy hoá đơn + chi tiết hoá đơn ứng mới mã kh đã đăng nhập
                    var listCTGD = db.ChiTietHoaDons.Where(n => n.HoaDon.MaKH.Equals(kh.MaKH));
                    return View(listCTGD);
                }
            }
        }
        [HttpPost]
        public ActionResult HuyDonHang(int maSP, int maHD)
        {
            ChiTietHoaDon cthd = db.ChiTietHoaDons.Single(n => n.MaSP.Equals(maSP) && n.MaHD.Equals(maHD));
            db.ChiTietHoaDons.DeleteOnSubmit(cthd);
            db.SubmitChanges();
            HoaDon hd = db.HoaDons.SingleOrDefault(n => n.MaHD.Equals(maHD));
            var cthd2 = db.ChiTietHoaDons.Where(n => n.MaHD.Equals(hd.MaHD)).ToList();
            if (cthd2.Count == 0)
            {
                db.HoaDons.DeleteOnSubmit(hd);
                db.SubmitChanges();
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP.Equals(maSP));
            sp.SoLuongTon += cthd.SoLuong;
            db.SubmitChanges();
            return RedirectToAction("LichSu", "User");
        }
    }
}