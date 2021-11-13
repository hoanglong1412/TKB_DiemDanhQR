namespace DiemDanhQR.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SinhVien")]
    public partial class SinhVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SinhVien()
        {
            ThoiKhoaBieu_DiemDanh = new HashSet<ThoiKhoaBieu_DiemDanh>();
        }

        [Key]
        [StringLength(11)]
        public string MSSV { get; set; }

        [StringLength(50)]
        public string HoTen { get; set; }

        public string MatKhau { get; set; }

        [StringLength(20)]
        public string AnhDaiDien { get; set; }

        public string DiaChi { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(11)]
        public string SDT { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgaySinh { get; set; }

        public bool? LaBanCanSu { get; set; }

        [StringLength(11)]
        public string MaLop { get; set; }

        public virtual Lop Lop { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThoiKhoaBieu_DiemDanh> ThoiKhoaBieu_DiemDanh { get; set; }
    }
}
