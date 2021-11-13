namespace DiemDanhQR.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ThoiKhoaBieu_DiemDanh
    {
        [Key]
        public int MaThoiKhoaBieu { get; set; }

        [StringLength(11)]
        public string MSSV { get; set; }

        public int? MaLopMon { get; set; }

        [StringLength(100)]
        public string BuoiHoc { get; set; }

        public string MoTa { get; set; }

        public bool? LaBanCanSu { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayDuyet { get; set; }

        public virtual LopMon LopMon { get; set; }

        public virtual SinhVien SinhVien { get; set; }
    }
}
