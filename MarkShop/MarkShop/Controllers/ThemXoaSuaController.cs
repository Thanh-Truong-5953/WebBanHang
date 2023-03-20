using MarkShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarkShop.Controllers
{
    public class ThemXoaSuaController : Controller
    {
        // GET: ThemXoaSua
        QLBanQuanAoDataContext db = new QLBanQuanAoDataContext();

        [HttpGet]
        public ActionResult ThemSanPham()
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC");
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ThemSanPham(SanPham sp, HttpPostedFileBase fUpload)
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC");
            if (fUpload != null) // Đã chọn hình ảnh rồi
            {
                if (fUpload.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(fUpload.FileName); // Lấy tên hình ảnh
                    var path = Path.Combine(Server.MapPath("/Images"), fileName); // Tìm đến thư mục Images trong MarkShop
                    if (System.IO.File.Exists(path)) // Nếu trong thư mục Images có hình ảnh đó rồi thì xuất thông báo
                    {
                        TempData["UploadFail"] = "Hình ảnh này đã tồn tại!";
                        return View();
                    }
                    else  // Chưa có hình ảnh trong thư mục Images
                    {
                        fUpload.SaveAs(path); // Lưu hình vừa thêm vào thư mục Images
                        sp.Anh = fUpload.FileName; // Cập nhật trong database
                    }
                }
            }
            else
            { // Chưa chọn hình ảnh
                TempData["UploadFail"] = "Vui lòng chọn hình ảnh!";
                return View();
            }
            db.SanPhams.InsertOnSubmit(sp);
            db.SubmitChanges();
            ModelState.Clear();
            TempData["Added"] = "Thêm sản phẩm thành công";
            return RedirectToAction("DanhMucCacSanPham", "Admin");
        }

        public ActionResult XoaSanPham(int maSP)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            SanPham sanPham = db.SanPhams.Single(ma => ma.MaSP == maSP);
            if (sanPham == null)
            {
                return HttpNotFound();
            }

            db.SanPhams.DeleteOnSubmit(sanPham);
            db.SubmitChanges();
            return RedirectToAction("DanhMucCacSanPham", "Admin");
        }

        public ActionResult ChiTietSanPham(int maSP)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            SanPham sanPham = db.SanPhams.Single(ma => ma.MaSP == maSP);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(sanPham);
            }
        }

        [HttpGet]
        public ActionResult SuaSanPham(int id)
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC");
            SanPham product = db.SanPhams.SingleOrDefault(n => n.MaSP.Equals(id));
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        //[ValidateInput(false)] // Cho phép nhập đoạn mã html vào csdl. Nhập đoạn mã html ở thẻ input nào thì khi binding ra giao diện nhớ ghi @Html.Raw(data hiển thị). VD: Xem ở view chi tiết chỗ @Html.Raw(Model.MoTa).
        // Do mô tả chứa đoạn mã html ( <br />) nên phải sử dụng cú pháp razor mvc @Html.Raw(). Đọc đoạn mã html
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SuaSanPham(SanPham sp, HttpPostedFileBase fUpload)
        {
            SanPham product = db.SanPhams.SingleOrDefault(n => n.MaSP.Equals(sp.MaSP));
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC");
            if (product == null)
            {
                return HttpNotFound();
            }
            product.TenSP = sp.TenSP;
            product.MoTa = sp.MoTa;
            product.GioiTinh = sp.GioiTinh;
            product.GiaBan = sp.GiaBan;
            product.GiaNhap = sp.GiaNhap;
            if (fUpload != null) // Đã chọn hình ảnh rồi
            {
                if (fUpload.ContentLength > 0)
                {
                    var filename = Path.GetFileName(fUpload.FileName);
                    var path = Path.Combine(Server.MapPath("/Images"), filename);
                    fUpload.SaveAs(path);
                    product.Anh = fUpload.FileName;
                }
            }
            else // Chưa chọn hình cần sửa
            {
                TempData["UploadFail"] = "Vui lòng chọn hình ảnh!";
                return View();
            }
            product.MaLoaiSP = sp.MaLoaiSP;
            product.MaNCC = sp.MaNCC;
            product.SoLuongTon = sp.SoLuongTon;
            db.SubmitChanges();
            TempData["Edited"] = "Sửa thông tin sản phẩm thành công";
            return RedirectToAction("DanhMucCacSanPham", "Admin");
        }

        public ActionResult timKiemSanPham(string tenSP)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            if (!string.IsNullOrEmpty(tenSP))
            {
                var query = from sp in db.SanPhams where sp.TenSP.Contains(tenSP) || sp.LoaiSanPham.TenLoaiSP.Contains(tenSP) select sp;
                if (query.Count() == 0)
                {
                    return RedirectToAction("thongBaoRong", "ThemXoaSua");
                }
                return View(query);
            }
            return View();
        }

        public ActionResult thongBaoRong()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            ViewBag.stringEmpty = "Không tìm thấy sản phẩm";
            return View();
        }

        private decimal TongDoanhThu() {
            decimal TongDoanhThu = 0;
            var check = db.ChiTietHoaDons.Count();
            if (check == 0)
            {
                return TongDoanhThu;
            }
            var cthh = db.ChiTietHoaDons.ToList();
            foreach (var item in cthh)
            {
                TongDoanhThu += (decimal)(item.SoLuong * item.DonGia);
            }
            return TongDoanhThu;
        }

        public ActionResult QuanLiDonHang()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap", "User");
            }
            var loadData = db.ChiTietHoaDons;
            var check = db.ChiTietHoaDons.Count();
            ViewBag.TongDoanhThu = TongDoanhThu();
            return View(loadData);
        }
        [HttpPost]
        public ActionResult DuyetDonHang(int maHD)
        {
            HoaDon hd = db.HoaDons.SingleOrDefault(n => n.MaHD.Equals(maHD));
            hd.TinhTrang = true;
            db.SubmitChanges();
            return RedirectToAction("QuanLiDonHang", "ThemXoaSua");
        }

        [HttpPost]
        public ActionResult HuyDH(int maHD)
        {
            HoaDon hd = db.HoaDons.SingleOrDefault(n => n.MaHD.Equals(maHD));
            db.HoaDons.DeleteOnSubmit(hd);
            db.SubmitChanges();
            return RedirectToAction("QuanLiDonHang", "ThemXoaSua");
        }

        public ActionResult QuanLiKhachHang()
        {
            ViewBag.GetList = from a in db.HoaDons
                              join b in db.KhachHangs
                              on a.MaKH equals b.MaKH
                              select new HDKhachHangModel
                              {
                                  MaKH = b.MaKH,
                                  TenKH = b.TenKH,
                                  TaiKhoan = b.TaiKhoan,
                                  MatKhau = b.MatKhau,
                                  SoDienThoai = b.SDT,
                                  MaHD = a.MaHD,
                                  TinhTrang = (bool)a.TinhTrang,
                              };
            return View(ViewBag.GetList);
        }
        [HttpPost]
        public ActionResult XoaTaiKhoan(int maKH)
        {
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH.Equals(maKH));
            db.KhachHangs.DeleteOnSubmit(kh);
            db.SubmitChanges();
            return RedirectToAction("QuanLiKhachHang", "ThemXoaSua");
        }
    }
}