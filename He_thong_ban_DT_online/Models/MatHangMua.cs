using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace He_thong_ban_DT_online.Models
{
    public class MatHangMua
    {
        QLBANDIENTHOAIEntities db = new QLBANDIENTHOAIEntities();
        public int MaDT { get; set; }
        public string TenDT { get; set; }
        public string HinhMinhHoa { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien()
        {
            return SoLuong * DonGia;
        }
        public MatHangMua(int MaDT)
        {
            this.MaDT = MaDT;
            var DT = db.SANPHAMs.Single(s => s.MaSP == this.MaDT);
            this.TenDT = DT.TenDT;
            this.HinhMinhHoa = DT.HinhMinhHoa;
            this.DonGia = double.Parse(DT.Dongia.ToString());
            this.SoLuong = 1;
        }
    }
}