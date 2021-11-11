namespace DiemDanhQR.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BuoiHoc")]
    public partial class BuoiHoc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BuoiHoc()
        {
            LopMons = new HashSet<LopMon>();
        }

        [Key]
        public int MaBuoi { get; set; }

        public int? TietBatDau { get; set; }

        public int? SoTiet { get; set; }

        public int? MaThu { get; set; }

        public virtual Thu Thu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LopMon> LopMons { get; set; }
    }
}
