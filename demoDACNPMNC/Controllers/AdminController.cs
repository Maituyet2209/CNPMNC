using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using demoDACNPMNC.Models;
using PagedList;
using System.IO;
using System.Data.Entity;
using System.Net;

namespace demoDACNPMNC.Controllers
{
    public class AdminController : Controller
    {
        // GET: AdminController
        QLBANDIENTHOAIEntities db = new QLBANDIENTHOAIEntities();
        public ActionResult SanPham(int? page)
        {
            List<SANPHAM> dsSanPham = db.SANPHAMs.ToList();
            int pageSize = 7;
            int pageNum = (page ?? 1);
            return View(dsSanPham.OrderBy(sp => sp.MaDT).ToPagedList(pageNum, pageSize));
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
                    sp.Hinhminhhoa = fileName;
                    db.SANPHAMs.Add(sp);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("SanPham");
        }
        public ActionResult ChiTietSP(int id)
        {
            var sanpham = db.SANPHAMs.FirstOrDefault(sp => sp.MaDT == id);
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
            var sanpham = db.SANPHAMs.FirstOrDefault(sp => sp.MaDT == id);
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
            var sanpham = db.SANPHAMs.FirstOrDefault(sp => sp.MaDT == id);
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
            var imgOld = db.SANPHAMs.FirstOrDefault(s => s.MaDT == sanpham.MaDT)?.Hinhminhhoa;

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
                        sanpham.Hinhminhhoa = fileName;
                    }
                } 
                else
                {
                    sanpham.Hinhminhhoa = imgOld;
                }
                
                //else
                //{
                //    // Giữ giá trị cũ của Hinhminhhoa trong trường hợp không chọn hình
                //    sanpham.Hinhminhhoa = db.SANPHAMs.FirstOrDefault(s => s.MaDT == sanpham.MaDT)?.Hinhminhhoa;
                //}

                db.Entry(sanpham).State = EntityState.Modified;
                db.Entry(sanpham).Property(p => p.Dongia).IsModified = false; // Không cho sửa đổi giá sản phẩm

                db.SaveChanges();
                return RedirectToAction("SanPham");
            }

            ViewBag.MaBrand = new SelectList(db.BRANDs, "BrandId", "TenBrand", sanpham.MaBrand);
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs, "MaNSX", "TenNSX", sanpham.MaNSX);

            return View(sanpham);
        }
        //public ActionResult SuaSP(SANPHAM sanpham, HttpPostedFileBase Hinhminhhoa)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (Hinhminhhoa != null && Hinhminhhoa.ContentLength > 0)
        //        {
        //            var fileName = Path.GetFileName(Hinhminhhoa.FileName);
        //            var path = Path.Combine(Server.MapPath("~/images"), fileName);

        //            if (System.IO.File.Exists(path))
        //            {
        //                ViewBag.ThongBao = "Hình đã tồn tại";
        //            }
        //            else
        //            {
        //                Hinhminhhoa.SaveAs(path);
        //                sanpham.Hinhminhhoa = fileName;
        //            }
        //        }


        //        // Sử dụng Attach để gắn kết đối tượng đã thay đổi vào context
        //        db.SANPHAMs.Attach(sanpham);
        //        db.Entry(sanpham).State = EntityState.Modified;
        //        db.Entry(sanpham).Property(p => p.Dongia).IsModified = false; // Không cho sửa đổi giá sản phẩm

        //        db.SaveChanges();
        //        return RedirectToAction("SanPham");
        //    }

        //    ViewBag.MaBrand = new SelectList(db.BRANDs, "BrandId", "TenBrand", sanpham.MaBrand);
        //    ViewBag.MaNSX = new SelectList(db.NHASANXUATs, "MaNSX", "TenNSX", sanpham.MaNSX);

        //    return View(sanpham);
        //}



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

            var sanpham = db.SANPHAMs.FirstOrDefault(sp => sp.MaDT == id);

            if (sanpham == null)
            {
                return HttpNotFound();
            }

            ViewBag.SanPhamId = sanpham.MaDT;
            ViewBag.SanPhamName = sanpham.TenDT;
            ViewBag.HinhMinhHoa = sanpham.Hinhminhhoa;
            ViewBag.DonGiaHienTai = sanpham.Dongia;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPrice(int id, decimal Dongia)
        {
            var sanpham = db.SANPHAMs.FirstOrDefault(sp => sp.MaDT == id);

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