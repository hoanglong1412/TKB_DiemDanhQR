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

        public bool? BuoiHoc1 { get; set; }

        public bool? BuoiHoc2 { get; set; }

        public bool? BuoiHoc3 { get; set; }

        public bool? BuoiHoc4 { get; set; }

        public bool? BuoiHoc5 { get; set; }

        public bool? BuoiHoc6 { get; set; }

        public bool? BuoiHoc7 { get; set; }

        public bool? BuoiHoc8 { get; set; }

        public bool? BuoiHoc9 { get; set; }

        public bool? BuoiHoc10 { get; set; }

        public bool? BuoiHoc11 { get; set; }

        public bool? BuoiHoc12 { get; set; }

        public bool? BuoiHoc13 { get; set; }

        public bool? BuoiHoc14 { get; set; }

        public bool? BuoiHoc15 { get; set; }

        public string MoTa { get; set; }

        public bool? LaBanCanSu { get; set; }

        public virtual LopMon LopMon { get; set; }

        public virtual SinhVien SinhVien { get; set; }
    }
}
