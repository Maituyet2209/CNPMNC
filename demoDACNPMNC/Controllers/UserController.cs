using demoDACNPMNC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace demoDACNPMNC.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        QLBANDIENTHOAIEntities db = new QLBANDIENTHOAIEntities();

        public ActionResult Index()
        {
            return View();
        }

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
                if (String.IsNullOrEmpty(kh.TenDN))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (String.IsNullOrEmpty(kh.Matkhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (String.IsNullOrEmpty(kh.Email))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
                if (String.IsNullOrEmpty(kh.DienthoaiKH))
                    ModelState.AddModelError(string.Empty, "Điện thoại không được để trống");
                if (String.IsNullOrEmpty(kh.DiachiKH))
                    ModelState.AddModelError(string.Empty, "Địa Chỉ không được để trống");

                var khachhang = db.KHACHHANGs.FirstOrDefault(k => k.TenDN == kh.TenDN);
                if (khachhang != null)
                    ModelState.AddModelError(string.Empty, "Đã có người đăng ký tên này");
                if (ModelState.IsValid)
                {
                    user newUser = new user()
                    {
                        email = kh.TenDN,
                        pass = kh.Matkhau,
                    };

                    profile newProfile = new profile()
                    {
                        id_user = newUser.id_user,
                        names = kh.HoTenKH,
                        phone = kh.DienthoaiKH,
                        addres = kh.DiachiKH,
                    };
                    var getUser = db.users.FirstOrDefault(x => x.email == newUser.email);
                    if (getUser == null)
                    {
                        db.users.Add(newUser);
                        db.profiles.Add(newProfile);
                    }
                   
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
                    var getUser = db.users.FirstOrDefault(x => x.email == kh.TenDN);
                    if (khach != null)
                    {
                        ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                        Session["TaiKhoan"] = khach;
                        if (getUser != null)
                        {
                            Session["user"] = getUser;
                            var getProfile = db.profiles.FirstOrDefault(x => x.id_user == getUser.id_user);
                            if (getProfile != null)
                            {
                                Session["Profile"] = getProfile;
                            }
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult ShoppingOrder()
        {
            //var temp = new user { id_user = 1, email = "abc@gmail.com", pass = "123" };
            //Session["user"] = temp;
            List<order> getOrderNow = new List<order>();
            var getUser = Session["user"] as user;
            if (getUser != null)
            {
                var getOrder = db.orders.Where(x => x.id_user == getUser.id_user).ToList();
                getOrderNow = getOrder.Where(x => x.pending == true).ToList();
                var allOredItem = db.order_item.ToList();
                ViewBag.allOredItem = allOredItem;
                return View(getOrderNow);
            }
            return View(getOrderNow);
        }

        /// History Shopping

        public ActionResult HistoryShopping()
        {
            List<order> getHistoryShopping = new List<order>();
            var getUser = Session["user"] as user;
            if (getUser != null)
            {
                var getOrder = db.orders.Where(x => x.id_user == getUser.id_user).ToList();
                getHistoryShopping = getOrder.Where(x => x.successed == true).ToList();
                var allOredItem = db.order_item.ToList();
                ViewBag.allOredItem = allOredItem;
                return View(getHistoryShopping);
            }
            return View(getHistoryShopping);
        }

        ///ADMIN Mangement Order
        /// pending
        public ActionResult ManagementPending()
        {
            var getOderPending = db.orders.Where(x => x.pending == true && x.delivering == null).ToList();
            return View(getOderPending);
        }
        public ActionResult DetailOrder(int id_order)
        {
            var getorder = db.orders.Where(x => x.id_order == id_order).FirstOrDefault();
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
                else ViewBag.Status = "Hoàn Tất";

                List<MatHangMua> tranListOderItem = new List<MatHangMua>();
                foreach (var i in getListOrderItem)
                {
                    var newPro = new MatHangMua(Convert.ToInt32(i.MaDT));
                    tranListOderItem.Add(newPro);
                }
                ViewBag.ListOrderItem = tranListOderItem;
            }
            return View(getorder);
        }

        public ActionResult AcceptPending(int id_order)
        {
            var getOrder = db.orders.FirstOrDefault(x => x.id_order == id_order);
            if (getOrder != null)
            {
                db.orders.FirstOrDefault(x => x.id_order == id_order).pending = false;
                db.orders.FirstOrDefault(x => x.id_order == id_order).delivering = true;
                db.SaveChanges();
            }
            return RedirectToAction("ManagementPending");
        }

        public ActionResult CancelOrder(int id_order)
        {

            var getOrder = db.orders.FirstOrDefault(x => x.id_order == id_order);
            if (getOrder != null)
            {
                db.orders.FirstOrDefault(x => x.id_order == id_order).pending = false;
                db.orders.FirstOrDefault(x => x.id_order == id_order).successed = false;
                db.SaveChanges();
            }
            return RedirectToAction("ManagementPending");
        }

        //management delivery
        public ActionResult DeliveryOrder()
        {
            var getOrderDelivery = db.orders.Where(x => x.delivering == true).ToList();
            return View(getOrderDelivery);
        }
        // completed delivery // comfirm
        public ActionResult ConfirmDelivery(int id_order)
        {

            var getOrder = db.orders.FirstOrDefault(x => x.id_order == id_order);
            if (getOrder != null)
            {
                db.orders.FirstOrDefault(x => x.id_order == id_order).successed = true;
                db.orders.FirstOrDefault(x => x.id_order == id_order).delivering = false;
                db.orders.FirstOrDefault(x => x.id_order == id_order).finished_at = DateTime.Now;
                db.SaveChanges();
            }
            return RedirectToAction("DeliveryOrder");
        }

        // completed order

        public ActionResult CompletedOrder()
        {
            var getOrderSuccessed = db.orders.Where(x => x.successed == true).ToList();
            return View(getOrderSuccessed);
        }

        

    }
}
