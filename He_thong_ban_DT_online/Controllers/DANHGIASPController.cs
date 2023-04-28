using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using He_thong_ban_DT_online.Models;

namespace He_thong_ban_DT_online.Controllers
{
    public class DANHGIASPController : Controller
    {
        QLBANDIENTHOAIEntities db = new QLBANDIENTHOAIEntities();
        // GET: DANHGIASP
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoDanhGia(DANHGIASP danhgia)
        {
            if (Session["TaiKhoan"] == null)
                return RedirectToAction("DangNhap", "Users");

            if (ModelState.IsValid)
            {
                danhgia.MaKH = (int)Session["MaKH"];
                danhgia.NgayDanhGia = DateTime.Now;
                db.DANHGIASPs.Add(danhgia);
                db.SaveChanges();
                return RedirectToAction("Details", "SanPham", new { id = danhgia.MaSP });
            }

            var sanpham = db.SANPHAMs.Find(danhgia.MaSP);
            if (sanpham == null)
            {
                return HttpNotFound();
            }

            ViewBag.SanPham = sanpham;

            return View(danhgia);
        }

    }
}