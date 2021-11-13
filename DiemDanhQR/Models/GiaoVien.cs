namespace DiemDanhQR.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GiaoVien")]
    public partial class GiaoVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GiaoVien()
        {
            LopMons = new HashSet<LopMon>();
        }

        [Key]
        [StringLength(11)]
        public string MaGiaoVien { get; set; }

        [StringLength(50)]
        public string HoTen { get; set; }

        public string MatKhau { get; set; }

        [StringLength(20)]
        public string AnhDaiDien { get; set; }

        public string DiaChi { get; set; }

        [StringLength(11)]
        public string SDT { get; set; }

        public int? MaKhoa { get; set; }

        public int? MaQuyen { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LopMon> LopMons { get; set; }

        public virtual Khoa Khoa { get; set; }

        public virtual Quyen Quyen { get; set; }
    }
}
