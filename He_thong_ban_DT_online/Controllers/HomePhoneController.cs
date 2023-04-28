using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using He_thong_ban_DT_online.Models;

namespace He_thong_ban_DT_online.Controllers
{
    public class HomePhoneController : Controller
    {
        QLBANDIENTHOAIEntities db = new QLBANDIENTHOAIEntities();
        // GET: HomePhone
        public ActionResult Index()
        {
            var dsSP = db.SANPHAMs.ToList();
            return View(dsSP);
        }
        public ActionResult Details(int id)
        {
            var sanpham = db.SANPHAMs.FirstOrDefault(s => s.MaSP == id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }

            var danhGiaSP = db.DANHGIASPs.Where(dg => dg.MaSP == id).ToList();

            return View(sanpham);
        }
        public ActionResult Brand()
        {
            var dsBrand = db.BRANDs.ToList();
            return PartialView(dsBrand);
        }
        //Hot Sale hoăc sản phẩm mới
        private List<SANPHAM> LaySPMoi(int soluong)
        {
            return db.SANPHAMs.OrderByDescending(sp => sp.NgayCapNhat).Take(soluong).ToList();
        }
        public ActionResult HotSale()
        {
            var dsSPMoi = LaySPMoi(5);
            return PartialView(dsSPMoi);
        }
        //Các loại điện theo theo Brand
        private List<SANPHAM> LaySPTheoBrand(string brandName)
        {
            return db.SANPHAMs.Where(sp => sp.BRAND.TenBrand == brandName).ToList();
        }
        public ActionResult Apple()
        {
            var dsSPApple = LaySPTheoBrand("Apple");
            return PartialView(dsSPApple);
        }
        public ActionResult SamSung()
        {
            var dsSPSamSung = LaySPTheoBrand("SamSung");
            return PartialView(dsSPSamSung);
        }
        public ActionResult Xiaomi()
        {
            var dsSPXiaomi = LaySPTheoBrand("Xiaomi");
            return PartialView(dsSPXiaomi);
        }
        /*        public ActionResult LaySPtheoTenBrand(int id)
                {
                    var dsSPTheoBrand = db.SANPHAMs.Where(sp => sp.MaBrand == id).ToList();
                    return View("Index", dsSPTheoBrand);
                }*/
        // Chức năng Tìm kiếm
        public ActionResult TimKiem(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                ViewBag.Message = "Vui lòng nhập từ khóa tìm kiếm.";
                return View();
            }

            var products = db.SANPHAMs
                .Where(p => p.TenDT.Contains(q))
                .ToList();

            if (products.Count == 0)
            {
                ViewBag.Message = "Shop hiện nay không có sản phẩm này.";
            }

            return View(products);
        }

    }
}