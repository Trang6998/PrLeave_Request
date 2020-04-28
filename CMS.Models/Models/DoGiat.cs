using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Models
{
    [Table("DoGiat")]
    public class DoGiat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int DoGiatID { get; set; }

        public int? LoaiDoGiatID { get; set; }

        [MaxLength(100)]
        public string TenDoGiat { get; set; }

        [MaxLength(100)]
        public string PCS { get; set; }

        [ForeignKey("LoaiDoGiatID")]
        public virtual LoaiDoGiat LoaiDoGiat { get; set; }

        [InverseProperty("DoGiat")]
        public virtual ICollection<ChiTietDoGiat> ChiTietDoGiat { get; set; }

        [InverseProperty("DoGiat")]
        public virtual ICollection<DonGia> DonGia { get; set; }

    }
}
