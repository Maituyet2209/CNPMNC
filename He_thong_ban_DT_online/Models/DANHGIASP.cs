//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace He_thong_ban_DT_online.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DANHGIASP
    {
        public int MaDG { get; set; }
        public int MaSP { get; set; }
        public int MaKH { get; set; }
        public string BinhLuan { get; set; }
        public int Rating { get; set; }
        public System.DateTime NgayDanhGia { get; set; }
    
        public virtual KHACHHANG KHACHHANG { get; set; }
        public virtual SANPHAM SANPHAM { get; set; }
    }
}
