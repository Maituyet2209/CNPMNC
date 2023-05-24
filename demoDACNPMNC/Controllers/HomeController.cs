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
            //var temp = new user { id_user = 1, email = "abc@gmail.com", pass = "123" };
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

            //get filter price
            //string myValue = HttpContext.Current.Request.Cookies["localStorage"]["selectedFilter"];
            var selectedFilter = HttpContext.Request.Cookies["selectedFilter"]?.Value;
            if (selectedFilter != null)
            {
                string digits = new String(selectedFilter.Where(Char.IsDigit).ToArray()); // Lấy chữ số từ chuỗi
                if (digits != null && digits != "")
                {
                    int num = Int32.Parse(digits);

                    ViewBag.selectedFilter = selectedFilter;
                    if (num <10 )
                        lstBrand = lstBrand.Where(x => x.Dongia <= (decimal)(num * 1000000)).ToList();
                    else
                        lstBrand = lstBrand.Where(x => x.Dongia >= (decimal)(num * 1000000)).ToList();
                }
            }

            // get filter ram
            var selectedFilter_ram = HttpContext.Request.Cookies["filter_ram"]?.Value;
            if (selectedFilter_ram != null)
            {
                string digits_ram = new String(selectedFilter_ram.Where(Char.IsDigit).ToArray()); // Lấy chữ số từ chuỗi
                if (digits_ram != null && digits_ram != "")
                {
                    int num_ram = Int32.Parse(digits_ram);
                    ViewBag.selectedFilter_ram = selectedFilter_ram;
                    lstBrand = lstBrand.Where(x => x.DungLuongRam == num_ram).ToList();
                }
            }

            // get filter rom
            var selectedFilter_rom = HttpContext.Request.Cookies["filter_rom"]?.Value;
            if (selectedFilter_rom != null)
            {
                    ViewBag.selectedFilter_rom = selectedFilter_rom;
                    lstBrand = lstBrand.Where(x => x.BoNhoTrong == selectedFilter_rom).ToList();
            }
            //get filer camera
            var selectedFilter_camera = HttpContext.Request.Cookies["filter_camera"]?.Value;
            if (selectedFilter_camera != null)
            {
                string digits = new String(selectedFilter_camera.Where(Char.IsDigit).ToArray()); // Lấy chữ số từ chuỗi
                if (digits != null && digits != "")
                {
                    int num_camera = Int32.Parse(digits);

                    ViewBag.selectedFilter_camera = selectedFilter_camera;
                    lstBrand = lstBrand.Where(x => x.Camera_main.Contains(num_camera.ToString())).ToList();
                }
            }

            //get filer screen
            var selectedFilter_screen = HttpContext.Request.Cookies["filter_screen"]?.Value;
            if (selectedFilter_screen != null)
            {
                    ViewBag.selectedFilter_screen = selectedFilter_screen;
                    lstBrand = lstBrand.Where(x => x.ManHinh.Contains(selectedFilter_screen)).ToList();
            }

            ViewBag.quantity = lstBrand.Count;
            ViewBag.namebrand = brand;
            return View(lstBrand);
        }

        public ActionResult Product_Detail(int idProduct)
        {
            var getUser = Session["user"] as user;
            if (getUser == null)
            {
                ViewBag.notifi_cmt = "Bạn cần phải đăng nhập để bình luận";
            } else
            {
                ViewBag.notifi_cmt = null;
                var getProfile = db.profiles.FirstOrDefault(x => x.id_user == idProduct) as profile;
                //ViewBag.nameUser = getProfile.names;
                //if (getProfile.avatar != null)
                //    ViewBag.avatar = getProfile.avatar;
            }
            var itemProduct = getProductFromId(idProduct);
            Session["Product"] = itemProduct;

            //get comment
            var getCmt = db.COMMENTS.Where(x => x.MaDT == idProduct).ToList();
            ViewBag.lstCmt = getCmt;
            return View(itemProduct);
        }

        [HttpPost]
        public ActionResult AddComment(FormCollection form)
        {
            var getContent_Cmt = form["content_cmt"];
            var getStars = form["stars"];
            var getUser = Session["user"] as user;
            var getProduct = Session["Product"] as SANPHAM;
            if (getUser != null && getProduct != null)
            {
                COMMENT comment = new COMMENT()
                {
                    id_user = getUser.id_user,
                    MaDT = getProduct.MaDT,
                    content_cmt = getContent_Cmt,
                    stars = int.Parse(getStars),
                    time_cmt = DateTime.Now,
                };
                db.COMMENTS.Add(comment);
                db.SaveChanges();
            }
            return RedirectToAction("Product_Detail", new { id = getProduct.MaDT });
        }
        public ActionResult Search(String search)
        {

            if (string.IsNullOrEmpty(search))
            {
                ViewBag.MessageSeach = "Vui lòng nhập từ khóa tìm kiếm.";
                ViewBag.inputSearch = search;
                var songs = db.SANPHAMs.ToList();
                return View(songs);
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
            ViewBag.name = "Nhập Họ Tên";
            ViewBag.phone = "Nhập Số Điện Thoại";
            ViewBag.address = "Nhập Địa chỉ nhận hàng";
            ViewBag.email = "Nhập Email";
            var getprofile = Session["Profile"] as profile;
            var getUser = Session["user"] as user;
            if (getprofile != null && getUser != null)
            {
                ViewBag.name = getprofile.names;
                ViewBag.phone = getprofile.phone;
                ViewBag.address = getprofile.addres;
                ViewBag.email = getUser.email;
            }
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
                else
                {
                    ViewBag.inputName = "Nhập Tên Người Nhận";
                    ViewBag.inputEmail = "Nhập Mail Người Nhận";
                    ViewBag.inputPhone = "Nhập SĐT";
                    ViewBag.inputAddress = "Nhập Địa Chỉ Người Nhận";
                }
            }
            else
            {
                ViewBag.inputName = "Nhập Tên Người Nhận";
                ViewBag.inputEmail = "Nhập Mail Người Nhận";
                ViewBag.inputPhone = "Nhập SĐT";
                ViewBag.inputAddress = "Nhập Địa Chỉ Người Nhận";
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