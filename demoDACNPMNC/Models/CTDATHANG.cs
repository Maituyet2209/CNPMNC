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
    
    public partial class CTDATHANG
    {
        public int SoDH { get; set; }
        public int MaDT { get; set; }
        public Nullable<int> SoLuong { get; set; }
        public Nullable<decimal> Dongia { get; set; }
        public Nullable<decimal> Thanhtien { get; set; }
    
        public virtual SANPHAM SANPHAM { get; set; }
        public virtual DONDATHANG DONDATHANG { get; set; }
    }
}
