using demoDACNPMNC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace demoDACNPMNC.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        QLBANDIENTHOAIEntities db = new QLBANDIENTHOAIEntities();
        public List<MatHangMua> LayGioHang()
        {
           
            //var temp = new user{ id_user = 1,email = "abc@gmail.com",pass = "123"};
            //db.users.Add(temp);
            //db.SaveChanges();
            //Session["user"] = temp;
            var getuser = Session["user"] as user;
            List<MatHangMua> gioHang = Session["GioHang"] as List<MatHangMua>;

            if (getuser != null)
            {
                //get cart of user when login
                var getcart = db.carts.Where(x => x.id_user == getuser.id_user).ToList();
                List<MatHangMua> listCart = new List<MatHangMua>();
                if (getcart != null || getcart.Count > 0)
                {
                    foreach (var i in getcart)
                    {
                        MatHangMua sp = new MatHangMua(Convert.ToInt32(i.MaDT));
                        sp.SoLuong =(int)(i.quantity);
                        listCart.Add(sp);
                    }
                    Session["GioHang"] = listCart;
                    return listCart;
                }
            }
            
            if (gioHang == null)
            {
                gioHang = new List<MatHangMua>();
                Session["GioHang"] = gioHang;
            }
            Session["totalCart"] = gioHang.Count;
            return gioHang;
        }
        public ActionResult ThemSanPhamVaoGio(int MaDT)
        {
            var getuser = Session["user"] as user;
            if (getuser != null)
            {
                var checkProduct = db.carts.Where(x => x.MaDT == MaDT).FirstOrDefault();
                if (checkProduct == null)
                {
                    var sp = new cart()
                    {
                        id_user = getuser.id_user,
                        MaDT = MaDT,
                        quantity = 1,
                    };
                    db.carts.Add(sp);
                    db.SaveChanges();
                }
                else
                {
                    checkProduct.quantity++;
                }
                return RedirectToAction("HienThiGioHang", "Cart");
            }
            List<MatHangMua> gioHang = LayGioHang();
            MatHangMua sanPham = gioHang.FirstOrDefault(s => s.MaDT == MaDT);
            if (sanPham == null)
            {
                sanPham = new MatHangMua(MaDT);
                gioHang.Add(sanPham);
            }
            else
            {
                sanPham.SoLuong++;
            }
            return RedirectToAction("HienThiGioHang", "Cart");
        }
        private int TinhTongSL()
        {
            int tongSL = 0;
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang != null)
                tongSL = gioHang.Sum(sp => sp.SoLuong);
            return tongSL;
        }
        private double TinhTongTien()
        {
            double TongTien = 0;
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang != null)
            {
                TongTien = gioHang.Sum(sp => sp.ThanhTien());
            }
            return TongTien;
        }
        public ActionResult HienThiGioHang()
        {
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang == null || gioHang.Count == 0)
            {
                Session["totalCart"] = 0;
                return View("CartNoProduct");
            }
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            Session["totalCart"] = (Session["GioHang"] as List<MatHangMua>).Count;
            return View(gioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }
        public ActionResult XoaMatHang(int MaDT)
        {
            var getuser = Session["user"] as user;
            if (getuser != null)
            {
                var checkProduct = db.carts.Where(x => x.MaDT == MaDT).FirstOrDefault();
                if (checkProduct != null)
                {
                    var sp = new cart()
                    {
                        id_user = getuser.id_user,
                        MaDT = MaDT,
                        quantity = 1,
                    };
                    db.carts.Remove(checkProduct);
                    db.SaveChanges();
                }
            }
            else
            {
                List<MatHangMua> gioHang = LayGioHang();
                var sanpham = gioHang.FirstOrDefault(s => s.MaDT == MaDT);
                if (sanpham != null)
                {
                    gioHang.RemoveAll(s => s.MaDT == MaDT);
                    return RedirectToAction("HienThiGioHang");
                }
                if (gioHang.Count == 0)
                    return RedirectToAction("Index", "Home");
            }
            
            return RedirectToAction("HienThiGioHang");
        }
        public ActionResult CapNhatMatHang(int MaDT, int SoLuong)
        {
            List<MatHangMua> gioHang = LayGioHang();
            var sanpham = gioHang.FirstOrDefault(s => s.MaDT == MaDT);
            if (sanpham != null)
            {
                sanpham.SoLuong = SoLuong;
                //db.carts.FirstOrDefault(x => x.MaDT == MaDT).quantity = SoLuong;

                var getuser = Session["user"] as user;

                if (getuser != null)
                {
                    //get cart of user when after login
                    var getcart = db.carts.Where(x => x.id_user == getuser.id_user).ToList();
                    db.carts.FirstOrDefault(x => x.id_user == getuser.id_user && x.MaDT == MaDT).quantity = SoLuong;
                    getcart.FirstOrDefault(x => x.MaDT == MaDT).quantity = SoLuong;
                    db.SaveChanges();
                }

                db.SaveChanges();
            }
            return RedirectToAction("HienThiGioHang"); ;
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}