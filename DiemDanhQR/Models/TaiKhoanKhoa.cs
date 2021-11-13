namespace DiemDanhQR.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoanKhoa")]
    public partial class TaiKhoanKhoa
    {
        [Key]
        [StringLength(11)]
        public string MaTaiKhoan { get; set; }

        [StringLength(50)]
        public string HoTen { get; set; }

        public string MatKhau { get; set; }

        [StringLength(20)]
        public string AnhDaiDien { get; set; }

        public int? MaKhoa { get; set; }

        public int? MaQuyen { get; set; }

        public virtual Khoa Khoa { get; set; }

        public virtual Quyen Quyen { get; set; }
    }
}
