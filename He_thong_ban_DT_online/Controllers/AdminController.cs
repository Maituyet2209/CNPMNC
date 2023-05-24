using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using He_thong_ban_DT_online.Models;
using PagedList;
using System.IO;
using System.Data.Entity;
using System.Net;

namespace He_thong_ban_DT_online.Controllers
{
    public class AdminController : Controller
    {
        QLBANDIENTHOAIEntities db = new QLBANDIENTHOAIEntities();
        // GET: Admin
        public ActionResult SanPham(int? page, string Search = "")
        {
            var dsSanPham = db.SANPHAMs.ToList();
            if (!string.IsNullOrEmpty(Search))
            {
                dsSanPham = dsSanPham.Where(s => s.TenDT.ToUpper().Contains(Search.ToUpper())).ToList();
                ViewBag.Search = Search;
            }
            int pageSize = 7;
            int pageNum = (page ?? 1);
            return View(dsSanPham.OrderBy(sp => sp.MaSP).ToPagedList(pageNum, pageSize));
        }


        [HttpGet]
        public ActionResult ThemSP()
        {
            ViewBag.MaBrand = new SelectList(db.BRANDs.ToList(), "BrandId", "TenBrand");
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList(), "MaNSX", "TenNSX");
            return View();
        }
        [HttpPost]
        public ActionResult ThemSP(SANPHAM sp, HttpPostedFileBase Hinhminhhoa)
        {
            ViewBag.MaBrand = new SelectList(db.BRANDs.ToList(), "BrandId", "TenBrand");
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList(), "MaNSX", "TenNSX");

