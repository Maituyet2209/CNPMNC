using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using He_thong_ban_DT_online.Models;

namespace He_thong_ban_DT_online.Controllers
{
    public class UsersController : Controller
    {
        QLBANDIENTHOAIEntities db = new QLBANDIENTHOAIEntities();
        [HttpGet]
        public ActionResult XemTrangCaNhan(int? id)
        {
            if (Session["TaiKhoan"] != null)
            {
                var kh = (KHACHHANG)Session["TaiKhoan"];

                ViewBag.TenDangNhap = kh.TenDN;
                ViewBag.HoTen = kh.HoTenKH;
                ViewBag.DiaChi = kh.DiachiKH;
                ViewBag.SDT = kh.DienthoaiKH;
                ViewBag.NgaySinh = kh.Ngaysinh;
                ViewBag.GioiTinh = kh.Gioitinh;
                ViewBag.Email = kh.Email;
                ViewBag.Ava = kh.avatar;

                return View("XemTrangCaNhan");
            }
            else
            {
                return RedirectToAction("DangNhap");
            }
        }
        [HttpGet]
        public ActionResult SuaThongTinCaNhan()
        {
            if (Session["TaiKhoan"] != null)
            {
                var khach = (KHACHHANG)Session["TaiKhoan"];
                return View(khach);
            }
            else
            {
                return RedirectToAction("DangNhap");
            }
        }


        [HttpPost, ActionName("SuaThongTinCaNhan")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmSuaThongTinCaNhan(KHACHHANG kh, HttpPostedFileBase HinhMinhHoa)
        {
            if (ModelState.IsValid)
            {
                var KHinfo = db.KHACHHANGs.FirstOrDefault(p => p.MaKH == kh.MaKH);
                if (KHinfo != null)
                {
                    KHinfo.HoTenKH = kh.HoTenKH;
                    if (HinhMinhHoa != null)
                    {
                        var fileName = Path.GetFileName(HinhMinhHoa.FileName);
                        var path = Path.Combine(Server.MapPath("~/images"), fileName);

                        KHinfo.avatar = fileName;
                        HinhMinhHoa.SaveAs(path);
                    }
                    KHinfo.DiachiKH = kh.DiachiKH;
                    KHinfo.DienthoaiKH = kh.DienthoaiKH;
                    KHinfo.Ngaysinh = kh.Ngaysinh;
                    KHinfo.Email = kh.Email;
                    KHinfo.Gioitinh = kh.Gioitinh;

                    Session.Remove("TaiKhoan");

                    Session["TaiKhoan"] = KHinfo;
                }

                db.SaveChanges();
                return RedirectToAction("XemTrangCaNhan");
            }
            return View(kh);
        }

        public ActionResult LichSuMuaHang()
        {
            if (Session["UserID"] != null) 
            {
                int maKhachHang = (int)Session["UserID"]; 

                var danhSachDonDatHang = db.DONDATHANGs.Where(dh => dh.MaKH == maKhachHang).ToList();
                if (TempData["SuccessMessage"] != null)
                {
                    ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
                }
                if (TempData["ErrorMessage"] != null)
                {
                    ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                }
                return View(danhSachDonDatHang);
            }
            else
            {
                return RedirectToAction("DangNhap"); 
            }
        }
        public ActionResult ChiTietDonHang(int? id)
        {
            if (Session["UserID"] != null)
            {
                int maKhachHang = (int)Session["UserID"];

                var donDatHang = db.DONDATHANGs.FirstOrDefault(dh => dh.MaDH == id && dh.MaKH == maKhachHang);

                if (donDatHang != null)
                {
                    var chiTietDonHang = db.CTDATHANGs.Where(ct => ct.MaDH == id).ToList();
                    return View(chiTietDonHang);
                }
                else
                {
                    return RedirectToAction("LichSuMuaHang");
                }
            }
            else
            {
                return RedirectToAction("DangNhap");
            }
        }


        public ActionResult HuyDH(int? id)
        {
            if (Session["UserID"] != null)
            {
                DONDATHANG donHang = db.DONDATHANGs.Find(id);
                if (donHang == null)
                {
                    return HttpNotFound();
                }

                if (donHang.MaTTDH == 3 || donHang.MaTTDH == 4)
                {
                    TempData["ErrorMessage"] = "Không thể hủy đơn hàng này.";
                    return RedirectToAction("LichSuMuaHang");
                }
                donHang.MaTTDH = 5;
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Hủy đơn hàng thành công.";

                return RedirectToAction("LichSuMuaHang");
            }
            else
            {
                return RedirectToAction("DangNhap");
            }
        }


        // GET: Users
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(kh.HoTenKH))
                    ModelState.AddModelError(string.Empty, "Họ tên không được để trống");
                if (String.IsNullOrEmpty(kh.DiachiKH))
                    ModelState.AddModelError(string.Empty, "Địa chỉ không được để trống");
                if (String.IsNullOrEmpty(kh.DienthoaiKH))
                    ModelState.AddModelError(string.Empty, "Điện thoại không được để trống");
                if (String.IsNullOrEmpty(kh.TenDN))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (String.IsNullOrEmpty(kh.Matkhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (String.IsNullOrEmpty(kh.Email))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");

                bool gioiTinh;
                if (!bool.TryParse(Request.Form["Gioitinh"], out gioiTinh))
                {
                    ModelState.AddModelError(string.Empty, "Giới tính không hợp lệ");
                    return View();
                }
                kh.Gioitinh = gioiTinh;

                var khachhang = db.KHACHHANGs.FirstOrDefault(k => k.TenDN == kh.TenDN);
                if (khachhang != null)
                    ModelState.AddModelError(string.Empty, "Đã có người đăng ký tên này");

                if (ModelState.IsValid)
                {
                    db.KHACHHANGs.Add(kh);
                    db.SaveChanges();
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("DangNhap");
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.TenDN))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(kh.Matkhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (ModelState.IsValid)
                {
                    var khach = db.KHACHHANGs.FirstOrDefault(k => k.TenDN == kh.TenDN && k.Matkhau == kh.Matkhau);
                    if (khach != null)
                    {
                        ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";

                        Session["TaiKhoan"] = khach;

                        Session["UserID"] = khach.MaKH;
                        return RedirectToAction("Index", "HomePhone");
                    }
                    else
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }

        public ActionResult DangXuat()
        {
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Index", "HomePhone");
        }


    }
}