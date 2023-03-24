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
        public ActionResult Index()
        {
            List<Phone> list = new List<Phone>();
            list.Add(
            new Phone("https://cdn2.cellphones.com.vn/358x358,webp,q100/media/catalog/product/s/m/sm-s908_galaxys22ultra_front_burgundy_211119.jpg",
                "SamSung galaxy s23","23.000.000","25.000.000",12));
            list.Add(
            new Phone("https://cdn2.cellphones.com.vn/358x358,webp,q100/media/catalog/product/8/0/800x800-1-640x640-5_2.png",
                "Xiaomi Redmi Note 11", "13.000.000", "17.000.000", 22));
            list.Add(
            new Phone("https://cdn2.cellphones.com.vn/358x358,webp,q100/media/catalog/product/1/_/1_250_1.jpg",
                "Nokia C31 4GB", "3.000.000", "6.000.000", 10));
            list.Add(
            new Phone("https://cdn2.cellphones.com.vn/358x358,webp,q100/media/catalog/product/1/4/14_1_9_2_9.jpg",
                "Iphone 13 128GB", "17.560.000", "19.000.000", 2));
            list.Add(
            new Phone("https://cdn2.cellphones.com.vn/358x358,webp,q100/media/catalog/product/i/p/iphone-14-storage-select-202209-6-1inch-y889.jpg",
                "Iphone 13 | chính hãng VN/A", "17.560.000d", "19.000.000", 2));
            list.Add(
           new Phone("https://cdn2.cellphones.com.vn/358x358,webp,q100/media/catalog/product/s/m/sm-s908_galaxys22ultra_front_burgundy_211119.jpg",
               "SamSung galaxy s23", "23.000.000", "25.000.000", 12));
            list.Add(
            new Phone("https://cdn2.cellphones.com.vn/358x358,webp,q100/media/catalog/product/8/0/800x800-1-640x640-5_2.png",
                "Xiaomi Redmi Note 11", "13.000.000", "17.000.000", 22));
            list.Add(
            new Phone("https://cdn2.cellphones.com.vn/358x358,webp,q100/media/catalog/product/1/_/1_250_1.jpg",
                "Nokia C31 4GB", "3.000.000", "6.000.000", 10));
            list.Add(
            new Phone("https://cdn2.cellphones.com.vn/358x358,webp,q100/media/catalog/product/1/4/14_1_9_2_9.jpg",
                "Iphone 13 128GB", "17.560.000", "19.000.000", 2));
            list.Add(
            new Phone("https://cdn2.cellphones.com.vn/358x358,webp,q100/media/catalog/product/i/p/iphone-14-storage-select-202209-6-1inch-y889.jpg",
                "Iphone 13 | chính hãng VN/A", "17.560.000d", "19.000.000", 2));

            return View(list);
        }
    }
}