            if (Hinhminhhoa == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn hình ảnh";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(Hinhminhhoa.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình đã tồn tại";
                    }
                    else
                    {
                        Hinhminhhoa.SaveAs(path);
                    }
                    sp.HinhMinhHoa = fileName;
                    db.SANPHAMs.Add(sp);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("SanPham");
        }
        public ActionResult ChiTietSP(int id)
        {
            var sanpham = db.SANPHAMs.FirstOrDefault(sp => sp.MaSP == id);
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }
        [HttpGet]
        public ActionResult XoaSP(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM product = db.SANPHAMs.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("XoaSP")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int?id)
        {
            var sanpham = db.SANPHAMs.FirstOrDefault(sp => sp.MaSP == id);
            db.SANPHAMs.Remove(sanpham);
            db.SaveChanges();
            return RedirectToAction("SanPham");
        }
        [HttpGet]
        public ActionResult SuaSP(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SANPHAM sanpham = db.SANPHAMs.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }

            ViewBag.MaBrand = new SelectList(db.BRANDs, "BrandId", "TenBrand", sanpham.MaBrand);
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs, "MaNSX", "TenNSX", sanpham.MaNSX);

            return View(sanpham);
        }

        [HttpPost, ActionName("SuaSP")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmSuaSP(SANPHAM sanpham, HttpPostedFileBase HinhMinhHoa)
        {
            if (ModelState.IsValid)
            {
                var productDB = db.SANPHAMs.FirstOrDefault(p => p.MaSP == sanpham.MaSP);
                if (productDB != null)
                {
                    productDB.TenDT = sanpham.TenDT;
                    productDB.MoTa = sanpham.MoTa;
                    if (HinhMinhHoa != null)
                    {
                        var fileName = Path.GetFileName(HinhMinhHoa.FileName);
                        var path = Path.Combine(Server.MapPath("~/images"), fileName);

                        productDB.HinhMinhHoa = fileName;
                        HinhMinhHoa.SaveAs(path);
                    }
                    productDB.MaBrand = sanpham.MaBrand;
                    productDB.MaNSX = sanpham.MaNSX;
                    productDB.SoluongTon = sanpham.SoluongTon;
                    productDB.BaoHanh = sanpham.BaoHanh;
                    productDB.NgaySX = sanpham.NgaySX;
                    productDB.MauSac = sanpham.MauSac;
                    productDB.ManHinh = sanpham.ManHinh;
                    productDB.CauHinh = sanpham.CauHinh;
                    productDB.BoNhoTrong = sanpham.BoNhoTrong;
                    productDB.Camera_main = sanpham.Camera_main;
                    productDB.HeDieuHanh = sanpham.HeDieuHanh;
                    productDB.TheSim = sanpham.TheSim;
                    productDB.DungLuongPin = sanpham.DungLuongPin;
                    productDB.DungLuongRam = sanpham.DungLuongRam;
                    productDB.CongNgheSac = sanpham.CongNgheSac;
                    productDB.DoPhanGiai = sanpham.DoPhanGiai;
                    productDB.CongSac = sanpham.CongSac;
                    productDB.KichThuoc = sanpham.KichThuoc;
                    productDB.KhoiLuong = sanpham.KhoiLuong;
                }

                db.SaveChanges();
                return RedirectToAction("SanPham");
            }

            ViewBag.MaBrand = new SelectList(db.BRANDs, "BrandId", "TenBrand", sanpham.MaBrand);
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs, "MaNSX", "TenNSX", sanpham.MaNSX);


            return View(sanpham);
        }


        public ActionResult QuanLyGia()
        {
            var giaCacSanPham = db.SANPHAMs.ToList();
            return View(giaCacSanPham);
        }

        [HttpGet]
        public ActionResult EditPrice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var sanpham = db.SANPHAMs.FirstOrDefault(sp => sp.MaSP == id);

            if (sanpham == null)
            {
                return HttpNotFound();
            }

            ViewBag.SanPhamId = sanpham.MaSP;
            ViewBag.SanPhamName = sanpham.TenDT;
            ViewBag.HinhMinhHoa = sanpham.HinhMinhHoa;
            ViewBag.DonGiaHienTai = sanpham.Dongia;

            return View(sanpham);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPrice(int id, decimal? Dongia)
        {
            var sanpham = db.SANPHAMs.FirstOrDefault(sp => sp.MaSP == id);

            if (sanpham == null)
            {
                return HttpNotFound();
            }

            sanpham.Dongia = Dongia;
            db.Entry(sanpham).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("QuanLyGia");
        }
        public ActionResult DonHangMoi()
        {
            var dsDonHangMoi = db.DONDATHANGs.Where(dh => dh.MaTTDH == 1).ToList();
            return View(dsDonHangMoi);
        }
        public ActionResult DonHangDangXuLy()
        {
            var dsDonHangDangXuLy = db.DONDATHANGs.Where(dh => dh.MaTTDH == 2).ToList();
            return View(dsDonHangDangXuLy);
        }
        public ActionResult DonHangDangVanChuyen()
        {
            var dsDonHangDangVanChuyen = db.DONDATHANGs.Where(dh => dh.MaTTDH == 3).ToList();
            return View(dsDonHangDangVanChuyen);
        }
        
         public ActionResult DonHangDangGiaoThanhCong()
        {
            var dsDonHangDangVanChuyen = db.DONDATHANGs.Where(dh => dh.MaTTDH == 4).ToList();
            return View(dsDonHangDangVanChuyen);
        }
        public ActionResult DonHangHuy()
        {
            var dsDonHangHuy = db.DONDATHANGs.Where(dh => dh.MaTTDH == 5).ToList();
            return View(dsDonHangHuy);
        }
        [HttpGet]
        public ActionResult CapNhatTTDH(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DONDATHANG donHang = db.DONDATHANGs.Find(id);

            if (donHang == null)
            {
                return HttpNotFound();
            }

            ViewBag.TrangThaiDonHang = new SelectList(db.TRANGTHAIDONHANGs, "MaTTDH", "TenTrangThai");
            return View(donHang);
        }

        [HttpPost]
        public ActionResult CapNhatTTDH(int? id, int? maTrangThaiDonHang)
        {
            DONDATHANG donHang = db.DONDATHANGs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }

            donHang.MaTTDH = maTrangThaiDonHang;
            db.Entry(donHang).State = EntityState.Modified;
            db.SaveChanges();

            TempData["SuccessMessage"] = "Cập nhật trạng thái đơn hàng thành công.";
            return RedirectToAction("DonHangMoi");
        }


        public ActionResult XemChiTietDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONDATHANG donHang = db.DONDATHANGs.Find(id);

            if (donHang != null)
            {

                List<CTDATHANG> chiTietDonHang = db.CTDATHANGs.Where(ct => ct.MaDH == id).ToList();


                List<string> hinhMinhHoaList = new List<string>();
                List<string> hoTenKHList = new List<string>();


                foreach (CTDATHANG chiTiet in chiTietDonHang)
                {

                    SANPHAM sanPham = db.SANPHAMs.Find(chiTiet.MaSP);

                    if (sanPham != null)
                    {

                        hinhMinhHoaList.Add(sanPham.HinhMinhHoa);
                    }

                    KHACHHANG khachHang = db.KHACHHANGs.Find(donHang.MaKH);

                    if (khachHang != null)
                    {
                        hoTenKHList.Add(khachHang.HoTenKH);
                    }
                }

                ViewBag.HinhMinhHoaList = hinhMinhHoaList;
                ViewBag.HoTenKHList = hoTenKHList;

                return View(donHang);
            }

            return RedirectToAction("DonHangMoi");
        }

        public ActionResult XoaDH(int id)
        {
            var donHang = db.DONDATHANGs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }

            var chiTietDonHangs = db.CTDATHANGs.Where(ct => ct.MaDH == id);
            foreach (var chiTiet in chiTietDonHangs)
            {
                db.CTDATHANGs.Remove(chiTiet);
            }

            db.DONDATHANGs.Remove(donHang);
            db.SaveChanges();

            return RedirectToAction("DonHangHuy");
        }


    }
}