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
            var dsSP = db.SANPHAMs.OrderByDescending(sp => sp.NgayCapNhat).Take(4).ToList();
            return View(dsSP);
        }

        public ActionResult Details(int id)
        {
            var sanpham = db.SANPHAMs.FirstOrDefault(s => s.MaSP == id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }

            var danhGiaList = db.DANHGIASPs.Where(dg => dg.MaSP == id).Join(db.KHACHHANGs, dg => dg.MaKH, kh => kh.MaKH, (dg, kh) => new { DanhGia = dg, KhachHang = kh }).ToList();

            ViewBag.SanPham = sanpham;
            ViewBag.DanhGiaList = danhGiaList;

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
        public ActionResult ShoppingInfo(FormCollection form)
        {
            var getUser = Session["TaiKhoan"] as KHACHHANG;

            ShoppingInfo newUserInf = new ShoppingInfo();
            string name = "", email = "", phone = "", address = "";
            if (getUser != null)
            {
                var getProfile = db.KHACHHANGs.FirstOrDefault(x => x.MaKH == getUser.MaKH);
                Session["Profile"] = getProfile;
                if (getProfile != null)
                {

                    ViewBag.inputName = getProfile.HoTenKH;
                    ViewBag.inputEmail = getUser.Email;
                    ViewBag.inputPhone = getProfile.DienthoaiKH;
                    ViewBag.inputAddress = getProfile.DiachiKH;
                    name = getProfile.HoTenKH;
                    email = getUser.Email;
                    phone = getProfile.DienthoaiKH;
                    address = getProfile.DiachiKH;

                    newUserInf = new ShoppingInfo(getProfile.HoTenKH, getUser.Email, getProfile.DienthoaiKH, getProfile.DiachiKH);
                    //Session["ShoppingInfo"] = newUserInf;
                }
                else
                {
                    ViewBag.inputName = "Nhập Tên Người Nhận";
                    ViewBag.inputEmail = "Nhập Mail Người Nhận";
                    ViewBag.inputPhone = "Nhập SĐT";
                    ViewBag.inputAddress = "Nhập Địa Chỉ Người Nhận";
                }
            }
            ViewBag.inputName = "Nhập Tên Người Nhận";
            ViewBag.inputEmail = "Nhập Mail Người Nhận";
            ViewBag.inputPhone = "Nhập SĐT";
            ViewBag.inputAddress = "Nhập Địa Chỉ Người Nhận";

            var getName = form["getName"];
            var getEmail = form["getEmail"];
            var getPhone = form["getPhone"];
            var getAddress = form["getAddress"];
            if (getName != "" && getName != null)
            {
                name = getName;
                ViewBag.inputName = getName;
            }
            if (getEmail != "" && getEmail != null)
            {
                email = getEmail;
                ViewBag.inputEmail = getEmail;
            }
            if (getPhone != "" && getPhone != null)
            {
                phone = getPhone;
                ViewBag.inputPhone = getPhone;
            }
            if (getAddress != "" && getEmail != null)
            {
                address = getAddress;
                ViewBag.inputAddress = getAddress;
            }
            if (name != "" && email != "" && phone != "" && address != "")
            {
                newUserInf = new ShoppingInfo(name, email, phone, address);
                Session["ShoppingInfo"] = newUserInf;
            }
            else
            {
                ViewBag.MessageErrorShopInf = "Bạn cần nhập thông tin đầy đủ";
            }
            return View();
        }

    }
}