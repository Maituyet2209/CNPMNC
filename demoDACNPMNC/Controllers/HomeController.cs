using demoDACNPMNC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace demoDACNPMNC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        QLBANDIENTHOAIEntities db = new QLBANDIENTHOAIEntities();

        public ActionResult Index()
        {
            //var temp = new user{ id_user = 1,email = "abc@gmail.com",pass = "123"};
            //db.users.Add(temp);
            //db.SaveChanges();
            //var crProfile = new profile()
            //{
            //    id_user = 1,
            //    names = "Le Tien thanh",
            //    phone = "0904962412",
            //    addres = "QL 22A HUFLIT Hoc Mon"
            //};
            //db.profiles.Add(crProfile);
            //db.SaveChanges();
            var temp = new user { id_user = 1, email = "abc@gmail.com", pass = "123" };
            Session["user"] = temp;
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
                        listCart.Add(sp);
                    }
                    Session["GioHang"] = listCart;
                    gioHang = listCart;
                }
            }
            //Session["totalCart"]
            var getQuantityCart = Session["totalCart"] ;
            if (getQuantityCart == null)
            {
                if (gioHang != null)
                    Session["totalCart"] = gioHang.Count;
            }
            var lstBrand = db.SANPHAMs.ToList();
            if (lstBrand.Count % 4 == 1)
            {
                lstBrand.RemoveAt(lstBrand.Count - 1);
            }
            var listProApple = LaySPTheoBrand("Apple");
            if (listProApple.Count % 4 == 1)
            {
                listProApple.RemoveAt(listProApple.Count -1);
            }
            ViewBag.listProApple = listProApple;

            //Session["totalCart"] = 0;
            if (Session["GioHang"]as List<MatHangMua> != null)
            {
                Session["totalCart"] = (Session["GioHang"] as List<MatHangMua>).Count;
            }
            return View(lstBrand);
        }

        public ActionResult Brand(String brand)
        {
            var lstBrand =LaySPTheoBrand(brand);
            ViewBag.quantity = lstBrand.Count;
            ViewBag.namebrand = brand;
            return View(lstBrand);
        }

        public ActionResult Product_Detail(int idProduct)
        {
            var itemProduct = getProductFromId(idProduct);
            return View(itemProduct);
        }
        public ActionResult Search(String search)
        {

            if (string.IsNullOrEmpty(search))
            {
                ViewBag.MessageSeach = "Vui lòng nhập từ khóa tìm kiếm.";
                ViewBag.inputSearch = search;
                return View();
            }
            ViewBag.inputSearch = search;
            var products = db.SANPHAMs
                .Where(p => p.TenDT.Contains(search))
                .ToList();

            if (products.Count == 0)
            {
                ViewBag.MessageSearch = "Shop hiện nay không có sản phẩm này.";
            }
            return View(products);
        }

        public ActionResult SearchOrder(FormCollection form)
        {
            string numberphone = form["numberphone"];
            string idOrder = form["idOrder"];
            int tranIdOrder = -1;
            if (idOrder != null)
            {
                tranIdOrder = Convert.ToInt32(idOrder);
            }
            var getorder = db.orders.Where(x => x.id_order == tranIdOrder).FirstOrDefault();
            if (getorder != null)
            {
                var getProFile = db.profiles.FirstOrDefault(x => x.id_user == getorder.id_user);
                ViewBag.Profile = getProFile;
                var getListOrderItem = db.order_item.Where(x => x.id_order == getorder.id_order).ToList();
                if (getorder.payment_type == true)
                    ViewBag.paymentType = "Thanh toán qua PayPal";
                else
                    ViewBag.paymentType = "Thanh toán khi nhận hàng";
                if (getorder.pending == true)
                    ViewBag.Status = "Đang chờ xác nhận.";
                else if (getorder.delivering == true)
                    ViewBag.Status = "Đang Vận Chuyển";
                else if (getorder.successed == true)
                    ViewBag.Status = "Hoàn Tất";
                else ViewBag.status = "Đơn Hàng Đã Hủy";

                List<MatHangMua> tranListOderItem = new List<MatHangMua>();
                foreach(var i in getListOrderItem)
                {
                    var newPro = new MatHangMua(Convert.ToInt32(i.MaDT));
                    tranListOderItem.Add(newPro);
                }
                ViewBag.ListOrderItem = tranListOderItem;
            }
            return View(getorder);
        }

        public ActionResult Profile(FormCollection form)
        {
            return View();
        }

        public ActionResult ShoppingInfo(FormCollection form)
        {
            var getUser = Session["user"] as user;
            
            ShoppingInfo newUserInf = new ShoppingInfo();
            string name = "", email = "", phone = "", address = "";
            if (getUser != null)
            {
                var getProfile = db.profiles.FirstOrDefault(x => x.id_user == getUser.id_user);
                Session["Profile"] = getProfile;
                if (getProfile != null)
                {

                    ViewBag.inputName = getProfile.names;
                    ViewBag.inputEmail = getUser.email;
                    ViewBag.inputPhone = getProfile.phone;
                    ViewBag.inputAddress = getProfile.addres;
                    name = getProfile.names;
                    email = getUser.email;
                    phone = getProfile.phone;
                    address = getProfile.addres;

                     newUserInf = new ShoppingInfo(getProfile.names, getUser.email, getProfile.phone, getProfile.addres);
                    //Session["ShoppingInfo"] = newUserInf;
                }
            }

            var getName = form["getName"];
            var getEmail = form["getEmail"];
            var getPhone = form["getPhone"];
            var getAddress = form["getAddress"];
            if (getName != "" && getName!= null)
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
                newUserInf = new ShoppingInfo(name,email, phone,address);
                Session["ShoppingInfo"] = newUserInf;
            }
            else
            {
                ViewBag.MessageErrorShopInf = "Bạn cần nhập thông tin đầy đủ";
            }
            return View();
        }

        private List<SANPHAM> LaySPTheoBrand(string brandName)
        {
            return db.SANPHAMs.Where(sp => sp.BRAND.TenBrand == brandName).ToList();
        }
        private SANPHAM getProductFromId(int id)
        {
            return db.SANPHAMs.Where(x => x.MaDT == id).FirstOrDefault();
        }

        
    }
}