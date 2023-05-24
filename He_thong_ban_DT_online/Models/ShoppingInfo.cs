using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace He_thong_ban_DT_online.Models
{
    public class ShoppingInfo
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address{ get; set; }

        public ShoppingInfo() { }

        public ShoppingInfo(string name, string email, string phone, string address)
        {
            this.name = name;
            this.email = email;
            this.phone = phone;
            this.address = address;
        }


    }
}