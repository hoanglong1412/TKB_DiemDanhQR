namespace DiemDanhQR.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LopMon")]
    public partial class LopMon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LopMon()
        {
            ThoiKhoaBieu_DiemDanh = new HashSet<ThoiKhoaBieu_DiemDanh>();
        }

        [Key]
        public int MaLopMon { get; set; }

        [StringLength(50)]
        public string TenLopMon { get; set; }

        [StringLength(50)]
        public string MaPhong { get; set; }

        public int? MaBuoi { get; set; }

        [StringLength(11)]
        public string MaMon { get; set; }

        [StringLength(11)]
        public string MaGiaoVien { get; set; }

        public int? MaNhomToThucHanh { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayBatDau { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayKetThuc { get; set; }

        public virtual BuoiHoc BuoiHoc { get; set; }

        public virtual GiaoVien GiaoVien { get; set; }

        public virtual MonHoc MonHoc { get; set; }

        public virtual Nhom_ToThucHanh Nhom_ToThucHanh { get; set; }

        public virtual Phong Phong { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThoiKhoaBieu_DiemDanh> ThoiKhoaBieu_DiemDanh { get; set; }
    }
}
