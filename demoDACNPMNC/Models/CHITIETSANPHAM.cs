//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace demoDACNPMNC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CHITIETSANPHAM
    {
        public int MaCTSP { get; set; }
        public int MaDT { get; set; }
        public string MauSac { get; set; }
        public string CauHinh { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
    
        public virtual SANPHAM SANPHAM { get; set; }
    }
}
