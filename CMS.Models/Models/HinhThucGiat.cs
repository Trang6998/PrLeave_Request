using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Models
{
    [Table("HinhThucGiat")]
    public class HinhThucGiat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int HinhThucGiatID { get; set; }

        [MaxLength(50)]
        public string TenHinhThuc { get; set; }

        public string GhiChu { get; set; }

        [InverseProperty("HinhThucGiat")]
        public virtual ICollection<DonGia> DonGia { get; set; }
        [InverseProperty("HinhThucGiat")]
        public virtual ICollection<ChiTietDoGiat> ChiTietDoGiat { get; set; }

    }
}
