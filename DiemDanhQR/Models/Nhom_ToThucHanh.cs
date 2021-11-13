namespace DiemDanhQR.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Nhom_ToThucHanh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Nhom_ToThucHanh()
        {
            LopMons = new HashSet<LopMon>();
        }

        [Key]
        public int MaNhomToThucHanh { get; set; }

        [StringLength(10)]
        public string Nhom { get; set; }

        public int? ToThucHanh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LopMon> LopMons { get; set; }
    }
}
