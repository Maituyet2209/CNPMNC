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
    
    public partial class KHACHHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KHACHHANG()
        {
            this.DONDATHANGs = new HashSet<DONDATHANG>();
        }
    
        public int MaKH { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> Ngaysinh { get; set; }
        public Nullable<bool> Gioitinh { get; set; }
        public Nullable<bool> Daduyet { get; set; }
        public Nullable<int> Role_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DONDATHANG> DONDATHANGs { get; set; }
        public virtual ROLE ROLE { get; set; }
        public virtual NGUOIDUNG NGUOIDUNG { get; set; }
    }
}
