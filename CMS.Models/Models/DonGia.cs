using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Models
{
    [Table("DonGia")]
    public class DonGia
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int DonGiaID { get; set; }

        public int? HinhThucGiatID { get; set; }

        public int? DoGiatID { get; set; }

        public int? DonGiaGiat { get; set; }

        public string GhiChu { get; set; }

        [ForeignKey("DoGiatID")]
        public virtual DoGiat DoGiat { get; set; }

        [ForeignKey("HinhThucGiatID")]
        public virtual HinhThucGiat HinhThucGiat { get; set; }

    }
}
