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
        public ActionResult SanPham(int ? page)
        {
            var dsSanPham = db.SANPHAMs.ToList();
            int pageSize = 7;
            int pageNum = (page ?? 1);
            return View(dsSanPham.OrderBy(sp=>sp.MaSP).ToPagedList(pageNum,pageSize));
        }
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
        public ActionResult XoaSP(int id)
        {
            var sanpham = db.SANPHAMs.FirstOrDefault(sp => sp.MaSP == id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            return View(sanpham);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var sanpham = db.SANPHAMs.FirstOrDefault(sp => sp.MaSP == id);
            db.SANPHAMs.Remove(sanpham);
            db.SaveChanges();
            return RedirectToAction("SanPham");
        }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaSP(SANPHAM sanpham, HttpPostedFileBase Hinhminhhoa)
        {
            if (ModelState.IsValid)
            {
                if (Hinhminhhoa != null && Hinhminhhoa.ContentLength > 0)
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
                        sanpham.HinhMinhHoa = fileName;
                    }
                }

                db.Entry(sanpham).State = EntityState.Modified;
                db.Entry(sanpham).Property(p => p.Dongia).IsModified = false; // Không cho sửa đổi giá sản phẩm

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

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPrice(int id, decimal Dongia)
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
    }
}