using demoDACNPMNC.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace demoDACNPMNC.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        QLBANDIENTHOAIEntities db = new QLBANDIENTHOAIEntities();
        public ActionResult CreatePayment()
        {
            // Khởi tạo đối tượng APIContext từ access token của PayPal
            //var apiContext = new APIContext(ConfigurationManager.AppSettings["AccessToken"]);
            var apiContext = GetApiContext();


            // baseURL is the url on which paypal sendsback the data.
            string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/ExecutePayment?";
            //here we are generating guid for storing the paymentID received in session
            //which will be used in the payment execution
            var guid = Convert.ToString((new Random()).Next(100000));

            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            List<MatHangMua> cart = Session["GioHang"] as List<MatHangMua>;

            if (cart == null || cart.Count == 0)
                return View("Error");
            double totalMoney = 0;
            decimal totalM = 0;
            foreach (var i in cart)
            {
                int tranUSDtoVND =(int)(i.DonGia / 23000);
                totalMoney += i.SoLuong * tranUSDtoVND;
                totalM += i.SoLuong * (decimal)i.DonGia;
                
                itemList.items.Add(new Item()
                {
                    name = i.TenDT.ToString(),
                    currency = "USD",
                    price = tranUSDtoVND.ToString(),
                    quantity = i.SoLuong.ToString(),
                    sku = "sku"
                });
            }

            Session["total"] = totalM;
            //return View("Error");
            //itemList.items.Add(new Item()
            //{
            //    name = "Samsung galaxy s22 untral",
            //    currency = "USD",
            //    price = "30999",
            //    quantity = "1",
            //    sku = "sku"
            //});



            // Thiết lập chi tiết thanh toán
            var payment = new Payment
            {
                intent = "sale",
                //intent = "authorize",
                payer = new Payer
                {
                    payment_method = "paypal"
                },
                transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        item_list = itemList,
                        amount = new Amount
                        {
                            currency = "USD",
                            total = totalMoney.ToString(),
                        },
                        description = "Payment description",
                    }
                },
                redirect_urls = new RedirectUrls
                {
                    //return_url = Url.Action("ExecutePayment", "Payment"),
                    cancel_url = Url.Action("Index", "Home"),
                    //cancel_url = baseURI + "guid=" + guid + "&Cancel=true",
                    return_url = baseURI + "guid=" + guid
                }
            };



            // Tạo thanh toán trên PayPal và lưu thông tin thanh toán vào database
            var createdPayment = payment.Create(apiContext);
            //SavePayment(createdPayment);

            // Lấy URL phê duyệt thanh toán PayPal và chuyển hướng người dùng đến đó
            var approvalUrl = createdPayment.links.FirstOrDefault(link => link.rel.Equals("approval_url"));
            if (approvalUrl != null)
            {
                return Redirect(approvalUrl.href);
            }
            else
            {
                // Hiển thị thông báo lỗi nếu không tạo được đường dẫn thanh toán
                return View("Error");
            }
            //return Redirect(approvalUrl.href);
        }

        public ActionResult ExecutePayment()
        {
            try
            {
                // Khởi tạo đối tượng APIContext từ access token của PayPal
                //var apiContext = new APIContext(ConfigurationManager.AppSettings["AccessToken"]);
                var apiContext = GetApiContext();

                string paymentId = Request.QueryString["paymentId"];
                string token = Request.QueryString["token"];
                string PayerID = Request.QueryString["PayerID"];
                // Thực hiện gọi API để xác nhận thanh toán
                var paymentExecution = new PaymentExecution { payer_id = PayerID };
                var payment = new Payment() { id = paymentId };
                var executedPayment = payment.Execute(apiContext, paymentExecution);

                if (executedPayment.state.ToLower() == "approved")
                {
                    // Lưu thông tin thanh toán vào cơ sở dữ liệu
                    var getUser = Session["user"] as user;
                    var getShoppingInf = Session["ShoppingInfo"] as ShoppingInfo;
                    
                        var orderTemp = new order()
                        {
                            id_user = getUser.id_user,
                            payment_type = true,
                            total_price = Session["total"] as decimal?,
                            pending = true,
                            created_at = DateTime.Now,
                            name = getShoppingInf.name,
                            email = getShoppingInf.email,
                            delireryAddress = getShoppingInf.address,
                            phone = getShoppingInf.phone,
                        };
                        db.orders.Add(orderTemp);
                        db.SaveChanges();
                        List<MatHangMua> cart = Session["GioHang"] as List<MatHangMua>;
                        ViewBag.Order = orderTemp;
                        ViewBag.ListOrderItem = cart;
                        ViewBag.paymentType = "Thanh Toán qua PayPal";
                        foreach (var i in cart)
                        {
                            var orderItemTemp = new order_item()
                            {
                                id_order = orderTemp.id_order,
                                MaDT = i.MaDT,
                                quantity = i.SoLuong,
                            };
                            db.order_item.Add(orderItemTemp);
                            var getCart = db.carts.Where(x => x.MaDT == i.MaDT).FirstOrDefault();
                            db.carts.Remove(getCart);
                            db.SaveChanges();
                        }
                        db.SaveChanges();
                    Session["totalCart"] = 0;


                    // Hoặc hiển thị thông báo thành công cho khách hàng
                    return View("Success");
                }
                else
                {
                    // Hiển thị thông báo lỗi nếu thanh toán không thành công
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        private APIContext GetApiContext()
        {
            // Khởi tạo đối tượng APIContext
            string clientId = "ARJHTqeKXz5m-3V7TgtAMuOqjhHjWmeUk9wVspQpFke4fP7kwVGmUlt4yhPEaP4d1Nb-e_teARoV89JV";
            string clientSecret = "EEDaWmbi4Ra2Sgpm3tqQh2XRVjzinHDIKiZqZ7wC3qigxWUF6Dy0TayQVB-IGh4q5yMOmo4Z7ywlkolx";
            var config = new Dictionary<string, string>();
            config.Add("mode", "sandbox"); // Thay đổi thành "live" khi sử dụng tài khoản PayPal thật
            var accessToken = new OAuthTokenCredential(clientId, clientSecret, config).GetAccessToken();
            var apiContext = new APIContext(accessToken);

            return apiContext;
        }

        public ActionResult PayCOD()
        {
            var getUser = Session["user"] as user;
            var getShoppingInf = Session["ShoppingInfo"] as ShoppingInfo;
            if (getShoppingInf == null)
            {
                return View("Error");
            }
            else
            {
                List<MatHangMua> cart = Session["GioHang"] as List<MatHangMua>;

                decimal totalM = 0;

                foreach (var i in cart)
                {
                    totalM += i.SoLuong * (decimal)i.DonGia;
                }
                Session["total"] = totalM;
                var orderTemp = new order()
                {
                    id_user = getUser.id_user,
                    payment_type = false,
                    total_price = Session["total"] as decimal?,
                    pending = true,
                    created_at = DateTime.Now,
                    name = getShoppingInf.name,
                    email = getShoppingInf.email,
                    delireryAddress = getShoppingInf.address,
                    phone = getShoppingInf.phone,
                };
                db.orders.Add(orderTemp);
                db.SaveChanges();
                ViewBag.Order = orderTemp;
                ViewBag.ListOrderItem = cart;
                ViewBag.paymentType = "Thanh Toán khi Nhận Hàng";

                foreach (var i in cart)
                {
                    totalM += i.SoLuong * (decimal)i.DonGia;
                    var orderItemTemp = new order_item()
                    {
                        id_order = orderTemp.id_order,
                        MaDT = i.MaDT,
                        quantity = i.SoLuong,
                    };
                    db.order_item.Add(orderItemTemp);
                    var getCart = db.carts.Where(x => x.MaDT == i.MaDT).FirstOrDefault();
                    db.carts.Remove(getCart);
                    db.SaveChanges();
                }
                Session["total"] = totalM;
                db.SaveChanges();

                Session["totalCart"] = 0;
            }
            return View();
        }
    }
}
