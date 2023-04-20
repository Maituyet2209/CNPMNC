using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace demoDACNPMNC.Models
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
            var DT = db.SANPHAMs.Single(s => s.MaDT == this.MaDT);
            this.TenDT = DT.TenDT;
            this.HinhMinhHoa = DT.Hinhminhhoa;
            this.DonGia = double.Parse(DT.Dongia.ToString());
            this.SoLuong = 1;
        }
    }
}