using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace demoDACNPMNC
{
    public class Phone
    {
        public string img { get; set; }
        public string ten { get; set; }
        public string gia { get; set; }
        public string giagiam { get; set; }
        public int danhgia { get; set; }
        public Phone() { }

        public Phone(string img, string ten, string gia, string giagiam, int danhgia)
        {
            this.img = img;
            this.ten = ten;
            this.gia = gia;
            this.giagiam = giagiam;
            this.danhgia = danhgia;
        }
    }
}