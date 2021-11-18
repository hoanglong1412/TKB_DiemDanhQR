namespace DiemDanhQR.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QR")]
    public partial class QR
    {
        [Key]
        [StringLength(200)]
        public string MaQR { get; set; }
        public string DuongDanQR;
    }
}
