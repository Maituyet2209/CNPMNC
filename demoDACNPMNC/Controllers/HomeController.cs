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
            if (numberphone != null)
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

        public ActionResult Profile()
        {
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